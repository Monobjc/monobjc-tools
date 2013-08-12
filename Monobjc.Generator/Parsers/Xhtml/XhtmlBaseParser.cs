//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2013 - Laurent Etiemble
//
// Monobjc is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// Monobjc is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Monobjc.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;
using System.IO;

namespace Monobjc.Tools.Generator.Parsers.Xhtml
{
	/// <summary>
	///   Base class for XHTML parsing.
	/// </summary>
	public abstract class XhtmlBaseParser : BaseParser
	{
		protected static readonly Regex ENUMERATION_REGEX = new Regex (@"(typedef\s)?enum(\s?[_A-z]+)?\s*(\/\*\s*:\s*(.+)?\s*\*\/)?\s*\{[\r\n]?(.+)\};?(\s?typedef)?(\s?[A-z0-9_]+\s?)?([A-z]+)?", RegexOptions.Singleline);
		protected static readonly Regex CONSTANT_REGEX = new Regex (@"(id ?|unsigned ?|double ?|float ?\*?|NSString ?\*? ?|CFStringRef ?|CIFormat|CATransform3D|CLLocationDistance ?)([A-z0-9]+)$");
		protected static readonly Regex PARAMETER_REGEX = new Regex (@"(const )?([0-9A-z]+ ?\*{0,2} ?)([0-9A-z]+)");
		protected static readonly Regex COMMENTS_REGEX = new Regex (@"(/\*(.|[\r\n])*?\*/)|(//.*)", RegexOptions.Singleline);

		/// <summary>
		///   Initializes a new instance of the <see cref = "XhtmlBaseParser" /> class.
		/// </summary>
		/// <param name = "settings">The settings.</param>
		/// <param name = "typeManager">The type manager.</param>
		protected XhtmlBaseParser (NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger)
		{
		}

		/// <summary>
		///   Gets the name of the method.
		/// </summary>
		/// <param name = "methodEntity">The method entity.</param>
		/// <returns></returns>
		internal static String GetMethodName (MethodEntity methodEntity)
		{
			String[] parts = methodEntity.Selector.Split (new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder builder = new StringBuilder ();
			foreach (String part in parts) {
				String token = part.UpperCaseFirstLetter ();
				builder.Append (token);
			}
			return builder.ToString ();
		}

		protected bool SplitEnumeration (String declaration, ref String name, ref String type, ref String values)
		{
			String trimmedDeclaration = declaration;
			while (trimmedDeclaration.Contains("  ")) {
				trimmedDeclaration = trimmedDeclaration.Replace ("  ", " ");
			}

			Match r = ENUMERATION_REGEX.Match (trimmedDeclaration);
			if (r.Success) {
				String v2 = r.Groups [2].Value.Trim (); // Name at beginning
				String v4 = r.Groups [4].Value.Trim (); // Type in comment
				String v7 = r.Groups [7].Value.Trim (); // Name at end (or type)
				String v8 = r.Groups [8].Value.Trim (); // Name at end (with preceding type)

				values = r.Groups [5].Value.Trim ();

				// Make sure the values declaration didn't
				// contain multiple enums like CFSocketStream 
				// by checking for '{'.
				if (values.Contains('{'))
					return false;

				// Name can be before enumeration values
				if (!String.IsNullOrEmpty (v4) && !String.IsNullOrEmpty (v7)) {
					type = v4;
					name = v7;
				} else if (!String.IsNullOrEmpty (v7) && !String.IsNullOrEmpty (v8)) {
					type = v7;
					name = v8;
				} else if (!String.IsNullOrEmpty (v7) && String.IsNullOrEmpty (v8)) {
					name = v7;
				} else if (!String.IsNullOrEmpty (v2) && !String.IsNullOrEmpty (v7)) {
					name = v7;
				} else if (!String.IsNullOrEmpty (v2) && String.IsNullOrEmpty (v8)) {
					name = v2;
				}

				// Clean results
				name = name.Trim (';');

				// Make sure name is ok
				name = " -:—".Aggregate (name, (current, c) => current.Replace (c, '_'));

				bool isOut;
				bool isByRef;
				bool isBlock;
				type = this.TypeManager.ConvertType (type, out isOut, out isByRef, out isBlock, this.Logger);

				// Clean values
				CleanEnumValues (name, ref values);

				//this.Logger.WriteLine("Enumeration found '{0}' of type '{1}'", name, type);

				return true;
			}

			this.Logger.WriteLine ("FAILED to parse enum '{0}'", declaration);

			return false;
		}

		/// <summary>
		/// Cleans the enum values section by stripping comments
		/// and formatting in preparation for key/value parsing.
		/// </summary>
		/// <param name="name">Name of enum.</param>
		/// <param name="values">Values to clean.</param>
		private static void CleanEnumValues (String name, ref String values)
		{
            // Remove single line comment that appears on their own line.
            // See NSAlignmentOptions.
            //
            // Example:
            //
            // enum {
            //   v1 = 1,
            //   // Comment
            //   v2 = 2,
            // }
            //
            // enum {
            //   v1 = 1,
            //   v2 = 2,
            // }
            var lines = values.Split(new [] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            values = String.Join("\n", lines.Where(l => !l.Trim().StartsWith("//")));

			// Rejoin wrapped lines that had no comma (unless that would make for two = assignements).
			// See UIInterfaceOrientationMask. Single line format is required for key / value parsing later.
			//
			// Example:
			//
			// enum {
			//   v1
			// = 1,
			//   v2 = 2,
			// }
			//
			// enum {
			//   v1 = 1,
			//   v2 = 2,
			// }
			lines = values.Split(new [] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			values = string.Empty;
			for (int i = 0; i < lines.Length; ++i) {
				var line = lines[i];
				values += line;
				if (line.Contains(',')) {
					values += "\n";
				} else if (line.Contains("=") && i + 1 < lines.Length && lines[i + 1].Contains("=")) {
					values += "\n";
				}
			}

			// Process lines with a comment adjacent to a value (like UIBarButtonSystem).
			// This must be performed before comment stripping to catch values.
			//
			// Example:
			//
			// enum {
			//  v1, // c1v2 // c2
			// }
			//
			// enum {
			//   v1, /* c1 */
			//   v2, /* c2 */
			// }
			lines = values.Split(new [] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			values = string.Empty;
			foreach (string line in lines) {
				// Split on each type name
				var nameSplit = line.Split(new string[] { name }, StringSplitOptions.None);
				// If there is a comment and more than one reference to the type
				if (line.Contains("//") && nameSplit.Length - 1 > 1) {
					// Keep the first reference as is or ignore it
					if (!string.IsNullOrEmpty(nameSplit[0].Trim()) && nameSplit[0].Contains(name))
						values += name + nameSplit[0] + "\n";
					// Add a newline before each type reference
					for (int i = 1; i < nameSplit.Length; ++i) {
						if (!string.IsNullOrEmpty(nameSplit[i].Trim())) {
							// Convert comment to format supporting post-comma
							nameSplit[i] = nameSplit[i].Replace("//", "/*").Trim();
							if (!values.EndsWith("\n"))
								values += "\n";
							values += name + nameSplit[i];
							if (nameSplit[i].Contains("/*"))
								values += " */";
							values += "\n";
						}
					}
				} else {
					values += line.Trim() + "\n";
				}
			}

			// Strip all comments before next phase so that commas in comments
			// or before comments don't throw off the new lines. Comments in
			// keys also interfere with doc generation, and comments in values
			// can interfere with comma placement during generation. It's best
			// to just strip them.
			values = COMMENTS_REGEX.Replace(values, "");

			// Split lines with commas (like NSCalendarUnit, UIPrintInfoOutputType) not 
			// between comments (like CDTextEncodings). Comments must be stripped first.
			//
			// Example:
			//
			// enum { v1, v2, /* A comment, here */ v3 }
			//
			// enum {
			//   v1,
			//   v2,
			//   v3
			// }
			lines = values.Split (new []{'\n'}, StringSplitOptions.RemoveEmptyEntries);
			values = string.Empty;
			foreach (string line in lines) {
				var lineSplit = line.Trim().Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
				foreach (var l in lineSplit) {
					values += l + ",\n";
				}
			}
		}

		/// <summary>
		///   Converts a four-char value to an unsigned integer.
		/// </summary>
		/// <param name = "fourCharValue">The four char value.</param>
		/// <returns>An unsigned integer</returns>
		protected static uint FourCharToInt (String fourCharValue)
		{
			if (fourCharValue.Length != 4) {
				throw new ArgumentException ();
			}
			return fourCharValue.Aggregate (0u, (i, c) => (i * 256) + c);
		}
	}
}