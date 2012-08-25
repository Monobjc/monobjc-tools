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
using System.IO;
using System.Linq;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;
using System.Text;

namespace Monobjc.Tools.Generator.Tasks.Output
{
	public class DumpGenerationTask : BaseTask
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "DumpGenerationTask" /> class.
		/// </summary>
		/// <param name = "name">The name.</param>
		public DumpGenerationTask (String name) : base(name)
		{
		}

		/// <summary>
		///   Executes this instance.
		/// </summary>
		public override void Execute ()
		{
			this.DisplayBanner ();

			List<String> lines = new List<String>();
			foreach (Entry entry in this.Entries) {
				String xmlFile = entry [EntryFolderType.Xml];
				if (!File.Exists (xmlFile)) {
					continue;
				}

				switch (entry.Nature) {
				case TypedEntity.CLASS_NATURE:
					ClassEntity classEntity = BaseEntity.LoadFrom<ClassEntity> (xmlFile);
					DumpClass(classEntity, lines);
					break;
				case TypedEntity.ENUMERATION_NATURE:
					EnumerationEntity enumerationEntity = BaseEntity.LoadFrom<EnumerationEntity>(xmlFile);
					DumpEnumeration(enumerationEntity, lines);
					break;
				case TypedEntity.PROTOCOL_NATURE:
					ProtocolEntity protocolEntity = BaseEntity.LoadFrom<ProtocolEntity>(xmlFile);
					DumpProtocol(protocolEntity, lines);
					break;
				case TypedEntity.STRUCTURE_NATURE:
					StructureEntity structureEntity = BaseEntity.LoadFrom<StructureEntity>(xmlFile);
					DumpStructure(structureEntity, lines);
					break;
				case TypedEntity.TYPE_NATURE:
					TypedEntity typedEntity = BaseEntity.LoadFrom<TypedEntity>(xmlFile);
					DumpType(typedEntity, lines);
					break;
				default:
					continue;
				}
			}

			lines.Sort();
			foreach(String line in lines) {
				Console.WriteLine(line);
			}
		}

		void DumpClass (ClassEntity entity, List<String> lines)
		{
			String prefix = "C:" + entity.Namespace + "." + entity.Name;
			if (!String.IsNullOrEmpty(entity.BaseType)) {
				prefix += "<" + entity.BaseType + ">";
			}
			DumpConstants(prefix, entity.Constants, lines);
			DumpEnumerations(entity.Enumerations, lines);
			DumpFunctions(prefix, entity.Functions, lines);
			DumpMethods(prefix, entity.Methods, lines);
			DumpMethods(prefix, entity.DelegateMethods, lines);
			DumpProperties(prefix, entity.Properties, lines);
		}

		void DumpEnumerations(List<EnumerationEntity> entities, List<String> lines)
		{
			foreach(EnumerationEntity entity in entities) {
				DumpEnumeration(entity, lines);
			}
		}

		void DumpEnumeration (EnumerationEntity entity, List<String> lines)
		{
			String prefix = "E:" + entity.Namespace + "." + entity.Name;
			if (!String.IsNullOrEmpty(entity.BaseType)) {
				prefix += "<" + entity.BaseType + ">";
			}
			DumpEnumerationValue(prefix, entity.Values, lines);
		}

		void DumpEnumerationValue (string prefix, List<EnumerationValueEntity> values, List<string> lines)
		{
			foreach(EnumerationValueEntity value in values) {
				StringBuilder builder = new StringBuilder();
				builder.Append(prefix);
				builder.Append(" ");
				builder.Append(value.Name);
				builder.Append("=");
				builder.Append(value.Value);
				lines.Add(builder.ToString());
			}
		}

		void DumpProtocol (ProtocolEntity entity, List<String> lines)
		{
			String prefix = "P:" + entity.Namespace + "." + entity.Name;
			if (!String.IsNullOrEmpty(entity.BaseType)) {
				prefix += "<" + entity.BaseType + ">";
			}
			DumpConstants(prefix, entity.Constants, lines);
			DumpEnumerations(entity.Enumerations, lines);
			DumpFunctions(prefix, entity.Functions, lines);
			DumpMethods(prefix, entity.Methods, lines);
			DumpMethods(prefix, entity.DelegateMethods, lines);
			DumpProperties(prefix, entity.Properties, lines);
		}		

		void DumpStructure (StructureEntity entity, List<String> lines)
		{
			String prefix = "S:" + entity.Namespace + "." + entity.Name;
			if (!String.IsNullOrEmpty(entity.BaseType)) {
				prefix += "<" + entity.BaseType + ">";
			}
		}

		void DumpType (TypedEntity entity, List<String> lines)
		{
			String prefix = "T:" + entity.Namespace + "." + entity.Name;
			DumpConstants(prefix, entity.Constants, lines);
			DumpEnumerations(entity.Enumerations, lines);
			DumpFunctions(prefix, entity.Functions, lines);
		}

		void DumpConstants (String prefix, List<ConstantEntity> constants, List<String> lines)
		{
			foreach(ConstantEntity constant in constants) {
				StringBuilder builder = new StringBuilder();
				builder.Append(prefix);
				builder.Append(" - C:");
				builder.Append(constant.Name);
				builder.Append("<");
				builder.Append(constant.Type);
				builder.Append(">");
				lines.Add(builder.ToString());
			}
		}

		void DumpFunctions (String prefix, List<FunctionEntity> functions, List<String> lines)
		{
			foreach(FunctionEntity function in functions) {
				StringBuilder builder = new StringBuilder();
				builder.Append(prefix);
				builder.Append(" - F:");
				builder.Append(function.Name);

				builder.Append("[");
				builder.Append(String.IsNullOrEmpty(function.ReturnType) ? "void" : function.ReturnType);
				builder.Append("]");
				builder.Append("(");

				int i = 0;
				foreach(MethodParameterEntity parameter in function.Parameters) {
					if (i > 0) {
						builder.Append(", ");
					}
					builder.Append(parameter.IsBlock ? "block " : String.Empty);
					builder.Append(parameter.IsByRef ? "ref " : String.Empty);
					builder.Append(parameter.IsOut ? "out " : String.Empty);
					builder.Append(parameter.Type);
					i++;
				}

				builder.Append(")");
				lines.Add(builder.ToString());
			}
		}

		void DumpMethods (String prefix, List<MethodEntity> methods, List<String> lines)
		{
			foreach(MethodEntity method in methods) {
				StringBuilder builder = new StringBuilder();
				builder.Append(prefix);
				builder.Append(" - M:");
				builder.Append(method.Name);

				builder.Append("[");
				builder.Append(String.IsNullOrEmpty(method.ReturnType) ? "void" : method.ReturnType);
				builder.Append("]");
				builder.Append("(");

				int i = 0;
				foreach(MethodParameterEntity parameter in method.Parameters) {
					if (i > 0) {
						builder.Append(", ");
					}
					builder.Append(parameter.IsBlock ? "block " : String.Empty);
					builder.Append(parameter.IsByRef ? "ref " : String.Empty);
					builder.Append(parameter.IsOut ? "out " : String.Empty);
					builder.Append(parameter.Type);
					i++;
				}

				builder.Append(")");
				lines.Add(builder.ToString());
			}
		}

		void DumpProperties (String prefix, List<PropertyEntity> properties, List<String> lines)
		{
			foreach(PropertyEntity property in properties) {
				StringBuilder builder = new StringBuilder();
				builder.Append(prefix);
				builder.Append(" - P:");
				builder.Append(property.Name);
				if (property.HasGetter) {
					builder.Append(property.HasSetter ? "[RW]" : "[RO]");
				}
				builder.Append("<");
				builder.Append(property.Type);
				builder.Append(">");
				lines.Add(builder.ToString());
			}
		}
	}
}
