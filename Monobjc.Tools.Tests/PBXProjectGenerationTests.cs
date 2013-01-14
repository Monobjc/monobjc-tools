//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2013 - Laurent Etiemble
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
using Monobjc.Tools.Xcode;
using NUnit.Framework;

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

            PBXFileReference file1 = new PBXFileReference();
            file1.LastKnownFileType = PBXFileType.WrapperFramework;
            file1.Name = "Cocoa.framework";
            file1.Path = "/System/Library/Frameworks/Cocoa.framework";
            file1.LastKnownFileType = PBXFileType.WrapperFramework;
            file1.SourceTree = PBXSourceTree.SdkRoot;

            PBXFileReference file2 = new PBXFileReference();
            file2.LastKnownFileType = PBXFileType.WrapperFramework;
            file2.Name = "SurrogateTestAppDelegate.h";
            file2.Path = "SurrogateTestAppDelegate.h";
            file2.LastKnownFileType = PBXFileType.SourcecodeCH;
            file2.SourceTree = PBXSourceTree.Group;

            PBXFileReference file3 = new PBXFileReference();
            file3.LastKnownFileType = PBXFileType.WrapperFramework;
            file3.Name = "en";
            file3.Path = "en.lproj/MainMenu.xib";
            file3.LastKnownFileType = PBXFileType.FileXib;
            file3.SourceTree = PBXSourceTree.Group;

            PBXFileReference file4 = new PBXFileReference();
            file4.LastKnownFileType = PBXFileType.WrapperFramework;
            file4.Name = "fr";
            file4.Path = "fr.lproj/MainMenu.xib";
            file4.LastKnownFileType = PBXFileType.FileXib;
            file4.SourceTree = PBXSourceTree.Group;

            PBXVariantGroup variantGroup = new PBXVariantGroup("MainMenu.xib");
            variantGroup.SourceTree = PBXSourceTree.Group;
            variantGroup.AddChild(file3);
            variantGroup.AddChild(file4);

            PBXGroup group1 = new PBXGroup("Products");
            group1.SourceTree = PBXSourceTree.Group;

            PBXGroup group2 = new PBXGroup("Frameworks");
            group2.SourceTree = PBXSourceTree.Group;
            group2.AddChild(file1);

            PBXGroup group3 = new PBXGroup("Classes");
            group3.SourceTree = PBXSourceTree.Group;
            group3.AddChild(file2);

            PBXGroup group4 = new PBXGroup("Resources");
            group4.SourceTree = PBXSourceTree.Group;
            group4.AddChild(variantGroup);

            PBXGroup group5 = document.Project.MainGroup;
            group5.SourceTree = PBXSourceTree.Group;
            group5.AddChild(group3);
            group5.AddChild(group4);
            group5.AddChild(group2);
            group5.AddChild(group1);

            document.Project.ProductRefGroup = group1;

            // Add build configuration "Release"
            XCBuildConfiguration releaseConfiguration = new XCBuildConfiguration();
            releaseConfiguration.Name = "Release";
            project.BuildConfigurationList.AddBuildConfiguration(releaseConfiguration);
            project.BuildConfigurationList.DefaultConfigurationName = "Release";

            document.WriteToFile("project-001.pbxproj");
        }
		
        [Test]
        public void TestProjectGeneration002()
        {
            // Create the document
            PBXDocument document = new PBXDocument();
            PBXProject project = document.Project;
			
			project.ProjectDirPath = "../..";

            PBXFileReference file1 = new PBXFileReference();
            file1.LastKnownFileType = PBXFileType.WrapperFramework;
            file1.Name = "Cocoa.framework";
            file1.Path = "/System/Library/Frameworks/Cocoa.framework";
            file1.LastKnownFileType = PBXFileType.WrapperFramework;
            file1.SourceTree = PBXSourceTree.SdkRoot;

            PBXFileReference file2 = new PBXFileReference();
            file2.LastKnownFileType = PBXFileType.WrapperFramework;
            file2.Name = "SurrogateTestAppDelegate.h";
            file2.Path = "SurrogateTestAppDelegate.h";
            file2.LastKnownFileType = PBXFileType.SourcecodeCH;
            file2.SourceTree = PBXSourceTree.Group;

            PBXFileReference file3 = new PBXFileReference();
            file3.LastKnownFileType = PBXFileType.WrapperFramework;
            file3.Name = "en";
            file3.Path = "en.lproj/MainMenu.xib";
            file3.LastKnownFileType = PBXFileType.FileXib;
            file3.SourceTree = PBXSourceTree.Group;

            PBXFileReference file4 = new PBXFileReference();
            file4.LastKnownFileType = PBXFileType.WrapperFramework;
            file4.Name = "fr";
            file4.Path = "fr.lproj/MainMenu.xib";
            file4.LastKnownFileType = PBXFileType.FileXib;
            file4.SourceTree = PBXSourceTree.Group;

            PBXVariantGroup variantGroup = new PBXVariantGroup("MainMenu.xib");
            variantGroup.SourceTree = PBXSourceTree.Group;
            variantGroup.AddChild(file3);
            variantGroup.AddChild(file4);

            PBXGroup group1 = new PBXGroup("Products");
            group1.SourceTree = PBXSourceTree.Group;

            PBXGroup group2 = new PBXGroup("Frameworks");
            group2.SourceTree = PBXSourceTree.Group;
            group2.AddChild(file1);

            PBXGroup group3 = new PBXGroup("Classes");
            group3.SourceTree = PBXSourceTree.Group;
            group3.AddChild(file2);

            PBXGroup group4 = new PBXGroup("Resources");
            group4.SourceTree = PBXSourceTree.Group;
            group4.AddChild(variantGroup);

            PBXGroup group5 = document.Project.MainGroup;
            group5.SourceTree = PBXSourceTree.Group;
            group5.AddChild(group3);
            group5.AddChild(group4);
            group5.AddChild(group2);
            group5.AddChild(group1);

            document.Project.ProductRefGroup = group1;

            // Add build configuration "Release"
            XCBuildConfiguration releaseConfiguration = new XCBuildConfiguration();
            releaseConfiguration.Name = "Release";
            project.BuildConfigurationList.AddBuildConfiguration(releaseConfiguration);
            project.BuildConfigurationList.DefaultConfigurationName = "Release";

            document.WriteToFile("project-002.pbxproj");
        }
	}
}