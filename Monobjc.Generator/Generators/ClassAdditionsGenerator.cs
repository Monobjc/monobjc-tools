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
using System;
using System.IO;
using System.Linq;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Generators
{
	/// <summary>
	///   Code generator for class's additions.
	/// </summary>
	public class ClassAdditionsGenerator : ClassGenerator
	{
		/// <summary>
		///   Generates the specified entity.
		/// </summary>
		/// <param name = "entity">The entity.</param>
		public override void Generate (BaseEntity entity)
		{
			ClassEntity classEntity = (ClassEntity)entity;
			ClassEntity extendedClass = classEntity.ExtendedClass ?? classEntity;
			bool sameClass = String.Equals (classEntity.Namespace, extendedClass.Namespace);

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
			if (sameClass) {
				this.Writer.WriteLineFormat (1, "public partial class {0}", extendedClass.Name);
			} else {
				this.Writer.WriteLineFormat (1, "public static partial class {0}_{1}Additions", extendedClass.Name, classEntity.Namespace);
			}
			this.Writer.WriteLineFormat (1, "{{");

			// Append methods
			foreach (MethodEntity methodEntity in classEntity.Methods.Where(e => e.Generate)) {
				this.MethodGenerator.Generate (extendedClass, methodEntity, true, !sameClass);
				this.Writer.WriteLine ();
			}

			// Append properties
			foreach (PropertyEntity propertyEntity in classEntity.Properties.Where(e => e.Generate)) {
				if (sameClass || propertyEntity.Static) {
					this.PropertyGenerator.Generate (extendedClass, propertyEntity);
				} else {
					this.MethodGenerator.Generate (extendedClass, propertyEntity.GetterAsMethodEntity (), true, true);
					if (propertyEntity.Setter != null) {
						this.MethodGenerator.Generate (extendedClass, propertyEntity.SetterAsMethodEntity (), true, true);
					}
				}
				this.Writer.WriteLine ();
			}

			// Append class ender
			this.Writer.WriteLineFormat (1, "}}");

			// Append static condition if needed
			this.AppendEndCondition (classEntity);

			// Append namespace ender
			this.Writer.WriteLineFormat (0, "}}");
		}
	}
}