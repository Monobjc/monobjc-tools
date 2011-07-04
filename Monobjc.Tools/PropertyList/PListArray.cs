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
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Monobjc.Tools.PropertyList
{
    /// <summary>
    ///   Represent an array of items in PList file.
    /// </summary>
    public class PListArray : PListItem<List<PListItemBase>>, ICollection<PListItemBase>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListArray" /> class.
        /// </summary>
        public PListArray()
            : this(new List<PListItemBase>()) {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListArray" /> class.
        /// </summary>
        /// <param name = "items">The items.</param>
        public PListArray(List<PListItemBase> items)
        {
            this.Value = items;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListArray" /> class.
        /// </summary>
        /// <param name = "items">The items.</param>
        public PListArray(IEnumerable<PListItemBase> items)
        {
            this.Value = new List<PListItemBase>(items);
        }

        #region ICollection<PListItemBase> Members

        /// <summary>
        ///   Adds an item to the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name = "item">The object to add to the <see cref = "T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref = "T:System.NotSupportedException">
        ///   The <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        public void Add(PListItemBase item)
        {
            this.Value.Add(item);
        }

        /// <summary>
        ///   Removes all items from the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <exception cref = "T:System.NotSupportedException">
        ///   The <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        public void Clear()
        {
            this.Value.Clear();
        }

        /// <summary>
        ///   Determines whether the <see cref = "T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name = "item">The object to locate in the <see cref = "T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        ///   true if <paramref name = "item" /> is found in the <see cref = "T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(PListItemBase item)
        {
            return this.Value.Contains(item);
        }

        /// <summary>
        ///   Copies the elements of the <see cref = "T:System.Collections.Generic.ICollection`1" /> to an <see cref = "T:System.Array" />, starting at a particular <see cref = "T:System.Array" /> index.
        /// </summary>
        /// <param name = "array">The one-dimensional <see cref = "T:System.Array" /> that is the destination of the elements copied from <see cref = "T:System.Collections.Generic.ICollection`1" />. The <see cref = "T:System.Array" /> must have zero-based indexing.</param>
        /// <param name = "arrayIndex">The zero-based index in <paramref name = "array" /> at which copying begins.</param>
        /// <exception cref = "T:System.ArgumentNullException">
        ///   <paramref name = "array" /> is null.
        /// </exception>
        /// <exception cref = "T:System.ArgumentOutOfRangeException">
        ///   <paramref name = "arrayIndex" /> is less than 0.
        /// </exception>
        /// <exception cref = "T:System.ArgumentException">
        ///   <paramref name = "array" /> is multidimensional.
        ///   -or-
        ///   <paramref name = "arrayIndex" /> is equal to or greater than the length of <paramref name = "array" />.
        ///   -or-
        ///   The number of elements in the source <see cref = "T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name = "arrayIndex" /> to the end of the destination <paramref name = "array" />.
        ///   -or-
        ///   Type <paramref name = "T" /> cannot be cast automatically to the type of the destination <paramref name = "array" />.
        /// </exception>
        public void CopyTo(PListItemBase[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Removes the first occurrence of a specific object from the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name = "item">The object to remove from the <see cref = "T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        ///   true if <paramref name = "item" /> was successfully removed from the <see cref = "T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name = "item" /> is not found in the original <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        /// <exception cref = "T:System.NotSupportedException">
        ///   The <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        public bool Remove(PListItemBase item)
        {
            return this.Value.Remove(item);
        }

        /// <summary>
        ///   Gets the number of elements contained in the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///   The number of elements contained in the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public int Count
        {
            get { return this.Value.Count; }
        }

        /// <summary>
        ///   Gets a value indicating whether the <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///   A <see cref = "T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<PListItemBase> GetEnumerator()
        {
            return this.Value.GetEnumerator();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///   An <see cref = "T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        /// <summary>
        ///   Appends the child.
        /// </summary>
        /// <param name = "item">The item.</param>
        public override void AppendChild(PListItemBase item)
        {
            this.Add(item);
        }

        /// <summary>
        ///   Writes this instance to Xml stream.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("array");
            foreach (PListItemBase child in this.Value)
            {
                child.WriteXml(writer);
            }
            writer.WriteEndElement();
        }
    }
}