//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2014 - Laurent Etiemble
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
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Xcode
{
    public class PBXGroup : PBXFileElement
    {
        private readonly IList<PBXFileElement> children;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXGroup" /> class.
        /// </summary>
        /// <param name = "part"></param>
        public PBXGroup()
        {
            this.children = new List<PBXFileElement>();
            this.SourceTree = PBXSourceTree.Group;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXGroup" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public PBXGroup(String name) : base(name)
        {
            this.children = new List<PBXFileElement>();
            this.SourceTree = PBXSourceTree.Group;
        }

        /// <summary>
        ///   Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public IEnumerable<PBXFileElement> Children
        {
            get { return this.children; }
        }

        /// <summary>
        ///   Adds the child.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void AddChild(PBXFileElement element)
        {
            this.children.Add(element);
        }

        /// <summary>
        ///   Removes the child.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void RemoveChild(PBXFileElement element)
        {
            this.children.Remove(element);
        }

		/// <summary>
		/// Clear all the children.
		/// </summary>
		public void Clear()
		{
			this.children.Clear();
		}

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
        public override void WriteTo(ProjectWriter writer, IDictionary<IPBXElement, string> map)
        {
            writer.WritePBXElementPrologue(2, map, this);
            writer.WritePBXProperty(3, map, "children", this.Children);
            writer.WritePBXProperty(3, map, "name", this.Name);
            writer.WritePBXProperty(3, map, "sourceTree", this.SourceTree.ToDescription());
            writer.WritePBXElementEpilogue(2);
        }

        /// <summary>
        ///   Finds the specified nature.
        /// </summary>
        /// <param name = "nature">The nature.</param>
        /// <param name = "name">The name.</param>
        /// <returns></returns>
        public PBXFileElement Find(PBXElementType nature, String name)
        {
            return this.children.FirstOrDefault(c => c.Nature == nature && String.Equals(c.Name, name));
        }

        /// <summary>
        ///   Finds the group.
        /// </summary>
        /// <param name = "name">The name.</param>
        /// <returns></returns>
        public PBXGroup FindGroup(String name)
        {
            return this.Find(PBXElementType.PBXGroup, name) as PBXGroup;
        }

        /// <summary>
        ///   Finds the file reference.
        /// </summary>
        /// <param name = "name">The name.</param>
        /// <returns></returns>
        public PBXFileReference FindFileReference(String name)
        {
            return this.Find(PBXElementType.PBXFileReference, name) as PBXFileReference;
        }

        /// <summary>
        ///   Finds the file reference.
        /// </summary>
        /// <param name = "name">The name.</param>
        /// <returns></returns>
        public PBXVariantGroup FindVariantGroup(String name)
        {
            return this.Find(PBXElementType.PBXVariantGroup, name) as PBXVariantGroup;
        }
    }
}