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
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Classic
{
    /// <summary>
    ///   XHTML parser dedicated to constants.
    /// </summary>
    public class XhtmlClassicConstantParser : XhtmlBaseParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlClassicConstantParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
		public XhtmlClassicConstantParser(NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger) {}

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "reader">The reader.</param>
        public override void Parse(BaseEntity entity, TextReader reader)
        {
            throw new NotImplementedException();
        }

		public ConstantEntity Parse(TypedEntity typedEntity, string constantElement, IEnumerable<XElement> elements)
        {
            ConstantEntity constantEntity = new ConstantEntity();

            XElement declarationElement = (from el in elements
                                           where el.Name == "div" &&
                                                 el.Attribute("class") != null &&
                                                 el.Attribute("class").Value == "declaration_indent"
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

            String stripped = declarationElement.TrimAll();
            stripped = stripped.Trim(';');
            while (stripped.Contains("  "))
            {
                stripped = stripped.Replace("  ", " ");
            }
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
				type = this.TypeManager.ConvertType(type, out isOut, out isByRef, out isBlock, this.Logger);

                constantEntity.Type = type;
                constantEntity.Name = r.Groups[2].Value.Trim();
            }
            else
            {
				this.Logger.WriteLine("FAILED to parse constant '{0}'", stripped);
                return constantEntity;
            }

            // Extract abstract
            IEnumerable<XElement> abstractElements = elements.SkipWhile(el => el.Name != "p").TakeWhile(el => el.Name == "p");
            foreach (XElement element in abstractElements)
            {
                String line = element.TrimAll();
                if (!String.IsNullOrEmpty(line))
                {
                    constantEntity.Summary.Add(line);
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
            //            constantEntity.Summary.Add(line);
            //        }
            //    }
            //}

            // Get the availability
            if (availabilityElement != null)
            {
                constantEntity.MinAvailability = CommentHelper.ExtractAvailability(availabilityElement.TrimAll());
            }

            return constantEntity;
        }
    }
}