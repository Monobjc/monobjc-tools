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
namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   This interface allows the visit of an IB archive, for various purpose (collection, dump, etc).
    /// </summary>
    public interface IIBVisitor
    {
        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBArchive item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBArray item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBBytes item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBBoolean item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBClassDescriber item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBDictionary item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBDouble item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBFloat item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBInteger item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBNil item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBObject item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBPartialClassDescription item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBReference item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBSet item);

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        void Visit(IBString item);
    }
}