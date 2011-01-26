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
using System.Linq;

namespace Monobjc.Tools.InterfaceBuilder.Visitors
{
    /// <summary>
    ///   This implementation of <see cref = "IIBVisitor" /> collects all the <see cref = "IBPartialClassDescription" /> instances of an <see cref = "IBArchive" />.
    /// </summary>
    public class ClassDescriptionCollector : IIBVisitor
    {
        private readonly IList<IBPartialClassDescription> allDescriptions = new List<IBPartialClassDescription>();

        /// <summary>
        ///   Returns the list of all the class names collected.
        /// </summary>
        /// <value>The class names.</value>
        public IEnumerable<string> ClassNames
        {
            get { return this.allDescriptions.Select(d => d.ClassName).Distinct(); }
        }

        /// <summary>
        ///   Returns all the <see cref = "IBPartialClassDescription" /> related to a given class Name.
        /// </summary>
        /// <value></value>
        public IEnumerable<IBPartialClassDescription> this[String className]
        {
            get { return this.allDescriptions.Where(d => String.Equals(d.ClassName, className)); }
        }

        #region IIBVisitor Members

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBArchive item)
        {
            this.allDescriptions.Clear();
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBArray item) {}

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBBytes item) {}

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBBoolean item) {}

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBClassDescriber item) {}

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBDictionary item) {}

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBDouble item) {}

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBFloat item) {}

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBInteger item) {}

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        public void Visit(IBNil item) {}

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBObject item) {}

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBPartialClassDescription item)
        {
            this.allDescriptions.Add(item);
        }

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBReference item) {}

        /// <summary>
        ///   Visits the specified item.
        /// </summary>
        /// <param Name = "item">The item.</param>
        public void Visit(IBString item) {}

        #endregion
    }
}