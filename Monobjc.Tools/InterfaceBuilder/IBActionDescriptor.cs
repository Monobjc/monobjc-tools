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
using System.Diagnostics;

namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   Structure to hold an action definition.
    /// </summary>
    [DebuggerDisplay("- (IBAction){Message}({Argument}) sender;")]
    public class IBActionDescriptor
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBActionDescriptor" /> struct.
        /// </summary>
        /// <param name = "message">The Message.</param>
        /// <param name = "argument">The Argument.</param>
        public IBActionDescriptor(string message, string argument)
        {
            this.Message = message;
            this.Argument = argument;
        }

        /// <summary>
        ///   Gets or sets the argument.
        /// </summary>
        /// <value>The argument.</value>
        public String Argument { get; private set; }

        /// <summary>
        ///   Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public String Message { get; private set; }
    }
}