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
    public abstract class PBXBuildPhase : PBXElement
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXBuildPhase" /> class.
        /// </summary>
        protected PBXBuildPhase()
        {
            this.BuildActionMask = Int32.MaxValue;
            this.Files = new List<PBXFileReference>();
            this.RunOnlyForDeploymentPostprocessing = 0;
        }

        /// <summary>
        ///   Gets or sets the build action mask.
        /// </summary>
        /// <value>The build action mask.</value>
        public int BuildActionMask { get; set; }

        /// <summary>
        ///   Gets or sets the files.
        /// </summary>
        /// <value>The files.</value>
        public IList<PBXFileReference> Files { get; set; }

        /// <summary>
        ///   Gets or sets the flag to run only for deployment postprocessing.
        /// </summary>
        /// <value>The flag to run only for deployment postprocessing.</value>
        public int RunOnlyForDeploymentPostprocessing { get; set; }

        /// <summary>
        ///   Writes this element to the writer.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "map">The map.</param>
        public override void WriteTo(TextWriter writer, IDictionary<IPBXElement, string> map)
        {
            writer.writeElementPrologue(map, this);
            writer.WriteAttribute("buildActionMask", this.BuildActionMask);
            writer.WriteReferences(map, "targets", this.Files);
            writer.WriteAttribute("runOnlyForDeploymentPostprocessing", this.RunOnlyForDeploymentPostprocessing);
            writer.writeElementEpilogue();
        }
    }
}