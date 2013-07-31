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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Cocoa
{
	/// <summary>
	///   XHTML parser dedicated to constants.
	/// </summary>
	public class XhtmlCocoaConstantParser : XhtmlBaseParser
	{
		protected static readonly Regex L_VALUE_REGEX = new Regex (@"(?<=\d)L");
		protected static readonly Regex UL_VALUE_REGEX = new Regex (@"(?<=\d)UL");
		protected static readonly Regex ULL_VALUE_REGEX = new Regex (@"(?<=\d)ULL");

		/// <summary>
		///   Initializes a new instance of the <see cref = "XhtmlNotificationParser" /> class.
		/// </summary>
		/// <param name = "settings">The settings.</param>
		/// <param name = "typeManager">The type manager.</param>
		public XhtmlCocoaConstantParser (NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger)
		{
		}

		/// <summary>
		///   Parses the specified entity.
		/// </summary>
		/// <param name = "entity">The entity.</param>
		/// <param name = "reader">The reader.</param>
		public override void Parse (BaseEntity entity, TextReader reader)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		///   Parses the specified constant element.
		/// </summary>
		/// <param name = "constantElement">The constant element.</param>
		public List<BaseEntity> Parse (TypedEntity typedEntity, XElement constantElement)
		{
			// Get the name
			String name = constantElement.TrimAll ();

			// Get the abstract
			XElement summaryElement = (from el in constantElement.ElementsAfterSelf ("p")
                                       where (String)el.Attribute ("class") == "abstract"
                                       select el).FirstOrDefault ();
			String summary = summaryElement.TrimAll ();

			// Get the declaration
			XElement declarationElement = (from el in constantElement.ElementsAfterSelf ("pre")
                                           where (String)el.Attribute ("class") == "declaration"
                                           select el).FirstOrDefault ();
			String declaration = declarationElement.TrimAll ();

			// Make various tests
			bool isDefine = declaration.StartsWith ("#define");
			bool isEnum = declaration.StartsWith ("enum") || declaration.StartsWith ("typedef enum") || declaration.Contains (" enum ");

			if (isDefine) {
				List<BaseEntity> entities = ExtractDefine (constantElement, name, summary, declaration);
				return entities;
			}

			if (isEnum) { // keep newlines for enum processing
				List<BaseEntity> entities = this.ExtractEnumeration (constantElement, name, summary, declarationElement.TrimSpaces());
				if (entities != null) {
					return entities;
				}
			}

			return this.ExtractConstants (constantElement, name, summary, declaration);
		}

		private static List<BaseEntity> ExtractDefine (XElement constantElement, String name, String summary, String declaration)
		{
			return null;
		}

		private static String RefineEnumBaseType (String values)
		{
			if (ULL_VALUE_REGEX.IsMatch(values))
				return "uint64_t";
			else if (UL_VALUE_REGEX.IsMatch(values))
				return "NSUInteger";
			else if (values.Contains("&lt;&lt;")) // Flags
				return "NSUInteger";
			else if (L_VALUE_REGEX.IsMatch(values))
				return "NSInteger";
			else
				return "NOTYPE";
				// Consider: 
				// return "NSInteger";
		}

		private static String ConvertNumericQualifier (String value)
		{
			if (L_VALUE_REGEX.IsMatch (value))
				value = L_VALUE_REGEX.Replace(value, "");
			else if (ULL_VALUE_REGEX.IsMatch (value))
				value = ULL_VALUE_REGEX.Replace(value, "UL");
			else if (UL_VALUE_REGEX.IsMatch (value))
				value = UL_VALUE_REGEX.Replace(value, "U");

			return value;
		}

		private List<BaseEntity> ExtractEnumeration (XElement constantElement, String name, String summary, String declaration)
		{
			declaration = declaration.Trim (';');

			// Default values
			String type = "NOTYPE";
			String values = String.Empty;

			// Match the enumeration definition
			bool result = this.SplitEnumeration (declaration, ref name, ref type, ref values);
			if (!result) {
				return null;
			}

			if (type == "NOTYPE") {
				type = RefineEnumBaseType(values);
			}

			// Create the enumeration
			EnumerationEntity enumerationEntity = new EnumerationEntity ();
			enumerationEntity.Name = name;
			enumerationEntity.BaseType = type;
			enumerationEntity.Namespace = "MISSING";
			enumerationEntity.Summary.Add (summary);

			// Parse the values
			var pairs = values.Split (new []{'\n'}, StringSplitOptions.RemoveEmptyEntries);
			foreach (string immutablePair in pairs) {
				String key;
				String value = String.Empty;
				String pair = immutablePair.Replace(",","");
				
				// Handle value assignment
				if (pair.IndexOf ('=') != -1) {
					string[] parts = pair.Split (new []{'='}, StringSplitOptions.RemoveEmptyEntries);
					key = parts [0].Trim ();
					value = parts [1].Trim ();
				} else {
					key = pair.Trim ();
				}

				// Add a new value
				EnumerationValueEntity enumerationValueEntity = new EnumerationValueEntity ();
				enumerationValueEntity.Name = key;

				if (value.Length == 6 && value.StartsWith ("'") && value.EndsWith ("'")) {
					String v = value.Trim ('\'');
					enumerationValueEntity.Value = "0x" + FourCharToInt (v).ToString ("X8");
				} else {
					enumerationValueEntity.Value = value;
				}

				// Convert number qualifiers from native to managed
				enumerationValueEntity.Value = ConvertNumericQualifier (enumerationValueEntity.Value);

				enumerationEntity.Values.Add (enumerationValueEntity);
			}

			// Get the definitions
			XElement termList = (from el in constantElement.ElementsAfterSelf ("dl")
                                 where (String)el.Attribute ("class") == "termdef"
                                 select el).FirstOrDefault ();
			if (termList != null) {
				IEnumerable<XElement> dtList = termList.Elements ("dt");
				IEnumerable<XElement> ddList = termList.Elements ("dd");

				if (dtList.Count () == ddList.Count ()) {
					// Iterate over definitions
					for (int i = 0; i < dtList.Count(); i++) {
						String term = dtList.ElementAt (i).Value.TrimAll ();
						IEnumerable<String> summaries = ddList.ElementAt (i).Elements ("p").Select (p => p.Value.TrimAll ());

						// Find the enumeration value
						EnumerationValueEntity enumerationValueEntity = enumerationEntity.Values.Find (v => v.Name == term);
						if (enumerationValueEntity != null) {
							foreach (string sum in summaries) {
								if (CommentHelper.IsAvailability (sum)) {
									enumerationValueEntity.MinAvailability = CommentHelper.ExtractAvailability (sum);
									break;
								}
								enumerationValueEntity.Summary.Add (sum);
							}
						} else {
							this.Logger.WriteLine ("Term with no match '" + term + "'");
						}
					}
				} else {
					this.Logger.WriteLine ("MISMATCH in terms");
				}
			}

			// Make sure availability is ok
			enumerationEntity.AdjustAvailability ();

			return new List<BaseEntity> {enumerationEntity};
		}

		private List<BaseEntity> ExtractConstants (XElement constantElement, String name, String summary, String declaration)
		{
			List<BaseEntity> constants = new List<BaseEntity> ();

			// Extract types and names
			string[] declarations = declaration.Split (new []{';'}, StringSplitOptions.RemoveEmptyEntries);
			foreach (string part in declarations) {
				//this.Logger.WriteLine("Parsing constant '{0}'...", part.Trim());

				String stripped = part.Trim ();
				stripped = stripped.Replace ("extern", String.Empty);
				stripped = stripped.Replace ("const", String.Empty);
				stripped = stripped.TrimAll ();

				Match r = CONSTANT_REGEX.Match (stripped);
				if (r.Success) {
					String type = r.Groups [1].Value.Trim (' ', '*', ' ');

					bool isOut;
					bool isByRef;
					bool isBlock;
					type = this.TypeManager.ConvertType (type, out isOut, out isByRef, out isBlock, this.Logger);

					ConstantEntity constantEntity = new ConstantEntity ();
					constantEntity.Type = type;
					constantEntity.Name = r.Groups [2].Value.Trim ();
					constants.Add (constantEntity);

					//this.Logger.WriteLine("Constant found '{0}' of type '{1}'", constantEntity.Name, constantEntity.Type);
				} else {
					this.Logger.WriteLine ("FAILED to parse constant '{0}'", stripped);
					return null;
				}
			}

			// Get the definitions
			XElement termDefinitions = (from el in constantElement.ElementsAfterSelf ("dl")
                                        where (String)el.Attribute ("class") == "termdef"
                                        select el).FirstOrDefault ();
			if (termDefinitions == null) {
				this.Logger.WriteLine ("MISSING terms");
				return null;
			}

			IEnumerable<XElement> termName = termDefinitions.Elements ("dt");
			IEnumerable<XElement> termDefinition = termDefinitions.Elements ("dd");

			if (termName.Count () == termDefinition.Count ()) {
				// Iterate over definitions
				for (int i = 0; i < termName.Count(); i++) {
					String term = termName.ElementAt (i).Value.TrimAll ();
					IEnumerable<String> summaries = termDefinition.ElementAt (i).Elements ("p").Select (p => p.Value.TrimAll ());

					// Find the enumeration value
					BaseEntity baseEntity = constants.Find (c => c.Name == term);
					if (baseEntity != null) {
						foreach (string sum in summaries) {
							if (CommentHelper.IsAvailability (sum)) {
								baseEntity.MinAvailability = CommentHelper.ExtractAvailability (sum);
								break;
							}
							baseEntity.Summary.Add (sum);
						}
					} else {
						this.Logger.WriteLine ("Term with no match '" + term + "'");
					}
				}
			} else {
				this.Logger.WriteLine ("MISMATCH in terms");
				return null;
			}

			return constants;
		}
	}
}