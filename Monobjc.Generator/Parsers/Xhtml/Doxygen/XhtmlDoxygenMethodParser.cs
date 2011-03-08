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
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;
using System.Text;

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
        public XhtmlDoxygenMethodParser(NameValueCollection settings, TypeManager typeManager) : base(settings, typeManager) { }

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
            String selector = methodElement.Element("name").Value;
            String definition = methodElement.Element("definition").Value;
            String returnType = methodElement.Element("type").Value;

            // Extract brief description
            IEnumerable<XElement> abstractElements = methodElement.Element("briefdescription").Elements("para");

            methodEntity.Selector = selector;
            methodEntity.Name = GetMethodName(methodEntity);
            methodEntity.Static = isStatic;
            methodEntity.ReturnType = returnType;

            StringBuilder signature = new StringBuilder();
            signature.Append(isStatic ? "+ " : "- ");
            signature.AppendFormat("({0})", returnType);

            IEnumerable<XElement> parameterElements = methodElement.Elements("param");
            string[] parts = selector.Split(':');
            for (int i = 0; i < parameterElements.Count(); i++)
            {
                XElement parameterElement = parameterElements.ElementAt(i);
                String parameterType = parameterElement.Element("type").Value;
                string parameterName = parameterType.Equals("...") ? String.Empty : parameterElement.Element("declname").Value;

                signature.Append(parts[i]);
                signature.AppendFormat("({0}){1} ", parameterType, parameterName);
            }

            methodEntity.Signature = signature.ToString().Trim();

            for (int i = 0; i < parameterElements.Count(); i++)
            {
                XElement parameterElement = parameterElements.ElementAt(i);
                String parameterType = parameterElement.Element("type").Value;
                String parameterName;
                if (parameterType.Equals("..."))
                {
                    parameterType = "params Object[]";
                    parameterName = "values";
                }
                else
                {
                    parameterName = parameterElement.Element("declname").Value;
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

            XElement detailedDescriptionElement = methodElement.Element("detaileddescription");
            if (detailedDescriptionElement != null)
            {
                
            }

            /*

            // Extract parameter type and name
            MethodParametersEnumerator parameterTypesEnumerator = new MethodParametersEnumerator(methodEntity.Signature, false);
            MethodParametersEnumerator parameterNamesEnumerator = new MethodParametersEnumerator(methodEntity.Signature, true);
            while (parameterTypesEnumerator.MoveNext() && parameterNamesEnumerator.MoveNext())
            {
                MethodParameterEntity parameterEntity = new MethodParameterEntity();
                bool isOut, isByRef, isBlock;
                parameterEntity.Type = this.TypeManager.ConvertType(parameterTypesEnumerator.Current, out isOut, out isByRef, out isBlock);
                parameterEntity.IsOut = isOut;
                parameterEntity.IsByRef = isByRef;
                parameterEntity.IsBlock = isBlock;
                parameterEntity.Name = parameterNamesEnumerator.Current.Trim();
                methodEntity.Parameters.Add(parameterEntity);
            }

            if (methodEntity.Parameters.Count > 0 && parameterElement != null)
            {
                XElement termList = parameterElement.Descendants("dl").FirstOrDefault();
                if (termList != null)
                {
                    IEnumerable<XElement> dtList = from el in termList.Elements("dt") select el;
                    IEnumerable<XElement> ddList = from el in termList.Elements("dd") select el;

                    if (dtList.Count() == ddList.Count())
                    {
                        // Iterate over definitions
                        for (int i = 0; i < dtList.Count(); i++)
                        {
                            String term = dtList.ElementAt(i).TrimAll();
                            //String summary = ddList.ElementAt(i).TrimAll();
                            IEnumerable<String> summaries = ddList.ElementAt(i).Elements("p").Select(p => p.Value.TrimAll());

                            // Find the parameter
                            MethodParameterEntity parameterEntity = methodEntity.Parameters.Find(p => String.Equals(p.Name, term));
                            if (parameterEntity != null)
                            {
                                //parameterEntity.Summary.Add(summary);
                                foreach (string sum in summaries)
                                {
                                    parameterEntity.Summary.Add(sum);
                                }
                            }
                        }
                    }
                }
            }

            // Fix the name only after looking for the documentation
            for (int i = 0; i < methodEntity.Parameters.Count; i++)
            {
                methodEntity.Parameters[i].Name = this.TypeManager.ConvertName(methodEntity.Parameters[i].Name);
            }

            // Get the summary for return type
            if (!String.Equals(methodEntity.ReturnType, "void", StringComparison.OrdinalIgnoreCase) && returnValueElement != null)
            {
                IEnumerable<XElement> returnTypeElements = returnValueElement.ElementsAfterSelf().TakeWhile(el => el.Name == "p");
                methodEntity.ReturnsDocumentation = String.Empty;
                foreach (XElement element in returnTypeElements)
                {
                    String line = element.TrimAll();
                    if (!String.IsNullOrEmpty(line))
                    {
                        methodEntity.ReturnsDocumentation += line;
                    }
                }
            }

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