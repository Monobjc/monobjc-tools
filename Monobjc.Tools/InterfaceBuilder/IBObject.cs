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
using System.Collections;
using System.Collections.Generic;

namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   An <see cref = "IBObject" /> element is an element that has children elements.
    ///   <para>These children elements can be a collection of elements, a collection of key/value pairs, or a collection of properties.</para>
    /// </summary>
    public class IBObject : IBItem<List<IIBItem>>, IList<IIBItem>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBObject" /> class.
        /// </summary>
        /// <param name = "attributes">The attributes.</param>
        public IBObject(IDictionary<String, String> attributes)
            : base(attributes)
        {
            this.Value = new List<IIBItem>();
            this.ObjectClass = attributes.ContainsKey("class") ? attributes["class"] : String.Empty;
        }

        /// <summary>
        ///   Gets or sets the object's class.
        /// </summary>
        /// <value>The object's class.</value>
        public String ObjectClass { get; private set; }

        #region IList<IIBItem> Members

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///   A <see cref = "T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IIBItem> GetEnumerator()
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

        /// <summary>
        ///   Adds an item to the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name = "item">The object to add to the <see cref = "T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref = "T:System.NotSupportedException">
        ///   The <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        public void Add(IIBItem item)
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
        public bool Contains(IIBItem item)
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
        public void CopyTo(IIBItem[] array, int arrayIndex)
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
        public bool Remove(IIBItem item)
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
        ///   Determines the index of a specific item in the <see cref = "T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <returns>
        ///   The index of <paramref name = "item" /> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name = "item">The object to locate in the <see cref = "T:System.Collections.Generic.IList`1" />.
        /// </param>
        public int IndexOf(IIBItem item)
        {
            return this.Value.IndexOf(item);
        }

        /// <summary>
        ///   Inserts an item to the <see cref = "T:System.Collections.Generic.IList`1" /> at the specified index.
        /// </summary>
        /// <param name = "index">The zero-based index at which <paramref name = "item" /> should be inserted.
        /// </param>
        /// <param name = "item">The object to insert into the <see cref = "T:System.Collections.Generic.IList`1" />.
        /// </param>
        /// <exception cref = "T:System.ArgumentOutOfRangeException"><paramref name = "index" /> is not a valid index in the <see cref = "T:System.Collections.Generic.IList`1" />.
        /// </exception>
        /// <exception cref = "T:System.NotSupportedException">The <see cref = "T:System.Collections.Generic.IList`1" /> is read-only.
        /// </exception>
        public void Insert(int index, IIBItem item)
        {
            this.Value.Insert(index, item);
        }

        /// <summary>
        ///   Removes the <see cref = "T:System.Collections.Generic.IList`1" /> item at the specified index.
        /// </summary>
        /// <param name = "index">The zero-based index of the item to remove.
        /// </param>
        /// <exception cref = "T:System.ArgumentOutOfRangeException"><paramref name = "index" /> is not a valid index in the <see cref = "T:System.Collections.Generic.IList`1" />.
        /// </exception>
        /// <exception cref = "T:System.NotSupportedException">The <see cref = "T:System.Collections.Generic.IList`1" /> is read-only.
        /// </exception>
        public void RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        /// <summary>
        ///   Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        ///   The element at the specified index.
        /// </returns>
        /// <param name = "index">The zero-based index of the element to get or set.
        /// </param>
        /// <exception cref = "T:System.ArgumentOutOfRangeException"><paramref name = "index" /> is not a valid index in the <see cref = "T:System.Collections.Generic.IList`1" />.
        /// </exception>
        /// <exception cref = "T:System.NotSupportedException">The property is set and the <see cref = "T:System.Collections.Generic.IList`1" /> is read-only.
        /// </exception>
        public IIBItem this[int index]
        {
            get { return this.Value[index]; }
            set { this.Value[index] = value; }
        }

        #endregion

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public override void Accept(IIBVisitor visitor)
        {
            visitor.Visit(this);
            foreach (IIBItem item in this)
            {
                item.Accept(visitor);
            }
        }

        /// <summary>
        ///   Adds the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        public override void AppendChild(IIBItem item)
        {
            this.Add(item);
        }

        /// <summary>
        ///   Finishes the processing of this instance.
        /// </summary>
        /// <param name = "resolver"></param>
        public override void Finish(IIBReferenceResolver resolver)
        {
            /*
            List<IIBItem> references = this.Value.FindAll(item => (item is IBReference));
            foreach (IIBItem item in references)
            {
                IBReference reference = (IBReference) item;
                IIBItem dereferencedItem = resolver.ResolveReference(reference);
                if (dereferencedItem!=null)
                {
                    this.Replace(item, dereferencedItem);
                }
            }
             */
        }

        /// <summary>
        ///   Dereferences the given item.
        /// </summary>
        /// <param name = "resolver">The resolver.</param>
        /// <param name = "item">The item.</param>
        /// <returns></returns>
        public IIBItem DereferenceItem(IIBReferenceResolver resolver, IIBItem item)
        {
            IBReference reference = item as IBReference;
            return (reference == null) ? item : resolver.ResolveReference(reference);
        }

        /// <summary>
        ///   Finds the element that has the specified key, with the given type.
        /// </summary>
        /// <typeparam name = "T">The parametric type</typeparam>
        /// <param name = "key">The key.</param>
        /// <returns>The found item, otherwise null</returns>
        protected T Find<T>(String key) where T : IIBItem
        {
            return (T) this.Value.Find(item => (String.Equals(key, item.Key) && (item is T)));
        }

        /// <summary>
        ///   Finds the specified key.
        /// </summary>
        /// <typeparam name = "T">The parametric type</typeparam>
        /// <param name = "key">The key.</param>
        /// <param name = "comparison">The comparison.</param>
        /// <returns>The found item, otherwise null</returns>
        protected T Find<T>(String key, StringComparison comparison) where T : IIBItem
        {
            return (T) this.Value.Find(item => (String.Equals(key, item.Key, comparison) && (item is T)));
        }

        /// <summary>
        ///   Replaces the specified item with a dereference one.
        /// </summary>
        /// <param name = "item">The item.</param>
        /// <param name = "dereferencedItem">The dereferenced item.</param>
        public void Replace(IIBItem item, IIBItem dereferencedItem)
        {
            this.Value[this.Value.IndexOf(item)] = dereferencedItem;
        }
    }
}