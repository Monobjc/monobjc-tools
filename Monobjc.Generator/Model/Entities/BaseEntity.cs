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
using System.Linq;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	/// <summary>
	///   This class serves as a base for all the documentation elements (classes, interfaces, methods, etc).
	/// </summary>
	[Serializable]
	[XmlRoot("Base")]
	public partial class BaseEntity : IEquatable<BaseEntity>
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "BaseEntity" /> class.
		/// </summary>
		public BaseEntity ()
		{
			this.Summary = new List<String> ();
			this.Generate = true;
		}

		/// <summary>
		///   Gets or sets the summary.
		/// </summary>
		/// <value>The summary.</value>
		[XmlArray]
		[XmlArrayItem ("Line")]
		public List<String> Summary {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the mininum availability.
		/// </summary>
		/// <value>The mininum availability.</value>
		[XmlElement]
		public String MinAvailability {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the maximum availability.
		/// </summary>
		/// <value>The maximum availability.</value>
		[XmlElement]
		public String MaxAvailability {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the obsolete attribute content.
		/// </summary>
		/// <value>The obsolete attribute content.</value>
		[XmlElement]
		public String Obsolete {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		[XmlElement]
		public String Name {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets whether this entity must be generated.
		/// </summary>
		/// <value><code>true</code> to generate; <code>false</code> otherwise.</value>
		[XmlAttribute]
		public bool Generate {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets a value indicating whether to copy this instance in descendants.
		/// </summary>
		/// <value><c>true</c> to copy this instance in descendants; otherwise, <c>false</c>.</value>
		[XmlAttribute]
		public bool CopyInDescendants {
			get;
			set;
		}

		/// <summary>
		///   Gets the children.
		/// </summary>
		/// <value>The children.</value>
		[XmlIgnore]
		public virtual List<BaseEntity> Children {
			get { return new List<BaseEntity> (); }
		}

		/// <summary>
		///   Adjusts the availability.
		/// </summary>
		public void AdjustAvailability ()
		{
			String minAvailability = "VERSION";
			String maxAvailability = "VERSION";

			foreach (BaseEntity value in this.Children.Where(c => c.Generate)) {
				string valueMinAvailability = value.MinAvailability ?? String.Empty;
				if (String.Compare (valueMinAvailability, minAvailability) < 0) {
					minAvailability = valueMinAvailability;
				}
				string valueMaxAvailability = value.MaxAvailability ?? String.Empty;
				if (String.Compare (valueMaxAvailability, maxAvailability) > 0) {
					maxAvailability = valueMaxAvailability;
				}
			}

			this.MinAvailability = minAvailability;
			this.MaxAvailability = maxAvailability;
		}

		/// <summary>
		///   Returns a <see cref = "System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		///   A <see cref = "System.String" /> that represents this instance.
		/// </returns>
		public override string ToString ()
		{
			return this.Name;
		}

		/// <summary>
		///   Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
		/// </returns>
		/// <param name = "other">An object to compare with this object.
		/// </param>
		public bool Equals (BaseEntity other)
		{
			if (ReferenceEquals (null, other)) {
				return false;
			}
			if (ReferenceEquals (this, other)) {
				return true;
			}
			return Equals (other.Name, this.Name);
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
			if (obj.GetType () != typeof(BaseEntity)) {
				return false;
			}
			return this.Equals ((BaseEntity)obj);
		}

		/// <summary>
		///   Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		///   A hash code for the current <see cref = "T:System.Object" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode ()
		{
			unchecked {
				int hash = 17;
				hash = hash * 23 + this.CopyInDescendants.GetHashCode ();
				hash = hash * 23 + this.Generate.GetHashCode ();
				hash = hash * 23 + (this.MaxAvailability != null ? this.MaxAvailability.GetHashCode () : 0);
				hash = hash * 23 + (this.MinAvailability != null ? this.MinAvailability.GetHashCode () : 0);
				hash = hash * 23 + (this.Name != null ? this.Name.GetHashCode () : 0);
				hash = hash * 23 + (this.Obsolete != null ? this.Obsolete.GetHashCode () : 0);
				return hash;
			}
		}

		/// <summary>
		///   Implements the operator ==.
		/// </summary>
		/// <param name = "left">The left.</param>
		/// <param name = "right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator == (BaseEntity left, BaseEntity right)
		{
			return Equals (left, right);
		}

		/// <summary>
		///   Implements the operator !=.
		/// </summary>
		/// <param name = "left">The left.</param>
		/// <param name = "right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator != (BaseEntity left, BaseEntity right)
		{
			return !Equals (left, right);
		}
	}
}