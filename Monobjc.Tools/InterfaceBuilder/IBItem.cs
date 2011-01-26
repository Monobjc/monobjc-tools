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
    ///   This is the base class of all the elements of an Interface Builder file.
    /// </summary>
    /// <typeparam name = "T">The parametric type</typeparam>
    public abstract class IBItem<T> : IIBItem, IEquatable<IBItem<T>>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "IBItem&lt;T&gt;" /> class.
        /// </summary>
        /// <param name = "attributes">The attributes.</param>
        protected IBItem(IDictionary<String, String> attributes)
        {
            this.Key = attributes.ContainsKey("key") ? attributes["key"] : String.Empty;
            this.Id = attributes.ContainsKey("id") ? attributes["id"] : String.Empty;
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; set; }

        #region IEquatable<IBItem<T>> Members

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name = "other">An object to compare with this object.</param>
        /// <returns>
        ///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(IBItem<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.Key, this.Key);
        }

        #endregion

        #region IIBItem Members

        /// <summary>
        ///   Gets the key of this instance.
        /// </summary>
        /// <value>The key of this instance.</value>
        public String Key { get; private set; }

        /// <summary>
        ///   Gets the identifier of this instance.
        /// </summary>
        /// <value>The identifier of this instance.</value>
        public String Id { get; private set; }

        /// <summary>
        ///   Appends a child to this instance.
        /// </summary>
        /// <param name = "item">The item to append.</param>
        public virtual void AppendChild(IIBItem item) {}

        /// <summary>
        ///   Finishes the processing of this instance.
        /// </summary>
        public virtual void Finish(IIBReferenceResolver resolver) {}

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public abstract void Accept(IIBVisitor visitor);

        /// <summary>
        ///   Sets the value of this instance.
        /// </summary>
        /// <param name = "content">The content.</param>
        public virtual void SetValue(String content) {}

        #endregion

        /// <summary>
        ///   Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///   A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <summary>
        ///   Determines whether the specified <see cref = "System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name = "obj">The <see cref = "System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref = "System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref = "T:System.NullReferenceException">
        ///   The <paramref name = "obj" /> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (IBItem<T>))
            {
                return false;
            }
            return this.Equals((IBItem<T>) obj);
        }

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        /// <summary>
        ///   Implements the operator ==.
        /// </summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(IBItem<T> left, IBItem<T> right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///   Implements the operator !=.
        /// </summary>
        /// <param name = "left">The left operand.</param>
        /// <param name = "right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(IBItem<T> left, IBItem<T> right)
        {
            return !Equals(left, right);
        }
    }
}