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
using System.Collections.Generic;
using System.Linq;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.Generators
{
	/// <summary>
	///   Base class for class entity.
	/// </summary>
	public abstract class ClassGenerator : TypedGenerator
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
		///   Gets all methods of the superclass and own.
		/// </summary>
		protected static IEnumerable<MethodEntity> GetAllMethods (ClassEntity classEntity, bool withOwn)
		{
			List<MethodEntity> methods = (classEntity.SuperClass != null) ? classEntity.SuperClass.GetMethods (true, true).ToList () : new List<MethodEntity> ();
			if (withOwn) {
				methods.AddRange (classEntity.Methods);
			}
			return methods.Distinct ();
		}

		/// <summary>
		///   Gets the properties of the superclass and own.
		/// </summary>
		protected static IEnumerable<PropertyEntity> GetProperties (ClassEntity classEntity, bool withOwn)
		{
			List<PropertyEntity> properties = (classEntity.SuperClass != null) ? classEntity.SuperClass.GetProperties (true, true).ToList () : new List<PropertyEntity> ();
			if (withOwn) {
				properties.AddRange (classEntity.Properties);
			}
			return properties.Distinct ();
		}
	}
}