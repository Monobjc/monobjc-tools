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

namespace Monobjc.Tools.InterfaceBuilder.Visitors
{
    /// <summary>
    ///   This implementation of <see cref = "IIBVisitor" /> dumps recursively the model to the console.
    ///   <para>This is mostly used for debugging purpose.</para>
    /// </summary>
    public class PrintVisitor : IIBVisitor
    {
        #region IIBVisitor Members

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBArchive item)
        {
            Console.WriteLine("Archive " + item.Type + "/" + item.Version);
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBArray item)
        {
            Console.WriteLine(item.ToString());
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBBytes item)
        {
            Console.WriteLine(item.ToString());
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBBoolean item)
        {
            Console.WriteLine(item.ToString());
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBClassDescriber item)
        {
            Console.WriteLine("ClassDescriber");
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBDictionary item)
        {
            Console.WriteLine(item.ToString());
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBDouble item)
        {
            Console.WriteLine(item.ToString());
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBFloat item)
        {
            Console.WriteLine(item.ToString());
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBInteger item)
        {
            Console.WriteLine(item.ToString());
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        public void Visit(IBNil item)
        {
            Console.WriteLine(item.ToString());
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBObject item)
        {
            Console.WriteLine(item.ToString());
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBPartialClassDescription item)
        {
            Console.WriteLine("PartialClassDescription " + item.ClassName);
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBReference item)
        {
            Console.WriteLine(item.ToString());
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBString item)
        {
            Console.WriteLine(item.ToString());
        }

        #endregion
    }
}