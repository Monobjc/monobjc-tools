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
	///   This class serves as a base for all the elements that have a namespace (classes, interfaces, etc).
	/// </summary>
	[Serializable]
	[XmlRoot("Type")]
	public partial class TypedEntity : BaseEntity
	{
		private string baseType;

		/// <summary>
		///   Initializes a new instance of the <see cref = "TypedEntity" /> class.
		/// </summary>
		public TypedEntity ()
		{
			this.Enumerations = new EnumerationCollection();
			this.Constants = new ConstantCollection();
			this.Functions = new FunctionCollection();
		}

		/// <summary>
		///   Gets or sets the nature.
		/// </summary>
		/// <value>The nature.</value>
		[XmlAttribute]
		public FrameworkEntityType Nature {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the namespace.
		/// </summary>
		/// <value>The namespace.</value>
		[XmlElement]
		public String Namespace {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the type of the base.
		/// </summary>
		/// <value>The type of the base.</value>
		[XmlElement]
		public String BaseType {
			get {
				return this.baseType;
			}
			set {
				this.baseType = value;
				switch (this.baseType) {
				case "CFIndex":
				case "NSInteger":
					this.MixedType = "int,long";
					break;
				case "NSUInteger":
					this.MixedType = "uint,ulong";
					break;
				default:
					break;
					this.MixedType = String.Empty;
				}
			}
		}

		/// <summary>
		///   Gets or sets the types to use in 32/64 bits.
		/// </summary>
		/// <value>The types as comma separated.</value>
		[XmlElement]
		public String MixedType {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the enumerations.
		/// </summary>
		/// <value>The enumerations.</value>
		[XmlArray]
		[XmlArrayItem (typeof(EnumerationEntity), ElementName = "Enumeration")]
		public EnumerationCollection Enumerations {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the constants.
		/// </summary>
		/// <value>The constants.</value>
		[XmlArray]
		[XmlArrayItem (typeof(ConstantEntity), ElementName = "Constant")]
		public ConstantCollection Constants {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the functions.
		/// </summary>
		/// <value>The functions.</value>
		[XmlArray]
		[XmlArrayItem (typeof(FunctionEntity), ElementName = "Function")]
		public FunctionCollection Functions {
			get;
			set;
		}

		/// <summary>
		///   Gets a value indicating whether this instance has constants.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has constants; otherwise, <c>false</c>.
		/// </value>
		public bool HasConstants {
			get { 
				return (this.Constants.Count > 0);
			}
		}

		/// <summary>
		///   Gets a value indicating whether this instance has enumerations.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has enumerations; otherwise, <c>false</c>.
		/// </value>
		public bool HasEnumerations {
			get { 
				return (this.Enumerations.Count > 0);
			}
		}

		/// <summary>
		///   Gets a value indicating whether this instance has functions.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has functions; otherwise, <c>false</c>.
		/// </value>
		public bool HasFunctions {
			get { 
				return (this.Functions.Count > 0);
			}
		}

		/// <summary>
		///   Adds the entities.
		/// </summary>
		/// <param name = "entities">The entities.</param>
		public virtual void AddRange (List<BaseEntity> entities)
		{
			foreach (BaseEntity entity in entities) {
				EnumerationEntity enumerationEntity = entity as EnumerationEntity;
				if (enumerationEntity != null) {
					if (this.Enumerations.Contains (enumerationEntity)) {
						continue;
					}
					this.Enumerations.Add (enumerationEntity);
				}
				ConstantEntity constantEntity = entity as ConstantEntity;
				if (constantEntity != null) {
					if (this.Constants.Contains (constantEntity)) {
						continue;
					}
					this.Constants.Add (constantEntity);
				}
				FunctionEntity functionEntity = entity as FunctionEntity;
				if (functionEntity != null) {
					this.Functions.Add (functionEntity);
				}
			}
		}

		/// <summary>
		///   Gets the children.
		/// </summary>
		/// <value>The children.</value>
		[XmlIgnore]
		public override List<BaseEntity> Children {
			get {
				List<BaseEntity> children = new List<BaseEntity> ();
				children.AddRange (this.Functions.Cast<BaseEntity> ());
				children.AddRange (this.Enumerations.Cast<BaseEntity> ());
				children.AddRange (this.Constants.Cast<BaseEntity> ());
				return children;
			}
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
				int hash = base.GetHashCode();
				hash = hash * 23 + (this.BaseType != null ? this.BaseType.GetHashCode () : 0);
				hash = hash * 23 + this.Constants.GetHashCode ();
				hash = hash * 23 + this.Enumerations.GetHashCode ();
				hash = hash * 23 + this.Functions.GetHashCode ();
				hash = hash * 23 + (this.MixedType != null ? this.MixedType.GetHashCode () : 0);
				hash = hash * 23 + (this.Namespace != null ? this.Namespace.GetHashCode () : 0);
				hash = hash * 23 + this.Nature.GetHashCode ();
				return hash;
			}
		}
	}
}
