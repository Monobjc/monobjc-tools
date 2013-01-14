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
using System.Diagnostics;

namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   Structure to hold an outlet definition.
    /// </summary>
    [DebuggerDisplay("IBOutlet {ClassName} {Name};")]
    public class IBOutletDescriptor
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBOutletDescriptor" /> struct.
        /// </summary>
        /// <param name = "name">The name.</param>
        /// <param name = "className">Name of the class.</param>
        public IBOutletDescriptor(string name, string className)
        {
            this.Name = name;
            this.ClassName = className;
        }

        /// <summary>
        ///   Gets or sets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        public String ClassName { get; private set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public String Name { get; private set; }
    }
}