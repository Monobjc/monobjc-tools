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
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model.Entities
{
    /// <summary>
    ///   Represents the model for a method.
    /// </summary>
    [Serializable]
    [XmlRoot("Method")]
    public class MethodEntity : BaseEntity, IEquatable<MethodEntity>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "MethodEntity" /> class.
        /// </summary>
        public MethodEntity()
        {
            this.Parameters = new List<MethodParameterEntity>();
            this.GenerateConstructor = true;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MethodEntity" /> class.
        /// </summary>
        /// <param name = "methodEntity">The method entity.</param>
        public MethodEntity(MethodEntity methodEntity) : this()
        {
            this.MinAvailability = methodEntity.MinAvailability;
            this.Generate = methodEntity.Generate;
            this.Name = methodEntity.Name;
            this.ReturnsDocumentation = methodEntity.ReturnsDocumentation;
            this.ReturnType = methodEntity.ReturnType;
            this.Selector = methodEntity.Selector;
            this.Signature = methodEntity.Signature;
            this.Static = methodEntity.Static;
            this.Summary = new List<string>(methodEntity.Summary);

            foreach (MethodParameterEntity methodParameterEntity in methodEntity.Parameters)
            {
                MethodParameterEntity parameter = new MethodParameterEntity(methodParameterEntity);
                this.Parameters.Add(parameter);
            }

            this.GenerateConstructor = methodEntity.GenerateConstructor;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "MethodEntity" /> is static.
        /// </summary>
        /// <value><c>true</c> if static; otherwise, <c>false</c>.</value>
        [XmlAttribute]
        public bool Static { get; set; }

        /// <summary>
        ///   Gets or sets the selector.
        /// </summary>
        /// <value>The selector.</value>
        [XmlElement]
        public String Selector { get; set; }

        /// <summary>
        ///   Gets or sets the signature.
        /// </summary>
        /// <value>The signature.</value>
        [XmlElement]
        public String Signature { get; set; }

        /// <summary>
        ///   Gets or sets the type of the return.
        /// </summary>
        /// <value>The type of the return.</value>
        [XmlElement]
        public String ReturnType { get; set; }

        /// <summary>
        ///   Gets or sets the returns documentation.
        /// </summary>
        /// <value>The returns documentation.</value>
        [XmlElement]
        public String ReturnsDocumentation { get; set; }

        /// <summary>
        ///   Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        [XmlArray]
        [XmlArrayItem(typeof (MethodParameterEntity), ElementName = "Parameter")]
        public List<MethodParameterEntity> Parameters { get; set; }

        /// <summary>
        ///   Gets or sets whether this entity must be generated as constructor.
        /// </summary>
        /// <value><code>true</code> to generate as constructor; <code>false</code> otherwise.</value>
        [XmlAttribute]
        public bool GenerateConstructor { get; set; }

        /// <summary>
        ///   Gets a value indicating whether this instance is a constructor.
        /// </summary>
        /// <value><c>true</c> if this instance is a getter; otherwise, <c>false</c>.</value>
        public bool IsConstructor
        {
            get
            {
                bool result = this.GenerateConstructor;
                result &= this.Name.StartsWith("InitWith", StringComparison.OrdinalIgnoreCase);
                return result;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is a getter.
        /// </summary>
        /// <value><c>true</c> if this instance is a getter; otherwise, <c>false</c>.</value>
        public bool IsGetter
        {
            get
            {
                bool result = true;
                result &= !String.Equals(this.ReturnType, "void", StringComparison.OrdinalIgnoreCase);
                result &= (this.Parameters.Count == 0);
                return result;
            }
        }

        /// <summary>
        ///   Determines whether the specified method is setter for this instance.
        /// </summary>
        /// <param name = "method">The method.</param>
        /// <returns>
        ///   <c>true</c> if the specified method is setter for this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSetterFor(MethodEntity method)
        {
            bool result = true;
            result &= String.Equals(this.ReturnType, "void", StringComparison.InvariantCultureIgnoreCase);
            result &= (this.Parameters.Count == 1);
            result &= (method.Static == this.Static);

            String name = method.Name;
            if (String.Equals(method.ReturnType, "bool", StringComparison.InvariantCultureIgnoreCase))
            {
                name = name.StartsWith("Is") ? name.Substring(2) : name;
                name = name.StartsWith("Has") ? name.Substring(3) : name;
            }
            name = "Set" + name;
            result &= String.Equals(this.Name, name, StringComparison.InvariantCultureIgnoreCase);

            return result;
        }


        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name = "other">An object to compare with this object.
        /// </param>
        public bool Equals(MethodEntity other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            bool result = base.Equals(other) &&
                          other.Static.Equals(this.Static) &&
                          Equals(other.ReturnType, this.ReturnType);
            result &= (other.Parameters.Count == this.Parameters.Count);
            if (result)
            {
                for (int i = 0; i < this.Parameters.Count; i++)
                {
                    result &= Equals(other.Parameters[i], this.Parameters[i]);
                }
            }
            return result;
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
            return this.Equals(obj as MethodEntity);
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
                result = (result*397) ^ (this.ReturnType != null ? this.ReturnType.GetHashCode() : 0);
                foreach (MethodParameterEntity methodParameterEntity in this.Parameters)
                {
                    result = (result*397) ^ methodParameterEntity.GetHashCode();
                }
                return result;
            }
        }

        public static bool operator ==(MethodEntity left, MethodEntity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MethodEntity left, MethodEntity right)
        {
            return !Equals(left, right);
        }
    }
}