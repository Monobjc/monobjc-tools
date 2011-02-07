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
    public class MethodGenerator : BaseGenerator
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "MethodGenerator" /> class.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "statistics">The statistics.</param>
        public MethodGenerator(StreamWriter writer, GenerationStatistics statistics) : base(writer, statistics) {}

        /// <summary>
        ///   Generates the specified entity.
        /// </summary>
        /// <param name = "classEntity">The class entity.</param>
        /// <param name = "methodEntity">The method entity.</param>
        /// <param name = "implementation">if set to <c>true</c> generate the implementation.</param>
        /// <param name = "extension">if set to <c>true</c> this method is an extension method.</param>
        public void Generate(ClassEntity classEntity, MethodEntity methodEntity, bool implementation = true, bool extension = false, bool markedAsNew = false)
        {
            // Don't generate if required
            if (!methodEntity.Generate)
            {
                return;
            }

            // Append static condition if needed
            this.AppendStartCondition(methodEntity);

            // Append documentation
            this.AppendDocumentation(methodEntity, extension && !methodEntity.Static);

            // Append method selector
            if (!implementation)
            {
                this.Writer.WriteLineFormat(2, "[ObjectiveCMessage(\"{0}\")]", methodEntity.Selector);
            }

            // Append Obsolete attribute
            this.AppendObsoleteAttribute(methodEntity);

            // Create Method signature
            StringBuilder signature = new StringBuilder();

            // Append keywords
            String keywords = GetKeywords(methodEntity, implementation, extension, markedAsNew);
            signature.Append(keywords);

            // Append return type and name
            signature.AppendFormat("{0} {1}", methodEntity.ReturnType, methodEntity.Name);
            signature.Append("(");

            // Append parameters
            List<String> parameters = new List<String>();
            String extraParameter = GetExtraParameter(classEntity, methodEntity, extension);
            if (!String.IsNullOrEmpty(extraParameter))
            {
                parameters.Add(extraParameter);
            }
            foreach (MethodParameterEntity methodParameterEntity in methodEntity.Parameters.Where(p => p.Generate))
            {
                parameters.Add(GetTypeSignature(methodParameterEntity));
            }
            signature.Append(String.Join(", ", parameters.ToArray()));
            signature.Append(")");
            signature.Append(implementation ? String.Empty : ";");

            this.Writer.WriteLineFormat(2, signature.ToString());

            // Write implementation
            if (implementation)
            {
                // Collect information
                bool needStorage = false;
                bool varargs = false;
                CollectInformations(methodEntity, ref needStorage, ref varargs);

                // Collect information on 32/64 bits invocations to check if they differ
                MethodEntity methodEntity32 = DeriveMethodEntity(methodEntity, false);
                MethodEntity methodEntity64 = DeriveMethodEntity(methodEntity, true);
                bool useMixedInvocation = !AreMethodTypesEqual(methodEntity32, methodEntity64);

                this.Writer.WriteLineFormat(2, "{{");

                String target = GetTarget(classEntity, methodEntity, extension);

                if (useMixedInvocation)
                {
#if MIXED_MODE
                    this.Writer.WriteLineFormat(3, "if (ObjectiveCRuntime.Is64Bits)");
                    this.Writer.WriteLineFormat(3, "{{");

                    this.GenerateMethodBody(4, target, methodEntity, methodEntity64, needStorage, varargs);

                    this.Writer.WriteLineFormat(3, "}}");
                    this.Writer.WriteLineFormat(3, "else");
                    this.Writer.WriteLineFormat(3, "{{");
#endif
                    this.GenerateMethodBody(4, target, methodEntity, methodEntity32, needStorage, varargs);
#if MIXED_MODE
                    this.Writer.WriteLineFormat(3, "}}");
#endif
                }
                else
                {
                    this.GenerateMethodBody(3, target, methodEntity, null, needStorage, varargs);
                }

                this.Writer.WriteLineFormat(2, "}}");
            }

            // Append static condition if needed
            this.AppendEndCondition(methodEntity);

            // Update statistics
            this.Statistics.Methods++;
        }

        public void GenerateConstructor(ClassEntity classEntity, MethodEntity methodEntity)
        {
            // Don't generate if required
            if (!methodEntity.Generate)
            {
                return;
            }

            // Clone the method and change the return type
            methodEntity = new MethodEntity(methodEntity);
            methodEntity.ReturnType = "IntPtr";

            // Collect information
            bool needStorage = false;
            bool varargs = false;
            CollectInformations(methodEntity, ref needStorage, ref varargs);

            // Don't generate if varargs
            if (varargs)
            {
                return;
            }

            // Collect information about the return type and the parameters
            bool hasReturn = !String.Equals(methodEntity.ReturnType, "void");
            bool hasReturnParameters = hasReturn && needStorage;

            // Collect information on 32/64 bits invocations to check if they differ
            MethodEntity methodEntity32 = DeriveMethodEntity(methodEntity, false);
            MethodEntity methodEntity64 = DeriveMethodEntity(methodEntity, true);
            bool useMixedInvocation = !AreMethodTypesEqual(methodEntity32, methodEntity64);

            // Append static condition if needed
            this.AppendStartCondition(methodEntity);

            // Append documentation
            this.AppendDocumentation(methodEntity, false);

            // Append Method signature
            StringBuilder signature = new StringBuilder();
            signature.AppendFormat("public {0}(", classEntity.Name);

            // Append parameters
            List<String> parameters = new List<String>();
            foreach (MethodParameterEntity methodParameterEntity in methodEntity.Parameters.Where(p => p.Generate))
            {
                parameters.Add(GetTypeSignature(methodParameterEntity));
            }
            signature.Append(String.Join(", ", parameters.ToArray()));
            signature.Append(")");

            this.Writer.WriteLineFormat(2, signature.ToString());

            // Write base call
            StringBuilder baseCall = new StringBuilder();
            if (useMixedInvocation || hasReturnParameters)
            {
                baseCall.AppendFormat(" : this(ObjectiveCRuntime.SendMessage<IntPtr>({0}Class, \"alloc\"))", classEntity.Name);
            }
            else
            {
                baseCall.AppendFormat(" : base(\"{0}\"", methodEntity.Selector);
                foreach (MethodParameterEntity methodParameterEntity in methodEntity.Parameters.Where(p => p.Generate))
                {
                    baseCall.AppendFormat(", {0}", methodParameterEntity.Name);
                }
                baseCall.Append(")");
            }
            this.Writer.WriteLineFormat(3, baseCall.ToString());

            // Write implementation
            this.Writer.WriteLineFormat(2, "{{");

            String target = "this";
            if (useMixedInvocation)
            {
#if MIXED_MODE
                this.Writer.WriteLineFormat(3, "if (ObjectiveCRuntime.Is64Bits)");
                this.Writer.WriteLineFormat(3, "{{");

                this.GenerateConstructorBody(4, target, methodEntity, methodEntity64, needStorage, varargs);

                this.Writer.WriteLineFormat(3, "}}");
                this.Writer.WriteLineFormat(3, "else");
                this.Writer.WriteLineFormat(3, "{{");
#endif
                this.GenerateConstructorBody(4, target, methodEntity, methodEntity32, needStorage, varargs);
#if MIXED_MODE
                this.Writer.WriteLineFormat(3, "}}");
#endif
            }
            else if (hasReturnParameters)
            {
                this.GenerateConstructorBody(4, target, methodEntity, null, needStorage, varargs);
            }

            this.Writer.WriteLineFormat(2, "}}");

            // Append static condition if needed
            this.AppendEndCondition(methodEntity);

            // Update statistics
            this.Statistics.Methods++;
        }

        protected void AppendDocumentation(MethodEntity methodEntity, bool extensionParameter)
        {
            // Append method comments
            this.Writer.WriteLineFormat(2, "/// <summary>");
            foreach (String line in methodEntity.Summary)
            {
                this.Writer.WriteLineFormat(2, "/// <para>{0}</para>", line.EscapeAll());
            }
            this.Writer.WriteLineFormat(2, "/// <para>Original signature is '{0}'</para>", methodEntity.Signature);
            this.AppendAvailability(2, methodEntity);
            this.Writer.WriteLineFormat(2, "/// </summary>");

            // Add extra parameter
            if (extensionParameter)
            {
                this.Writer.WriteLineFormat(2, "/// <param name=\"__target\">The target instance.</param>");
            }

            // Append parameters' comments
            foreach (MethodParameterEntity methodParameterEntity in methodEntity.Parameters.Where(p => p.Generate))
            {
                // TODO
                this.Writer.WriteLineFormat(2, "/// <param name=\"{0}\">{1}</param>", methodParameterEntity.Name.Trim('@'), methodParameterEntity.Summary.Count > 0 ? methodParameterEntity.Summary[0].EscapeAll() : "MISSING");
            }

            // Append returns' comments
            if (!String.Equals(methodEntity.ReturnType, "void"))
            {
                this.Writer.WriteLineFormat(2, "/// <returns>{0}</returns>", methodEntity.ReturnsDocumentation.EscapeAll());
            }
        }

        private static String GetKeywords(MethodEntity methodEntity, bool implementation, bool extension, bool markedAsNew)
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
            if (methodEntity.Static || extension)
            {
                keywords += "static ";
            }
			else if (!markedAsNew)
			{
                keywords += "virtual ";
			}
            return keywords;
        }

        private static string GetExtraParameter(ClassEntity classEntity, MethodEntity methodEntity, bool extension)
        {
            if (extension && !methodEntity.Static)
            {
                return String.Format("this {0} __target", classEntity.Name);
            }
            return String.Empty;
        }

        private static String GetTarget(ClassEntity classEntity, MethodEntity methodEntity, bool extension)
        {
            if (methodEntity.Static)
            {
                if (extension)
                {
                    return String.Format("{0}.{0}Class", classEntity.Name);
                }
                return String.Format("{0}Class", classEntity.Name);
            }
            if (extension)
            {
                return "__target";
            }
            return "this";
        }

        protected static void CollectInformations(MethodEntity methodEntity, ref bool needStorage, ref bool varargs)
        {
            int outParameters = 0;
            int refParameters = 0;
            int blockParameters = 0;
            varargs = false;

            foreach (MethodParameterEntity methodParameterEntity in methodEntity.Parameters.Where(p => p.Generate))
            {
                if (methodParameterEntity.IsOut)
                {
                    outParameters++;
                }
                if (methodParameterEntity.IsByRef)
                {
                    refParameters++;
                }
                if (methodParameterEntity.IsBlock)
                {
                    blockParameters++;
                }
                if (!varargs && methodParameterEntity.Type.StartsWith("params"))
                {
                    varargs = true;
                }
            }

            needStorage = (outParameters + refParameters + blockParameters) > 0;
        }

        protected void GenerateMethodBody(int indent, string target, MethodEntity methodEntity, MethodEntity innerMethodEntity, bool needStorage, bool varargs)
        {
            bool hasReturn = !String.Equals(methodEntity.ReturnType, "void");
            bool mixed = (innerMethodEntity != null);
            bool mixedReturnType = mixed && !String.Equals(methodEntity.ReturnType, innerMethodEntity.ReturnType);

            this.GenerateLocalsAllocation(indent, methodEntity, innerMethodEntity);
            this.GenerateLocalsMarshalling(indent, methodEntity, innerMethodEntity);

            String invocation = GetMethodInvocation(target, methodEntity, innerMethodEntity, hasReturn, varargs);
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

        private void GenerateConstructorBody(int indent, string target, MethodEntity methodEntity, MethodEntity innerMethodEntity, bool needStorage, bool varargs)
        {
            bool hasReturn = !String.Equals(methodEntity.ReturnType, "void");
            bool mixed = (innerMethodEntity != null);

            this.GenerateLocalsAllocation(indent, methodEntity, innerMethodEntity);
            this.GenerateLocalsMarshalling(indent, methodEntity, innerMethodEntity);

            String invocation = GetMethodInvocation(target, methodEntity, innerMethodEntity, true, false);
            this.Writer.WriteLineFormat(indent, "this.NativePointer = {0}", invocation);

            this.GenerateLocalsUnmarshalling(indent, methodEntity, innerMethodEntity);
            this.GenerateLocalsDeallocation(indent, methodEntity, innerMethodEntity);
        }

        protected void GenerateLocalsAllocation(int indent, MethodEntity methodEntity, MethodEntity innerMethodEntity)
        {
            int index = 1;
            for (int i = 0; i < methodEntity.Parameters.Count; i++)
            {
                MethodParameterEntity methodParameterEntity = methodEntity.Parameters[i];
                MethodParameterEntity innerMethodParameterEntity = innerMethodEntity != null ? innerMethodEntity.Parameters[i] : methodParameterEntity;

                if (methodParameterEntity.IsOut || methodParameterEntity.IsByRef)
                {
                    if ("bool,char,byte,short,ushort,int,uint,long,ulong".Contains(methodParameterEntity.Type)) // Boolean
                    {
                        this.Writer.WriteLineFormat(indent, "IntPtr __local{0} = Marshal.AllocHGlobal(Marshal.SizeOf(typeof ({1})));", index++, methodParameterEntity.Type);
                    }
                    else if ("float,double".Contains(methodParameterEntity.Type))
                    {
                        this.Writer.WriteLineFormat(indent, "IntPtr __local{0} = Marshal.AllocHGlobal(Marshal.SizeOf(typeof ({1})));", index++, methodParameterEntity.Type);
                    }
                    else if (IsMixedType(methodParameterEntity.Type))
                    {
                        this.Writer.WriteLineFormat(indent, "IntPtr __local{0} = Marshal.AllocHGlobal(Marshal.SizeOf(typeof ({1})));", index++, innerMethodParameterEntity.Type);
                    }
                    else
                    {
                        this.Writer.WriteLineFormat(indent, "IntPtr __local{0} = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (IntPtr)));", index++);
                    }
                }
                else if (methodParameterEntity.IsBlock)
                {
                    this.Writer.WriteLineFormat(indent, "Block __local{0} = ObjectiveCRuntime.CreateBlock({1});", index++, methodParameterEntity.Name);
                }
            }
        }

        protected void GenerateLocalsMarshalling(int indent, MethodEntity methodEntity, MethodEntity innerMethodEntity)
        {
            int index = 1;
            for (int i = 0; i < methodEntity.Parameters.Count; i++)
            {
                MethodParameterEntity methodParameterEntity = methodEntity.Parameters[i];
                MethodParameterEntity innerMethodParameterEntity = innerMethodEntity != null ? innerMethodEntity.Parameters[i] : methodParameterEntity;

                if (methodParameterEntity.IsOut)
                {
                    index++;
                }
                else if (methodParameterEntity.IsByRef)
                {
                    if (IsMixedType(methodParameterEntity.Type))
                    {
                        this.Writer.WriteLineFormat(indent, "Marshal.StructureToPtr(({0}) {1}, __local{2}, false);", innerMethodParameterEntity.Type, methodParameterEntity.Name, index++);
                    }
                    else
                    {
                        this.Writer.WriteLineFormat(indent, "Marshal.StructureToPtr({0}, __local{1}, false);", methodParameterEntity.Name, index++);
                    }
                }
                else if (methodParameterEntity.IsBlock)
                {
                    index++;
                }
            }
        }

        protected void GenerateLocalsUnmarshalling(int indent, MethodEntity methodEntity, MethodEntity innerMethodEntity)
        {
            int index = 1;
            for (int i = 0; i < methodEntity.Parameters.Count; i++)
            {
                MethodParameterEntity methodParameterEntity = methodEntity.Parameters[i];
                MethodParameterEntity innerMethodParameterEntity = innerMethodEntity != null ? innerMethodEntity.Parameters[i] : methodParameterEntity;

                if (methodParameterEntity.IsOut || methodParameterEntity.IsByRef)
                {
                    switch (methodParameterEntity.Type)
                    {
                        case "bool":
                        case "byte":
                            this.Writer.WriteLineFormat(indent, "{0} = (Marshal.ReadByte(__local{1}) != 0);", methodParameterEntity.Name, index++);
                            break;
                        case "short":
                            this.Writer.WriteLineFormat(indent, "{0} = Marshal.ReadInt16(__local{1});", methodParameterEntity.Name, index++);
                            break;
                        case "ushort":
                            this.Writer.WriteLineFormat(indent, "{0} = (short) Marshal.ReadInt16(__local{1});", methodParameterEntity.Name, index++);
                            break;
                        case "int":
                            this.Writer.WriteLineFormat(indent, "{0} = Marshal.ReadInt32(__local{1});", methodParameterEntity.Name, index++);
                            break;
                        case "uint":
                            this.Writer.WriteLineFormat(indent, "{0} = (uint) Marshal.ReadInt32(__local{1});", methodParameterEntity.Name, index++);
                            break;
                        case "long":
                            this.Writer.WriteLineFormat(indent, "{0} = Marshal.ReadInt64(__local{1});", methodParameterEntity.Name, index++);
                            break;
                        case "ulong":
                            this.Writer.WriteLineFormat(indent, "{0} = (ulong) Marshal.ReadInt64(__local{1});", methodParameterEntity.Name, index++);
                            break;
                        case "float":
                        case "double":
                            this.Writer.WriteLineFormat(indent, "{0} = ({1}) Marshal.PtrToStructure(__local{2}, typeof({1}));", methodParameterEntity.Name, methodParameterEntity.Type, index++);
                            break;
                        default:
                            if (IsMixedType(methodParameterEntity.Type))
                            {
                                this.Writer.WriteLineFormat(indent, "{0} = ({1}) Marshal.PtrToStructure(__local{2}, typeof({3}));", methodParameterEntity.Name, methodParameterEntity.Type, index++, innerMethodParameterEntity.Type);
                            }
                            else
                            {
                                this.Writer.WriteLineFormat(indent, "{0} = ObjectiveCRuntime.GetInstance<{1}>(Marshal.ReadIntPtr(__local{2}));", methodParameterEntity.Name, methodParameterEntity.Type, index++);
                            }
                            break;
                    }
                }
                else if (methodParameterEntity.IsBlock)
                {
                    index++;
                }
            }
        }

        protected void GenerateLocalsDeallocation(int indent, MethodEntity methodEntity, MethodEntity innerMethodEntity)
        {
            int index = 1;
            for (int i = 0; i < methodEntity.Parameters.Count; i++)
            {
                MethodParameterEntity methodParameterEntity = methodEntity.Parameters[i];
                MethodParameterEntity innerMethodParameterEntity = innerMethodEntity != null ? innerMethodEntity.Parameters[i] : methodParameterEntity;

                if (methodParameterEntity.IsOut || methodParameterEntity.IsByRef)
                {
                    this.Writer.WriteLineFormat(indent, "Marshal.FreeHGlobal(__local{0});", index++);
                }
                else if (methodParameterEntity.IsBlock)
                {
                    this.Writer.WriteLineFormat(indent, "__local{0}.Dispose();", index++);
                }
            }
        }

        private static string GetMethodInvocation(string target, MethodEntity methodEntity, MethodEntity innerMethodEntity, bool hasReturn, bool varargs)
        {
            StringBuilder builder = new StringBuilder();

            String suffix = varargs ? "VarArgs" : String.Empty;
            if (hasReturn)
            {
                String returnType = innerMethodEntity != null ? innerMethodEntity.ReturnType : methodEntity.ReturnType;
                builder.AppendFormat("ObjectiveCRuntime.SendMessage{0}<{1}>", suffix, returnType);
            }
            else
            {
                builder.AppendFormat("ObjectiveCRuntime.SendMessage{0}", suffix);
            }

            String selector = innerMethodEntity != null ? innerMethodEntity.Selector : methodEntity.Selector;
            builder.AppendFormat("({0}, \"{1}\"", target, selector);
            builder.Append(GetMessageParameterList(methodEntity, innerMethodEntity, true));
            builder.Append(");");

            return builder.ToString();
        }

        protected static String GetMessageParameterList(MethodEntity methodEntity, MethodEntity innerMethodEntity, bool withColonFirst)
        {
            StringBuilder builder = new StringBuilder();

            int index = 1;
            for (int i = 0; i < methodEntity.Parameters.Count; i++)
            {
                MethodParameterEntity methodParameterEntity = methodEntity.Parameters[i];
                MethodParameterEntity innerMethodParameterEntity = innerMethodEntity != null ? innerMethodEntity.Parameters[i] : methodParameterEntity;

                if (!innerMethodParameterEntity.Generate)
                {
                    continue;
                }

                if (i > 0 || withColonFirst)
                {
                    builder.Append(", ");
                }

                if (innerMethodParameterEntity.IsOut)
                {
                    builder.AppendFormat("__local{0}", index++);
                }
                else if (innerMethodParameterEntity.IsByRef)
                {
                    builder.AppendFormat("__local{0}", index++);
                }
                else if (innerMethodParameterEntity.IsBlock)
                {
                    builder.AppendFormat("__local{0}", index++);
                }
                else
                {
                    if (String.Equals(methodParameterEntity.Type, innerMethodParameterEntity.Type))
                    {
                        builder.AppendFormat("{0}", innerMethodParameterEntity.Name);
                    }
                    else
                    {
                        builder.AppendFormat("({0}) {1}", innerMethodParameterEntity.Type, innerMethodParameterEntity.Name);
                    }
                }
            }

            return builder.ToString();
        }

        private static MethodEntity DeriveMethodEntity(MethodEntity methodEntity, bool is64Bits)
        {
            MethodEntity result = new MethodEntity(methodEntity);

            result.ReturnType = GetRealType(result.ReturnType, is64Bits);
            foreach (MethodParameterEntity parameter in result.Parameters)
            {
                parameter.Type = GetRealType(parameter.Type, is64Bits);
            }

            return result;
        }

        protected static bool AreMethodTypesEqual(MethodEntity methodEntity1, MethodEntity methodEntity2)
        {
            if (!String.Equals(methodEntity1.ReturnType, methodEntity2.ReturnType))
            {
                return false;
            }

            for (int i = 0; i < methodEntity1.Parameters.Count; i++)
            {
                if (!String.Equals(methodEntity1.Parameters[i].Type, methodEntity2.Parameters[i].Type))
                {
                    return false;
                }
            }

            return true;
        }
    }
}