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
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Doxygen
{
    /// <summary>
    ///   XHTML parser dedicated to classes and protocols.
    /// </summary>
    public class XhtmlDoxygenClassParser : XhtmlDoxygenTypeParser, IXhtmlClassParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlDoxygenClassParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        public XhtmlDoxygenClassParser(NameValueCollection settings, TypeManager typeManager)
            : base(settings, typeManager)
        {
            this.MethodParser = new XhtmlDoxygenMethodParser(settings, typeManager);
            this.PropertyParser = new XhtmlDoxygenPropertyParser(settings, typeManager);
        }

        /// <summary>
        ///   Gets or sets the method parser.
        /// </summary>
        /// <value>The method parser.</value>
        protected XhtmlDoxygenMethodParser MethodParser { get; private set; }

        /// <summary>
        ///   Gets or sets the property parser.
        /// </summary>
        /// <value>The property parser.</value>
        protected XhtmlDoxygenPropertyParser PropertyParser { get; private set; }

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "reader">The reader.</param>
        public override void Parse(BaseEntity entity, TextReader reader)
        {
            ClassEntity classEntity = (ClassEntity) entity;
            if (classEntity == null)
            {
                throw new ArgumentException("ClassEntity expected", "entity");
            }

            using (XmlTextReader xmlTextReader = new XmlTextReader(reader))
            {
                XElement root = XElement.Load(xmlTextReader);

                // Extract inheritance
                IEnumerable<String> conformsElements = (from el in root.Descendants("basecompoundref")
                                                        select el.TrimAll());
                List<String> protocols = new List<string>();
                foreach (string conformsElement in conformsElements)
                {
                    if (conformsElement.StartsWith("<"))
                    {
                        String protocol = conformsElement.Trim('<', '>');
                        if (protocol.EndsWith("-p"))
                        {
                            protocol = protocol.Replace("-p", String.Empty);
                        }
                        protocols.Add(protocol);
                    }
                    else
                    {
                        classEntity.BaseType = conformsElement.Trim();
                    }
                }

                if (!(classEntity is ProtocolEntity) && String.IsNullOrEmpty(classEntity.BaseType))
                {
                    classEntity.BaseType = "NSObject";
                }
                classEntity.ConformsTo = String.Join(",", protocols.Distinct().ToArray());

                // Functions
                this.ExtractFunctions(classEntity, root);

                // Methods
                this.ExtractMethods(classEntity, root);

                // Properties
                this.ExtractProperties(classEntity, root);

                // Constants
                this.ExtractConstants(classEntity, root);

                // Enumerations
                this.ExtractEnumerations(classEntity, root);
            }

            // Extract getter and search for setter
            List<String> notGetterMethods = this.Settings["NotGetterMethods"].Split(',').ToList();
            List<MethodEntity> methodModels = new List<MethodEntity>(classEntity.Methods);
            foreach (MethodEntity methodModel in methodModels)
            {
                if (notGetterMethods.Contains(methodModel.Name) || !methodModel.IsGetter)
                {
                    continue;
                }

                classEntity.Methods.Remove(methodModel);
                MethodEntity getter = methodModel;

                MethodEntity setter = classEntity.Methods.Find(m => String.Equals("Set" + getter.Name, m.Name) && String.Equals(m.ReturnType, "void"));
                if (setter == null)
                {
                    setter = classEntity.Methods.Find(m => m.IsSetterFor(getter));
                }
                if (setter != null)
                {
                    classEntity.Methods.Remove(setter);
                }

                Console.WriteLine("Converting " + getter.Name + " to property");

                PropertyEntity property = new PropertyEntity();
                property.Name = getter.Name;
                property.Type = getter.ReturnType;
                property.Summary = getter.Summary;
                property.MinAvailability = getter.MinAvailability;
                property.Static = getter.Static;
                property.Getter = getter;
                property.Setter = setter;

                classEntity.Properties.Add(property);
            }

            // Ensure that availability is set on entity.
            entity.AdjustAvailability();
        }

        protected void ExtractProperties(ClassEntity classEntity, XElement root)
        {
            // Extract name
            String compoundname = (from el in root.Descendants("compoundname")
                                   select el.TrimAll()).FirstOrDefault();

            IEnumerable<XElement> memberDefs = (from el in root.Descendants("memberdef")
                                                where el.Attribute("kind").Value == "property"
                                                select el);
            foreach (XElement memberDef in memberDefs)
            {
                String definition = memberDef.Element("definition").TrimAll();
                if (!definition.Contains(compoundname + "::"))
                {
                    continue;
                }
                PropertyEntity propertyEntity = this.PropertyParser.Parse(memberDef);
                classEntity.Properties.Add(propertyEntity);
            }
        }

        protected void ExtractMethods(ClassEntity classEntity, XElement root)
        {
            // Extract name
            String compoundname = (from el in root.Descendants("compoundname")
                                   select el.TrimAll()).FirstOrDefault();

            IEnumerable<XElement> memberDefs = (from el in root.Descendants("memberdef")
                                                where el.Attribute("kind").Value == "function"
                                                select el);
            foreach (XElement memberDef in memberDefs)
            {
                String definition = memberDef.Element("definition").TrimAll();
                if (!definition.Contains(compoundname + "::"))
                {
                    continue;
                }
                MethodEntity methodEntity = this.MethodParser.Parse(memberDef);
                classEntity.Methods.Add(methodEntity);
            }
        }
    }
}