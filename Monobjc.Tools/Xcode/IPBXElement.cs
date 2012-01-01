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
using System.IO;

namespace Monobjc.Tools.Xcode
{
    /// <summary>
    ///   Base interface for a project element.
    /// </summary>
    public interface IPBXElement
    {
        /// <summary>
        ///   Gets the elemnt's nature.
        /// </summary>
        /// <value>The nature.</value>
        PBXElementType Nature { get; }

        /// <summary>
        ///   Gets the nature as a string.
        /// </summary>
        /// <value>The nature.</value>
        String Isa { get; }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        String Description { get; }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        void Accept(IPBXVisitor visitor);

        /// <summary>
        ///   Writes this element to the writer.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "map">The map.</param>
        void WriteTo(ProjectWriter writer, IDictionary<IPBXElement, string> map);
    }
}