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
using System.IO;

namespace Monobjc.Tools.Xcode
{
    public class PBXLegacyTarget : PBXTarget
    {
        /// <summary>
        ///   Gets or sets the build arguments string.
        /// </summary>
        /// <value>The build arguments string.</value>
        public String BuildArgumentsString { get; set; }

        /// <summary>
        ///   Gets or sets the build tool path.
        /// </summary>
        /// <value>The build tool path.</value>
        public String BuildToolPath { get; set; }

        /// <summary>
        ///   Gets or sets the build working directory.
        /// </summary>
        /// <value>The build working directory.</value>
        public String BuildWorkingDirectory { get; set; }

        /// <summary>
        ///   Gets or sets the pass build settings in environment.
        /// </summary>
        /// <value>The pass build settings in environment.</value>
        public int PassBuildSettingsInEnvironment { get; set; }

        /// <summary>
        ///   Gets the nature.
        /// </summary>
        /// <value>The nature.</value>
        public override PBXElementType Nature
        {
            get { return PBXElementType.PBXLegacyTarget; }
        }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public override void Accept(IPBXVisitor visitor)
        {
            visitor.Visit(this);

            if (this.BuildConfigurationList != null)
            {
                this.BuildConfigurationList.Accept(visitor);
            }
            if (this.BuildPhases != null)
            {
                foreach (PBXBuildPhase phase in this.BuildPhases)
                {
                    phase.Accept(visitor);
                }
            }
            if (this.Dependencies != null)
            {
                foreach (PBXTargetDependency dependency in this.Dependencies)
                {
                    dependency.Accept(visitor);
                }
            }
        }

        /// <summary>
        ///   Writes this element to the writer.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "map">The map.</param>
        public override void WriteTo(ProjectWriter writer, IDictionary<IPBXElement, string> map)
        {
            writer.WritePBXElementPrologue(2, map, this);
            writer.WritePBXProperty(3, map, "buildArgumentsString", this.BuildArgumentsString);
            if (this.BuildConfigurationList != null)
            {
                writer.WritePBXProperty(3, map, "buildConfigurationList", this.BuildConfigurationList);
            }
            writer.WritePBXProperty(3, map, "buildPhases", this.BuildPhases);
            writer.WritePBXProperty(3, map, "buildToolPath", this.BuildToolPath);
            writer.WritePBXProperty(3, map, "buildWorkingDirectory", this.BuildWorkingDirectory);
            writer.WritePBXProperty(3, map, "dependencies", this.Dependencies);
            writer.WritePBXProperty(3, map, "name", this.Name);
            writer.WritePBXProperty(3, map, "passBuildSettingsInEnvironment", this.PassBuildSettingsInEnvironment);
            writer.WritePBXProperty(3, map, "productName", this.ProductName);
            writer.WritePBXElementEpilogue(2);
        }
    }
}