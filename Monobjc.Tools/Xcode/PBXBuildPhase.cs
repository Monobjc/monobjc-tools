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
using System.Linq;

namespace Monobjc.Tools.Xcode
{
    public abstract class PBXBuildPhase : PBXElement
    {
        private readonly IList<PBXBuildFile> files;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXBuildPhase" /> class.
        /// </summary>
        protected PBXBuildPhase()
        {
            this.BuildActionMask = Int32.MaxValue;
            this.files = new List<PBXBuildFile>();
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
        public IEnumerable<PBXBuildFile> Files
        {
            get { return this.files; }
        }

        /// <summary>
        ///   Adds the file.
        /// </summary>
        /// <param name = "file">The file.</param>
        public void AddFile(PBXBuildFile file)
        {
            file.BuildPhase = this;
            this.files.Add(file);
        }

        /// <summary>
        ///   Removes the file.
        /// </summary>
        /// <param name = "file">The file.</param>
        public void RemoveFile(PBXBuildFile file)
        {
            file.BuildPhase = null;
            this.files.Remove(file);
        }

        public PBXBuildFile FindFile(PBXFileElement file)
        {
            return this.files.FirstOrDefault(f => f.FileRef == file);
        }

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
            writer.WritePBXElementPrologue(2, map, this);
            writer.WritePBXProperty(3, map, "buildActionMask", this.BuildActionMask);
            writer.WritePBXProperty(3, map, "files", this.Files);
            writer.WritePBXProperty(3, map, "runOnlyForDeploymentPostprocessing", this.RunOnlyForDeploymentPostprocessing);
            writer.WritePBXElementEpilogue(2);
        }
    }
}