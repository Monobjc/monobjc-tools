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

namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   A <see cref = "IBDouble" /> represents an element that contains a double precision floating point value.
    ///   <para>Example of XML code:</para>
    ///   <example>
    ///     <pre>
    ///       &lt;double key="NSSize"&gt;1.300000e+01&lt;/double&gt;
    ///     </pre>
    ///   </example>
    /// </summary>
    public class IBDouble : IBItem<double>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBDouble" /> class.
        /// </summary>
        /// <param name = "attributes">The attributes.</param>
        public IBDouble(IDictionary<String, String> attributes)
            : base(attributes) {}

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public override void Accept(IIBVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        ///   Sets the value.
        /// </summary>
        /// <param name = "content">The content.</param>
        public override void SetValue(String content)
        {
            double value;
            if (Double.TryParse(content, out value))
            {
                this.Value = value;
            }
        }
    }
}