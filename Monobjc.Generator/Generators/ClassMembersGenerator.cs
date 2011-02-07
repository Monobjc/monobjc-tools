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
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Generators
{
    /// <summary>
    ///   Code generator for class's members.
    /// </summary>
    public class ClassMembersGenerator : ClassGenerator
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ClassMembersGenerator" /> class.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "statistics">The statistics.</param>
        public ClassMembersGenerator(StreamWriter writer, GenerationStatistics statistics) : base(writer, statistics) {}

        /// <summary>
        ///   Generates the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        public override void Generate(BaseEntity entity)
        {
            ClassEntity classEntity = (ClassEntity) entity;

            // Append License
            this.Writer.WriteLineFormat(0, License);

            // Append usings
            this.AppendStandardNamespaces();

            // Append namespace starter
            this.Writer.WriteLineFormat(0, "namespace Monobjc.{0}", classEntity.Namespace);
            this.Writer.WriteLineFormat(0, "{{");

            // Append static condition if needed
            this.AppendStartCondition(classEntity);

            // Append class starter
            this.Writer.WriteLineFormat(1, "public partial class {0}", classEntity.Name);
            this.Writer.WriteLineFormat(1, "{{");

            // Collect all the existing methods
            IEnumerable<MethodEntity> methods = GetAllMethods(classEntity, false);

            // Collect all the existing properties
            IEnumerable<PropertyEntity> properties = GetProperties(classEntity, false);

            // Append methods
            foreach (MethodEntity methodEntity in classEntity.Methods.Where(e => e.Generate))
            {
                if (methods.Contains(methodEntity))
                {
                    continue;
                }
                this.MethodGenerator.Generate(classEntity, methodEntity, true, false);
                this.Writer.WriteLine();
            }

            // Append methods that need to be copy from the parent
            foreach (MethodEntity methodEntity in methods.Where(e => e.CopyInDescendants))
            {
                if (classEntity.Methods.Contains(methodEntity))
                {
                    continue;
                }
                methodEntity.ReturnType = classEntity.Name;
                this.MethodGenerator.Generate(classEntity, methodEntity, true, false, true);
                this.Writer.WriteLine();
            }

            // Append properties
            foreach (PropertyEntity propertyEntity in classEntity.Properties.Where(e => e.Generate))
            {
                if (properties.Contains(propertyEntity))
                {
                    continue;
                }
                this.PropertyGenerator.Generate(classEntity, propertyEntity);
                this.Writer.WriteLine();
            }

            // Append properties that need to be copy from the parent
            foreach (PropertyEntity propertyEntity in properties.Where(e => e.CopyInDescendants))
            {
                if (classEntity.Properties.Contains(propertyEntity))
                {
                    continue;
                }
                propertyEntity.Type = classEntity.Name;
                this.PropertyGenerator.Generate(classEntity, propertyEntity, true, true);
                this.Writer.WriteLine();
            }

            // Output the functions
            foreach (FunctionEntity functionEntity in classEntity.Functions.Where(e => e.Generate))
            {
                this.FunctionGenerator.Generate(classEntity, functionEntity);
                this.Writer.WriteLine();
            }

            // Append class ender
            this.Writer.WriteLineFormat(1, "}}");

            // Append static condition if needed
            this.AppendEndCondition(classEntity);

            // Append namespace ender
            this.Writer.WriteLineFormat(0, "}}");
        }
    }
}