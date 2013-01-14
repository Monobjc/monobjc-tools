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
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Cocoa
{
    /// <summary>
    ///   XHTML parser dedicated to methods.
    /// </summary>
    public class XhtmlCocoaMethodParser : XhtmlBaseParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlMethodParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
		public XhtmlCocoaMethodParser(NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger) {}

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
        ///   Parses the specified method element.
        /// </summary>
        /// <param name = "methodElement">The method element.</param>
        /// <returns></returns>
		public MethodEntity Parse(TypedEntity typedEntity, XElement methodElement)
        {
            MethodEntity methodEntity = new MethodEntity();

            XElement selectorElement = methodElement.Element("h3");
            methodEntity.Selector = selectorElement.TrimAll();
            methodEntity.Name = GetMethodName(methodEntity);

			this.Logger.WriteLine("  Method '" + methodEntity.Selector + "'");

            // Extract signature
            XElement signatureElement = (from el in methodElement.Elements("div")
                                         where (String) el.Attribute("class") == "declaration"
                                         select el).FirstOrDefault();
            methodEntity.Signature = signatureElement.TrimAll();
            methodEntity.Signature = methodEntity.Signature.TrimEnd(';');

            methodEntity.Static = methodEntity.Signature.StartsWith("+");

            // Extract abstract
            XElement abstractElement = (from el in methodElement.Elements("p")
                                        where (String) el.Attribute("class") == "abstract"
                                        select el).FirstOrDefault();
            methodEntity.Summary.Add(abstractElement.TrimAll());

            //// Extract discussion
            //IEnumerable<XElement> paragraphs = (from el in methodElement.Elements("p").Elements("p")
            //                                    where (String) el.Parent.Attribute("class") == "api discussion"
            //                                    select el);
            //foreach (XElement paragraph in paragraphs)
            //{
            //    methodEntity.Summary.Add(paragraph.TrimAll());
            //}

            // Extract return type
            MethodSignatureEnumerator signatureEnumerator = new MethodSignatureEnumerator(methodEntity.Signature);
            if (signatureEnumerator.MoveNext())
            {
				methodEntity.ReturnType = this.TypeManager.ConvertType(signatureEnumerator.Current.TrimAll(), this.Logger);
            }
            else
            {
                methodEntity.ReturnType = "Id";
            }

            // Extract parameter type and name
            MethodParametersEnumerator parameterTypesEnumerator = new MethodParametersEnumerator(methodEntity.Signature, false);
            MethodParametersEnumerator parameterNamesEnumerator = new MethodParametersEnumerator(methodEntity.Signature, true);
            while (parameterTypesEnumerator.MoveNext() && parameterNamesEnumerator.MoveNext())
            {
                MethodParameterEntity parameterEntity = new MethodParameterEntity();
                bool isOut, isByRef, isBlock;
				parameterEntity.Type = this.TypeManager.ConvertType(parameterTypesEnumerator.Current, out isOut, out isByRef, out isBlock, this.Logger);
                parameterEntity.IsOut = isOut;
                parameterEntity.IsByRef = isByRef;
                parameterEntity.IsBlock = isBlock;
                parameterEntity.Name = parameterNamesEnumerator.Current.Trim();
                methodEntity.Parameters.Add(parameterEntity);
            }

            // Extract parameter documentation
            if (methodEntity.Parameters.Count > 0)
            {
                XElement termList = (from el in methodElement.Elements("div").Elements("dl")
                                     where (String) el.Parent.Attribute("class") == "api parameters"
                                           && (String) el.Attribute("class") == "termdef"
                                     select el).FirstOrDefault();
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
            if (!String.Equals(methodEntity.ReturnType, "void", StringComparison.OrdinalIgnoreCase))
            {
                IEnumerable<String> documentations = (from el in methodElement.Elements("div").Elements("p")
                                                      where (String) el.Parent.Attribute("class") == "return_value"
                                                      select el.Value.TrimAll());
                methodEntity.ReturnsDocumentation = String.Join(" ", documentations.ToArray());
            }

            // Get the availability
            String minAvailability = (from el in methodElement.Elements("div").Elements("ul").Elements("li")
                                      where (String) el.Parent.Parent.Attribute("class") == "api availability"
                                      select el.Value).FirstOrDefault();
            methodEntity.MinAvailability = CommentHelper.ExtractAvailability(minAvailability.TrimAll());

            return methodEntity;
        }
    }
}