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
using Monobjc.Tools.Xcode;
using NUnit.Framework;

namespace Monobjc.Tools
{
    [TestFixture]
    [Category("Xcode")]
    [Category("Generation")]
    public class XcodeProjectGenerationTests
    {
        [Test]
        public void TestProjectGeneration001()
        {
            // Create the dependant project
            XcodeProject project1 = new XcodeProject(".", "MyLibrary");

            project1.AddFile("Classes", "Classes/AppDelegate.h");
            project1.AddFile("Classes", "Classes/Wrong.h");
            project1.RemoveFile("Classes", "Classes/Wrong.h");

            project1.AddFile("Resources", "en.lproj/MainMenu.xib");
            project1.AddFile("Resources", "fr.lproj/MainMenu.xib");
            project1.RemoveFile("Resources", "fr.lproj/MainMenu.xib");

            project1.AddFramework("Frameworks", "Cocoa");
            project1.AddFramework("Frameworks", "AddressBook");
            project1.RemoveFramework("Frameworks", "AddressBook");

            project1.AddGroup("Products");

            XCBuildConfiguration buildConfiguration1 = new XCBuildConfiguration("Release");
            buildConfiguration1.BuildSettings.Add("ARCHS", "$(ARCHS_STANDARD_32_64_BIT)");
            buildConfiguration1.BuildSettings.Add("MACOSX_DEPLOYMENT_TARGET", "10.6");
            buildConfiguration1.BuildSettings.Add("SDKROOT", "macosx");
            project1.AddBuildConfiguration(buildConfiguration1, null);

            project1.Save();

            // Create the main project
            XcodeProject project2 = new XcodeProject(".", "MyApplication");

            project2.AddFile("Classes", "Classes/AppDelegate.h");
            project2.AddFile("Classes", "Classes/Wrong.h");
            project2.RemoveFile("Classes", "Classes/Wrong.h");

            project2.AddFile("Resources", "en.lproj/MainMenu.xib");
            project2.AddFile("Resources", "fr.lproj/MainMenu.xib");
            project2.RemoveFile("Resources", "fr.lproj/MainMenu.xib");

            project2.AddFramework("Frameworks", "Cocoa");
            project2.AddFramework("Frameworks", "AddressBook");
            project2.RemoveFramework("Frameworks", "AddressBook");

            project2.AddGroup("Products");

            XCBuildConfiguration buildConfiguration2 = new XCBuildConfiguration("Release");
            buildConfiguration2.BuildSettings.Add("ARCHS", "$(ARCHS_STANDARD_32_64_BIT)");
            buildConfiguration2.BuildSettings.Add("MACOSX_DEPLOYMENT_TARGET", "10.6");
            buildConfiguration2.BuildSettings.Add("SDKROOT", "macosx");
            project2.AddBuildConfiguration(buildConfiguration2, null);

            project2.AddDependantProject(project1);

            project2.Save();
        }
    }
}