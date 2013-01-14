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
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	/// <summary>
	///   Represents the model for a method's parameter.
	/// </summary>
	[Serializable]
	[XmlRoot("Parameter")]
	public partial class MethodParameterEntity : BaseEntity, IEquatable<MethodParameterEntity>
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "MethodParameterEntity" /> class.
		/// </summary>
		public MethodParameterEntity ()
		{
		}

		/// <summary>
		///   Initializes a new instance of the <see cref = "MethodParameterEntity" /> class.
		/// </summary>
		/// <param name = "methodParameterEntity">The method parameter entity.</param>
		public MethodParameterEntity (MethodParameterEntity methodParameterEntity) : this()
		{
			this.MinAvailability = methodParameterEntity.MinAvailability;
			this.Generate = methodParameterEntity.Generate;
			this.IsBlock = methodParameterEntity.IsBlock;
			this.IsByRef = methodParameterEntity.IsByRef;
			this.IsOut = methodParameterEntity.IsOut;
			this.Name = methodParameterEntity.Name;
			this.Summary = new List<string> (methodParameterEntity.Summary);
			this.Type = methodParameterEntity.Type;
		}

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
		///   Gets or sets a value indicating whether this instance is out.
		/// </summary>
		/// <value><c>true</c> if this instance is out; otherwise, <c>false</c>.</value>
		[XmlAttribute]
		public bool IsOut {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets a value indicating whether this instance is by ref.
		/// </summary>
		/// <value><c>true</c> if this instance is by ref; otherwise, <c>false</c>.</value>
		[XmlAttribute]
		public bool IsByRef {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets a value indicating whether this instance is a block.
		/// </summary>
		/// <value><c>true</c> if this instance is a block; otherwise, <c>false</c>.</value>
		[XmlAttribute]
		public bool IsBlock {
			get;
			set;
		}
		
		/// <summary>
		///   Gets or sets a value indicating whether this instance is an array.
		/// </summary>
		/// <value><c>true</c> if this instance is an array; otherwise, <c>false</c>.</value>
		[XmlAttribute]
		public bool IsArray {
			get;
			set;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Monobjc.Tools.Generator.Model.MethodParameterEntity"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode ()
		{
			unchecked {
				int hash = base.GetHashCode();
				hash = hash * 23 + this.IsBlock.GetHashCode ();
				hash = hash * 23 + this.IsByRef.GetHashCode ();
				hash = hash * 23 + this.IsOut.GetHashCode ();
				hash = hash * 23 + (this.Type != null ? this.Type.GetHashCode () : 0);
				return hash;
			}
		}

		/// <summary>
		///   Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
		/// </returns>
		/// <param name = "other">An object to compare with this object.
		/// </param>
		public bool Equals (MethodParameterEntity other)
		{
			if (ReferenceEquals (null, other)) {
				return false;
			}
			if (ReferenceEquals (this, other)) {
				return true;
			}
			return Equals (other.Type, this.Type) &&
				other.IsOut.Equals (this.IsOut) &&
				other.IsByRef.Equals (this.IsByRef) &&
				other.IsBlock.Equals (this.IsBlock);
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
			return this.Equals (obj as MethodParameterEntity);
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
				hash = hash * 23 + this.IsBlock.GetHashCode ();
				hash = hash * 23 + this.IsByRef.GetHashCode ();
				hash = hash * 23 + this.IsOut.GetHashCode ();
				hash = hash * 23 + (this.Type != null ? this.Type.GetHashCode () : 0);
				return hash;
			}
		}

		public static bool operator == (MethodParameterEntity left, MethodParameterEntity right)
		{
			return Equals (left, right);
		}

		public static bool operator != (MethodParameterEntity left, MethodParameterEntity right)
		{
			return !Equals (left, right);
		}

		public override string ToString ()
		{
			return string.Format ("[MethodParameterEntity: Name={0}, Type={1}, IsOut={2}, IsByRef={3}, IsBlock={4}]", Name, Type, IsOut, IsByRef, IsBlock);
		}
	}
}