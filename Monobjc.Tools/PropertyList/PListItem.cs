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

namespace Monobjc.Tools.PropertyList
{
    /// <summary>
    ///   Generic implementation of <see cref = "PListItemBase" /> interface that holds a value.
    /// </summary>
    /// <typeparam name = "T">The parametric type</typeparam>
    public abstract class PListItem<T> : PListItemBase, IEquatable<PListItem<T>>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListItem&lt;T&gt;" /> class.
        /// </summary>
        protected PListItem() {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListItem&lt;T&gt;" /> class.
        /// </summary>
        /// <param name = "value">The value.</param>
        protected PListItem(T value)
        {
            this.Value = value;
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; set; }

        #region IEquatable<PListItem<T>> Members

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name = "other">An object to compare with this object.</param>
        /// <returns>
        ///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(PListItem<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.Value, this.Value);
        }

        #endregion

        #region PListItemBase Members

        /// <summary>
        ///   Appends the child to this instance.
        /// </summary>
        /// <param name = "child">The child.</param>
        public override void AppendChild(PListItemBase child)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Sets the value of this instance.
        /// </summary>
        /// <param name = "value">The value.</param>
        public override void SetValue(string value) {}

        #endregion

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
            if (obj.GetType() != typeof (PListItem<T>))
            {
                return false;
            }
            return this.Equals((PListItem<T>) obj);
        }

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        ///   Implements the operator ==.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(PListItem<T> left, PListItem<T> right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///   Implements the operator !=.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(PListItem<T> left, PListItem<T> right)
        {
            return !Equals(left, right);
        }
    }
}