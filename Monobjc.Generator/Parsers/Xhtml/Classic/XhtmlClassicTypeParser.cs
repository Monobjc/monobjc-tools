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
using System.Xml;
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Classic
{
    /// <summary>
    ///   Base class for XHTML parsing.
    /// </summary>
    public class XhtmlClassicTypeParser : XhtmlBaseParser, IXhtmlTypeParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlClassicTypeParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        public XhtmlClassicTypeParser(NameValueCollection settings, TypeManager typeManager)
            : base(settings, typeManager)
        {
            this.ConstantParser = new XhtmlClassicConstantParser(settings, typeManager);
            this.EnumerationParser = new XhtmlClassicEnumerationParser(settings, typeManager);
            this.FunctionParser = new XhtmlClassicFunctionParser(settings, typeManager);
        }

        /// <summary>
        ///   Gets or sets the constant parser.
        /// </summary>
        /// <value>The constant parser.</value>
        protected XhtmlClassicConstantParser ConstantParser { get; set; }

        /// <summary>
        ///   Gets or sets the constant parser.
        /// </summary>
        /// <value>The constant parser.</value>
        protected XhtmlClassicEnumerationParser EnumerationParser { get; set; }

        /// <summary>
        ///   Gets or sets the function parser.
        /// </summary>
        /// <value>The function parser.</value>
        protected XhtmlClassicFunctionParser FunctionParser { get; set; }

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "reader">The reader.</param>
        public override void Parse(BaseEntity entity, TextReader reader)
        {
            TypedEntity typedEntity = (TypedEntity) entity;
            if (typedEntity == null)
            {
                throw new ArgumentException("FunctionEntity expected", "entity");
            }

            using (XmlTextReader xmlTextReader = new XmlTextReader(reader))
            {
                XElement root = XElement.Load(xmlTextReader);

                // Functions
                this.ExtractFunctions(typedEntity, root);

                // Constants
                this.ExtractConstants(typedEntity, root);

                // Enumerations
                this.ExtractEnumerations(typedEntity, root);
            }

            // Make sure availability is ok
            entity.AdjustAvailability();
        }

        /// <summary>
        ///   Extracts the constants.
        /// </summary>
        /// <param name = "typedEntity">The typed entity.</param>
        /// <param name = "root">The root element.</param>
        protected void ExtractFunctions(TypedEntity typedEntity, XElement root)
        {
            XElement listMarker = (from el in root.Descendants("h2")
                                   where el.Value == "Functions"
                                   select el).FirstOrDefault();
            XElement list = listMarker != null ? listMarker.ElementsAfterSelf("dl").FirstOrDefault() : null;
            if (list != null)
            {
                // Collect names
                List<String> names = new List<string>();
                foreach (XElement term in list.Descendants("dt"))
                {
                    String name = term.Value.TrimAll();
                    names.Add(name);
                }

                // Search for a table with preceding by an anchor containing the name
                foreach (String name in names)
                {
                    XElement marker = (from el in list.ElementsAfterSelf("a")
                                       where el.Attribute("name") != null &&
                                             el.Attribute("name").Value.EndsWith("/" + name)
                                       select el).FirstOrDefault();
                    if (marker != null)
                    {
                        XElement startElement = marker.ElementsAfterSelf("table").FirstOrDefault();
                        IEnumerable<XElement> elements = marker.ElementsAfterSelf().TakeWhile(el => el.Name != "a");

                        FunctionEntity entity = this.FunctionParser.Parse(name, elements);
                        if (entity != null)
                        {
                            typedEntity.Functions.Add(entity);
                        }
                    }
                    else
                    {
                        Console.WriteLine("MISSING marker for function " + name);
                    }
                }
            }
        }

        /// <summary>
        ///   Extracts the constants.
        /// </summary>
        /// <param name = "typedEntity">The typed entity.</param>
        /// <param name = "root">The root element.</param>
        protected void ExtractConstants(TypedEntity typedEntity, XElement root)
        {
            XElement listMarker = (from el in root.Descendants("h2")
                                   where el.Value == "Constants"
                                   select el).FirstOrDefault();
            XElement list = listMarker != null ? listMarker.ElementsAfterSelf("dl").FirstOrDefault() : null;
            if (list != null)
            {
                // Collect names
                List<String> names = new List<string>();
                foreach (XElement term in list.Descendants("dt"))
                {
                    String name = term.Value.TrimAll();
                    names.Add(name);
                }

                // Search for a table with preceding by an anchor containing the name
                foreach (String name in names)
                {
                    XElement marker = (from el in list.ElementsAfterSelf("a")
                                       where el.Attribute("name") != null &&
                                             el.Attribute("name").Value.EndsWith("/" + name)
                                       select el).FirstOrDefault();
                    if (marker != null)
                    {
                        XElement startElement = marker.ElementsAfterSelf("table").FirstOrDefault();
                        IEnumerable<XElement> elements = marker.ElementsAfterSelf().TakeWhile(el => el.Name != "a");

                        ConstantEntity entity = this.ConstantParser.Parse(name, elements);
                        if (entity != null)
                        {
                            typedEntity.Constants.Add(entity);
                        }
                    }
                    else
                    {
                        Console.WriteLine("MISSING marker for constant " + name);
                    }
                }
            }
        }

        /// <summary>
        ///   Extracts the enumerations.
        /// </summary>
        /// <param name = "typedEntity">The typed entity.</param>
        /// <param name = "root">The root element.</param>
        protected void ExtractEnumerations(TypedEntity typedEntity, XElement root)
        {
            foreach (string title in new[] {"Typedefs", "Enumerated Types"})
            {
                XElement listMarker = (from el in root.Descendants("h2")
                                       where el.Value == title
                                       select el).FirstOrDefault();
                XElement list = listMarker != null ? listMarker.ElementsAfterSelf("dl").FirstOrDefault() : null;
                if (list != null)
                {
                    // Collect names
                    List<String> names = new List<string>();
                    foreach (XElement term in list.Descendants("dt"))
                    {
                        String name = term.Value.TrimAll();
                        names.Add(name);
                    }

                    // Search for a table with preceding by an anchor containing the name
                    foreach (String name in names)
                    {
                        String n = name.Replace(" ", String.Empty);

                        XElement marker = (from el in list.ElementsAfterSelf("a")
                                           where el.Attribute("name") != null &&
                                                 el.Attribute("name").Value.EndsWith("/" + n)
                                           select el).FirstOrDefault();
                        if (marker != null)
                        {
                            XElement startElement = marker.ElementsAfterSelf("table").FirstOrDefault();
                            IEnumerable<XElement> elements = marker.ElementsAfterSelf().TakeWhile(el => el.Name != "a");

                            EnumerationEntity entity = this.EnumerationParser.Parse(name, elements);
                            if (entity != null)
                            {
                                typedEntity.Enumerations.Add(entity);
                            }
                        }
                        else
                        {
                            Console.WriteLine("MISSING marker for constant " + name);
                        }
                    }
                }
            }
        }
    }
}