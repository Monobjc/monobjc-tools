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
using System.Globalization;

namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   A <see cref = "IBInteger" /> represents an element that contains an integer value.
    ///   <para>Examples of XML code:</para>
    ///   <example>
    ///     &lt;int key="IBDocument.SystemTarget"&gt;1050&lt;/int&gt;
    ///     &lt;integer value="1" id="9"/&gt;
    ///   </example>
    /// </summary>
    public class IBInteger : IBItem<int>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBInteger" /> class.
        /// </summary>
        /// <param name = "attributes">The attributes.</param>
        public IBInteger(IDictionary<String, String> attributes)
            : base(attributes)
        {
            if (attributes.ContainsKey("value"))
            {
                this.Value = Int32.Parse(attributes["value"], CultureInfo.InvariantCulture);
            }
        }

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
            int value;
            if (Int32.TryParse(content, out value))
            {
                this.Value = value;
            }
        }
    }
}