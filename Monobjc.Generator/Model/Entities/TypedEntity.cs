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
    ///   This class serves as a base for all the elements that have a namespace (classes, interfaces, etc).
    /// </summary>
    [Serializable]
    [XmlRoot("Type")]
    public class TypedEntity : BaseEntity
    {
        /// <summary>
        ///   This entity represents a class.
        /// </summary>
        public const String CLASS_NATURE = "C";

        /// <summary>
        ///   This entity represents a protocol.
        /// </summary>
        public const String PROTOCOL_NATURE = "P";

        /// <summary>
        ///   This entity represents an enumeration.
        /// </summary>
        public const String ENUMERATION_NATURE = "E";

        /// <summary>
        ///   This entity represents a structure.
        /// </summary>
        public const String STRUCTURE_NATURE = "S";

        /// <summary>
        ///   This entity represents a type.
        /// </summary>
        public const String TYPE_NATURE = "T";

        private string baseType;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TypedEntity" /> class.
        /// </summary>
        public TypedEntity()
        {
            this.Enumerations = new List<EnumerationEntity>();
            this.Constants = new List<ConstantEntity>();
            this.Functions = new List<FunctionEntity>();
        }

        /// <summary>
        ///   Gets or sets the nature.
        /// </summary>
        /// <value>The nature.</value>
        [XmlAttribute("nature")]
        public String Nature { get; set; }

        /// <summary>
        ///   Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        [XmlElement]
        public String Namespace { get; set; }

        /// <summary>
        ///   Gets or sets the type of the base.
        /// </summary>
        /// <value>The type of the base.</value>
        [XmlElement]
        public String BaseType
        {
            get { return this.baseType; }
            set
            {
                this.baseType = value;
                switch (this.baseType)
                {
                    case "NSInteger":
                        this.MixedType = "int,long";
                        break;
                    case "NSUInteger":
                        this.MixedType = "uint,ulong";
                        break;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the types to use in 32/64 bits.
        /// </summary>
        /// <value>The types as comma separated.</value>
        [XmlElement]
        public String MixedType { get; set; }

        /// <summary>
        ///   Gets or sets the enumerations.
        /// </summary>
        /// <value>The enumerations.</value>
        [XmlArray]
        [XmlArrayItem(typeof (EnumerationEntity), ElementName = "Enumeration")]
        public List<EnumerationEntity> Enumerations { get; set; }

        /// <summary>
        ///   Gets or sets the constants.
        /// </summary>
        /// <value>The constants.</value>
        [XmlArray]
        [XmlArrayItem(typeof (ConstantEntity), ElementName = "Constant")]
        public List<ConstantEntity> Constants { get; set; }

        /// <summary>
        ///   Gets or sets the functions.
        /// </summary>
        /// <value>The functions.</value>
        [XmlArray]
        [XmlArrayItem(typeof (FunctionEntity), ElementName = "Function")]
        public List<FunctionEntity> Functions { get; set; }

        /// <summary>
        ///   Gets a value indicating whether this instance has constants.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has constants; otherwise, <c>false</c>.
        /// </value>
        public bool HasConstants
        {
            get { return (this.Constants.Count > 0); }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance has enumerations.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has enumerations; otherwise, <c>false</c>.
        /// </value>
        public bool HasEnumerations
        {
            get { return (this.Enumerations.Count > 0); }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance has functions.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has functions; otherwise, <c>false</c>.
        /// </value>
        public bool HasFunctions
        {
            get { return (this.Functions.Count > 0); }
        }

        /// <summary>
        ///   Adds the entities.
        /// </summary>
        /// <param name = "entities">The entities.</param>
        public virtual void AddRange(List<BaseEntity> entities)
        {
            foreach (BaseEntity entity in entities)
            {
                EnumerationEntity enumerationEntity = entity as EnumerationEntity;
                if (enumerationEntity != null)
                {
                    if (this.Enumerations.Contains(enumerationEntity))
                    {
                        continue;
                    }
                    this.Enumerations.Add(enumerationEntity);
                }
                ConstantEntity constantEntity = entity as ConstantEntity;
                if (constantEntity != null)
                {
                    if (this.Constants.Contains(constantEntity))
                    {
                        continue;
                    }
                    this.Constants.Add(constantEntity);
                }
                FunctionEntity functionEntity = entity as FunctionEntity;
                if (functionEntity != null)
                {
                    this.Functions.Add(functionEntity);
                }
            }
        }
    }
}