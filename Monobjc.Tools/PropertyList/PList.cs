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
using System.Xml;

namespace Monobjc.Tools.PropertyList
{
    /// <summary>
    ///   Root element of a PList file.
    /// </summary>
    public class PList : PListItem<List<PListItemBase>>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PList" /> class.
        /// </summary>
        public PList()
            : this("1.0") {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PList" /> class.
        /// </summary>
        /// <param name = "version">The version.</param>
        public PList(string version)
        {
            this.Version = version;
            this.Value = new List<PListItemBase>();
        }

        /// <summary>
        ///   Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public String Version { get; set; }

        /// <summary>
        ///   Gets the root dictionary.
        /// </summary>
        /// <value>The root dictionary.</value>
        public PListDict Dict
        {
            get { return this.Value.Count == 1 ? this.Value[0] as PListDict : null; }
            set
            {
                this.Value.Clear();
                this.Value.Add(value);
            }
        }

        /// <summary>
        ///   Appends the child to this instance.
        /// </summary>
        /// <param name = "child">The child.</param>
        public override void AppendChild(PListItemBase child)
        {
            this.Value.Add(child);
        }

        /// <summary>
        ///   Writes this instance to Xml stream.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteDocType("plist", PListDocument.PUBLIC_ID, PListDocument.SYSTEM_ID, null);
            writer.WriteStartElement("plist");
            writer.WriteAttributeString("version", this.Version);
            foreach (PListItemBase child in this.Value)
            {
                child.WriteXml(writer);
            }
            writer.WriteEndElement();
        }
    }
}