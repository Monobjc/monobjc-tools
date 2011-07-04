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

namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   This subclass of <see cref = "IBObject" /> is used to represents a dictionary (mutable or not).
    ///   <para>When the <see cref = "IBObject.ObjectClass" /> property of the <see cref = "IBArray" /> is either "NSDictionary" or "NSMutableDictionary", the inner elements may contains:</para>
    ///   <list type = "bullet">
    ///     <item>a <see cref = "IBBoolean" /> element to indicate the XML serialization and two <see cref = "IBArray" /> instances (one for the keys and one for the values).</item>
    ///     <item>a series of pair elements (a key element followed by a value element).</item>
    ///   </list>
    ///   <para>This class takes care of the decoding of the keys and the values. When the population is finished, all the keys and values are accessible like a <see cref = "IDictionary{TKey,TValue}" /> instance.</para>
    /// </summary>
    public class IBDictionary : IBObject, IDictionary<String, IIBItem>
    {
        private bool usesXmlCoder;
        private readonly Dictionary<String, IIBItem> dictionary = new Dictionary<String, IIBItem>();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBDictionary" /> class.
        /// </summary>
        /// <param name = "attributes">The attributes.</param>
        public IBDictionary(IDictionary<String, String> attributes)
            : base(attributes) {}

        #region IDictionary<string,IIBItem> Members

        /// <summary>
        ///   Adds an item to the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name = "item">The object to add to the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </param>
        /// <exception cref = "T:System.NotSupportedException">The <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        public void Add(KeyValuePair<string, IIBItem> item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Determines whether the <see cref = "T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <returns>
        ///   true if <paramref name = "item" /> is found in the <see cref = "T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        /// <param name = "item">The object to locate in the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </param>
        public bool Contains(KeyValuePair<string, IIBItem> item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Copies the elements of the <see cref = "T:System.Collections.Generic.ICollection`1" /> to an <see cref = "T:System.Array" />, starting at a particular <see cref = "T:System.Array" /> index.
        /// </summary>
        /// <param name = "array">The one-dimensional <see cref = "T:System.Array" /> that is the destination of the elements copied from <see cref = "T:System.Collections.Generic.ICollection`1" />. The <see cref = "T:System.Array" /> must have zero-based indexing.
        /// </param>
        /// <param name = "arrayIndex">The zero-based index in <paramref name = "array" /> at which copying begins.
        /// </param>
        /// <exception cref = "T:System.ArgumentNullException"><paramref name = "array" /> is null.
        /// </exception>
        /// <exception cref = "T:System.ArgumentOutOfRangeException"><paramref name = "arrayIndex" /> is less than 0.
        /// </exception>
        /// <exception cref = "T:System.ArgumentException"><paramref name = "array" /> is multidimensional.
        ///   -or-
        ///   <paramref name = "arrayIndex" /> is equal to or greater than the length of <paramref name = "array" />.
        ///   -or-
        ///   The number of elements in the source <see cref = "T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name = "arrayIndex" /> to the end of the destination <paramref name = "array" />.
        ///   -or-
        ///   Type <paramref name = "T" /> cannot be cast automatically to the type of the destination <paramref name = "array" />.
        /// </exception>
        public void CopyTo(KeyValuePair<string, IIBItem>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Removes the first occurrence of a specific object from the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        ///   true if <paramref name = "item" /> was successfully removed from the <see cref = "T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name = "item" /> is not found in the original <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        /// <param name = "item">The object to remove from the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </param>
        /// <exception cref = "T:System.NotSupportedException">The <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        public bool Remove(KeyValuePair<string, IIBItem> item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Determines whether the <see cref = "T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <returns>
        ///   true if the <see cref = "T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
        /// </returns>
        /// <param name = "key">The key to locate in the <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </param>
        /// <exception cref = "T:System.ArgumentNullException"><paramref name = "key" /> is null.
        /// </exception>
        public bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <summary>
        ///   Adds an element with the provided key and value to the <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name = "key">The object to use as the key of the element to add.
        /// </param>
        /// <param name = "value">The object to use as the value of the element to add.
        /// </param>
        /// <exception cref = "T:System.ArgumentNullException"><paramref name = "key" /> is null.
        /// </exception>
        /// <exception cref = "T:System.ArgumentException">An element with the same key already exists in the <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </exception>
        /// <exception cref = "T:System.NotSupportedException">The <see cref = "T:System.Collections.Generic.IDictionary`2" /> is read-only.
        /// </exception>
        public void Add(string key, IIBItem value)
        {
            this.dictionary.Add(key, value);
        }

        /// <summary>
        ///   Removes the element with the specified key from the <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>
        ///   true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name = "key" /> was not found in the original <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        /// <param name = "key">The key of the element to remove.
        /// </param>
        /// <exception cref = "T:System.ArgumentNullException"><paramref name = "key" /> is null.
        /// </exception>
        /// <exception cref = "T:System.NotSupportedException">The <see cref = "T:System.Collections.Generic.IDictionary`2" /> is read-only.
        /// </exception>
        public bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        /// <summary>
        ///   Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        ///   true if the object that implements <see cref = "T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <param name = "key">The key whose value to get.
        /// </param>
        /// <param name = "value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name = "value" /> parameter. This parameter is passed uninitialized.
        /// </param>
        /// <exception cref = "T:System.ArgumentNullException"><paramref name = "key" /> is null.
        /// </exception>
        public bool TryGetValue(string key, out IIBItem value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        ///   Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        ///   The element with the specified key.
        /// </returns>
        /// <param name = "key">The key of the element to get or set.
        /// </param>
        /// <exception cref = "T:System.ArgumentNullException"><paramref name = "key" /> is null.
        /// </exception>
        /// <exception cref = "T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name = "key" /> is not found.
        /// </exception>
        /// <exception cref = "T:System.NotSupportedException">The property is set and the <see cref = "T:System.Collections.Generic.IDictionary`2" /> is read-only.
        /// </exception>
        public IIBItem this[string key]
        {
            get { return this.dictionary[key]; }
            set { this.dictionary[key] = value; }
        }

        /// <summary>
        ///   Gets an <see cref = "T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
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
        /// <returns>
        ///   An <see cref = "T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements <see cref = "T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public ICollection<IIBItem> Values
        {
            get { return this.dictionary.Values; }
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///   A <see cref = "T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        IEnumerator<KeyValuePair<string, IIBItem>> IEnumerable<KeyValuePair<string, IIBItem>>.GetEnumerator()
        {
            throw new NotSupportedException();
        }

        #endregion

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public override void Accept(IIBVisitor visitor)
        {
            visitor.Visit(this);
            foreach (String key in this.Keys)
            {
                IIBItem var = this[key];
                var.Accept(visitor);
            }
        }

        /// <summary>
        ///   Adds the specified item.
        /// </summary>
        /// <param name = "item">The item.</param>
        public override void AppendChild(IIBItem item)
        {
            IBBoolean boolean = item as IBBoolean;
            if (!this.usesXmlCoder && (boolean != null && boolean.Key.Equals("EncodedWithXMLCoder")))
            {
                this.usesXmlCoder = true;
                return;
            }
            base.AppendChild(item);
        }

        public override void Finish(IIBReferenceResolver resolver)
        {
            base.Finish(resolver);

            if (this.usesXmlCoder)
            {
                IIBItem item1 = this.Find<IIBItem>("dict.sortedKeys");
                IBArray keys = this.DereferenceItem(resolver, item1) as IBArray;
                IIBItem item2 = this.Find<IIBItem>("dict.values");
                IBArray values = this.DereferenceItem(resolver, item2) as IBArray;
                if (keys != null && values != null)
                {
                    for (int i = 0; i < keys.Count; i++)
                    {
                        IBString key = keys[i] as IBString;
                        IIBItem value = values[i];
                        if (key != null)
                        {
                            this.Add(key.ToString(), value);
                        }
                    }
                }
                else
                {
                    throw new Exception("It seems that the parsing has really gone wrong. Can't find either keys or values...");
                }
            }
            else if (this.Value.Count > 0)
            {
                bool useKeyValuePair;

                // Probe for the keys used.
                IIBItem item = this[0];
                useKeyValuePair = item.Key.StartsWith("NS.key.");

                if (useKeyValuePair)
                {
                    for (int i = 0; i < this.Value.Count;)
                    {
                        IIBItem keyItem = this[i++];
                        IIBItem valueItem = this[i++];
                        this.Add(keyItem.ToString(), valueItem);
                    }
                }
                else
                {
                    for (int i = 0; i < this.Value.Count; i++)
                    {
                        item = this[i];
                        this.Add(item.Key, item);
                    }
                }
            }
        }
    }
}