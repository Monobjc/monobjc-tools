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
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml
{
    /// <summary>
    ///   XHTML parser dedicated to functions.
    /// </summary>
    public class XhtmlFunctionParser : XhtmlBaseParser
    {
        private static readonly Regex PARAMETER_REGEX = new Regex(@"(const )?([0-9A-z]+ ?\*{0,2} ?)([0-9A-z]+)");

        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlFunctionParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        public XhtmlFunctionParser(NameValueCollection settings, TypeManager typeManager) : base(settings, typeManager) {}

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
        /// <param name = "functionElement">The function element.</param>
        /// <returns></returns>
        public FunctionEntity Parse(XElement functionElement)
        {
            FunctionEntity functionEntity = new FunctionEntity();

            // Extract name
            String name = functionElement.TrimAll();
            functionEntity.Name = name;

            // Extract abstract
            XElement abstractElement = (from el in functionElement.ElementsAfterSelf("p")
                                        where (String) el.Attribute("class") == "abstract"
                                        select el).FirstOrDefault();
            functionEntity.Summary.Add(abstractElement.TrimAll());

            // Extract declaration
            XElement declarationElement = (from el in functionElement.ElementsAfterSelf("pre")
                                           where (String) el.Attribute("class") == "declaration"
                                           select el).FirstOrDefault();
            String signature = declarationElement.TrimAll();
            if (signature.StartsWith("#define"))
            {
                Console.WriteLine("SKIPPING define statement: " + name);
                return null;
            }
            functionEntity.Signature = signature;

            // Parse signature
            int pos = signature.IndexOf(name);
            if (pos == -1)
            {
                Console.WriteLine("MISMATCH between name and declaration: " + name);
                return null;
            }
            String returnType = signature.Substring(0, pos);
            String parameters = signature.Substring(pos + name.Length).Trim();
            parameters = parameters.Trim(';', '(', ')');
            if (parameters != "void")
            {
                foreach (string parameter in parameters.Split(','))
                {
                    String parameterType = "NOTYPE";
                    String parameterName = "NONAME";

                    Match r = PARAMETER_REGEX.Match(parameter);
                    if (r.Success)
                    {
                        parameterType = r.Groups[2].Value.Trim();
                        parameterName = r.Groups[3].Value.Trim();
                    }
                    else if (parameter.Trim() == "...")
                    {
                        parameterType = "params Object[]";
                        parameterName = "values";
                    }
                    else
                    {
                        Console.WriteLine("FAILED to parse parameter: " + parameter);
                        return null;
                    }

                    MethodParameterEntity parameterEntity = new MethodParameterEntity();
                    bool isOut, isByRef, isBlock;
                    parameterEntity.Type = this.TypeManager.ConvertType(parameterType, out isOut, out isByRef, out isBlock);
                    parameterEntity.IsOut = isOut;
                    parameterEntity.IsByRef = isByRef;
                    parameterEntity.IsBlock = isBlock;
                    parameterEntity.Name = parameterName;
                    functionEntity.Parameters.Add(parameterEntity);
                }
            }

            // Extract return type
            functionEntity.ReturnType = this.TypeManager.ConvertType(returnType);

            // Extract parameter documentation
            if (functionEntity.Parameters.Count > 0)
            {
                XElement termList = (from el in functionElement.Elements("div").Elements("dl")
                                     where (String)el.Parent.Attribute("class") == "api parameters"
                                           && (String)el.Attribute("class") == "termdef"
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
                            IEnumerable<String> summaries = ddList.ElementAt(i).Elements("p").Select(p => p.Value.TrimAll());

                            // Find the parameter
                            MethodParameterEntity parameterEntity = functionEntity.Parameters.Find(p => String.Equals(p.Name, term));
                            if (parameterEntity != null)
                            {
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
            for (int i = 0; i < functionEntity.Parameters.Count; i++)
            {
                functionEntity.Parameters[i].Name = this.TypeManager.ConvertName(functionEntity.Parameters[i].Name);
            }

            // Get the summary for return type
            if (!String.Equals(functionEntity.ReturnType, "void", StringComparison.OrdinalIgnoreCase))
            {
                XElement returnValueElement = (from el in functionElement.ElementsAfterSelf("div")
                                               where (String) el.Attribute("class") == "return_value"
                                               select el).FirstOrDefault();
                if (returnValueElement != null)
                {
                    IEnumerable<String> documentations = returnValueElement.Elements("p").Select(p => p.Value.TrimAll());
                    functionEntity.ReturnsDocumentation = String.Join(String.Empty, documentations.ToArray());
                }
            }

            //// Extract discussion
            //XElement discussionElement = (from el in functionElement.ElementsAfterSelf("div")
            //                              where (String) el.Attribute("class") == "api discussion"
            //                              select el).FirstOrDefault();
            //if (discussionElement != null)
            //{
            //    foreach (XElement paragraph in discussionElement.Elements("p"))
            //    {
            //        functionEntity.Summary.Add(paragraph.TrimAll());
            //    }
            //}

            // Get the availability
            XElement availabilityElement = (from el in functionElement.ElementsAfterSelf("div")
                                            where (String) el.Attribute("class") == "api availability"
                                            select el).FirstOrDefault();
            String minAvailability = availabilityElement.Elements("ul").Elements("li").FirstOrDefault().TrimAll();
            functionEntity.MinAvailability = CommentHelper.ExtractAvailability(minAvailability);

            return functionEntity;
        }
    }
}