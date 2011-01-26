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
using System.IO;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Generators
{
    /// <summary>
    ///   Code generator for protocol.
    /// </summary>
    public class EnumerationGenerator : TypedGenerator
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "EnumerationGenerator" /> class.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "statistics">The statistics.</param>
        public EnumerationGenerator(StreamWriter writer, GenerationStatistics statistics) : base(writer, statistics) {}

        /// <summary>
        ///   Generates the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        public override void Generate(BaseEntity entity)
        {
            this.Generate(entity, true);
        }

        /// <summary>
        ///   Generates the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "standlone">if set to <c>true</c>, generate a standlone file.</param>
        public void Generate(BaseEntity entity, bool standlone)
        {
            // Don't generate if required
            if (!entity.Generate)
            {
                return;
            }

            EnumerationEntity enumerationEntity = (EnumerationEntity) entity;

            if (standlone)
            {
                // Append License
                this.Writer.WriteLineFormat(0, License);

                // Append usings
                this.AppendStandardNamespaces();

                // Append namespace starter
                this.Writer.WriteLineFormat(0, "namespace Monobjc.{0}", enumerationEntity.Namespace);
                this.Writer.WriteLineFormat(0, "{{");
            }

            // Append static condition if needed
            this.AppendStartCondition(entity);

            // Append class starter
            this.Writer.WriteLineFormat(1, "/// <summary>");
            foreach (String line in enumerationEntity.Summary)
            {
                this.Writer.WriteLineFormat(1, "/// <para>{0}</para>", line);
            }
            this.AppendAvailability(1, enumerationEntity);
            this.Writer.WriteLineFormat(1, "/// </summary>");

            // If the type is a mixed type, then output the attributes
            String type32 = GetRealType(enumerationEntity.BaseType, false);
            String type64 = GetRealType(enumerationEntity.BaseType, true);
            if (!String.Equals(type32, type64))
            {
#if MIXED_MODE
                this.Writer.WriteLineFormat(1, "[ObjectiveCUnderlyingTypeAttribute(typeof({0}), Is64Bits = false)]", type32);
                this.Writer.WriteLineFormat(1, "[ObjectiveCUnderlyingTypeAttribute(typeof({0}), Is64Bits = true)]", type64);
#endif
            }

#if GENERATED_ATTRIBUTE
            this.Writer.WriteLineFormat(1, "[GeneratedCodeAttribute(\"{0}\", \"{1}\")]", GenerationHelper.ToolName, GenerationHelper.ToolVersion);
#endif

            if (enumerationEntity.Flags)
            {
                this.Writer.WriteLineFormat(1, "[Flags]");
            }

            // Append enumeration declaration
            this.Writer.WriteLineFormat(1, "public enum {0} : {1}", enumerationEntity.Name, type32);
            this.Writer.WriteLineFormat(1, "{{");

            // Append methods
            foreach (EnumerationValueEntity enumerationValueEntity in enumerationEntity.Values)
            {
                // Don't generate if required
                if (!enumerationValueEntity.Generate)
                {
                    continue;
                }

                // Append static condition if needed
                this.AppendStartCondition(enumerationValueEntity);

                this.Writer.WriteLineFormat(2, "/// <summary>");
                foreach (String line in enumerationValueEntity.Summary)
                {
                    this.Writer.WriteLineFormat(2, "/// <para>{0}</para>", line);
                }
                this.AppendAvailability(2, enumerationValueEntity);
                this.Writer.WriteLineFormat(2, "/// </summary>");
                if (String.IsNullOrEmpty(enumerationValueEntity.Value))
                {
                    this.Writer.WriteLineFormat(2, "{0},", enumerationValueEntity.Name);
                }
                else
                {
                    String value = enumerationValueEntity.Value;
                    value = value.Replace("&lt;&lt;", "<<");
                    this.Writer.WriteLineFormat(2, "{0} = {1},", enumerationValueEntity.Name, value);
                }

                // Append static condition if needed
                this.AppendEndCondition(enumerationValueEntity);
            }

            // Append class ender
            this.Writer.WriteLineFormat(1, "}}");

            // Append static condition if needed
            this.AppendEndCondition(entity);

            if (standlone)
            {
                // Append namespace ender
                this.Writer.WriteLineFormat(0, "}}");
            }

            // Update statistics
            this.Statistics.Enumerations++;
        }
    }
}