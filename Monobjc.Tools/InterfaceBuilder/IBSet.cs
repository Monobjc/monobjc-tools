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
    ///   This subclass of <see cref = "IBObject" /> is used to represents a set (mutable or not).
    /// </summary>
    public class IBSet : IBObject
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBSet" /> class.
        /// </summary>
        /// <param name = "attributes">The attributes.</param>
        public IBSet(IDictionary<String, String> attributes)
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