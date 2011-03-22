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
        private XCConfigurationList buildConfigurationList;

        private readonly IList<PBXBuildPhase> buildPhases;

        private readonly IList<PBXTargetDependency> dependencies;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXTarget" /> class.
        /// </summary>
        protected PBXTarget()
        {
            this.BuildConfigurationList = new XCConfigurationList();
            this.buildPhases = new List<PBXBuildPhase>();
            this.dependencies = new List<PBXTargetDependency>();
        }

        /// <summary>
        ///   Gets or sets the build configuration list.
        /// </summary>
        /// <value>The build configuration list.</value>
        public XCConfigurationList BuildConfigurationList
        {
            get { return this.buildConfigurationList; }
            set
            {
                this.buildConfigurationList = value;
                this.buildConfigurationList.Target = this;
            }
        }

        /// <summary>
        ///   Gets or sets the build phases.
        /// </summary>
        /// <value>The build phases.</value>
        public IEnumerable<PBXBuildPhase> BuildPhases
        {
            get { return this.buildPhases; }
        }

        /// <summary>
        ///   Adds the build phase.
        /// </summary>
        /// <param name = "phase">The phase.</param>
        public void AddBuildPhase(PBXBuildPhase phase)
        {
            this.buildPhases.Add(phase);
        }

        /// <summary>
        ///   Removes the build phase.
        /// </summary>
        /// <param name = "phase">The phase.</param>
        public void RemoveBuildPhase(PBXBuildPhase phase)
        {
            this.buildPhases.Remove(phase);
        }

        /// <summary>
        ///   Gets or sets the dependencies.
        /// </summary>
        /// <value>The dependencies.</value>
        public IEnumerable<PBXTargetDependency> Dependencies
        {
            get { return this.dependencies; }
        }

        /// <summary>
        ///   Adds the target dependency.
        /// </summary>
        /// <param name = "dependency">The dependency.</param>
        public void AddTargetDependency(PBXTargetDependency dependency)
        {
            this.dependencies.Add(dependency);
        }

        /// <summary>
        ///   Removes the target dependency.
        /// </summary>
        /// <param name = "dependency">The dependency.</param>
        public void RemoveTargetDependency(PBXTargetDependency dependency)
        {
            this.dependencies.Remove(dependency);
        }

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
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get { return this.Name; }
        }

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