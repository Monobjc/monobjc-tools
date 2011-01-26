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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Generators
{
    /// <summary>
    ///   Base class for class entity.
    /// </summary>
    public abstract class ClassGenerator : TypedGenerator
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ClassGenerator" /> class.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "statistics">The statistics.</param>
        protected ClassGenerator(StreamWriter writer, GenerationStatistics statistics) : base(writer, statistics)
        {
            this.MethodGenerator = new MethodGenerator(writer, statistics);
            this.PropertyGenerator = new PropertyGenerator(writer, statistics);
        }

        /// <summary>
        ///   Gets or sets the method generator.
        /// </summary>
        /// <value>The method generator.</value>
        protected MethodGenerator MethodGenerator { get; private set; }

        /// <summary>
        ///   Gets or sets the property generator.
        /// </summary>
        /// <value>The property generator.</value>
        protected PropertyGenerator PropertyGenerator { get; private set; }

        /// <summary>
        ///   Gets all methods of the superclass and own.
        /// </summary>
        protected static IEnumerable<MethodEntity> GetAllMethods(ClassEntity classEntity, bool withOwn)
        {
            List<MethodEntity> methods = (classEntity.SuperClass != null) ? classEntity.SuperClass.GetMethods(true, true).ToList() : new List<MethodEntity>();
            if (withOwn)
            {
                methods.AddRange(classEntity.Methods);
            }
            return methods;
        }

        /// <summary>
        ///   Gets the properties of the superclass and own.
        /// </summary>
        protected static IEnumerable<PropertyEntity> GetProperties(ClassEntity classEntity, bool withOwn)
        {
            List<PropertyEntity> properties = (classEntity.SuperClass != null) ? classEntity.SuperClass.GetProperties(true, true).ToList() : new List<PropertyEntity>();
            if (withOwn)
            {
                properties.AddRange(classEntity.Properties);
            }
            return properties;
        }
    }
}