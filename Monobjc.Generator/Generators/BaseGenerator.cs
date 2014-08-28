//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2014 - Laurent Etiemble
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
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Generators
{
	/// <summary>
	///   Base class for class generator.
	/// </summary>
	public abstract class BaseGenerator
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "BaseGenerator" /> class.
		/// </summary>
		protected BaseGenerator ()
		{
		}

		/// <summary>
		///   Gets or sets the writer.
		/// </summary>
		public StreamWriter Writer { get; set; }
		
		/// <summary>
		///   Gets or sets the statistics.
		/// </summary>
		public GenerationStatistics Statistics { get; set; }

		/// <summary>
		///   Gets or sets the License txt.
		/// </summary>
		public String License { get; set; }

		/// <summary>
		///   Gets or sets the type manager.
		/// </summary>
		public TypeManager TypeManager { get; set; }

		/// <summary>
		/// Gets or sets the mixed types.
		/// </summary>
		public IDictionary<String, String> MixedTypes { get; set; }

		/// <summary>
		///   Gets or sets the usings.
		/// </summary>
		public String[] Usings { get; set; }

		/// <summary>
		/// Generate the code for the specified entity.
		/// </summary>
		public abstract void Generate (BaseEntity entity);

		/// <summary>
		///   Append the standard namespaces
		/// </summary>
		protected void AppendStandardNamespaces ()
		{
			this.Writer.WriteLineFormat (0, "using System;");
			this.Writer.WriteLineFormat (0, "using System.CodeDom.Compiler;");
			this.Writer.WriteLineFormat (0, "using System.Globalization;");
			this.Writer.WriteLineFormat (0, "using System.Runtime.InteropServices;");
			this.Writer.WriteLine ();
			this.Writer.WriteLineFormat (0, "using Monobjc;");
			foreach (string @using in this.Usings) {
				this.Writer.WriteLineFormat (0, "using Monobjc.{0};",  @using);
			}
			this.Writer.WriteLine ();
		}

		/// <summary>
		///   Appends the availability paragraph.
		/// </summary>
		/// <param name = "indent">The indent.</param>
		/// <param name = "entity">The entity.</param>
		protected void AppendAvailability (int indent, BaseEntity entity)
		{
			if (!String.IsNullOrEmpty (entity.MinAvailability)) {
				this.Writer.WriteLineFormat (indent, "/// <para>{0}</para>", CommentHelper.GetAvailability (entity.MinAvailability));
			}
		}

		/// <summary>
		///   Appends the start condition for OS version.
		/// </summary>
		/// <param name = "entity">The entity.</param>
        protected void AppendStartCondition (BaseEntity entity)
		{
			String define = AvailabilityHelper.GetDefine (entity.MinAvailability);
            if (!String.IsNullOrEmpty (define)) {
				this.Writer.WriteLineFormat (0, "#if {0}", define);
			}
			if (entity.Obsolete == null) {
				define = AvailabilityHelper.GetDefine (entity.MaxAvailability);
				if (!String.IsNullOrEmpty (define)) {
					this.Writer.WriteLineFormat (0, "#if !{0}", define);
				}
			}
		}

		/// <summary>
		///   Appends the end condition for OS version.
		/// </summary>
		/// <param name = "entity">The entity.</param>
        protected void AppendEndCondition (BaseEntity entity)
		{
			String define = AvailabilityHelper.GetDefine (entity.MinAvailability);
            if (!String.IsNullOrEmpty (define)) {
				this.Writer.WriteLineFormat (0, "#endif");
			}
			if (entity.Obsolete == null) {
				define = AvailabilityHelper.GetDefine (entity.MaxAvailability);
				if (!String.IsNullOrEmpty (define)) {
					this.Writer.WriteLineFormat (0, "#endif");
				}
			}
		}

		/// <summary>
		///   Appends the obsolete attribute.
		/// </summary>
		/// <param name = "entity">The entity.</param>
		protected void AppendObsoleteAttribute (BaseEntity entity)
		{
			if (entity.Obsolete != null) {
				String message = String.Empty;
				if (entity.Obsolete != String.Empty) {
					message = " " + entity.Obsolete;
				}

				String define = AvailabilityHelper.GetDefine (entity.MaxAvailability);
				if (!String.IsNullOrEmpty (define)) {
					this.Writer.WriteLineFormat (0, "#if {0}", define);
				}
				this.Writer.WriteLineFormat (2, "[Obsolete(\"Deprecated in {0}.{1}\")]", entity.MaxAvailability, message);
				if (!String.IsNullOrEmpty (define)) {
					this.Writer.WriteLineFormat (0, "#endif");
				}
			}
		}

		/// <summary>
		///   Determines whether the specified type is a mixed type.
		/// </summary>
		/// <param name = "type">The type.</param>
		/// <returns>
		///   <c>true</c> if the specified type is a mixed type; otherwise, <c>false</c>.
		/// </returns>
		protected bool IsMixedType (String type)
		{
			return this.MixedTypes.ContainsKey (type);
		}

		/// <summary>
		///   Gets the real type of the given type.
		/// </summary>
		/// <param name = "type">The type.</param>
		/// <param name = "is64Bits">if set to <c>true</c>, return the 64 bits real type.</param>
		/// <returns>The real type.</returns>
		protected String GetRealType (String type, bool is64Bits)
		{
			if (this.MixedTypes.ContainsKey (type)) {
				string[] types = this.MixedTypes [type].Split (',');
				return types [is64Bits ? 1 : 0];
			}
			return type;
		}

		/// <summary>
		///   Gets the type signature.
		/// </summary>
		protected static String GetTypeSignature (MethodParameterEntity methodParameterEntity)
		{
			return String.Format (CultureInfo.CurrentCulture,
                                 "{0}{1} {2}",
                                 methodParameterEntity.IsOut ? "out " : (!methodParameterEntity.IsBlock && methodParameterEntity.IsByRef) ? "ref " : String.Empty,
                                 methodParameterEntity.Type,
                                 methodParameterEntity.Name);
		}

		/// <summary>
		///   Gets the parameter for an invocation.
		/// </summary>
		protected static String GetInvocationParameter (MethodParameterEntity methodParameterEntity, MethodParameterEntity destinationMethodParameterEntity = null)
		{
			if (destinationMethodParameterEntity != null && !methodParameterEntity.Type.Equals (destinationMethodParameterEntity.Type)) {
				return String.Format (CultureInfo.CurrentCulture,
                                     "{0}({1}) {2}",
                                     methodParameterEntity.IsOut ? "out " : methodParameterEntity.IsByRef ? "ref " : String.Empty,
                                     destinationMethodParameterEntity.Type,
                                     methodParameterEntity.Name);
			}
			return String.Format (CultureInfo.CurrentCulture,
                                 "{0}{1}",
                                 methodParameterEntity.IsOut ? "out " : methodParameterEntity.IsByRef ? "ref " : String.Empty,
                                 methodParameterEntity.Name);
		}
	}
}