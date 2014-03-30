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
using System.Xml;

namespace Monobjc.Tools.PropertyList
{
    /// <summary>
    ///   Desribes an arbitrary element in an PList file.
    /// </summary>
    public abstract class PListItemBase
    {
        /// <summary>
        ///   Appends the child to this instance.
        /// </summary>
        /// <param name = "child">The child.</param>
        public abstract void AppendChild(PListItemBase child);

        /// <summary>
        ///   Sets the value of this instance.
        /// </summary>
        /// <param name = "value">The value.</param>
        public abstract void SetValue(String value);

        /// <summary>
        ///   Writes this instance to Xml stream.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        public abstract void WriteXml(XmlWriter writer);

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "System.Boolean" /> to <see cref = "Monobjc.Tools.PropertyList.PListItemBase" />.
        /// </summary>
        /// <param name = "value">if set to <c>true</c> [value].</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PListItemBase(bool value)
        {
            return new PListBoolean(value);
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "System.Byte[]" /> to <see cref = "Monobjc.Tools.PropertyList.PListItemBase" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PListItemBase(byte[] value)
        {
            return new PListData(value);
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "System.DateTime" /> to <see cref = "Monobjc.Tools.PropertyList.PListItemBase" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PListItemBase(DateTime value)
        {
            return new PListDate(value);
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "System.Int32" /> to <see cref = "Monobjc.Tools.PropertyList.PListItemBase" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PListItemBase(int value)
        {
            return new PListInteger(value);
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "System.Double" /> to <see cref = "Monobjc.Tools.PropertyList.PListItemBase" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PListItemBase(double value)
        {
            return new PListReal(value);
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "System.String" /> to <see cref = "Monobjc.Tools.PropertyList.PListItemBase" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PListItemBase(String value)
        {
            return new PListString(value);
        }
    }
}