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

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Doxygen
{
    /// <summary>
    ///   XHTML parser dedicated to constants.
    /// </summary>
    public class XhtmlDoxygenConstantParser : XhtmlBaseParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlDoxygenConstantParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        public XhtmlDoxygenConstantParser(NameValueCollection settings, TypeManager typeManager) : base(settings, typeManager) {}

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "reader">The reader.</param>
        public override void Parse(BaseEntity entity, TextReader reader)
        {
            throw new NotImplementedException();
        }

        public ConstantEntity Parse(XElement constantElement)
        {
            ConstantEntity constantEntity = new ConstantEntity();

            String kind = constantElement.Attribute("kind").Value;
            constantEntity.Name = constantElement.Element("name").TrimAll();

            // Elements for brief description
            IEnumerable<XElement> abstractElements = constantElement.Element("briefdescription").Elements("para");

            // Extract for detailed description
            IEnumerable<XElement> detailsElements = (from el in constantElement.Element("detaileddescription").Elements("para")
                                                     where !el.Elements("simplesect").Any()
                                                           && el.Elements("parameterlist").Any()
                                                           && el.Elements("xrefsect").Any()
                                                     select el);

            // Add brief description
            foreach (XElement paragraph in abstractElements)
            {
                constantEntity.Summary.Add(paragraph.TrimAll());
            }
            foreach (XElement paragraph in detailsElements)
            {
                constantEntity.Summary.Add(paragraph.TrimAll());
            }

            switch (kind)
            {
                case "define":
                    {
                        constantEntity.Type = "MISSING";
                        constantEntity.Static = true;
                        constantEntity.Value = constantElement.Element("initializer").TrimAll();
                    }
                    break;
                case "variable":
                    {
                        String type = constantElement.Element("type").TrimAll();
                        type = type.Replace("const", String.Empty).Trim();

                        bool isOut, isByRef, isBlock;
                        constantEntity.Type = this.TypeManager.ConvertType(type, out isOut, out isByRef, out isBlock);
                    }
                    break;
            }

            /*
            // Get the availability
            if (availabilityElement != null)
            {
                constantEntity.MinAvailability = CommentHelper.ExtractAvailability(availabilityElement.TrimAll());
            }
             */

            return constantEntity;
        }
    }
}