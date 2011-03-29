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
using System.IO;
using System.Linq;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Generators
{
    /// <summary>
    ///   Code generator for protocol.
    /// </summary>
    public class ProtocolGenerator : TypedGenerator
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ProtocolGenerator" /> class.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "statistics">The statistics.</param>
        public ProtocolGenerator(StreamWriter writer, GenerationStatistics statistics)
            : base(writer, statistics)
        {
            this.MethodGenerator = new MethodGenerator(writer, statistics);
            this.PropertyGenerator = new PropertyGenerator(writer, statistics);
        }

        /// <summary>
        ///   Gets or sets the method generator.
        /// </summary>
        /// <value>The method generator.</value>
        private MethodGenerator MethodGenerator { get; set; }

        /// <summary>
        ///   Gets or sets the property generator.
        /// </summary>
        /// <value>The property generator.</value>
        private PropertyGenerator PropertyGenerator { get; set; }

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
            this.Writer.WriteLineFormat(1, "[ObjectiveCProtocol(\"{0}\")]", classEntity.Name);
#if GENERATED_ATTRIBUTE
            this.Writer.WriteLineFormat(1, "[GeneratedCodeAttribute(\"{0}\", \"{1}\")]", GenerationHelper.ToolName, GenerationHelper.ToolVersion);
#endif
            if (string.IsNullOrEmpty(classEntity.BaseType))
            {
                this.Writer.WriteLineFormat(1, "public partial interface I{0} : {1}", classEntity.Name, "IManagedWrapper");
            }
            else
            {
                this.Writer.WriteLineFormat(1, "public partial interface I{0} : I{1}", classEntity.Name, classEntity.BaseType);
            }
            this.Writer.WriteLineFormat(1, "{{");

            // Append methods
            foreach (MethodEntity methodEntity in classEntity.Methods.Where(e => e.Generate))
            {
                if (methodEntity.Static)
                {
                    continue;
                }
                this.MethodGenerator.Generate(classEntity, methodEntity, false, false);
                this.Writer.WriteLine();
            }

            // Append properties
            foreach (PropertyEntity propertyEntity in classEntity.Properties.Where(e => e.Generate))
            {
                if (propertyEntity.Static)
                {
                    continue;
                }
                this.PropertyGenerator.Generate(classEntity, propertyEntity, false);
                this.Writer.WriteLine();
            }

            // Append class ender
            this.Writer.WriteLineFormat(1, "}}");

            // Append static condition if needed
            this.AppendEndCondition(classEntity);

            // Append namespace ender
            this.Writer.WriteLineFormat(0, "}}");

            // Update statistics
            this.Statistics.Protocols++;
        }
    }
}