//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2013 - Laurent Etiemble
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
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Generators
{
	/// <summary>
	///   Code generator for class's definition.
	/// </summary>
	public class ClassDefinitionGenerator : ClassGenerator
	{
		/// <summary>
		///   Generates the specified entity.
		/// </summary>
		/// <param name = "entity">The entity.</param>
		public override void Generate (BaseEntity entity)
		{
			ClassEntity classEntity = (ClassEntity)entity;

			// Append License
			this.Writer.WriteLineFormat (0, License);

			// Append usings
			this.AppendStandardNamespaces ();

			// Append namespace starter
			this.Writer.WriteLineFormat (0, "namespace Monobjc.{0}", classEntity.Namespace);
			this.Writer.WriteLineFormat (0, "{{");

			// Append static condition if needed
			this.AppendStartCondition (classEntity);

			// Append class starter
			this.Writer.WriteLineFormat (1, "[ObjectiveCClass(\"{0}\", IsNative = true)]", classEntity.Name);
#if GENERATED_ATTRIBUTE
            this.Writer.WriteLineFormat(1, "[GeneratedCodeAttribute(\"{0}\", \"{1}\")]", GenerationHelper.ToolName, GenerationHelper.ToolVersion);
#endif
			this.Writer.WriteLineFormat (1, "public partial class {0}{1}", classEntity.Name, (classEntity.BaseType != null) ? (" : " + classEntity.BaseType) : string.Empty);
			this.Writer.WriteLineFormat (1, "{{");

			// Add constant for class access
			this.Writer.WriteLineFormat (2, "/// <summary>");
			this.Writer.WriteLineFormat (2, "/// Static field for a quick access to the {0} class.", classEntity.Name);
			this.Writer.WriteLineFormat (2, "/// </summary>");
			this.Writer.WriteLineFormat (2, "public static readonly Class {0}Class = Class.Get(typeof ({0}));", classEntity.Name);
			this.Writer.WriteLine ();

			// Add default constructor
			this.Writer.WriteLineFormat (2, "/// <summary>");
			this.Writer.WriteLineFormat (2, "/// Initializes a new instance of the <see cref=\"{0}\"/> class.", classEntity.Name);
			this.Writer.WriteLineFormat (2, "/// </summary>");
			this.Writer.WriteLineFormat (2, "public {0}() {{}}", classEntity.Name);
			this.Writer.WriteLine ();

			// Add constructor with pointer parameter
			this.Writer.WriteLineFormat (2, "/// <summary>");
			this.Writer.WriteLineFormat (2, "/// Initializes a new instance of the <see cref=\"{0}\"/> class.", classEntity.Name);
			this.Writer.WriteLineFormat (2, "/// </summary>");
			this.Writer.WriteLineFormat (2, "/// <param name=\"value\">The native pointer.</param>");
			this.Writer.WriteLineFormat (2, "public {0}(IntPtr value) : base(value) {{}}", classEntity.Name);
			this.Writer.WriteLine ();

			// Add constructor with varargs parameters
			this.Writer.WriteLineFormat (2, "/// <summary>");
			this.Writer.WriteLineFormat (2, "/// Initializes a new instance of the <see cref=\"{0}\"/> class.", classEntity.Name);
			this.Writer.WriteLineFormat (2, "/// </summary>");
			this.Writer.WriteLineFormat (2, "/// <param name=\"selector\">The selector.</param>");
			this.Writer.WriteLineFormat (2, "/// <param name=\"firstParameter\">The first paramater.</param>");
			this.Writer.WriteLineFormat (2, "/// <param name=\"otherParameters\">The other parameters.</param>");
			this.Writer.WriteLineFormat (2, "protected {0}(String selector, Object firstParameter, params Object[] otherParameters) : base(selector, firstParameter, otherParameters) {{}}", classEntity.Name);

			// Append class ender
			this.Writer.WriteLineFormat (1, "}}");

			// Append static condition if needed
			this.AppendEndCondition (classEntity);

			// Append namespace ender
			this.Writer.WriteLineFormat (0, "}}");

			// Update statistics
			this.Statistics.Classes++;
		}
	}
}