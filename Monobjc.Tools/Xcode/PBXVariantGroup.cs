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
namespace Monobjc.Tools.Xcode
{
    public class PBXVariantGroup : PBXGroup
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXVariantGroup" /> class.
        /// </summary>
        public PBXVariantGroup() {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXVariantGroup" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public PBXVariantGroup(string name) : base(name) {}

        /// <summary>
        ///   Gets the elemnt's nature.
        /// </summary>
        /// <value>The nature.</value>
        public override PBXElementType Nature
        {
            get { return PBXElementType.PBXVariantGroup; }
        }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get { return "Variant"; }
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
    }
}