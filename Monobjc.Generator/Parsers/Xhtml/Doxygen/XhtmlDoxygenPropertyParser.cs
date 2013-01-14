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
using System.Text;
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Doxygen
{
	/// <summary>
	///   XHTML parser dedicated to properties.
	/// </summary>
	public class XhtmlDoxygenPropertyParser : XhtmlBaseParser
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "XhtmlPropertyParser" /> class.
		/// </summary>
		/// <param name = "settings">The settings.</param>
		/// <param name = "typeManager">The type manager.</param>
		public XhtmlDoxygenPropertyParser (NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger)
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
		///   Parses the specified property element.
		/// </summary>
		/// <param name = "propertyElement">The property element.</param>
		/// <returns></returns>
		public PropertyEntity Parse (TypedEntity typedEntity, XElement propertyElement)
		{
			String name = propertyElement.Element ("name").Value;

			// Rebuild the signature from the documentation
			String returnType = propertyElement.Element ("type").TrimAll ();
			bool isStatic = (propertyElement.Attribute ("static").Value == "yes");
			bool readable = (propertyElement.Attribute ("readable").Value == "yes");
			bool writable = (propertyElement.Attribute ("writable").Value == "yes");
			String accessor = propertyElement.Attribute ("accessor").Value;

			// Remove weak modifier
			if (returnType.StartsWith ("__weak")) {
				returnType = returnType.Replace ("__weak", String.Empty).TrimAll ();
			}

			// Extract brief description
			IEnumerable<XElement> abstractElements = propertyElement.Element ("briefdescription").Elements ("para");

			// Extract for detailed description
			IEnumerable<XElement> detailsElements = (from el in propertyElement.Element ("detaileddescription").Elements ("para")
                                                     where !el.Elements ("simplesect").Any ()
				&& el.Elements ("parameterlist").Any ()
				&& el.Elements ("xrefsect").Any ()
                                                     select el);

			// Add brief description
			List<String> summary = new List<String> ();
			foreach (XElement paragraph in abstractElements) {
				summary.Add (paragraph.TrimAll ());
			}
			foreach (XElement paragraph in detailsElements) {
				summary.Add (paragraph.TrimAll ());
			}

			// Recreate the signature
			StringBuilder signatureBuilder = new StringBuilder ();
			signatureBuilder.Append ("@property (nonatomic");
			if (readable) {
				if (!writable) {
					signatureBuilder.Append (",readonly");
				}
			}
			signatureBuilder.Append (",");
			signatureBuilder.Append (accessor);
			signatureBuilder.Append (") ");
			signatureBuilder.AppendFormat ("{0} {1};", returnType, name);

			String signature = signatureBuilder.ToString ();

			String getterSelector = name;

			// Alter the name
			name = name.UpperCaseFirstLetter ();

			String setterSelector = "set" + name + ":";

			bool isOut, isByRef, isBlock;
			String type = this.TypeManager.ConvertType (returnType, out isOut, out isByRef, out isBlock, this.Logger);

			PropertyEntity propertyEntity = new PropertyEntity ();

			//propertyEntity.MinAvailability = minAvailability;
			propertyEntity.Name = getterSelector.UpperCaseFirstLetter ();
			propertyEntity.Static = isStatic; // Should always be false...
			propertyEntity.Summary = summary;
			propertyEntity.Type = type;

			propertyEntity.Getter = new MethodEntity ();
			propertyEntity.Getter.Signature = signature;
			propertyEntity.Getter.Selector = getterSelector;

			if (readable && !writable) {
				propertyEntity.Setter = null;
			} else {
				propertyEntity.Setter = new MethodEntity ();
				propertyEntity.Setter.Signature = signature;
				propertyEntity.Setter.Selector = setterSelector;
			}

			return propertyEntity;
		}
	}
}