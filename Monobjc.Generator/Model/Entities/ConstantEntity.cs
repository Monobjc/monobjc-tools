//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2012 - Laurent Etiemble
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
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	/// <summary>
	///   Represents the model for a method.
	/// </summary>
	[Serializable]
	[XmlRoot("Constant")]
	public partial class ConstantEntity : BaseEntity, IEquatable<ConstantEntity>
	{
		/// <summary>
		///   Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		[XmlElement]
		public String Type {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		[XmlElement]
		public String Value {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets whether this entity use a static value.
		/// </summary>
		/// <value><code>true</code> use a static value; <code>false</code> otherwise.</value>
		[XmlElement]
		public bool Static {
			get;
			set;
		}

		/// <summary>
		///   Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
		/// </returns>
		/// <param name = "other">An object to compare with this object.
		/// </param>
		public bool Equals (ConstantEntity other)
		{
			if (ReferenceEquals (null, other)) {
				return false;
			}
			if (ReferenceEquals (this, other)) {
				return true;
			}
			return base.Equals (other) && Equals (other.Type, this.Type);
		}

		/// <summary>
		///   Determines whether the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />.
		/// </summary>
		/// <returns>
		///   true if the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />; otherwise, false.
		/// </returns>
		/// <param name = "obj">The <see cref = "T:System.Object" /> to compare with the current <see cref = "T:System.Object" />. 
		/// </param>
		/// <exception cref = "T:System.NullReferenceException">The <paramref name = "obj" /> parameter is null.
		/// </exception>
		/// <filterpriority>2</filterpriority>
		public override bool Equals (object obj)
		{
			if (ReferenceEquals (null, obj)) {
				return false;
			}
			if (ReferenceEquals (this, obj)) {
				return true;
			}
			return this.Equals (obj as ConstantEntity);
		}

		/// <summary>
		///   Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		///   A hash code for the current <see cref = "T:System.Object" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashValue ()
		{
			unchecked {
				int hash = base.GetHashValue();
				hash = hash * 23 + this.Static.GetHashCode ();
				hash = hash * 23 + (this.Type != null ? this.Type.GetHashCode () : 0);
				hash = hash * 23 + (this.Value != null ? this.Value.GetHashCode () : 0);
				return hash;
			}
		}

		public static bool operator == (ConstantEntity left, ConstantEntity right)
		{
			return Equals (left, right);
		}

		public static bool operator != (ConstantEntity left, ConstantEntity right)
		{
			return !Equals (left, right);
		}
	}
}