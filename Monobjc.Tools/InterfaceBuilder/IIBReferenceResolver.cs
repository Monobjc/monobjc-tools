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
namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   Reference resolver for <see cref = "IBDocument" /> file.
    /// </summary>
    public interface IIBReferenceResolver
    {
        /// <summary>
        ///   Clears all the references.
        /// </summary>
        void Clear();

        /// <summary>
        ///   Adds a reference.
        /// </summary>
        /// <param name = "item">The item.</param>
        void AddReference(IIBItem item);

        /// <summary>
        ///   Resolves the reference.
        /// </summary>
        /// <param name = "reference">The reference.</param>
        /// <returns></returns>
        IIBItem ResolveReference(IBReference reference);
    }
}