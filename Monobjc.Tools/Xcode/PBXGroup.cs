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
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Xcode
{
    public class PBXGroup : PBXFileElement
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXGroup" /> class.
        /// </summary>
        public PBXGroup()
        {
            this.Children = new List<PBXFileElement>();
            this.SourceTree = PBXSourceTree.Group;
        }

        /// <summary>
        ///   Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public IList<PBXFileElement> Children { get; set; }

        /// <summary>
        ///   Gets or sets the source tree.
        /// </summary>
        /// <value>The source tree.</value>
        public PBXSourceTree SourceTree { get; set; }

        /// <summary>
        ///   Gets the elemnt's nature.
        /// </summary>
        /// <value>The nature.</value>
        public override PBXElementType Nature
        {
            get { return PBXElementType.PBXGroup; }
        }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get { return "Group"; }
        }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public override void Accept(IPBXVisitor visitor)
        {
            visitor.Visit(this);

            if (this.Children != null)
            {
                foreach (PBXFileElement target in this.Children)
                {
                    target.Accept(visitor);
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
            writer.WriteReferences(map, "children", this.Children);
            writer.WriteAttribute("compatibilityVersion", this.Name);
            writer.WriteAttribute("sourceTree", this.SourceTree.ToDescription());
            writer.writeElementEpilogue();
        }
    }
}