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

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Doxygen
{
    /// <summary>
    ///   XHTML parser dedicated to methods.
    /// </summary>
    public class XhtmlDoxygenEnumerationParser : XhtmlBaseParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlDoxygenEnumerationParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
		public XhtmlDoxygenEnumerationParser(NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger) {}

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "reader">The reader.</param>
        public override void Parse(BaseEntity entity, TextReader reader)
        {
            throw new NotImplementedException();
        }

		public EnumerationEntity Parse(TypedEntity typedEntity, XElement enumerationElement)
        {
            EnumerationEntity enumerationEntity = new EnumerationEntity();

            String name = enumerationElement.Element("name").TrimAll();
            enumerationEntity.Name = name.Trim('_');
            enumerationEntity.BaseType = "MISSING";
            enumerationEntity.Namespace = "MISSING";

            // Elements for brief description
            IEnumerable<XElement> abstractElements = enumerationElement.Element("briefdescription").Elements("para");

            // Extract for detailed description
            IEnumerable<XElement> detailsElements = (from el in enumerationElement.Element("detaileddescription").Elements("para")
                                                     where !el.Elements("simplesect").Any()
                                                           && el.Elements("parameterlist").Any()
                                                           && el.Elements("xrefsect").Any()
                                                     select el);

            // Elements for values
            IEnumerable<XElement> enumerationValueElements = enumerationElement.Elements("enumvalue");

            // Add brief description
            foreach (XElement paragraph in abstractElements)
            {
                enumerationEntity.Summary.Add(paragraph.TrimAll());
            }
            foreach (XElement paragraph in detailsElements)
            {
                enumerationEntity.Summary.Add(paragraph.TrimAll());
            }

            // Add each value
            foreach (XElement enumerationValueElement in enumerationValueElements)
            {
                String key = enumerationValueElement.Element("name").TrimAll();
                String value;
                if (enumerationValueElement.Element("initializer") != null)
                {
                    value = enumerationValueElement.Element("initializer").Value;
                    value = value.Replace("=", String.Empty);
                    value = value.TrimAll();
                }
                else
                {
                    value = String.Empty;
                }

                // Add a new value
                EnumerationValueEntity enumerationValueEntity = new EnumerationValueEntity();
                enumerationValueEntity.Name = key;
                enumerationValueEntity.Value = value;
                enumerationEntity.Values.Add(enumerationValueEntity);

                // Elements for brief description
                abstractElements = enumerationValueElement.Element("briefdescription").Elements("para");

                // Add brief description
                foreach (XElement paragraph in abstractElements)
                {
                    enumerationValueEntity.Summary.Add(paragraph.TrimAll());
                }
            }

            /*
            // Get the availability
            if (availabilityElement != null)
            {
                enumerationEntity.MinAvailability = CommentHelper.ExtractAvailability(availabilityElement.TrimAll());
            }
             */

            return enumerationEntity;
        }
    }
}