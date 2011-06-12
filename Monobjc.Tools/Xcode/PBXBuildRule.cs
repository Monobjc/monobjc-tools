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
    public class PBXBuildRule : PBXElement
    {
        private readonly IList<string> outputFiles;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXBuildRule" /> class.
        /// </summary>
        public PBXBuildRule()
        {
            this.outputFiles = new List<String>();
        }

        /// <summary>
        ///   Gets or sets the compiler spec.
        /// </summary>
        /// <value>The compiler spec.</value>
        public PBXCompilerSpec CompilerSpec { get; set; }

        /// <summary>
        ///   Gets or sets the file patterns.
        /// </summary>
        /// <value>The file patterns.</value>
        public String FilePatterns { get; set; }

        /// <summary>
        ///   Gets or sets the type of the file.
        /// </summary>
        /// <value>The type of the file.</value>
        public PBXFileType FileType { get; set; }

        /// <summary>
        ///   Gets or sets the is editable.
        /// </summary>
        /// <value>The is editable.</value>
        public int IsEditable { get; set; }

        /// <summary>
        ///   Gets the output files.
        /// </summary>
        /// <value>The output files.</value>
        public IEnumerable<String> OutputFiles
        {
            get { return this.outputFiles; }
        }

        /// <summary>
        ///   Adds the output file.
        /// </summary>
        /// <param name = "file">The file.</param>
        public void AddOutputFile(String file)
        {
            this.outputFiles.Add(file);
        }

        /// <summary>
        ///   Removes the output file.
        /// </summary>
        /// <param name = "file">The file.</param>
        public void RemoveOutputFile(String file)
        {
            this.outputFiles.Remove(file);
        }

        /// <summary>
        ///   Gets or sets the script.
        /// </summary>
        /// <value>The script.</value>
        public String Script { get; set; }

        /// <summary>
        ///   Gets the nature.
        /// </summary>
        /// <value>The nature.</value>
        public override PBXElementType Nature
        {
            get { return PBXElementType.PBXBuildRule; }
        }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override String Description
        {
            get { return this.Isa; }
        }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public override void Accept(IPBXVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        ///   Writes this element to the writer.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "map">The map.</param>
        public override void WriteTo(TextWriter writer, IDictionary<IPBXElement, string> map)
        {
            writer.WritePBXElementPrologue(2, map, this);
            writer.WritePBXProperty(3, map, "compilerSpec", this.CompilerSpec);
            writer.WritePBXProperty(3, map, "filePatterns", this.FilePatterns);
            writer.WritePBXProperty(3, map, "fileType", this.FileType);
            writer.WritePBXProperty(3, map, "isEditable", this.IsEditable);
            writer.WritePBXProperty(3, map, "outputFiles", this.OutputFiles);
            writer.WritePBXProperty(3, map, "script", this.Script);
            writer.WritePBXElementEpilogue(2);
        }
    }
}