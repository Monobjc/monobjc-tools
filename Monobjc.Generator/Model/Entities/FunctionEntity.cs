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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	/// <summary>
	///   Represents the model for a method.
	/// </summary>
	[Serializable]
	[XmlRoot("Function")]
	public partial class FunctionEntity : MethodEntity
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Monobjc.Tools.Generator.Model.Entities.FunctionEntity"/> class.
		/// </summary>
		public FunctionEntity ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Monobjc.Tools.Generator.Model.Entities.FunctionEntity"/> class.
		/// </summary>
		public FunctionEntity (FunctionEntity functionEntity) : this()
		{
			this.MinAvailability = functionEntity.MinAvailability;
			this.Generate = functionEntity.Generate;
			this.Name = functionEntity.Name;
			this.ReturnsDocumentation = functionEntity.ReturnsDocumentation;
			this.ReturnType = functionEntity.ReturnType;
			this.Selector = functionEntity.Selector;
			this.Signature = functionEntity.Signature;
			this.Static = functionEntity.Static;
			this.Summary = new List<String> (functionEntity.Summary);

			foreach (MethodParameterEntity methodParameterEntity in functionEntity.Parameters) {
				MethodParameterEntity parameter = new MethodParameterEntity (methodParameterEntity);
				this.Parameters.Add (parameter);
			}

			this.GenerateConstructor = functionEntity.GenerateConstructor;
			this.SharedLibrary = functionEntity.SharedLibrary;
			this.EntryPoint = functionEntity.EntryPoint;
		}

		/// <summary>
		///   Gets or sets the shared library.
		/// </summary>
		[XmlElement]
		public String SharedLibrary {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the entry point.
		/// </summary>
		[XmlAttribute]
		public String EntryPoint {
			get;
			set;
		}

		/// <summary>
		///   Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		///   A hash code for the current <see cref = "T:System.Object" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashValue ()
		{
			unchecked {
				int hash = base.GetHashValue();
				hash = hash * 23 + (this.SharedLibrary != null ? this.SharedLibrary.GetHashCode () : 0);
				hash = hash * 23 + (this.EntryPoint != null ? this.EntryPoint.GetHashCode () : 0);
				return hash;
			}
		}
	}
}