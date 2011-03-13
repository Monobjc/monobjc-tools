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
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model.Entities
{
    /// <summary>
    ///   Represents the model for a property value.
    /// </summary>
    [Serializable]
    [XmlRoot("Property")]
    public class PropertyEntity : BaseEntity, IEquatable<PropertyEntity>
    {
        /// <summary>
        ///   Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlElement]
        public String Type { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "PropertyEntity" /> is static.
        /// </summary>
        /// <value><c>true</c> if static; otherwise, <c>false</c>.</value>
        [XmlAttribute]
        public bool Static { get; set; }

        /// <summary>
        ///   Gets or sets the getter.
        /// </summary>
        /// <value>The getter.</value>
        [XmlElement]
        public MethodEntity Getter { get; set; }

        /// <summary>
        ///   Gets or sets the setter.
        /// </summary>
        /// <value>The setter.</value>
        [XmlElement]
        public MethodEntity Setter { get; set; }

        /// <summary>
        ///   Convert this instance getter to a method entity.
        /// </summary>
        /// <returns>A method entity.</returns>
        public MethodEntity GetterAsMethodEntity()
        {
            MethodEntity methodEntity = this.Getter;
            methodEntity.MinAvailability = this.MinAvailability;
            methodEntity.MaxAvailability = this.MaxAvailability;
            methodEntity.Name = this.Name;
            methodEntity.ReturnType = this.Type;
            methodEntity.Static = this.Static;
            methodEntity.Summary = this.Summary;
            return methodEntity;
        }

        /// <summary>
        ///   Convert this instance setter to a method entity.
        /// </summary>
        /// <returns>A method entity.</returns>
        public MethodEntity SetterAsMethodEntity()
        {
            MethodEntity methodEntity = this.Setter;
            if (methodEntity != null)
            {
                methodEntity.MinAvailability = this.MinAvailability;
                methodEntity.MaxAvailability = this.MaxAvailability;
                methodEntity.Name = "Set" + this.Name;
                methodEntity.ReturnType = "void";
                MethodParameterEntity methodParameterEntity = new MethodParameterEntity();
                methodParameterEntity.Type = this.Type;
                methodParameterEntity.Name = "value";
                methodEntity.Parameters.Add(methodParameterEntity);
                methodEntity.Static = this.Static;
                methodEntity.Summary = this.Summary;
            }
            return methodEntity;
        }

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name = "other">An object to compare with this object.
        /// </param>
        public bool Equals(PropertyEntity other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (other.Static != this.Static)
            {
                return false;
            }
            /*
			if (!Equals(other.Getter, this.Getter))
			{
				return false;
			}
			if (!Equals(other.Setter, this.Setter))
			{
				return false;
			}
			*/
            return base.Equals(other);
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
            return this.Equals(obj as PropertyEntity);
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref = "T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ this.Static.GetHashCode();
                result = (result*397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(PropertyEntity left, PropertyEntity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PropertyEntity left, PropertyEntity right)
        {
            return !Equals(left, right);
        }
    }
}