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
using System.Xml;
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Classic
{
	/// <summary>
	///   XHTML parser dedicated to classes and protocols.
	/// </summary>
	public class XhtmlClassicClassParser : XhtmlClassicTypeParser, IXhtmlClassParser
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "XhtmlClassicClassParser" /> class.
		/// </summary>
		/// <param name = "settings">The settings.</param>
		/// <param name = "typeManager">The type manager.</param>
		public XhtmlClassicClassParser (NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger)
		{
			this.MethodParser = new XhtmlClassicMethodParser (settings, typeManager, logger);
			this.PropertyParser = new XhtmlClassicPropertyParser (settings, typeManager, logger);
		}

		/// <summary>
		///   Gets or sets the method parser.
		/// </summary>
		/// <value>The method parser.</value>
		protected XhtmlClassicMethodParser MethodParser { get; private set; }

		/// <summary>
		///   Gets or sets the property parser.
		/// </summary>
		/// <value>The property parser.</value>
		protected XhtmlClassicPropertyParser PropertyParser { get; private set; }

		/// <summary>
		///   Parses the specified entity.
		/// </summary>
		/// <param name = "entity">The entity.</param>
		/// <param name = "reader">The reader.</param>
		public override void Parse (BaseEntity entity, TextReader reader)
		{
			ClassEntity classEntity = (ClassEntity)entity;
			if (classEntity == null) {
				throw new ArgumentException ("ClassEntity expected", "entity");
			}

			using (XmlTextReader xmlTextReader = new XmlTextReader(reader)) {
				XElement root = XElement.Load (xmlTextReader);

				// Get the spec box
				XElement specElement = (from el in root.Descendants ("div")
                                        where (String)el.Attribute ("class") == "spec_sheet_info_box"
                                        select el).FirstOrDefault ();
				if (specElement != null) {
					// Inherits
					XElement inheritElement = (from el in specElement.Descendants ("td")
                                               let prev = el.ElementsBeforeSelf ("td").FirstOrDefault ()
                                               where prev != null &&
						(prev.Value.TrimAll () == "Inherits from:" ||
						prev.Value.TrimAll () == "Inherits&nbsp;from:")
                                               select el).FirstOrDefault ();
					if (inheritElement != null) {
						string hierarchy = inheritElement.TrimAll ();
						string[] parents = hierarchy.Split (new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
						classEntity.BaseType = parents [0].Trim ();
					}

					// Conforms To
					XElement conformsElements = (from el in specElement.Descendants ("td")
                                                 let prev = el.ElementsBeforeSelf ("td").FirstOrDefault ()
                                                 where prev != null &&
						(prev.Value.TrimAll () == "Conforms to:" ||
						prev.Value.TrimAll () == "Conforms&nbsp;to:")
                                                 select el).FirstOrDefault ();
					if (conformsElements != null) {
						List<String> protocols = new List<string> ();
						foreach (String protocol in conformsElements.TrimAll().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)) {
							if (protocol.Contains ("NSObject") || protocol.Contains ("NSProxy")) {
								continue;
							}
							protocols.Add (protocol.Trim ());
						}
						classEntity.ConformsTo = String.Join (",", protocols.Distinct ().ToArray ());
					}
				}

				// Functions
				this.ExtractFunctions (classEntity, root);

				// Methods
				this.ExtractMethods (classEntity, root);

				// Properties
				this.ExtractProperties (classEntity, root);

				// Constants
				this.ExtractConstants (classEntity, root);

				// Enumerations
				this.ExtractEnumerations (classEntity, root);
			}

			// Extract getter and search for setter
			List<String> notGetterMethods = this.Settings ["NotGetterMethods"].Split (new []{','}, StringSplitOptions.RemoveEmptyEntries).ToList ();
			List<MethodEntity> methodModels = new List<MethodEntity> (classEntity.Methods);
			foreach (MethodEntity methodModel in methodModels) {
				if (notGetterMethods.Contains (methodModel.Name) || !methodModel.IsGetter) {
					continue;
				}

				classEntity.Methods.Remove (methodModel);
				MethodEntity getter = methodModel;

				MethodEntity setter = classEntity.Methods.Find (m => String.Equals ("Set" + getter.Name, m.Name) && String.Equals (m.ReturnType, "void"));
				if (setter == null) {
					setter = classEntity.Methods.Find (m => m.IsSetterFor (getter));
				}
				if (setter != null) {
					classEntity.Methods.Remove (setter);
				}

				PropertyEntity property = new PropertyEntity ();
				property.Name = getter.Name;
				property.Type = getter.ReturnType;
				property.Summary = getter.Summary;
				property.MinAvailability = getter.MinAvailability;
				property.Static = getter.Static;
				property.Getter = getter;
				property.Setter = setter;

				classEntity.Properties.Add (property);
			}

			// Ensure that availability is set on entity.
			entity.AdjustAvailability ();
		}

		protected void ExtractProperties (ClassEntity classEntity, XElement root)
		{
			XElement listMaker = (from el in root.Descendants ("h2")
                                  where el.Value == "Properties"
                                  select el).FirstOrDefault ();
			XElement list = listMaker != null ? listMaker.ElementsAfterSelf ("dl").FirstOrDefault () : null;
			if (list != null) {
				// Collect names
				List<String> names = new List<string> ();
				foreach (XElement term in list.Descendants("dt")) {
					String name = term.Value.TrimAll ();
					names.Add (name);
				}

				// Search for a table with preceding by an anchor containing the name
				foreach (String name in names) {
					XElement marker = (from el in list.ElementsAfterSelf ("a")
                                       where el.Attribute ("name") != null &&
						el.Attribute ("name").Value.EndsWith ("/" + name)
                                       select el).FirstOrDefault ();
					if (marker != null) {
						//XElement startElement = marker.ElementsAfterSelf ("table").FirstOrDefault ();
						IEnumerable<XElement> elements = marker.ElementsAfterSelf ().TakeWhile (el => el.Name != "a");

						PropertyEntity propertyEntity = this.PropertyParser.Parse (classEntity, name, elements);
						classEntity.Properties.Add (propertyEntity);
					} else {
						this.Logger.WriteLine ("MISSING marker for property " + name);
					}
				}
			}
		}

		protected void ExtractMethods (ClassEntity classEntity, XElement root)
		{
			XElement listMarker = (from el in root.Descendants ("h2")
                                   where el.Value == "Methods"
                                   select el).FirstOrDefault ();
			XElement list = listMarker != null ? listMarker.ElementsAfterSelf ("dl").FirstOrDefault () : null;
			if (list != null) {
				// Collect names
				List<String> names = new List<string> ();
				foreach (XElement term in list.Descendants("dt")) {
					String name = term.Value.TrimAll ();
					names.Add (name.Trim ('-', '+'));
				}

				// Search for a table with preceding by an anchor containing the name
				foreach (String name in names) {
					XElement marker = (from el in list.ElementsAfterSelf ("a")
                                       where el.Attribute ("name") != null &&
						el.Attribute ("name").Value.EndsWith ("/" + name)
                                       select el).FirstOrDefault ();
					if (marker != null) {
						//XElement startElement = marker.ElementsAfterSelf ("table").FirstOrDefault ();
						IEnumerable<XElement> elements = marker.ElementsAfterSelf ().TakeWhile (el => el.Name != "a");

						MethodEntity methodEntity = this.MethodParser.Parse (classEntity, name, elements);
						classEntity.Methods.Add (methodEntity);
					} else {
						this.Logger.WriteLine ("MISSING marker for method " + name);
					}
				}
			}
		}
	}
}