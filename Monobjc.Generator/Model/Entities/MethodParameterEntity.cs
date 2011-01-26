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
    ///   Represents the model for a method's parameter.
    /// </summary>
    [Serializable]
    [XmlRoot("Parameter")]
    public class MethodParameterEntity : BaseEntity
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "MethodParameterEntity" /> class.
        /// </summary>
        public MethodParameterEntity() {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MethodParameterEntity" /> class.
        /// </summary>
        /// <param name = "methodParameterEntity">The method parameter entity.</param>
        public MethodParameterEntity(MethodParameterEntity methodParameterEntity) : this()
        {
            this.MinAvailability = methodParameterEntity.MinAvailability;
            this.Generate = methodParameterEntity.Generate;
            this.IsBlock = methodParameterEntity.IsBlock;
            this.IsByRef = methodParameterEntity.IsByRef;
            this.IsOut = methodParameterEntity.IsOut;
            this.Name = methodParameterEntity.Name;
            this.Summary = new List<string>(methodParameterEntity.Summary);
            this.Type = methodParameterEntity.Type;
        }

        /// <summary>
        ///   Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlElement]
        public String Type { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is out.
        /// </summary>
        /// <value><c>true</c> if this instance is out; otherwise, <c>false</c>.</value>
        [XmlAttribute]
        public bool IsOut { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is by ref.
        /// </summary>
        /// <value><c>true</c> if this instance is by ref; otherwise, <c>false</c>.</value>
        [XmlAttribute]
        public bool IsByRef { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is a block.
        /// </summary>
        /// <value><c>true</c> if this instance is a block; otherwise, <c>false</c>.</value>
        [XmlAttribute]
        public bool IsBlock { get; set; }
    }
}