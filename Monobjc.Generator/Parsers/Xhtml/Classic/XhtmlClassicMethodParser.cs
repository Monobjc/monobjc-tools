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
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Classic
{
    /// <summary>
    ///   XHTML parser dedicated to methods.
    /// </summary>
    public class XhtmlClassicMethodParser : XhtmlBaseParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlClassicMethodParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
		public XhtmlClassicMethodParser(NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger) {}

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "reader">The reader.</param>
        public override void Parse(BaseEntity entity, TextReader reader)
        {
            throw new NotImplementedException();
        }

		public MethodEntity Parse(TypedEntity typedEntity, String selector, IEnumerable<XElement> elements)
        {
            MethodEntity methodEntity = new MethodEntity();

            XElement declarationElement = (from el in elements
                                           where el.Name == "div" &&
                                                 el.Attribute("class") != null &&
                                                 el.Attribute("class").Value == "declaration_indent"
                                           select el).FirstOrDefault();

            XElement parameterElement = (from el in elements
                                         where el.Name == "div" &&
                                               el.Attribute("class") != null &&
                                               el.Attribute("class").Value == "param_indent"
                                         select el).FirstOrDefault();

            XElement returnValueElement = (from el in elements
                                           where el.Name == "h5" && el.Value.Trim() == "Return Value"
                                           select el).FirstOrDefault();

            //XElement discussionElement = (from el in elements
            //                              where el.Name == "h5" && el.Value.Trim() == "Discussion"
            //                              select el).FirstOrDefault();

            XElement availabilityElement = (from el in elements
                                            let term = el.Descendants("dt").FirstOrDefault()
                                            let definition = el.Descendants("dd").FirstOrDefault()
                                            where el.Name == "dl" &&
                                                  term != null &&
                                                  term.Value.Trim() == "Availability"
                                            select definition).FirstOrDefault();

            methodEntity.Selector = selector;
            methodEntity.Name = GetMethodName(methodEntity);

            methodEntity.Signature = declarationElement.TrimAll();
            methodEntity.Signature = methodEntity.Signature.TrimEnd(';');

            methodEntity.Static = methodEntity.Signature.StartsWith("+");

            // Extract abstract
            IEnumerable<XElement> abstractElements = elements.SkipWhile(el => el.Name != "p").TakeWhile(el => el.Name == "p");
            foreach (XElement element in abstractElements)
            {
                String line = element.TrimAll();
                if (!String.IsNullOrEmpty(line))
                {
                    methodEntity.Summary.Add(line);
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
            //            methodEntity.Summary.Add(line);
            //        }
            //    }
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

            return methodEntity;
        }
    }
}