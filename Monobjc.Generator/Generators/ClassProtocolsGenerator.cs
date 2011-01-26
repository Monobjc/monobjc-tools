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
    public class ClassProtocolsGenerator : ClassGenerator
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ClassMembersGenerator" /> class.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "statistics">The statistics.</param>
        public ClassProtocolsGenerator(StreamWriter writer, GenerationStatistics statistics) : base(writer, statistics) {}

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
            this.Writer.WriteLineFormat(1, "public partial class {0} : ", classEntity.Name);

            // Sort protocols and emit them in order
            classEntity.Protocols.Sort((p1, p2) => p1.MinAvailability.CompareTo(p2.MinAvailability));
            for (int i = 0; i < classEntity.Protocols.Count; i++)
            {
                ClassEntity protocolEntity = classEntity.Protocols[i];

                this.AppendStartCondition(protocolEntity);
                this.Writer.WriteLineFormat(1, (i == 0) ? "I{0}" : ", I{0}", protocolEntity.Name);
                this.AppendEndCondition(protocolEntity);
            }

            this.Writer.WriteLineFormat(1, "{{");

            // Collect all the existing methods
            List<MethodEntity> methods = GetAllMethods(classEntity, true).ToList();

            // Collect all the existing properties
            List<PropertyEntity> properties = GetProperties(classEntity, true).ToList();

            // Implement each protocol
            foreach (ClassEntity protocolEntity in classEntity.Protocols)
            {
                // Append static condition if needed
                this.AppendStartCondition(protocolEntity);

                this.Writer.WriteLineFormat(2, "#region ----- " + protocolEntity.Name + " -----");
                this.Writer.WriteLine();

                // Append methods
                foreach (MethodEntity methodEntity in protocolEntity.Methods.Where(e => e.Generate))
                {
                    if (methods.Contains(methodEntity))
                    {
                        continue;
                    }
                    this.MethodGenerator.Generate(classEntity, methodEntity, true, false);
                    this.Writer.WriteLine();
                }

                // Append properties
                foreach (PropertyEntity propertyEntity in protocolEntity.Properties.Where(e => e.Generate))
                {
                    if (properties.Contains(propertyEntity))
                    {
                        continue;
                    }
                    this.PropertyGenerator.Generate(classEntity, propertyEntity);
                    this.Writer.WriteLine();
                }

                this.Writer.WriteLineFormat(2, "#endregion");
                this.Writer.WriteLine();

                // Append static condition if needed
                this.AppendEndCondition(protocolEntity);

                // Add methods so they are now ignored
                methods.AddRange(protocolEntity.Methods);
                properties.AddRange(protocolEntity.Properties);
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