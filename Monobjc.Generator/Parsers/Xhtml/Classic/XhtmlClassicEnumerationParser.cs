//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2012 - Laurent Etiemble
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
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Classic
{
	/// <summary>
	///   XHTML parser dedicated to methods.
	/// </summary>
	public class XhtmlClassicEnumerationParser : XhtmlBaseParser
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "XhtmlClassicEnumerationParser" /> class.
		/// </summary>
		/// <param name = "settings">The settings.</param>
		/// <param name = "typeManager">The type manager.</param>
		public XhtmlClassicEnumerationParser (NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger)
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

		public EnumerationEntity Parse (TypedEntity typedEntity, String name, IEnumerable<XElement> elements)
		{
			EnumerationEntity enumerationEntity = new EnumerationEntity ();

			XElement declarationElement = (from el in elements
                                           where el.Name == "div" &&
				el.Attribute ("class") != null &&
				el.Attribute ("class").Value == "declaration_indent"
                                           select el).FirstOrDefault ();

			XElement parameterElement = (from el in elements
                                         where el.Name == "div" &&
				el.Attribute ("class") != null &&
				el.Attribute ("class").Value == "param_indent"
                                         select el).FirstOrDefault ();

			//XElement discussionElement = (from el in elements
			//                              where el.Name == "h5" && el.Value.Trim() == "Discussion"
			//                              select el).FirstOrDefault();

			XElement availabilityElement = (from el in elements
                                            let term = el.Descendants ("dt").FirstOrDefault ()
                                            let definition = el.Descendants ("dd").FirstOrDefault ()
                                            where el.Name == "dl" &&
				term != null &&
				term.Value.Trim () == "Availability"
                                            select definition).FirstOrDefault ();

			String declaration = declarationElement.TrimAll ();
			String type = "NOTYPE";
			String values = String.Empty;

			bool result = this.SplitEnumeration (declaration, ref name, ref type, ref values);
			if (!result) {
				return null;
			}

			enumerationEntity.Name = name;
			enumerationEntity.BaseType = type;
			enumerationEntity.Namespace = "MISSING";

			// Extract abstract
			IEnumerable<XElement> abstractElements = elements.SkipWhile (el => el.Name != "p").TakeWhile (el => el.Name == "p");
			foreach (XElement element in abstractElements) {
				String line = element.TrimAll ();
				if (!String.IsNullOrEmpty (line)) {
					enumerationEntity.Summary.Add (line);
				}
			}

			//// Extract discussion
			//if (discussionElement != null)
			//{
			//    IEnumerable<XElement> discussionElements = discussionElement.ElementsAfterSelf().TakeWhile(el => el.Name == "p");
			//    foreach (XElement element in discussionElements)
			//    {
			//        String line = element.TrimAll();
			//        if (!String.IsNullOrEmpty(line))
			//        {
			//            enumerationEntity.Summary.Add(line);
			//        }
			//    }
			//}

			// Parse the values
			string[] pairs = values.Split (new []{','}, StringSplitOptions.RemoveEmptyEntries);
			foreach (string pair in pairs) {
				if (String.Equals (pair.TrimAll (), String.Empty)) {
					continue;
				}

				String key;
				String value = String.Empty;
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

				enumerationEntity.Values.Add (enumerationValueEntity);
			}

			XElement termList = parameterElement != null ? parameterElement.Descendants ("dl").FirstOrDefault () : null;
			if (termList != null) {
				IEnumerable<XElement> dtList = from el in termList.Elements ("dt") select el;
				IEnumerable<XElement> ddList = from el in termList.Elements ("dd") select el;

				if (dtList.Count () == ddList.Count ()) {
					// Iterate over definitions
					for (int i = 0; i < dtList.Count(); i++) {
						String term = dtList.ElementAt (i).TrimAll ();
						//String summary = ddList.ElementAt(i).TrimAll();
						IEnumerable<String> summaries = ddList.ElementAt (i).Elements ("p").Select (p => p.Value.TrimAll ());

						// Find the parameter
						EnumerationValueEntity enumerationValueEntity = enumerationEntity.Values.Find (p => String.Equals (p.Name, term));
						if (enumerationValueEntity != null) {
							foreach (string sum in summaries) {
								enumerationValueEntity.Summary.Add (sum);
							}
						}
					}
				}
			}

			// Get the availability
			if (availabilityElement != null) {
				enumerationEntity.MinAvailability = CommentHelper.ExtractAvailability (availabilityElement.TrimAll ());
			}

			return enumerationEntity;
		}
	}
}