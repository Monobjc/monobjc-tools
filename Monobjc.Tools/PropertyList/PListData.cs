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
using System.Xml;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.PropertyList
{
    /// <summary>
    ///   Represent a base64 encoded data value in PList file.
    /// </summary>
    public class PListData : PListItem<byte[]>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListData" /> class.
        /// </summary>
        public PListData() {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListData" /> class.
        /// </summary>
        /// <param name = "value">The value.</param>
        public PListData(byte[] value) : base(value) {}

        /// <summary>
        ///   Sets the value of this instance.
        /// </summary>
        /// <param name = "value">The value.</param>
        public override void SetValue(string value)
        {
            this.Value = Converter.FromBase64(value);
        }

        /// <summary>
        ///   Writes this instance to Xml stream.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("data");
            writer.WriteBase64(this.Value, 0, this.Value.Length);
            writer.WriteEndElement();
        }
    }
}