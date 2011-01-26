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
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Cocoa
{
    /// <summary>
    ///   XHTML parser dedicated to notifications.
    /// </summary>
    public class XhtmlCocoaNotificationParser : XhtmlBaseParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlNotificationParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        public XhtmlCocoaNotificationParser(NameValueCollection settings, TypeManager typeManager) : base(settings, typeManager) {}

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
        ///   Parses the specified notification element.
        /// </summary>
        /// <param name = "notificationElement">The notification element.</param>
        /// <returns></returns>
        public ConstantEntity Parse(XElement notificationElement)
        {
            ConstantEntity notificationEntity = new ConstantEntity();
            notificationEntity.Type = "NSString";

            // Get the name
            notificationEntity.Name = notificationElement.TrimAll();

            // Get the content and the discussion
            XElement summaryElement = (from el in notificationElement.ElementsAfterSelf("section")
                                       where (String) el.Attribute("class") == "spaceabove"
                                       select el).FirstOrDefault();
            if (summaryElement != null)
            {
                foreach (XElement element in summaryElement.Elements())
                {
                    notificationEntity.Summary.Add(element.Value.TrimAll());
                }
            }

            // Get the availability
            String minAvailability = (from el in notificationElement.ElementsAfterSelf("div").Elements("ul").Elements("li")
                                      where (String) el.Parent.Parent.Attribute("class") == "api availability"
                                      select el.Value).FirstOrDefault();
            notificationEntity.MinAvailability = CommentHelper.ExtractAvailability(minAvailability.TrimAll());

            return notificationEntity;
        }
    }
}