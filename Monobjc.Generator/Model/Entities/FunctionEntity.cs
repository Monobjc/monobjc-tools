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

namespace Monobjc.Tools.Generator.Model.Entities
{
    /// <summary>
    ///   Represents the model for a method.
    /// </summary>
    [Serializable]
    [XmlRoot("Function")]
    public class FunctionEntity : MethodEntity
    {
        public FunctionEntity() {}

        public FunctionEntity(FunctionEntity functionEntity) : this()
        {
            this.MinAvailability = functionEntity.MinAvailability;
            this.Generate = functionEntity.Generate;
            this.Name = functionEntity.Name;
            this.ReturnsDocumentation = functionEntity.ReturnsDocumentation;
            this.ReturnType = functionEntity.ReturnType;
            this.Selector = functionEntity.Selector;
            this.Signature = functionEntity.Signature;
            this.Static = functionEntity.Static;
            this.Summary = new List<String>(functionEntity.Summary);

            foreach (MethodParameterEntity methodParameterEntity in functionEntity.Parameters)
            {
                MethodParameterEntity parameter = new MethodParameterEntity(methodParameterEntity);
                this.Parameters.Add(parameter);
            }

            this.GenerateConstructor = functionEntity.GenerateConstructor;
            this.SharedLibrary = functionEntity.SharedLibrary;
            this.EntryPoint = functionEntity.EntryPoint;
        }

        /// <summary>
        ///   Gets or sets the shared library.
        /// </summary>
        /// <value>The shared library.</value>
        public String SharedLibrary { get; set; }

        /// <summary>
        ///   Gets or sets the entry point.
        /// </summary>
        /// <value>The entry point.</value>
        public String EntryPoint { get; set; }
    }
}