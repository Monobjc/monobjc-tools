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
using System.IO;
using Monobjc.Tools.Properties;
using Monobjc.Tools.PropertyList;
using NUnit.Framework;
using Monobjc.Tools.Xcode;

namespace Monobjc.Tools
{
    [TestFixture]
    [Category("PBX")]
    [Category("Generation")]
    public class PBXProjectGenerationTests
    {
        [Test]
        public void TestProjectGeneration001()
        {
            // Create the document
            PBXDocument document = new PBXDocument();
            PBXProject project = document.Project;

            // Add build configuration "Debug"
            XCBuildConfiguration debugConfiguration = new XCBuildConfiguration();
            project.BuildConfigurationList.AddBuildConfiguration(debugConfiguration);

            // Add build configuration "Release"
            XCBuildConfiguration releaseConfiguration = new XCBuildConfiguration();
            project.BuildConfigurationList.AddBuildConfiguration(releaseConfiguration);

            PBXFileReference cocoaFrameworkFile = new PBXFileReference();
            cocoaFrameworkFile.LastKnownFileType = PBXFileType.WrapperFramework;
            cocoaFrameworkFile.Name = "Cocoa.framework";
            cocoaFrameworkFile.Path = "/System/Library/Frameworks/Cocoa.framework";
            cocoaFrameworkFile.SourceTree = PBXSourceTree.Absolute;

            PBXBuildFile cocoaFrameworkBuildFile = new PBXBuildFile();
            cocoaFrameworkBuildFile.FileRef = cocoaFrameworkFile;

            // Add groups
            project.MainGroup.Name = "MyApplication";

            PBXGroup frameworkGroup = new PBXGroup();
            frameworkGroup.Name = "Linked Frameworks";
            frameworkGroup.AddChild(cocoaFrameworkFile);

            project.MainGroup.AddChild(frameworkGroup);

            // Add target for Application
            PBXResourcesBuildPhase resources = new PBXResourcesBuildPhase();
            // TODO
            PBXSourcesBuildPhase sources = new PBXSourcesBuildPhase();
            // TODO
            PBXFrameworksBuildPhase frameworks = new PBXFrameworksBuildPhase();
            frameworks.AddFile(cocoaFrameworkBuildFile);

            PBXNativeTarget nativeTarget = new PBXNativeTarget();
            nativeTarget.Name = "MyApplication";
            nativeTarget.AddBuildPhase(resources);
            nativeTarget.AddBuildPhase(sources);
            nativeTarget.AddBuildPhase(frameworks);

            project.AddTarget(nativeTarget);

            document.WriteToFile("project-001.pbxproj");
        }
    }
}
