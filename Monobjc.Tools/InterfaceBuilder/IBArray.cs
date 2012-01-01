//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2012 - Laurent Etiemble
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
    ///   This subclass of <see cref = "IBObject" /> is used to represents an array (mutable or not).
    ///   <para>When the <see cref = "IBObject.ObjectClass" /> property of the <see cref = "IBArray" /> is either "NSArray" or "NSMutableArray", the inner elements contains an element to indicate the XML serialization followed by the elements of the array.</para>
    ///   <para>Examples of XML code:</para>
    ///   <example>
    ///     <pre>
    ///       &lt;object class="NSMutableArray" key="children"&gt;
    ///       &lt;bool key="EncodedWithXMLCoder"&gt;YES&lt;/bool&gt;
    ///       &lt;reference ref="601701719"/&gt;
    ///       &lt;reference ref="334170355"/&gt;
    ///       &lt;reference ref="1001122846"/&gt;
    ///       &lt;reference ref="431464563"/&gt;
    ///       &lt;reference ref="692045935"/&gt;
    ///       &lt;reference ref="383511723"/&gt;
    ///       &lt;/object>
    ///     </pre>
    ///   </example>
    /// </summary>
    public class IBArray : IBObject
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBArray" /> class.
        /// </summary>
        /// <param name = "attributes">The attributes.</param>
        public IBArray(IDictionary<String, String> attributes)
            : base(attributes) {}

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public override void Accept(IIBVisitor visitor)
        {
            visitor.Visit(this);
            foreach (IIBItem item in this)
            {
                item.Accept(visitor);
            }
        }
    }
}