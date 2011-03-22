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
    /// <summary>
    ///   Visitor contract to visit a tree of project element.
    /// </summary>
    public interface IPBXVisitor
    {
        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXAggregateTarget element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXAppleScriptBuildPhase element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXBuildFile element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXContainerItemProxy element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXCopyFilesBuildPhase element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXFileReference element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXFrameworksBuildPhase element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXGroup element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXHeadersBuildPhase element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXLegacyTarget element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXNativeTarget element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXProject element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXResourcesBuildPhase element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXShellScriptBuildPhase element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXSourcesBuildPhase element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXTargetDependency element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(PBXVariantGroup element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(XCBuildConfiguration element);

        /// <summary>
        ///   Visits the specified element.
        /// </summary>
        /// <param name = "element">The element.</param>
        void Visit(XCConfigurationList element);
    }
}