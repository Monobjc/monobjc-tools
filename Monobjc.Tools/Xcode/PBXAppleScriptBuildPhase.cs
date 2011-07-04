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
using System.Collections.Generic;
using System.IO;

namespace Monobjc.Tools.Xcode
{
    public class PBXAppleScriptBuildPhase : PBXBuildPhase
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXAppleScriptBuildPhase" /> class.
        /// </summary>
        public PBXAppleScriptBuildPhase()
        {
            this.ContextName = string.Empty;
            this.IsSharedContext = 0;
        }

        /// <summary>
        ///   Gets or sets the name of the context.
        /// </summary>
        /// <value>The name of the context.</value>
        public string ContextName { get; set; }

        /// <summary>
        ///   Gets or sets the is shared context.
        /// </summary>
        /// <value>The is shared context.</value>
        public int IsSharedContext { get; set; }

        /// <summary>
        ///   Gets the elemnt's nature.
        /// </summary>
        /// <value>
        ///   The nature.
        /// </value>
        public override PBXElementType Nature
        {
            get { return PBXElementType.PBXAppleScriptBuildPhase; }
        }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get { return "AppleScript"; }
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
                foreach (PBXBuildFile file in this.Files)
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
            writer.WritePBXElementPrologue(2, map, this);
            writer.WritePBXProperty(3, map, "buildActionMask", this.BuildActionMask);
            writer.WritePBXProperty(3, map, "contextName", this.ContextName);
            writer.WritePBXProperty(3, map, "files", this.Files);
            writer.WritePBXProperty(3, map, "isSharedContext", this.IsSharedContext);
            writer.WritePBXProperty(3, map, "runOnlyForDeploymentPostprocessing", this.RunOnlyForDeploymentPostprocessing);
            writer.WritePBXElementEpilogue(2);
        }
    }
}