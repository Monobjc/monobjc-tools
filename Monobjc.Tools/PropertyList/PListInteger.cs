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
using System.Globalization;
using System.Xml;

namespace Monobjc.Tools.PropertyList
{
    /// <summary>
    ///   Represent an integer value in PList file.
    /// </summary>
    public class PListInteger : PListItem<int>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListInteger" /> class.
        /// </summary>
        public PListInteger() {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListInteger" /> class.
        /// </summary>
        /// <param name = "value">The value.</param>
        public PListInteger(int value) : base(value) {}

        /// <summary>
        ///   Sets the value of this instance.
        /// </summary>
        /// <param name = "value">The value.</param>
        public override void SetValue(string value)
        {
            int val;
            if (Int32.TryParse(value, out val))
            {
                this.Value = val;
            }
        }

        /// <summary>
        ///   Writes this instance to Xml stream.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("integer", this.Value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "Monobjc.Tools.PropertyList.PListInteger" /> to <see cref = "System.Int32" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Int32(PListInteger value)
        {
            return value.Value;
        }
    }
}