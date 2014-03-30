//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2014 - Laurent Etiemble
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

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Cocoa
{
	/// <summary>
	///   XHTML parser dedicated to classes and protocols.
	/// </summary>
	public class XhtmlCocoaClassParser : XhtmlCocoaTypeParser, IXhtmlClassParser
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "XhtmlClassParser" /> class.
		/// </summary>
		/// <param name = "settings">The settings.</param>
		public XhtmlCocoaClassParser (NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger)
		{
			this.MethodParser = new XhtmlCocoaMethodParser (settings, typeManager, logger);
			this.PropertyParser = new XhtmlCocoaPropertyParser (settings, typeManager, logger);
		}

		/// <summary>
		///   Gets or sets the method parser.
		/// </summary>
		/// <value>The method parser.</value>
		protected XhtmlCocoaMethodParser MethodParser { get; private set; }

		/// <summary>
		///   Gets or sets the property parser.
		/// </summary>
		/// <value>The property parser.</value>
		protected XhtmlCocoaPropertyParser PropertyParser { get; private set; }

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
                                        where el.Attribute ("class").Contains("spec_sheet_info_box")
                                        select el).FirstOrDefault ();
				if (specElement != null) {
					// Availability
					XElement availabilityElement = (from el in specElement.Descendants ("td")
                                                    let prev = el.ElementsBeforeSelf ("td").FirstOrDefault ()
                                                    where prev != null && prev.Value.TrimAll () == "Availability"
                                                    select el).FirstOrDefault ();
					if (availabilityElement != null) {
						entity.MinAvailability = CommentHelper.ExtractAvailability (availabilityElement.TrimAll ());
					}

					// Inherits
					XElement inheritElement = (from el in specElement.Descendants ("td")
                                               let prev = el.ElementsBeforeSelf ("td").FirstOrDefault ()
                                               where prev != null && prev.Value.TrimAll () == "Inherits from"
                                               select el).FirstOrDefault ();
					if (inheritElement != null) {
						string hierarchy = inheritElement.TrimAll ();
						string[] parents = hierarchy.Split (new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
						classEntity.BaseType = parents [0].Trim ();
					}

					// Conforms To
					XElement conformsElements = (from el in specElement.Descendants ("td")
                                                 let prev = el.ElementsBeforeSelf ("td").FirstOrDefault ()
                                                 where prev != null && prev.Value.TrimAll () == "Conforms to"
                                                 select el).FirstOrDefault ();
					if (conformsElements != null) {
						List<String> protocols = new List<string> ();
						foreach (XElement conformsElement in conformsElements.Descendants("span")) {
							String protocol = conformsElement.TrimAll ();
							if (protocol.Contains ("(NSObject)") || protocol.Contains ("(NSProxy)")) {
								continue;
							}
							int pos = protocol.IndexOf ('(');
							if (pos != -1) {
								protocol = protocol.Substring (0, pos).Trim ();
							}
							protocols.Add (protocol);
						}
						classEntity.ConformsTo = String.Join (",", protocols.Distinct ().ToArray ());
					}
				}

				// Class Methods
				this.ExtractClassMethods (classEntity, root);

				// Instance Methods
				this.ExtractInstanceMethods (classEntity, root);

				// Delegate Methods
				this.ExtractDelegateMethods (classEntity, root);

				// Properties
				this.ExtractProperties (classEntity, root);

				// Constants
				this.ExtractConstants (classEntity, root);

				// Notifications
				this.ExtractNotifications (classEntity, root);

				// Functions
				this.ExtractFunctions (classEntity, root);
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

				// TODO: Refactor to use the IsSetterFor with an optional parameter to strip the prefix
				MethodEntity setter = classEntity.Methods.Find (m => String.Equals ("Set" + getter.Name, m.Name) &&
					String.Equals (m.ReturnType, "void") &&
					m.Parameters.Count == 1 &&
					m.Static == getter.Static);
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
			IEnumerable<XElement> propertyElements = from el in root.Descendants ("div")
                                                     where (String)el.Attribute ("class") == "api propertyObjC"
                                                     select el;
			foreach (XElement propertyElement in propertyElements) {
				PropertyEntity propertyEntity = this.PropertyParser.Parse (classEntity, propertyElement);
				classEntity.Properties.Add (propertyEntity);
			}
		}

		protected void ExtractInstanceMethods (ClassEntity classEntity, XElement root)
		{
			IEnumerable<XElement> methods = from el in root.Descendants ("div")
                                            where (String)el.Attribute ("class") == "api instanceMethod"
                                            select el;
			foreach (XElement methodElement in methods) {
				MethodEntity methodEntity = this.MethodParser.Parse (classEntity, methodElement);
				classEntity.Methods.Add (methodEntity);
			}
		}

		protected void ExtractClassMethods (ClassEntity classEntity, XElement root)
		{
			IEnumerable<XElement> methods = from el in root.Descendants ("div")
                                            where (String)el.Attribute ("class") == "api classMethod"
                                            select el;
			foreach (XElement methodElement in methods) {
				MethodEntity methodEntity = this.MethodParser.Parse (classEntity, methodElement);
				classEntity.Methods.Add (methodEntity);
			}
		}

		protected void ExtractDelegateMethods (ClassEntity classEntity, XElement root)
		{
			IEnumerable<XElement> methods = from el in root.Descendants ("div")
                                            where (String)el.Attribute ("class") == "api delegateMethod"
                                            select el;
			foreach (XElement methodElement in methods) {
				MethodEntity methodEntity = this.MethodParser.Parse (classEntity, methodElement);
				classEntity.DelegateMethods.Add (methodEntity);
			}
		}
	}
}