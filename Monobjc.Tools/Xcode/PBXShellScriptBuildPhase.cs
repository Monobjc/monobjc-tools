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
    public class PBXShellScriptBuildPhase : PBXBuildPhase
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXShellScriptBuildPhase" /> class.
        /// </summary>
        public PBXShellScriptBuildPhase()
        {
            this.InputPaths = new List<String>();
            this.OutputPaths = new List<String>();
            this.ShellPath = String.Empty;
            this.ShellScript = String.Empty;
        }

        /// <summary>
        ///   Gets or sets the input paths.
        /// </summary>
        /// <value>The input paths.</value>
        public IList<string> InputPaths { get; set; }

        /// <summary>
        ///   Gets or sets the output paths.
        /// </summary>
        /// <value>The output paths.</value>
        public IList<string> OutputPaths { get; set; }

        /// <summary>
        ///   Gets or sets the shell path.
        /// </summary>
        /// <value>The shell path.</value>
        public String ShellPath { get; set; }

        /// <summary>
        ///   Gets or sets the shell script.
        /// </summary>
        /// <value>The shell script.</value>
        public String ShellScript { get; set; }

        /// <summary>
        ///   Gets the elemnt's nature.
        /// </summary>
        /// <value>The nature.</value>
        public override PBXElementType Nature
        {
            get { return PBXElementType.PBXShellScriptBuildPhase; }
        }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get { return "Script"; }
        }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public override void Accept(IPBXVisitor visitor)
        {
            visitor.Visit(this);

            if (this.Files != null)
            {
                foreach (PBXFileReference file in this.Files)
                {
                    file.Accept(visitor);
                }
            }
        }

        /// <summary>
        ///   Writes this element to the writer.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "map">The map.</param>
        public override void WriteTo(TextWriter writer, IDictionary<IPBXElement, string> map)
        {
            writer.writeElementPrologue(map, this);
            writer.WriteAttribute("buildActionMask", this.BuildActionMask);
            writer.WriteReferences(map, "files", this.Files);
            writer.WriteList("inputPaths", this.InputPaths);
            writer.WriteList("outputPaths", this.OutputPaths);
            writer.WriteAttribute("runOnlyForDeploymentPostprocessing", this.RunOnlyForDeploymentPostprocessing);
            writer.WriteAttribute("shellPath", this.ShellPath);
            writer.WriteAttribute("shellScript", this.ShellScript);
            writer.writeElementEpilogue();
        }
    }
}