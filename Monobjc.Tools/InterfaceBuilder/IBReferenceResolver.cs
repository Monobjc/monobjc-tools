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
    /// <summary>
    ///   Default implement of a reference resolver.
    /// </summary>
    internal class IBReferenceResolver : IIBReferenceResolver
    {
        private readonly Dictionary<String, IIBItem> references = new Dictionary<String, IIBItem>();

        #region IIBReferenceResolver Members

        /// <summary>
        ///   Clears all the references.
        /// </summary>
        public void Clear()
        {
            this.references.Clear();
        }

        /// <summary>
        ///   Adds a reference.
        /// </summary>
        /// <param name = "item">The item.</param>
        public void AddReference(IIBItem item)
        {
            if (!String.IsNullOrEmpty(item.Id))
            {
                this.references.Add(item.Id, item);
            }
        }

        /// <summary>
        ///   Resolves the reference.
        /// </summary>
        /// <param name = "reference">The reference.</param>
        /// <returns></returns>
        public IIBItem ResolveReference(IBReference reference)
        {
            if (this.references.ContainsKey(reference.Value))
            {
                return this.references[reference.Value];
            }
            return null;
        }

        #endregion
    }
}