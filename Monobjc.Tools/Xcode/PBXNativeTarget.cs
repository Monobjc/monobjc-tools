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
using System.IO;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Xcode
{
	public class PBXNativeTarget : PBXTarget
	{
		private readonly IList<PBXBuildRule> buildRules;

		/// <summary>
		///   Initializes a new instance of the <see cref = "PBXNativeTarget" /> class.
		/// </summary>
		public PBXNativeTarget ()
		{
			this.buildRules = new List<PBXBuildRule> ();
		}

		/// <summary>
		///   Gets or sets the build rules.
		/// </summary>
		/// <value>The build rules.</value>
		public IEnumerable<PBXBuildRule> BuildRules {
			get { return this.buildRules; }
		}
		
		/// <summary>
		/// Adds the build rule.
		/// </summary>
		/// <param name='rule'>
		/// Rule.
		/// </param>
		public void AddBuildRule (PBXBuildRule rule)
		{
			this.buildRules.Add (rule);
		}
		
		/// <summary>
		/// Removes the build rule.
		/// </summary>
		/// <param name='rule'>
		/// Rule.
		/// </param>
		public void RemoveBuildRule (PBXBuildRule rule)
		{
			this.buildRules.Remove (rule);
		}

		/// <summary>
		///   Gets or sets the product install path.
		/// </summary>
		/// <value>The product install path.</value>
		public String ProductInstallPath { get; set; }

		/// <summary>
		///   Gets or sets the product reference.
		/// </summary>
		/// <value>The product reference.</value>
		public PBXFileReference ProductReference { get; set; }

		/// <summary>
		///   Gets or sets the type of the product.
		/// </summary>
		/// <value>The type of the product.</value>
		public PBXProductType ProductType { get; set; }

		/// <summary>
		///   Gets the elemnt's nature.
		/// </summary>
		/// <value>The nature.</value>
		public override PBXElementType Nature {
			get { return PBXElementType.PBXNativeTarget; }
		}

		/// <summary>
		///   Accepts the specified visitor.
		/// </summary>
		/// <param name = "visitor">The visitor.</param>
		public override void Accept (IPBXVisitor visitor)
		{
			visitor.Visit (this);

			if (this.BuildConfigurationList != null) {
				this.BuildConfigurationList.Accept (visitor);
			}
			if (this.BuildPhases != null) {
				foreach (PBXBuildPhase phase in this.BuildPhases) {
					phase.Accept (visitor);
				}
			}
			if (this.Dependencies != null) {
				foreach (PBXTargetDependency dependency in this.Dependencies) {
					dependency.Accept (visitor);
				}
			}
		}

		/// <summary>
		///   Writes this element to the writer.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "map">The map.</param>
		public override void WriteTo (ProjectWriter writer, IDictionary<IPBXElement, string> map)
		{
			writer.WritePBXElementPrologue (2, map, this);
			if (this.BuildConfigurationList != null) {
				writer.WritePBXProperty (3, map, "buildConfigurationList", this.BuildConfigurationList);
			}
			writer.WritePBXProperty (3, map, "buildPhases", this.BuildPhases);
			writer.WritePBXProperty (3, map, "buildRules", this.BuildRules);
			writer.WritePBXProperty (3, map, "dependencies", this.Dependencies);
			writer.WritePBXProperty (3, map, "name", this.Name);
			writer.WritePBXProperty (3, map, "productInstallPath", this.ProductInstallPath);
			writer.WritePBXProperty (3, map, "productName", this.ProductName);
			if (this.ProductReference != null) {
				writer.WritePBXProperty (3, map, "productReference", this.ProductReference);
			}
			writer.WritePBXProperty (3, map, "productType", this.ProductType.ToDescription ());
			writer.WritePBXElementEpilogue (2);
		}
	}
}