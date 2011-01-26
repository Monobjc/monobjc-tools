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

namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   Desribes an arbitrary element in an Interface Builder file.
    /// </summary>
    public interface IIBItem
    {
        /// <summary>
        ///   Gets the key.
        /// </summary>
        /// <value>The key.</value>
        String Key { get; }

        /// <summary>
        ///   Gets the identifier.
        /// </summary>
        /// <value>The id.</value>
        String Id { get; }

        /// <summary>
        ///   Appends a child to this instance.
        /// </summary>
        /// <param name = "item">The item.</param>
        void AppendChild(IIBItem item);

        /// <summary>
        ///   Finishes the processing of this instance.
        /// </summary>
        void Finish(IIBReferenceResolver resolver);

        /// <summary>
        ///   Sets the value of this instance.
        /// </summary>
        /// <param name = "content">The content.</param>
        void SetValue(String content);

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        void Accept(IIBVisitor visitor);
    }
}