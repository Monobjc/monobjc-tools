//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2011 - Laurent Etiemble
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
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Doxygen
{
    /// <summary>
    ///   XHTML parser dedicated to methods.
    /// </summary>
    public class XhtmlDoxygenMethodParser : XhtmlBaseParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlDoxygenMethodParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        public XhtmlDoxygenMethodParser(NameValueCollection settings, TypeManager typeManager) : base(settings, typeManager) {}

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "reader">The reader.</param>
        public override void Parse(BaseEntity entity, TextReader reader)
        {
            throw new NotImplementedException();
        }

        public MethodEntity Parse(XElement methodElement)
        {
            MethodEntity methodEntity = new MethodEntity();

            bool isStatic = (methodElement.Attribute("static").Value == "yes");
            String selector = methodElement.Element("name").TrimAll();
            String returnType = methodElement.Element("type").TrimAll();

            // Elements for brief description
            IEnumerable<XElement> abstractElements = methodElement.Element("briefdescription").Elements("para");

            // Extract for detailed description
            IEnumerable<XElement> detailsElements = (from el in methodElement.Element("detaileddescription").Elements("para")
                                                     where !el.Elements("simplesect").Any()
                                                           && el.Elements("parameterlist").Any()
                                                           && el.Elements("xrefsect").Any()
                                                     select el);

            // Element for parameters
            IEnumerable<XElement> parameterElements = methodElement.Elements("param");

            // Element for detailed description
            XElement detailedDescriptionElement = methodElement.Element("detaileddescription");

            // Sets some data
            methodEntity.Selector = selector;
            methodEntity.Name = GetMethodName(methodEntity);
            methodEntity.Static = isStatic;

            // Add brief description
            foreach (XElement paragraph in abstractElements)
            {
                methodEntity.Summary.Add(paragraph.TrimAll());
            }
            foreach (XElement paragraph in detailsElements)
            {
                methodEntity.Summary.Add(paragraph.TrimAll());
            }

            // Recreate the signature
            StringBuilder signature = new StringBuilder();
            signature.Append(isStatic ? "+ " : "- ");
            signature.AppendFormat("({0})", returnType);
            if (selector.IndexOf(":") != -1)
            {
                String[] parts = selector.Split(':');
                for (int i = 0; i < parameterElements.Count(); i++)
                {
                    XElement parameterElement = parameterElements.ElementAt(i);
                    String parameterType = parameterElement.Element("type").TrimAll();

                    String parameterName;
                    if (parameterType.Equals("..."))
                    {
                        parameterName = String.Empty;
                    }
                    else
                    {
                        parameterName = parameterElement.Element("declname").TrimAll();
                        if (parameterElement.Element("defname") != null)
                        {
                            parameterName = parameterElement.Element("defname").TrimAll();
                        }
                    }

                    signature.Append(parts[i]);
                    signature.AppendFormat(":({0}){1} ", parameterType, parameterName);
                }
            }
            else
            {
                signature.Append(selector);
            }
            methodEntity.Signature = signature.ToString().Trim() + ";";

            // Set the return type
            methodEntity.ReturnType = this.TypeManager.ConvertType(returnType);

            // Extract documentation for return type
            if (!String.Equals(returnType, "void", StringComparison.OrdinalIgnoreCase))
            {
                XElement returnTypeSectionElement = (from el in detailedDescriptionElement.Descendants("simplesect")
                                                     where el.Attribute("kind") != null
                                                           && el.Attribute("kind").Value == "return"
                                                     select el).FirstOrDefault();
                if (returnTypeSectionElement != null)
                {
                    IEnumerable<String> documentations = (from el in returnTypeSectionElement.Elements("para")
                                                          select el.TrimAll());
                    methodEntity.ReturnsDocumentation = String.Join(" ", documentations.ToArray());
                }
            }

            // Create the parameters
            for (int i = 0; i < parameterElements.Count(); i++)
            {
                XElement parameterElement = parameterElements.ElementAt(i);
                String parameterType = parameterElement.Element("type").TrimAll();

                String parameterName;
                if (parameterType.Equals("..."))
                {
                    parameterType = "params Object[]";
                    parameterName = "values";
                }
                else
                {
                    parameterName = parameterElement.Element("declname").TrimAll();
                    if (parameterElement.Element("defname") != null)
                    {
                        parameterName = parameterElement.Element("defname").TrimAll();
                    }
                }

                MethodParameterEntity parameterEntity = new MethodParameterEntity();
                bool isOut, isByRef, isBlock;
                parameterEntity.Type = this.TypeManager.ConvertType(parameterType, out isOut, out isByRef, out isBlock);
                parameterEntity.IsOut = isOut;
                parameterEntity.IsByRef = isByRef;
                parameterEntity.IsBlock = isBlock;
                parameterEntity.Name = parameterName;
                methodEntity.Parameters.Add(parameterEntity);
            }

            // Extract documentation for parameters
            XElement parameterSectionElement = (from el in detailedDescriptionElement.Descendants("parameterlist")
                                                where el.Attribute("kind") != null
                                                      && el.Attribute("kind").Value == "param"
                                                select el).FirstOrDefault();
            if (parameterSectionElement != null)
            {
                IEnumerable<XElement> parameterItemElements = parameterSectionElement.Elements("parameteritem");
                for (int i = 0; i < parameterElements.Count(); i++)
                {
                    XElement parameterElement = parameterElements.ElementAt(i);
                    String parameterType = parameterElement.Element("type").TrimAll();
                    String parameterName;
                    if (parameterType.Equals("..."))
                    {
                        continue;
                    }
                    else
                    {
                        parameterName = parameterElement.Element("declname").TrimAll();
                        if (parameterElement.Element("defname") != null)
                        {
                            parameterName = parameterElement.Element("defname").TrimAll();
                        }
                    }

                    MethodParameterEntity parameterEntity = methodEntity.Parameters.Find(p => String.Equals(p.Name, parameterName));

                    IEnumerable<XElement> documentations = (from el in parameterItemElements
                                                            let filter = el.Element("parameternamelist").Value.TrimAll()
                                                            where String.Equals(filter, parameterName)
                                                            select el);
                    if (documentations.Count() > 0)
                    {
                        XElement documentation = documentations.Elements("parameterdescription").First();
                        foreach (XElement element in documentation.Elements("para"))
                        {
                            parameterEntity.Summary.Add(element.TrimAll());
                        }
                    }
                }
            }

            // Fix the name only after looking for the documentation
            for (int i = 0; i < methodEntity.Parameters.Count; i++)
            {
                methodEntity.Parameters[i].Name = this.TypeManager.ConvertName(methodEntity.Parameters[i].Name);
            }

            /*
             * 
            // Get the availability
            if (availabilityElement != null)
            {
                methodEntity.MinAvailability = CommentHelper.ExtractAvailability(availabilityElement.TrimAll());
            }
             */

            return methodEntity;
        }
    }
}