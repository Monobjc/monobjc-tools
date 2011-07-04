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
    ///   This class is the top-level object contained in an IB file.
    ///   <para>It contains information of the format of the archive</para>
    ///   <para>The <see cref = "Type" /> attribute can have the following values:</para>
    ///   <list type = "bullet">
    ///     <item>"com.apple.InterfaceBuilder3.Cocoa.XIB"</item>
    ///     <item>"com.apple.InterfaceBuilder3.CocoaTouch.XIB"</item>
    ///   </list>
    /// </summary>
    public class IBArchive : IBArray
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBArchive" /> class.
        /// </summary>
        /// <param name = "attributes">The attributes.</param>
        public IBArchive(IDictionary<String, String> attributes)
            : base(attributes)
        {
            this.Type = attributes["type"];
            this.Version = attributes["version"];
        }

        /// <summary>
        ///   Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public String Type { get; private set; }

        /// <summary>
        ///   Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public String Version { get; private set; }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public override void Accept(IIBVisitor visitor)
        {
            visitor.Visit(this);
            foreach (IIBItem item in this.Value)
            {
                item.Accept(visitor);
            }
        }
    }
}