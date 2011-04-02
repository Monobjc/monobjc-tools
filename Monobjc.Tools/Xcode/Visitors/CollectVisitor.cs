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

namespace Monobjc.Tools.Xcode.Visitors
{
    /// <summary>
    /// </summary>
    public class CollectVisitor : IPBXVisitor
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "CollectVisitor" /> class.
        /// </summary>
        public CollectVisitor()
        {
            this.Map = new Dictionary<IPBXElement, String>();
        }

        /// <summary>
        ///   Gets or sets the map.
        /// </summary>
        /// <value>The map.</value>
        public IDictionary<IPBXElement, String> Map { get; private set; }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXAggregateTarget element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXAppleScriptBuildPhase element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXBuildFile element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXContainerItemProxy element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXCopyFilesBuildPhase element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXFileReference element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXFrameworksBuildPhase element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXGroup element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXHeadersBuildPhase element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXLegacyTarget element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXNativeTarget element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXProject element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXResourcesBuildPhase element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXShellScriptBuildPhase element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXSourcesBuildPhase element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXTargetDependency element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(PBXVariantGroup element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(XCBuildConfiguration element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void Visit(XCConfigurationList element)
        {
            this.Add(element);
        }

        /// <summary>
        ///   Adds the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        private void Add(IPBXElement element)
        {
            if (this.Map.ContainsKey(element))
            {
                return;
            }
            this.Map.Add(element, this.GetElementUID(element));
        }

        /// <summary>
        ///   Gets the element UID.
        /// </summary>
        /// <param name = "element">The element.</param>
        /// <returns></returns>
        private string GetElementUID(IPBXElement element)
        {
            return String.Format(CultureInfo.CurrentCulture, "{0}{1}", element.Nature.GetHashCode().ToString("X8"), element.GetHashCode().ToString("X16"));
        }
    }
}