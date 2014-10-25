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

namespace Monobjc.Tools.Utilities
{
    /// <summary>
    ///   The available Mac architectures.
    /// </summary>
    [Flags]
    public enum MacOSArchitecture
    {
        /// <summary>
        ///   Constant none.
        /// </summary>
        None = 0,
        /// <summary>
        ///   The PowerPC/32 bits architecture
        /// </summary>
        PPC = 1,
        /// <summary>
        ///   The PowerPC/64 bits architecture
        /// </summary>
        PPC64 = 2,
        /// <summary>
        ///   The Intel/32 bits architecture
        /// </summary>
        X86 = 4,
        /// <summary>
        ///   The Intel/64 bits architecture
        /// </summary>
        X8664 = 8,
        /// <summary>
        ///   All the PowerPC architectures
        /// </summary>
        PowerPC = PPC,
        /// <summary>
        ///   All the Intel architectures
        /// </summary>
        Intel = X86 | X8664,
        /// <summary>
        ///   The architectures for a 32 bits universal binary
        /// </summary>
        Universal32 = PPC | X86,
        /// <summary>
        ///   The architectures for a 32/64 bits universal binary
        /// </summary>
        Universal3264 = PPC | X86 | X8664,
    }
}