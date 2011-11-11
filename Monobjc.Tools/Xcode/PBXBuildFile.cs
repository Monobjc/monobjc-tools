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
using System.Globalization;
using System.IO;

namespace Monobjc.Tools.Xcode
{
    public class PBXBuildFile : PBXElement
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXBuildFile" /> class.
        /// </summary>
        public PBXBuildFile() {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXBuildFile" /> class.
        /// </summary>
        /// <param name = "fileRef">The file ref.</param>
        public PBXBuildFile(PBXFileElement fileRef)
        {
            this.FileRef = fileRef;
        }

        /// <summary>
        ///   Gets or sets the file reference.
        /// </summary>
        /// <value>The file reference.</value>
        public PBXFileElement FileRef { get; set; }

        /// <summary>
        ///   Gets or sets the build phase.
        /// </summary>
        /// <value>The build phase.</value>
        internal PBXBuildPhase BuildPhase { get; set; }

        /// <summary>
        ///   Gets the nature.
        /// </summary>
        /// <value>The nature.</value>
        public override PBXElementType Nature
        {
            get { return PBXElementType.PBXBuildFile; }
        }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get { return String.Format(CultureInfo.CurrentCulture, "{0} in {1}", this.FileRef.Description, this.BuildPhase.Description); }
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
        public override void WriteTo(ProjectWriter writer, IDictionary<IPBXElement, string> map)
        {
            writer.WritePBXElementPrologue(2, map, this, true);
            if (this.FileRef != null)
            {
                writer.WritePBXProperty(3, map, "fileRef", this.FileRef, true);
            }
            writer.WritePBXElementEpilogue(2, true);
        }
    }
}