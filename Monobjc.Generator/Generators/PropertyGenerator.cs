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
    ///   Code generator for properties.
    /// </summary>
    public class PropertyGenerator : BaseGenerator
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PropertyGenerator" /> class.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "statistics">The statistics.</param>
        public PropertyGenerator(StreamWriter writer, GenerationStatistics statistics)
            : base(writer, statistics)
        {
            this.MethodGenerator = new MethodGenerator(writer, statistics);
        }

        /// <summary>
        ///   Gets or sets the method generator.
        /// </summary>
        /// <value>The method generator.</value>
        private MethodGenerator MethodGenerator { get; set; }

        /// <summary>
        ///   Generates the specified entity.
        /// </summary>
        /// <param name = "classEntity">The class entity.</param>
        /// <param name = "propertyEntity">The property entity.</param>
        /// <param name = "implementation">if set to <c>true</c> generate the implementation.</param>
        public void Generate(ClassEntity classEntity, PropertyEntity propertyEntity, bool implementation = true, bool markedAsNew = false)
        {
            // Don't generate if required
            if (!propertyEntity.Generate)
            {
                return;
            }

            // Append static condition if needed
            this.AppendStartCondition(propertyEntity);

            // Append property comments
            this.AppendDocumentation(propertyEntity);

            // Append Obsolete attribute
            this.AppendObsoleteAttribute(propertyEntity);

            // Special case for delegates
            String type = propertyEntity.Type;
            if (String.Equals(propertyEntity.Name, "Delegate"))
            {
                type = "Id";
            }

            this.Writer.WriteLineFormat(2, "{0}{1} {2}",
										GetKeywords(propertyEntity, implementation, markedAsNew),
                                        type,
                                        propertyEntity.Name);

            String type32 = GetRealType(type, false);
            String type64 = GetRealType(type, true);
            bool useMixedInvocation = !String.Equals(type32, type64);

            // Append getter
            if (implementation)
            {
                this.Writer.WriteLineFormat(2, "{{");

//                if (useMixedInvocation)
//                {
//                    this.Writer.WriteLineFormat(3, "get {{");
//#if MIXED_MODE
//                    this.Writer.WriteLineFormat(4, "if (ObjectiveCRuntime.Is64Bits)");
//                    this.Writer.WriteLineFormat(4, "{{");
//
//                    this.Writer.WriteLineFormat(5, "return ({0}) ObjectiveCRuntime.SendMessage<{1}>({2}, \"{3}\");", type, GetRealType(type, true), propertyEntity.Static ? classEntity.Name + "Class" : "this",
//                                                "" + propertyEntity.Getter.Selector);
//
//                    this.Writer.WriteLineFormat(4, "}}");
//                    this.Writer.WriteLineFormat(4, "else");
//                    this.Writer.WriteLineFormat(4, "{{");
//#endif
//                    this.Writer.WriteLineFormat(5, "return ({0}) ObjectiveCRuntime.SendMessage<{1}>({2}, \"{3}\");", type, GetRealType(type, false), propertyEntity.Static ? classEntity.Name + "Class" : "this",
//                                                "" + propertyEntity.Getter.Selector);
//#if MIXED_MODE
//                    this.Writer.WriteLineFormat(4, "}}");
//#endif
//                    this.Writer.WriteLineFormat(3, "}}");
//                }
//                else
                {
                    this.Writer.WriteLineFormat(3, "get {{ return ObjectiveCRuntime.SendMessage<{0}>({1}, \"{2}\"); }}", type, propertyEntity.Static ? classEntity.Name + "Class" : "this", "" + propertyEntity.Getter.Selector);
                }

                // Append setter
                if (propertyEntity.Setter != null)
                {
//                    if (useMixedInvocation)
//                    {
//                        this.Writer.WriteLineFormat(3, "set {{");
//#if MIXED_MODE
//                        this.Writer.WriteLineFormat(4, "if (ObjectiveCRuntime.Is64Bits)");
//                        this.Writer.WriteLineFormat(4, "{{");
//
//                        this.Writer.WriteLineFormat(5, "ObjectiveCRuntime.SendMessage({0}, \"{1}\", ({2}) value);", propertyEntity.Static ? classEntity.Name + "Class" : "this", "" + propertyEntity.Setter.Selector, GetRealType(type, true));
//
//                        this.Writer.WriteLineFormat(4, "}}");
//                        this.Writer.WriteLineFormat(4, "else");
//                        this.Writer.WriteLineFormat(4, "{{");
//#endif
//                        this.Writer.WriteLineFormat(5, "ObjectiveCRuntime.SendMessage({0}, \"{1}\", ({2}) value);", propertyEntity.Static ? classEntity.Name + "Class" : "this", "" + propertyEntity.Setter.Selector, GetRealType(type, false));
//#if MIXED_MODE
//                        this.Writer.WriteLineFormat(4, "}}");
//#endif
//                        this.Writer.WriteLineFormat(3, "}}");
//                    }
//                    else
                    {
                        this.Writer.WriteLineFormat(3, "set {{ ObjectiveCRuntime.SendMessage({0}, \"{1}\", value); }}", propertyEntity.Static ? classEntity.Name + "Class" : "this", "" + propertyEntity.Setter.Selector);
                    }
                }

                this.Writer.WriteLineFormat(2, "}}");
            }
            else
            {
                this.Writer.WriteLineFormat(2, "{{");

                // Append getter
                this.Writer.WriteLineFormat(3, "[ObjectiveCMessage(\"{0}\")]", propertyEntity.Getter.Selector);
                this.Writer.WriteLineFormat(3, "get;");

                // Append setter
                if (propertyEntity.Setter != null)
                {
                    this.Writer.WriteLineFormat(3, "[ObjectiveCMessage(\"{0}\")]", propertyEntity.Setter.Selector);
                    this.Writer.WriteLineFormat(3, "set;");
                }

                this.Writer.WriteLineFormat(2, "}}");
            }

            // Append static condition if needed
            this.AppendEndCondition(propertyEntity);

            // Update statistics
            this.Statistics.Properties++;
        }

        /// <summary>
        ///   Appends the documentation.
        /// </summary>
        private void AppendDocumentation(PropertyEntity propertyEntity)
        {
            this.Writer.WriteLineFormat(2, "/// <summary>");
            foreach (String line in propertyEntity.Summary)
            {
                this.Writer.WriteLineFormat(2, "/// <para>{0}</para>", line.EscapeAll());
            }
            this.Writer.WriteLineFormat(2, "/// <para>Original signature is '{0}'</para>", propertyEntity.Getter.Signature);
            this.AppendAvailability(2, propertyEntity);
            this.Writer.WriteLineFormat(2, "/// </summary>");
        }
		
        private static String GetKeywords(PropertyEntity propertyEntity, bool implementation, bool markedAsNew)
        {
			String keywords = String.Empty;
            if (!implementation)
            {
                return keywords;
            }
			keywords = "public ";
			if (markedAsNew)
			{
				keywords += "new ";
			}
            if (propertyEntity.Static)
            {
                keywords += "static ";
            }
			else if (!markedAsNew)
			{
                keywords += "virtual ";
			}
            return keywords;
        }
    }
}