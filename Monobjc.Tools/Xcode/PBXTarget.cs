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

namespace Monobjc.Tools.Xcode
{
    public abstract class PBXTarget : PBXElement
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXTarget" /> class.
        /// </summary>
        protected PBXTarget()
        {
            this.BuildPhases = new List<PBXBuildPhase>();
            this.Dependencies = new List<PBXTargetDependency>();
        }

        /// <summary>
        ///   Gets or sets the build configuration list.
        /// </summary>
        /// <value>The build configuration list.</value>
        public XCConfigurationList BuildConfigurationList { get; set; }

        /// <summary>
        ///   Gets or sets the dependencies.
        /// </summary>
        /// <value>The dependencies.</value>
        public IList<PBXTargetDependency> Dependencies { get; set; }

        /// <summary>
        ///   Gets or sets the build phases.
        /// </summary>
        /// <value>The build phases.</value>
        public IList<PBXBuildPhase> BuildPhases { get; set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public String Name { get; set; }

        /// <summary>
        ///   Gets or sets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        public String ProductName { get; set; }

        /// <summary>
        ///   Writes this element to the writer.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "map">The map.</param>
        public override void WriteTo(TextWriter writer, IDictionary<IPBXElement, string> map)
        {
            writer.writeElementPrologue(map, this);
            if (this.BuildConfigurationList != null)
            {
                writer.WriteReference(map, "buildConfigurationList", this.BuildConfigurationList);
            }
            writer.WriteReferences(map, "buildPhases", this.BuildPhases);
            writer.WriteReferences(map, "dependencies", this.Dependencies);
            writer.WriteAttribute("name", this.Name);
            writer.WriteAttribute("productName", this.ProductName);
            writer.writeElementEpilogue();
        }
    }
}