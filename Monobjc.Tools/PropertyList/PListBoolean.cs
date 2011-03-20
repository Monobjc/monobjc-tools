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
using System.Xml;

namespace Monobjc.Tools.PropertyList
{
    /// <summary>
    ///   Represent a boolean value in PList file.
    /// </summary>
    public class PListBoolean : PListItem<bool>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListBoolean" /> class.
        /// </summary>
        public PListBoolean() {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListBoolean" /> class.
        /// </summary>
        /// <param name = "value">if set to <c>true</c> [value].</param>
        public PListBoolean(bool value) : base(value) {}

        /// <summary>
        ///   Writes this instance to Xml stream.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(this.Value ? "true" : "false");
            writer.WriteEndElement();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Monobjc.Tools.PropertyList.PListBoolean"/> to <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator bool(PListBoolean value)
        {
            return value.Value;
        }
    }
}