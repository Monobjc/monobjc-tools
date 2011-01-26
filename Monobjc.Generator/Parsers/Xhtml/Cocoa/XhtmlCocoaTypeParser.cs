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

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Cocoa
{
    /// <summary>
    ///   Base class for XHTML parsing.
    /// </summary>
    public class XhtmlCocoaTypeParser : XhtmlBaseParser, IXhtmlTypeParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlTypeParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        public XhtmlCocoaTypeParser(NameValueCollection settings, TypeManager typeManager)
            : base(settings, typeManager)
        {
            this.ConstantParser = new XhtmlCocoaConstantParser(settings, typeManager);
            this.NotificationParser = new XhtmlCocoaNotificationParser(settings, typeManager);
            this.FunctionParser = new XhtmlCocoaFunctionParser(settings, typeManager);
        }

        /// <summary>
        ///   Gets or sets the constant parser.
        /// </summary>
        /// <value>The constant parser.</value>
        protected XhtmlCocoaConstantParser ConstantParser { get; set; }

        /// <summary>
        ///   Gets or sets the notification parser.
        /// </summary>
        /// <value>The notification parser.</value>
        protected XhtmlCocoaNotificationParser NotificationParser { get; set; }

        /// <summary>
        ///   Gets or sets the function parser.
        /// </summary>
        /// <value>The function parser.</value>
        protected XhtmlCocoaFunctionParser FunctionParser { get; set; }

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

                // Constants
                this.ExtractConstants(typedEntity, root);

                // Notifications
                this.ExtractNotifications(typedEntity, root);

                // Functions
                this.ExtractFunctions(typedEntity, root);
            }

            // Make sure availability is ok
            entity.AdjustAvailability();
        }

        protected void ExtractFunctions(TypedEntity typedEntity, XElement root)
        {
            IEnumerable<XElement> functionElements = from el in root.Descendants("h3")
                                                     where (String) el.Attribute("class") == "tight jump function"
                                                     select el;
            foreach (XElement functionElement in functionElements)
            {
                FunctionEntity functionEntity = this.FunctionParser.Parse(functionElement);
                if (functionEntity != null)
                {
                    typedEntity.Functions.Add(functionEntity);
                }
            }
        }

        protected void ExtractNotifications(TypedEntity typedEntity, XElement root)
        {
            IEnumerable<XElement> notificationElements = from el in root.Descendants("h3")
                                                         where (String) el.Attribute("class") == "tight jump notification"
                                                         select el;
            foreach (XElement notificationElement in notificationElements)
            {
                ConstantEntity notificationEntity = this.NotificationParser.Parse(notificationElement);
                typedEntity.Constants.Add(notificationEntity);
            }
        }

        protected void ExtractConstants(TypedEntity typedEntity, XElement root)
        {
            IEnumerable<XElement> constantElements = from el in root.Descendants("h3")
                                                     where (String) el.Attribute("class") == "constantGroup"
                                                           || (String) el.Attribute("class") == "tight jump typeDef"
                                                     select el;
            foreach (XElement constantElement in constantElements)
            {
                List<BaseEntity> entities = this.ConstantParser.Parse(constantElement);
                if (entities != null)
                {
                    typedEntity.AddRange(entities);
                }
            }

            // Constants
            constantElements = from el in root.Descendants("h4")
                               select el;
            foreach (XElement constantElement in constantElements)
            {
                List<BaseEntity> entities = this.ConstantParser.Parse(constantElement);
                if (entities != null)
                {
                    typedEntity.AddRange(entities);
                }
            }
        }
    }
}