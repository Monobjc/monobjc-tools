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
using System.Text;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   A <see cref = "IBString" /> represents an element that contains a string value.
    ///   <para>Examples of XML code:</para>
    ///   <example>
    ///     &lt;string key="IBDocument.SystemVersion"&gt;9J61&lt;/string>
    ///     &lt;string type="base64-UTF8" key="objectName"&gt;RmlsZSdzIE93bmVyA&lt;/string&gt;
    ///   </example>
    /// </summary>
    public class IBString : IBItem<String>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBString" /> class.
        /// </summary>
        /// <param name = "attributes">The attributes.</param>
        public IBString(IDictionary<String, String> attributes)
            : base(attributes)
        {
            this.Type = attributes.ContainsKey("type") ? attributes["type"] : "UTF8";
        }

        /// <summary>
        ///   Gets or sets the type of the string.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; private set; }

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
            switch (this.Type)
            {
                case "UTF8":
                    this.Value = content;
                    break;
                case "base64-UTF8":
                    this.Value = Encoding.UTF8.GetString(Converter.FromBase64(content));
                    break;
            }
        }
    }
}