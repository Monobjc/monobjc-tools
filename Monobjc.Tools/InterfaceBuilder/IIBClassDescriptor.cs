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

namespace Monobjc.Tools.InterfaceBuilder
{
    public interface IIBClassDescriptor
    {
        /// <summary>
        ///   Gets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        String ClassName { get; }

        /// <summary>
        ///   Gets the name of the super class.
        /// </summary>
        /// <value>The name of the super class.</value>
        String SuperClassName { get; }

        /// <summary>
        ///   Gets the actions.
        /// </summary>
        /// <value>The actions.</value>
        IEnumerable<IBActionDescriptor> Actions { get; }

        /// <summary>
        ///   Gets the outlets.
        /// </summary>
        /// <value>The outlets.</value>
        IEnumerable<IBOutletDescriptor> Outlets { get; }
    }
}

