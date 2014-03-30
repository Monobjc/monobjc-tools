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
using System.Xml;

namespace Monobjc.Tools.PropertyList
{
    /// <summary>
    ///   Represent a dictionary of key/value pairs in PList file.
    /// </summary>
    public class PListDict : PListItemBase, IDictionary<String, PListItemBase>
    {
        private readonly IDictionary<String, PListItemBase> dictionary;
        private PListKey currentKey;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListDict" /> class.
        /// </summary>
        public PListDict() : this(new Dictionary<string, PListItemBase>()) {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListDict" /> class.
        /// </summary>
        /// <param name = "items">The items.</param>
        public PListDict(IDictionary<String, PListItemBase> items)
        {
            this.dictionary = items;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListDict" /> class.
        /// </summary>
        /// <param name = "enumerator">The enumerator.</param>
        public PListDict(IEnumerator<KeyValuePair<String, PListItemBase>> enumerator)
        {
            this.dictionary = new Dictionary<string, PListItemBase>();
            while (enumerator.MoveNext())
            {
                this.dictionary.Add(enumerator.Current.Key, enumerator.Current.Value);
            }
        }

        #region IDictionary<string,PListItemBase> Members

        /// <summary>
        ///   Gets an <see cref = "T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///   An <see cref = "T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public ICollection<string> Keys
        {
            get { return this.dictionary.Keys; }
        }

        /// <summary>
        ///   Gets an <see cref = "T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///   An <see cref = "T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public ICollection<PListItemBase> Values
        {
            get { return this.dictionary.Values; }
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
        ///   Adds an element with the provided key and value to the <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name = "key">The object to use as the key of the element to add.</param>
        /// <param name = "value">The object to use as the value of the element to add.</param>
        /// <exception cref = "T:System.ArgumentNullException">
        ///   <paramref name = "key" /> is null.
        /// </exception>
        /// <exception cref = "T:System.ArgumentException">
        ///   An element with the same key already exists in the <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </exception>
        /// <exception cref = "T:System.NotSupportedException">
        ///   The <see cref = "T:System.Collections.Generic.IDictionary`2" /> is read-only.
        /// </exception>
        public void Add(string key, PListItemBase value)
        {
            this.dictionary.Add(key, value);
        }

        /// <summary>
        ///   Adds an item to the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name = "item">The object to add to the <see cref = "T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref = "T:System.NotSupportedException">
        ///   The <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        public void Add(KeyValuePair<string, PListItemBase> item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Removes all items from the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <exception cref = "T:System.NotSupportedException">
        ///   The <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        public void Clear()
        {
            this.dictionary.Clear();
        }

        /// <summary>
        ///   Determines whether the <see cref = "T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name = "item">The object to locate in the <see cref = "T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        ///   true if <paramref name = "item" /> is found in the <see cref = "T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<string, PListItemBase> item)
        {
            throw new NotSupportedException();
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
        public void CopyTo(KeyValuePair<string, PListItemBase>[] array, int arrayIndex)
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
        public bool Remove(KeyValuePair<string, PListItemBase> item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Determines whether the <see cref = "T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name = "key">The key to locate in the <see cref = "T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns>
        ///   true if the <see cref = "T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
        /// </returns>
        /// <exception cref = "T:System.ArgumentNullException">
        ///   <paramref name = "key" /> is null.
        /// </exception>
        public bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <summary>
        ///   Removes the element with the specified key from the <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name = "key">The key of the element to remove.</param>
        /// <returns>
        ///   true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name = "key" /> was not found in the original <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        /// <exception cref = "T:System.ArgumentNullException">
        ///   <paramref name = "key" /> is null.
        /// </exception>
        /// <exception cref = "T:System.NotSupportedException">
        ///   The <see cref = "T:System.Collections.Generic.IDictionary`2" /> is read-only.
        /// </exception>
        public bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        /// <summary>
        ///   Gets the value associated with the specified key.
        /// </summary>
        /// <param name = "key">The key whose value to get.</param>
        /// <param name = "value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name = "value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        ///   true if the object that implements <see cref = "T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <exception cref = "T:System.ArgumentNullException">
        ///   <paramref name = "key" /> is null.
        /// </exception>
        public bool TryGetValue(string key, out PListItemBase value)
        {
            return this.dictionary.TryGetValue(key, out value);
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
            get { return this.dictionary.Count; }
        }

        /// <summary>
        ///   Gets or sets the <see cref = "PListItemBase" /> with the specified key.
        /// </summary>
        /// <value></value>
        public PListItemBase this[string key]
        {
            get { return this.dictionary[key]; }
            set { this.dictionary[key] = value; }
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///   A <see cref = "T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<string, PListItemBase>> IEnumerable<KeyValuePair<string, PListItemBase>>.GetEnumerator()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///   An <see cref = "T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        #endregion

        #region PListItemBase Members

        /// <summary>
        ///   Appends the child to this instance.
        /// </summary>
        /// <param name = "child">The child.</param>
        public override void AppendChild(PListItemBase child)
        {
            if ((this.currentKey == null) ^ (child is PListKey))
            {
                throw new InvalidOperationException();
            }
            if (this.currentKey != null)
            {
                this.dictionary.Add(this.currentKey.Value, child);
                this.currentKey = null;
            }
            else
            {
                this.currentKey = child as PListKey;
            }
        }

        /// <summary>
        ///   Sets the value of this instance.
        /// </summary>
        /// <param name = "value">The value.</param>
        public override void SetValue(string value) {}

        /// <summary>
        ///   Writes this instance to Xml stream.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("dict");
            foreach (String key in this.Keys)
            {
                writer.WriteElementString("key", key);
                this[key].WriteXml(writer);
            }
            writer.WriteEndElement();
        }

        #endregion
    }
}