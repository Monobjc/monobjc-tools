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

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Cocoa
{
    /// <summary>
    ///   XHTML parser dedicated to constants.
    /// </summary>
    public class XhtmlCocoaConstantParser : XhtmlBaseParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlNotificationParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        public XhtmlCocoaConstantParser(NameValueCollection settings, TypeManager typeManager) : base(settings, typeManager) {}

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
        ///   Parses the specified constant element.
        /// </summary>
        /// <param name = "constantElement">The constant element.</param>
        public List<BaseEntity> Parse(XElement constantElement)
        {
            // Get the name
            String name = constantElement.TrimAll();

            // Get the abstract
            XElement summaryElement = (from el in constantElement.ElementsAfterSelf("p")
                                       where (String) el.Attribute("class") == "abstract"
                                       select el).FirstOrDefault();
            String summary = summaryElement.TrimAll();

            // Get the declaration
            XElement declarationElement = (from el in constantElement.ElementsAfterSelf("pre")
                                           where (String) el.Attribute("class") == "declaration"
                                           select el).FirstOrDefault();
            String declaration = declarationElement.TrimAll();

            // Make various tests
            bool isDefine = declaration.StartsWith("#define");
            bool isEnum = declaration.StartsWith("enum") || declaration.StartsWith("typedef enum") || declaration.Contains(" enum ");

            if (isDefine)
            {
                List<BaseEntity> entities = ExtractDefine(constantElement, name, summary, declaration);
                return entities;
            }

            if (isEnum)
            {
                List<BaseEntity> entities = this.ExtractEnumeration(constantElement, name, summary, declaration);
                if (entities != null)
                {
                    return entities;
                }
            }

            return this.ExtractConstants(constantElement, name, summary, declaration);
        }

        private static List<BaseEntity> ExtractDefine(XElement constantElement, String name, String summary, String declaration)
        {
            return null;
        }

        private List<BaseEntity> ExtractEnumeration(XElement constantElement, String name, String summary, String declaration)
        {
            declaration = declaration.Trim(';');

            // Default values
            String type = "NOTYPE";
            String values = String.Empty;

            // Match the enumeration definition
            bool result = this.SplitEnumeration(declaration, ref name, ref type, ref values);
            if (!result)
            {
                return null;
            }

            // Create the enumeration
            EnumerationEntity enumerationEntity = new EnumerationEntity();
            enumerationEntity.Name = name;
            enumerationEntity.BaseType = type;
            enumerationEntity.Namespace = "MISSING";
            enumerationEntity.Summary.Add(summary);

            // Parse the values
            string[] pairs = values.Split(',');
            foreach (string pair in pairs)
            {
                if (String.Equals(pair.TrimAll(), String.Empty))
                {
                    continue;
                }

                String key;
                String value = String.Empty;
                if (pair.IndexOf('=') != -1)
                {
                    string[] parts = pair.Split('=');
                    key = parts[0].Trim();
                    value = parts[1].Trim();
                }
                else
                {
                    key = pair.Trim();
                }

                // Add a new value
                EnumerationValueEntity enumerationValueEntity = new EnumerationValueEntity();
                enumerationValueEntity.Name = key;
                enumerationValueEntity.Value = value;
                enumerationEntity.Values.Add(enumerationValueEntity);
            }

            // Get the definitions
            XElement termList = (from el in constantElement.ElementsAfterSelf("dl")
                                 where (String) el.Attribute("class") == "termdef"
                                 select el).FirstOrDefault();
            if (termList != null)
            {
                IEnumerable<XElement> dtList = termList.Elements("dt");
                IEnumerable<XElement> ddList = termList.Elements("dd");

                if (dtList.Count() == ddList.Count())
                {
                    // Iterate over definitions
                    for (int i = 0; i < dtList.Count(); i++)
                    {
                        String term = dtList.ElementAt(i).Value.TrimAll();
                        IEnumerable<String> summaries = ddList.ElementAt(i).Elements("p").Select(p => p.Value.TrimAll());

                        // Find the enumeration value
                        EnumerationValueEntity enumerationValueEntity = enumerationEntity.Values.Find(v => v.Name == term);
                        if (enumerationValueEntity != null)
                        {
                            foreach (string sum in summaries)
                            {
                                if (CommentHelper.IsAvailability(sum))
                                {
                                    enumerationValueEntity.MinAvailability = CommentHelper.ExtractAvailability(sum);
                                    break;
                                }
                                enumerationValueEntity.Summary.Add(sum);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Term with no match '" + term + "'");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("MISMATCH in terms");
                }
            }

            // Make sure availability is ok
            enumerationEntity.AdjustAvailability();

            return new List<BaseEntity> {enumerationEntity};
        }


        private List<BaseEntity> ExtractConstants(XElement constantElement, String name, String summary, String declaration)
        {
            List<BaseEntity> constants = new List<BaseEntity>();

            // Extract types and names
            string[] declarations = declaration.Split(';');
            foreach (string part in declarations)
            {
                if (part.Trim() == String.Empty)
                {
                    continue;
                }

                //Console.WriteLine("Parsing constant '{0}'...", part.Trim());

                String stripped = part;
                stripped = stripped.Replace("extern", String.Empty);
                stripped = stripped.Replace("const", String.Empty);
                stripped = stripped.TrimAll();

                Match r = CONSTANT_REGEX.Match(stripped);
                if (r.Success)
                {
                    String type = r.Groups[1].Value.Trim(' ', '*', ' ');

                    bool isOut;
                    bool isByRef;
                    bool isBlock;
                    type = this.TypeManager.ConvertType(type, out isOut, out isByRef, out isBlock);

                    ConstantEntity constantEntity = new ConstantEntity();
                    constantEntity.Type = type;
                    constantEntity.Name = r.Groups[2].Value.Trim();
                    constants.Add(constantEntity);

                    //Console.WriteLine("Constant found '{0}' of type '{1}'", constantEntity.Name, constantEntity.Type);
                }
                else
                {
                    Console.WriteLine("FAILED to parse constant '{0}'", stripped);
                    return null;
                }
            }

            // Get the definitions
            XElement termDefinitions = (from el in constantElement.ElementsAfterSelf("dl")
                                        where (String) el.Attribute("class") == "termdef"
                                        select el).FirstOrDefault();
            if (termDefinitions == null)
            {
                Console.WriteLine("MISSING terms");
                return null;
            }

            IEnumerable<XElement> termName = termDefinitions.Elements("dt");
            IEnumerable<XElement> termDefinition = termDefinitions.Elements("dd");

            if (termName.Count() == termDefinition.Count())
            {
                // Iterate over definitions
                for (int i = 0; i < termName.Count(); i++)
                {
                    String term = termName.ElementAt(i).Value.TrimAll();
                    IEnumerable<String> summaries = termDefinition.ElementAt(i).Elements("p").Select(p => p.Value.TrimAll());

                    // Find the enumeration value
                    BaseEntity baseEntity = constants.Find(c => c.Name == term);
                    if (baseEntity != null)
                    {
                        foreach (string sum in summaries)
                        {
                            if (CommentHelper.IsAvailability(sum))
                            {
                                baseEntity.MinAvailability = CommentHelper.ExtractAvailability(sum);
                                break;
                            }
                            baseEntity.Summary.Add(sum);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Term with no match '" + term + "'");
                    }
                }
            }
            else
            {
                Console.WriteLine("MISMATCH in terms");
                return null;
            }

            return constants;
        }
    }
}