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
            // Create the main project
            XcodeProject project = new XcodeProject(".", "MyApplication");

            string targetName = "MyApplication";

            project.AddTarget(targetName, PBXProductType.Application);

            project.AddFile("Files", "MyApplicationAppDelegate.h", targetName);
            project.AddFile("Files", "MyApplicationAppDelegate.m", targetName);
            project.AddFile("Files", "Wrong.h", targetName);
            project.RemoveFile("Files", "Wrong.h", targetName);

            project.AddFile("Files", "main.m", targetName);
            project.AddFile("Files", "MyApplication-Info.plist", targetName);

            project.AddFile("Files", "en.lproj/MainMenu.xib", targetName);
            project.AddFile("Files", "fr.lproj/MainMenu.xib", targetName);
            project.RemoveFile("Files", "fr.lproj/MainMenu.xib", targetName);

            project.AddFramework("Frameworks", "Cocoa", targetName);
            project.AddFramework("Frameworks", "AddressBook", targetName);
            project.RemoveFramework("Frameworks", "AddressBook", targetName);

            var frameworks = project.GetFrameworks(targetName);

            project.AddBuildConfigurationSettings("Release", null, "ARCHS", "$(ARCHS_STANDARD_32_64_BIT)");
            project.AddBuildConfigurationSettings("Release", null, "SDKROOT", "macosx");
            project.AddBuildConfigurationSettings("Release", null, "GCC_VERSION", "com.apple.compilers.llvm.clang.1_0");
            project.AddBuildConfigurationSettings("Release", null, "MACOSX_DEPLOYMENT_TARGET", "10.6");
            project.AddBuildConfigurationSettings("Release", null, "GCC_C_LANGUAGE_STANDARD", "gnu99");
            project.AddBuildConfigurationSettings("Release", null, "GCC_WARN_64_TO_32_BIT_CONVERSION", "YES");
            project.AddBuildConfigurationSettings("Release", null, "GCC_WARN_ABOUT_RETURN_TYPE", "YES");
            project.AddBuildConfigurationSettings("Release", null, "GCC_WARN_UNUSED_VARIABLE", "YES");

            project.AddBuildConfigurationSettings("Release", targetName, "DEBUG_INFORMATION_FORMAT", "dwarf-with-dsym");
            project.AddBuildConfigurationSettings("Release", targetName, "COPY_PHASE_STRIP", "YES");
            project.AddBuildConfigurationSettings("Release", targetName, "INFOPLIST_FILE", "MyApplication-Info.plist");
            project.AddBuildConfigurationSettings("Release", targetName, "PRODUCT_NAME", "$(TARGET_NAME)");
            project.AddBuildConfigurationSettings("Release", targetName, "WRAPPER_EXTENSION", "app");
            project.AddBuildConfigurationSettings("Release", targetName, "ALWAYS_SEARCH_USER_PATHS", "NO");
            project.AddBuildConfigurationSettings("Release", targetName, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");

            project.Save();
        }

        //[Test]
        //public void TestProjectGeneration002()
        //{
        //    // Create the dependant project
        //    XcodeProject project1 = new XcodeProject(".", "MyLibrary");

        //    project1.AddFile("Classes", "Classes/AppDelegate.h");
        //    project1.AddFile("Classes", "Classes/Wrong.h");
        //    project1.RemoveFile("Classes", "Classes/Wrong.h");

        //    project1.AddFile("Resources", "en.lproj/MainMenu.xib");
        //    project1.AddFile("Resources", "fr.lproj/MainMenu.xib");
        //    project1.RemoveFile("Resources", "fr.lproj/MainMenu.xib");

        //    project1.AddFramework("Frameworks", "Cocoa");
        //    project1.AddFramework("Frameworks", "AddressBook");
        //    project1.RemoveFramework("Frameworks", "AddressBook");

        //    project1.AddGroup("Products");

        //    XCBuildConfiguration buildConfiguration1 = new XCBuildConfiguration("Release");
        //    buildConfiguration1.BuildSettings.Add("ARCHS", "$(ARCHS_STANDARD_32_64_BIT)");
        //    buildConfiguration1.BuildSettings.Add("MACOSX_DEPLOYMENT_TARGET", "10.6");
        //    buildConfiguration1.BuildSettings.Add("SDKROOT", "macosx");
        //    project1.AddBuildConfiguration(buildConfiguration1, null);

        //    project1.Save();

        //    // Create the main project
        //    XcodeProject project2 = new XcodeProject(".", targetName);

        //    project2.AddFile("Classes", "Classes/AppDelegate.h");
        //    project2.AddFile("Classes", "Classes/Wrong.h");
        //    project2.RemoveFile("Classes", "Classes/Wrong.h");

        //    project2.AddFile("Resources", "en.lproj/MainMenu.xib");
        //    project2.AddFile("Resources", "fr.lproj/MainMenu.xib");
        //    project2.RemoveFile("Resources", "fr.lproj/MainMenu.xib");

        //    project2.AddFramework("Frameworks", "Cocoa");
        //    project2.AddFramework("Frameworks", "AddressBook");
        //    project2.RemoveFramework("Frameworks", "AddressBook");

        //    project2.AddGroup("Products");

        //    XCBuildConfiguration buildConfiguration2 = new XCBuildConfiguration("Release");
        //    buildConfiguration2.BuildSettings.Add("ARCHS", "$(ARCHS_STANDARD_32_64_BIT)");
        //    buildConfiguration2.BuildSettings.Add("MACOSX_DEPLOYMENT_TARGET", "10.6");
        //    buildConfiguration2.BuildSettings.Add("SDKROOT", "macosx");
        //    project2.AddBuildConfiguration(buildConfiguration2, null);

        //    project2.AddDependantProject(project1);

        //    project2.Save();
        //}
    }
}