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
    ///   Base class for typed entity (class or protocol).
    /// </summary>
    public class TypedGenerator : BaseGenerator
    {
        private ConstantGenerator constantGenerator;
        private FunctionGenerator functionGenerator;
        private EnumerationGenerator enumerationGenerator;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TypedGenerator" /> class.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "statistics">The statistics.</param>
        public TypedGenerator(StreamWriter writer, GenerationStatistics statistics)
            : base(writer, statistics) {}

        /// <summary>
        ///   Gets or sets the constant generator.
        /// </summary>
        /// <value>The constant generator.</value>
        protected ConstantGenerator ConstantGenerator
        {
            get
            {
                if (this.constantGenerator == null)
                {
                    this.constantGenerator = new ConstantGenerator(this.Writer, this.Statistics);
                }
                return this.constantGenerator;
            }
        }

        /// <summary>
        ///   Gets or sets the function generator.
        /// </summary>
        /// <value>The function generator.</value>
        protected FunctionGenerator FunctionGenerator
        {
            get
            {
                if (this.functionGenerator == null)
                {
                    this.functionGenerator = new FunctionGenerator(this.Writer, this.Statistics);
                }
                return this.functionGenerator;
            }
        }

        /// <summary>
        ///   Gets or sets the enumeration generator.
        /// </summary>
        /// <value>The enumeration generator.</value>
        protected EnumerationGenerator EnumerationGenerator
        {
            get
            {
                if (this.enumerationGenerator == null)
                {
                    this.enumerationGenerator = new EnumerationGenerator(this.Writer, this.Statistics);
                }
                return this.enumerationGenerator;
            }
        }

        /// <summary>
        ///   Generates the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        public virtual void Generate(BaseEntity entity)
        {
            TypedEntity typedEntity = (TypedEntity) entity;

            // Append License
            this.Writer.WriteLineFormat(0, License);

            // Append usings
            this.AppendStandardNamespaces();

            // Append namespace starter
            this.Writer.WriteLineFormat(0, "namespace Monobjc.{0}", typedEntity.Namespace);
            this.Writer.WriteLineFormat(0, "{{");

            if (typedEntity.HasConstants || typedEntity.HasFunctions)
            {
                // Append static condition if needed
                this.AppendStartCondition(typedEntity);

                // Append class starter
#if GENERATED_ATTRIBUTE
                //this.Writer.WriteLineFormat(1, "[GeneratedCodeAttribute(\"{0}\", \"{1}\")]", this.ToolName, this.ToolVersion);
#endif
                this.Writer.WriteLineFormat(1, "public static partial class {0}", typedEntity.Name.Split('.')[0]);
                this.Writer.WriteLineFormat(1, "{{");

                // Output the constants
                foreach (ConstantEntity constantEntity in typedEntity.Constants.Where(e => e.Generate))
                {
                    this.ConstantGenerator.Generate(typedEntity.Namespace, constantEntity);
                    this.Writer.WriteLine();
                }

                // Output the functions
                foreach (FunctionEntity functionEntity in typedEntity.Functions.Where(e => e.Generate))
                {
                    this.FunctionGenerator.Generate(typedEntity, functionEntity);
                    this.Writer.WriteLine();
                }

                // Append class ender
                this.Writer.WriteLineFormat(1, "}}");

                // Append static condition if needed
                this.AppendEndCondition(typedEntity);
            }

            // Output the enumerations
            foreach (EnumerationEntity enumerationEntity in typedEntity.Enumerations.Where(e => e.Generate))
            {
                this.Writer.WriteLine();
                this.EnumerationGenerator.Generate(enumerationEntity, false);
            }

            // Append namespace ender
            this.Writer.WriteLineFormat(0, "}}");

            // Update statistics
            this.Statistics.Types++;
        }
    }
}