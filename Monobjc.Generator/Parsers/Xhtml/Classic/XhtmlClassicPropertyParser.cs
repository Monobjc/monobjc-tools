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
    ///   XHTML parser dedicated to properties.
    /// </summary>
    public class XhtmlClassicPropertyParser : XhtmlBaseParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlPropertyParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
		public XhtmlClassicPropertyParser(NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger) {}

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "reader">The reader.</param>
        public override void Parse(BaseEntity entity, TextReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Parses the specified property element.
        /// </summary>
        /// <param name = "propertyElement">The property element.</param>
        /// <returns></returns>
		public PropertyEntity Parse(TypedEntity typedEntity, XElement propertyElement)
        {
            XElement nameElement = propertyElement.Element("h3");
            String name = nameElement.TrimAll();

            // Extract the declaration
            XElement signatureElement = (from el in propertyElement.Elements("div")
                                         where (String) el.Attribute("class") == "declaration"
                                         select el).FirstOrDefault();
            // Extract the abstract
            XElement abstractElement = (from el in propertyElement.Elements("p")
                                        where (String) el.Attribute("class") == "abstract"
                                        select el).FirstOrDefault();

            //// Extract discussion
            //XElement discussionElement = (from el in propertyElement.Elements("div")
            //                              where (String) el.Attribute("class") == "api discussion"
            //                              select el).FirstOrDefault();

            // Get the availability
            XElement availabilityElement = (from el in propertyElement.Elements("div")
                                            where (String) el.Attribute("class") == "api availability"
                                            select el).FirstOrDefault();

            String signature = signatureElement.TrimAll();

            List<String> summary = new List<String>();
            summary.Add(abstractElement.TrimAll());

            //if (discussionElement != null)
            //{
            //    foreach (XElement paragraph in discussionElement.Elements("p"))
            //    {
            //        summary.Add(paragraph.TrimAll());
            //    }
            //}

            String minAvailability = availabilityElement.Elements("ul").Elements("li").FirstOrDefault().TrimAll();
            minAvailability = CommentHelper.ExtractAvailability(minAvailability);

            // Extract property's attribute
            List<String> attributes = new List<String>();
            int attributesStart = signature.IndexOf('(');
            int attributesEnd = signature.IndexOf(')');
            if (attributesStart > 0 && attributesEnd > attributesStart)
            {
                String attributesAll = signature.Substring(attributesStart + 1, attributesEnd - attributesStart - 1);
                attributes.AddRange(attributesAll.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(attr => attr.Trim()));
            }
            int typeStart = attributesEnd > 0 ? (attributesEnd + 1) : "@property".Length;
            int typeEnd = signature.LastIndexOf(name);
            string returnType = (typeStart > 0 && typeEnd > typeStart) ? signature.Substring(typeStart, typeEnd - typeStart).Trim() : "MISSING";

            // Is the property readonly ?
            bool readOnly = attributes.Contains("readonly");

            // Is there an explicit getter
            String getterSelector = attributes.Find(a => a.Contains("getter"));
            if (getterSelector != null)
            {
                getterSelector = getterSelector.Substring(getterSelector.IndexOf('=') + 1);
            }
            else
            {
                getterSelector = name;
            }

            // Alter the name
            name = name.UpperCaseFirstLetter();

            // Is there an explicit setter
            String setterSelector = attributes.Find(a => a.Contains("setter"));
            if (setterSelector != null)
            {
                setterSelector = setterSelector.Substring(setterSelector.IndexOf('=') + 1);
            }
            else
            {
                setterSelector = "set" + name + ":";
            }

            bool isOut, isByRef, isBlock;
			String type = this.TypeManager.ConvertType(returnType, out isOut, out isByRef, out isBlock, this.Logger);

            PropertyEntity propertyEntity = new PropertyEntity();
            propertyEntity.MinAvailability = minAvailability;
            propertyEntity.Name = getterSelector.UpperCaseFirstLetter();
            propertyEntity.Static = false;
            propertyEntity.Summary = summary;
            propertyEntity.Type = type;

            propertyEntity.Getter = new MethodEntity();
            propertyEntity.Getter.Signature = signature;
            propertyEntity.Getter.Selector = getterSelector;

            if (readOnly)
            {
                propertyEntity.Setter = null;
            }
            else
            {
                propertyEntity.Setter = new MethodEntity();
                propertyEntity.Setter.Signature = signature;
                propertyEntity.Setter.Selector = setterSelector;
            }

            return propertyEntity;
        }

        public PropertyEntity Parse(TypedEntity typedEntity, string name, IEnumerable<XElement> elements)
        {
            XElement declarationElement = (from el in elements
                                           where el.Name == "div" &&
                                                 el.Attribute("class") != null &&
                                                 el.Attribute("class").Value == "declaration_indent"
                                           select el).FirstOrDefault();

            XElement discussionElement = (from el in elements
                                          where el.Name == "h5" && el.Value.Trim() == "Discussion"
                                          select el).FirstOrDefault();

            XElement availabilityElement = (from el in elements
                                            let term = el.Descendants("dt").FirstOrDefault()
                                            let definition = el.Descendants("dd").FirstOrDefault()
                                            where el.Name == "dl" &&
                                                  term != null &&
                                                  term.Value.Trim() == "Availability"
                                            select definition).FirstOrDefault();

            String signature = declarationElement.TrimAll();

            // Extract abstract
            List<String> summary = new List<String>();
            IEnumerable<XElement> abstractElements = elements.SkipWhile(el => el.Name != "p").TakeWhile(el => el.Name == "p");
            foreach (XElement element in abstractElements)
            {
                String line = element.TrimAll();
                if (!String.IsNullOrEmpty(line))
                {
                    summary.Add(line);
                }
            }

            // Extract discussion
            if (discussionElement != null)
            {
                IEnumerable<XElement> discussionElements = discussionElement.ElementsAfterSelf().TakeWhile(el => el.Name == "p");
                foreach (XElement element in discussionElements)
                {
                    String line = element.TrimAll();
                    if (!String.IsNullOrEmpty(line))
                    {
                        summary.Add(line);
                    }
                }
            }

            // Get the availability
            String minAvailability = "MISSING";
            if (availabilityElement != null)
            {
                minAvailability = CommentHelper.ExtractAvailability(availabilityElement.TrimAll());
            }

            // Extract property's attribute
            List<String> attributes = new List<String>();
            int attributesStart = signature.IndexOf('(');
            int attributesEnd = signature.IndexOf(')');
            if (attributesStart > 0 && attributesEnd > attributesStart)
            {
                String attributesAll = signature.Substring(attributesStart + 1, attributesEnd - attributesStart - 1);
                attributes.AddRange(attributesAll.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(attr => attr.Trim()));
            }
            int typeStart = attributesEnd > 0 ? (attributesEnd + 1) : "@property".Length;
            int typeEnd = signature.LastIndexOf(name);
            string returnType = (typeStart > 0 && typeEnd > typeStart) ? signature.Substring(typeStart, typeEnd - typeStart).Trim() : "MISSING";

            // Is the property readonly ?
            bool readOnly = attributes.Contains("readonly");

            // Is there an explicit getter
            String getterSelector = attributes.Find(a => a.Contains("getter"));
            if (getterSelector != null)
            {
                getterSelector = getterSelector.Substring(getterSelector.IndexOf('=') + 1);
            }
            else
            {
                getterSelector = name;
            }

            // Alter the name
            name = name.UpperCaseFirstLetter();

            // Is there an explicit setter
            String setterSelector = attributes.Find(a => a.Contains("setter"));
            if (setterSelector != null)
            {
                setterSelector = setterSelector.Substring(setterSelector.IndexOf('=') + 1);
            }
            else
            {
                setterSelector = "set" + name + ":";
            }

            bool isOut, isByRef, isBlock;
			String type = this.TypeManager.ConvertType(returnType, out isOut, out isByRef, out isBlock, this.Logger);

            PropertyEntity propertyEntity = new PropertyEntity();
            propertyEntity.MinAvailability = minAvailability;
            propertyEntity.Name = getterSelector.UpperCaseFirstLetter();
            propertyEntity.Static = false;
            propertyEntity.Summary = summary;
            propertyEntity.Type = type;

            propertyEntity.Getter = new MethodEntity();
            propertyEntity.Getter.Signature = signature;
            propertyEntity.Getter.Selector = getterSelector;

            if (readOnly)
            {
                propertyEntity.Setter = null;
            }
            else
            {
                propertyEntity.Setter = new MethodEntity();
                propertyEntity.Setter.Signature = signature;
                propertyEntity.Setter.Selector = setterSelector;
            }

            return propertyEntity;
        }
    }
}