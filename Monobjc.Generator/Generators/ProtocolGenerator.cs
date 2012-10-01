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
using System.IO;
using System.Linq;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Generators
{
	/// <summary>
	///   Code generator for protocol.
	/// </summary>
	public class ProtocolGenerator : TypedGenerator
	{
		private MethodGenerator methodGenerator;
		private PropertyGenerator propertyGenerator;

		/// <summary>
		/// Gets the method generator.
		/// </summary>
		protected MethodGenerator MethodGenerator {
			get {
				if (this.methodGenerator == null) {
					this.methodGenerator = new MethodGenerator () {
						Writer = this.Writer,
						Statistics = this.Statistics,
						MixedTypes = this.MixedTypes,
						TypeManager = this.TypeManager,
					};
				}
				return this.methodGenerator;
			}
		}

		/// <summary>
		/// Gets the property generator.
		/// </summary>
		protected PropertyGenerator PropertyGenerator {
			get {
				if (this.propertyGenerator == null) {
					this.propertyGenerator = new PropertyGenerator () {
						Writer = this.Writer,
						Statistics = this.Statistics,
						MixedTypes = this.MixedTypes,
						TypeManager = this.TypeManager,
					};
				}
				return this.propertyGenerator;
			}
		}

		/// <summary>
		///   Generates the specified entity.
		/// </summary>
		/// <param name = "entity">The entity.</param>
		public override void Generate (BaseEntity entity)
		{
			ProtocolEntity protocolEntity = (ProtocolEntity)entity;

			// Append License
			this.Writer.WriteLineFormat (0, License);

			// Append usings
			this.AppendStandardNamespaces ();

			// Append namespace starter
			this.Writer.WriteLineFormat (0, "namespace Monobjc.{0}", protocolEntity.Namespace);
			this.Writer.WriteLineFormat (0, "{{");

			// Append static condition if needed
			this.AppendStartCondition (protocolEntity);

			// Append class starter
			this.Writer.WriteLineFormat (1, "[ObjectiveCProtocol(\"{0}\")]", protocolEntity.Name);
#if GENERATED_ATTRIBUTE
            this.Writer.WriteLineFormat(1, "[GeneratedCodeAttribute(\"{0}\", \"{1}\")]", GenerationHelper.ToolName, GenerationHelper.ToolVersion);
#endif
			if (string.IsNullOrEmpty (protocolEntity.BaseType)) {
				this.Writer.WriteLineFormat (1, "public partial interface I{0} : {1}", protocolEntity.Name, "IManagedWrapper");
			} else {
				this.Writer.WriteLineFormat (1, "public partial interface I{0} : I{1}", protocolEntity.Name, protocolEntity.BaseType);
			}
			this.Writer.WriteLineFormat (1, "{{");

			// Append methods
			foreach (MethodEntity methodEntity in protocolEntity.Methods.Where(e => e.Generate)) {
				if (methodEntity.Static) {
					continue;
				}
				this.MethodGenerator.Generate (protocolEntity, methodEntity, false, false);
				this.Writer.WriteLine ();
			}

			// Append properties
			foreach (PropertyEntity propertyEntity in protocolEntity.Properties.Where(e => e.Generate)) {
				if (propertyEntity.Static) {
					continue;
				}
				this.PropertyGenerator.Generate (protocolEntity, propertyEntity, false);
				this.Writer.WriteLine ();
			}

			// Append class ender
			this.Writer.WriteLineFormat (1, "}}");

			// Append static condition if needed
			this.AppendEndCondition (protocolEntity);

			// Append namespace ender
			this.Writer.WriteLineFormat (0, "}}");

			// Update statistics
			this.Statistics.Protocols++;
		}
	}
}