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
	///   Represents the model for a class.
	/// </summary>
	[Serializable]
	[XmlRoot("Class")]
	public partial class ClassEntity : TypedEntity
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "ClassEntity" /> class.
		/// </summary>
		public ClassEntity ()
		{
			this.Methods = new MethodCollection();
			this.Properties = new PropertyCollection();
			this.DelegateMethods = new MethodCollection();
		}

		/// <summary>
		///   Gets or sets the pattern used for description.
		/// </summary>
		/// <value>The pattern used for description.</value>
		[XmlElement ("Description")]
		public String Description {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the class owner.
		/// </summary>
		/// <value>The class owner.</value>
		[XmlElement ("AdditionFor")]
		public String AdditionFor {
			get;
			set;
		}
		
		/// <summary>
		///   Gets or sets the conforms to.
		/// </summary>
		/// <value>The conforms to.</value>
		[XmlElement ("Protocols")]
		public String ConformsTo {
			get;
			set;
		}
		
		/// <summary>
		///   Gets or sets the methods.
		/// </summary>
		/// <value>The methods.</value>
		[XmlArray]
		[XmlArrayItem (typeof(MethodEntity), ElementName = "Method")]
		public MethodCollection Methods {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the properties.
		/// </summary>
		/// <value>The properties.</value>
		[XmlArray]
		[XmlArrayItem (typeof(PropertyEntity), ElementName = "Property")]
		public PropertyCollection Properties {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the delegate methods.
		/// </summary>
		/// <value>The delegate methods.</value>
		[XmlArray]
		[XmlArrayItem (typeof(MethodEntity), ElementName = "DelegateMethod")]
		public MethodCollection DelegateMethods {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the super class.
		/// </summary>
		/// <value>The super class.</value>
		[XmlIgnore]
		public ClassEntity SuperClass { get; set; }

		/// <summary>
		///   Gets or sets the protocols implemented.
		/// </summary>
		/// <value>The protocols implemented</value>
		[XmlIgnore]
		public List<ProtocolEntity> Protocols { get; set; }

		/// <summary>
		///   Gets or sets the extended class.
		/// </summary>
		/// <value>The extended class.</value>
		[XmlIgnore]
		public ClassEntity ExtendedClass { get; set; }

		/// <summary>
		///   Determines whether the specified instance has method.
		/// </summary>
		/// <param name = "name">The name.</param>
		/// <param name = "isStatic">if set to <c>true</c> [is static].</param>
		/// <returns>
		///   <c>true</c> if the specified instance has method; otherwise, <c>false</c>.
		/// </returns>
		public bool HasMethod (String name, bool isStatic)
		{
			MethodEntity entity = this.Methods.SingleOrDefault (m => m.Name.Equals (name) && m.Static == isStatic);
			return (entity != null);
		}

		/// <summary>
		///   Gets a value indicating whether this instance has constructors.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has constructors; otherwise, <c>false</c>.
		/// </value>
		public bool HasConstructors {
			get { 
				return (this.GetConstructors (true, true).Count () > 0);
			}
		}

		/// <summary>
		///   Gets all methods (superclass and protocols).
		/// </summary>
		public IEnumerable<MethodEntity> GetMethods (bool includeSuper, bool includeProtocols)
		{
			List<MethodEntity> methods = new List<MethodEntity> (this.Methods);

			if (this.SuperClass != null && includeSuper) {
				methods.AddRange (this.SuperClass.GetMethods (true, true));
			}

			if (this.Protocols != null && includeProtocols) {
				foreach (ProtocolEntity protocol in this.Protocols) {
					methods.AddRange (protocol.Methods);
				}
			}
			return methods.Distinct ();
		}

		/// <summary>
		///   Gets all constructors (superclass and protocols).
		/// </summary>
		public IEnumerable<MethodEntity> GetConstructors (bool includeSuper, bool includeProtocols)
		{
			IEnumerable<MethodEntity> methods = this.GetMethods (includeSuper, includeProtocols);

			return methods.Where (m => m.IsConstructor);
		}

		/// <summary>
		///   Gets all methods (superclass and protocols).
		/// </summary>
		public IEnumerable<PropertyEntity> GetProperties (bool includeSuper, bool includeProtocols)
		{
			List<PropertyEntity> properties = new List<PropertyEntity> (this.Properties);

			if (this.SuperClass != null && includeSuper) {
				properties.AddRange (this.SuperClass.GetProperties (true, true));
			}

			if (this.Protocols != null && includeProtocols) {
				foreach (ProtocolEntity protocol in this.Protocols) {
					properties.AddRange (protocol.Properties);
				}
			}
			return properties.Distinct ();
		}

		/// <summary>
		///   Adds the entities.
		/// </summary>
		/// <param name = "entities">The entities.</param>
		public override void AddRange (List<BaseEntity> entities)
		{
			base.AddRange (entities);

			foreach (BaseEntity entity in entities) {
				MethodEntity methodEntity = entity as MethodEntity;
				if (methodEntity != null) {
					this.Methods.Add (methodEntity);
				}
				PropertyEntity propertyEntity = entity as PropertyEntity;
				if (propertyEntity != null) {
					this.Properties.Add (propertyEntity);
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
				children.AddRange (this.Methods.Cast<BaseEntity> ());
				children.AddRange (this.Properties.Cast<BaseEntity> ());
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
		public override int GetHashValue ()
		{
			unchecked {
				int hash = base.GetHashValue();
				hash = hash * 23 + (this.AdditionFor != null ? this.AdditionFor.GetHashCode () : 0);
				hash = hash * 23 + (this.ConformsTo != null ? this.ConformsTo.GetHashCode () : 0);
				hash = hash * 23 + this.DelegateMethods.GetHashValue ();
				hash = hash * 23 + (this.Description != null ? this.Description.GetHashCode () : 0);
				hash = hash * 23 + this.Methods.GetHashValue ();
				hash = hash * 23 + this.Properties.GetHashValue ();
				return hash;
			}
		}
	}
}