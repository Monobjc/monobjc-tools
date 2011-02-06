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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Generators
{
    /// <summary>
    ///   Code generator for method.
    /// </summary>
    public class FunctionGenerator : MethodGenerator
    {
        private const string SUFFIX_32 = "_32";
        private const string SUFFIX_64 = "_64";
        private const string SUFFIX_INNER = "_Inner";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "FunctionGenerator" /> class.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "statistics">The statistics.</param>
        public FunctionGenerator(StreamWriter writer, GenerationStatistics statistics) : base(writer, statistics) {}

        /// <summary>
        ///   Generates the specified entity.
        /// </summary>
        /// <param name = "typedEntity">The type entity.</param>
        /// <param name = "functionEntity">The function entity.</param>
        public void Generate(TypedEntity typedEntity, FunctionEntity functionEntity)
        {
            // Don't generate if required
            if (!functionEntity.Generate)
            {
                return;
            }

            // Append static condition if needed
            this.AppendStartCondition(functionEntity);

            // Append documentation
            this.AppendDocumentation(functionEntity, false);

            // Collect information
            bool needStorage = false;
            bool varargs = false;
            CollectInformations(functionEntity, ref needStorage, ref varargs);

            // Collect information on 32/64 bits invocations to check if they differ
            FunctionEntity functionEntity32 = DeriveFunctionEntity(functionEntity, false);
            FunctionEntity functionEntity64 = DeriveFunctionEntity(functionEntity, true);
            bool useMixedInvocation = !AreMethodTypesEqual(functionEntity32, functionEntity64);

            if (needStorage || useMixedInvocation)
            {
                if (useMixedInvocation)
                {
                    this.GenerateWrapperFunction(typedEntity, functionEntity, functionEntity32, functionEntity64, needStorage);
#if MIXED_MODE
                    this.GenerateNativeFunction(typedEntity, typedEntity, functionEntity64, SUFFIX_64, false);
#endif
                    this.GenerateNativeFunction(typedEntity, functionEntity32, SUFFIX_32, false);
                }
                else
                {
                    this.GenerateWrapperFunction(typedEntity, functionEntity, null, null, needStorage);

                    this.GenerateNativeFunction(typedEntity, functionEntity, SUFFIX_INNER, false);
                }
            }
            else
            {
                this.GenerateNativeFunction(typedEntity, functionEntity, null, true);
            }

            // Append static condition if needed
            this.AppendEndCondition(functionEntity);

            // Update statistics
            this.Statistics.Functions++;
        }

        private void GenerateWrapperFunction(TypedEntity typedEntity, FunctionEntity functionEntity, FunctionEntity functionEntity32, FunctionEntity functionEntity64, bool needStorage)
        {
            bool useMixedInvocation = functionEntity32 != null && functionEntity64 != null;

            // Strip name if the prefix is the same
            String name = functionEntity.Name;
            if (name.StartsWith(typedEntity.Name))
            {
                name = name.Substring(typedEntity.Name.Length);
            }

            StringBuilder signature = new StringBuilder();
            signature.AppendFormat("public static {0} {1}(", functionEntity.ReturnType, name);

            // Append parameters
            List<String> parameters = new List<String>();
            foreach (MethodParameterEntity methodParameterEntity in functionEntity.Parameters.Where(p => p.Generate))
            {
                parameters.Add(GetTypeSignature(methodParameterEntity));
            }
            signature.Append(String.Join(", ", parameters.ToArray()));
            signature.Append(")");
            this.Writer.WriteLineFormat(2, signature.ToString());
            this.Writer.WriteLineFormat(2, "{{");

            if (useMixedInvocation)
            {
#if MIXED_MODE
                this.Writer.WriteLineFormat(3, "if (ObjectiveCRuntime.Is64Bits)");
                this.Writer.WriteLineFormat(3, "{{");

                this.GenerateFunctionBody(4, typedEntity, functionEntity, functionEntity64, needStorage, SUFFIX_64);

                this.Writer.WriteLineFormat(3, "}}");
                this.Writer.WriteLineFormat(3, "else");
                this.Writer.WriteLineFormat(3, "{{");
#endif
                this.GenerateFunctionBody(4, typedEntity, functionEntity, functionEntity32, needStorage, SUFFIX_32);
#if MIXED_MODE
                this.Writer.WriteLineFormat(3, "}}");
#endif
            }
            else
            {
                this.GenerateFunctionBody(3, typedEntity, functionEntity, null, needStorage, SUFFIX_INNER);
            }

            this.Writer.WriteLineFormat(2, "}}");
            this.Writer.WriteLine();
        }

        protected void GenerateFunctionBody(int indent, TypedEntity typedEntity, FunctionEntity methodEntity, FunctionEntity innerMethodEntity, bool needStorage, String suffix = null)
        {
            bool hasReturn = !String.Equals(methodEntity.ReturnType, "void");
            bool mixed = (innerMethodEntity != null);
            bool mixedReturnType = mixed && !String.Equals(methodEntity.ReturnType, innerMethodEntity.ReturnType);

            this.GenerateLocalsAllocation(indent, methodEntity, innerMethodEntity);
            this.GenerateLocalsMarshalling(indent, methodEntity, innerMethodEntity);

            String invocation = GetFunctionInvocation(typedEntity, methodEntity, innerMethodEntity, suffix);
            if (hasReturn)
            {
                if (needStorage)
                {
                    String prefix = methodEntity.ReturnType + " __result = ";
                    if (mixedReturnType)
                    {
                        prefix += "(" + methodEntity.ReturnType + ") ";
                    }
                    invocation = prefix + invocation;
                }
                else
                {
                    String prefix = "return ";
                    if (mixedReturnType)
                    {
                        prefix += "(" + methodEntity.ReturnType + ") ";
                    }
                    invocation = prefix + invocation;
                }
            }
            this.Writer.WriteLineFormat(indent, invocation);

            this.GenerateLocalsUnmarshalling(indent, methodEntity, innerMethodEntity);
            this.GenerateLocalsDeallocation(indent, methodEntity, innerMethodEntity);

            if (hasReturn && needStorage)
            {
                this.Writer.WriteLineFormat(indent, "return __result;");
            }
        }

        private static string GetFunctionInvocation(TypedEntity typedEntity, FunctionEntity functionEntity, FunctionEntity innerFunctionEntity, String suffix = null)
        {
            // Strip name if the prefix is the same
            String name = functionEntity.Name;
            if (name.StartsWith(typedEntity.Name))
            {
                name = name.Substring(typedEntity.Name.Length);
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("{0}{1}(", name, suffix ?? String.Empty);
            builder.Append(GetMessageParameterList(functionEntity, innerFunctionEntity, false));
            builder.Append(");");

            return builder.ToString();
        }

        private void GenerateNativeFunction(TypedEntity typedEntity, FunctionEntity functionEntity, String suffix, bool isPublic)
        {
            // Strip name if the prefix is the same
            String name = functionEntity.Name;
            if (name.StartsWith(typedEntity.Name))
            {
                name = name.Substring(typedEntity.Name.Length);
            }

            // TODO: Compute proper framework path...
            // TODO: Test with embedded frameworks...
            this.Writer.WriteLineFormat(2, "[DllImport(\"{0}\", EntryPoint=\"{1}\")]", GetFrameworkPath(typedEntity.Namespace), functionEntity.Name);

            // Add custom tag for return type
            if (TypeManager.HasClass(functionEntity.ReturnType))
            {
                this.Writer.WriteLineFormat(2, "[return : MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof (IdMarshaler<{0}>))]", functionEntity.ReturnType);
            }

            StringBuilder signature = new StringBuilder();
            signature.AppendFormat("{0} static extern {1} {2}(", isPublic ? "public" : "private", functionEntity.ReturnType, name + suffix ?? String.Empty);

            // Append parameters
            List<String> parameters = new List<String>();
            foreach (MethodParameterEntity methodParameterEntity in functionEntity.Parameters.Where(p => p.Generate))
            {
                if (methodParameterEntity.IsOut || methodParameterEntity.IsByRef || methodParameterEntity.IsBlock)
                {
                    parameters.Add("IntPtr " + methodParameterEntity.Name);
                }
                else
                {
                    String parameter = GetTypeSignature(methodParameterEntity);
                    if (TypeManager.HasClass(methodParameterEntity.Type))
                    {
                        parameter = String.Format("[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof (IdMarshaler<{0}>))]", methodParameterEntity.Type) + " " + parameter;
                    }
                    parameters.Add(parameter);
                }
            }
            signature.Append(String.Join(", ", parameters.ToArray()));
            signature.Append(");");

            this.Writer.WriteLineFormat(2, signature.ToString());
            this.Writer.WriteLine();
        }

        private static String GetFrameworkPath(String framework)
        {
            return String.Format(CultureInfo.CurrentCulture, "/System/Library/Frameworks/{0}.framework/{0}", framework);
        }

        private static FunctionEntity DeriveFunctionEntity(FunctionEntity functionEntity, bool is64Bits)
        {
            FunctionEntity result = new FunctionEntity(functionEntity);

            result.ReturnType = GetRealType(result.ReturnType, is64Bits);
            foreach (MethodParameterEntity parameter in result.Parameters)
            {
                parameter.Type = GetRealType(parameter.Type, is64Bits);
            }

            return result;
        }
    }
}