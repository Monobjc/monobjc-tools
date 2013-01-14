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
    [Category("Xcode")]
    [Category("Generation")]
    public class XcodeProjectGenerationTests
    {
        [Test]
        public void TestProjectGeneration001()
        {
            // Create the main project
            string targetName = "MyApplication001";
            XcodeProject project = new XcodeProject(".", targetName);

            project.AddTarget(targetName, PBXProductType.Application);
			
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
			project.AddBuildConfigurationSettings ("Release", targetName, "INFOPLIST_FILE", "../../Info.plist");
            project.AddBuildConfigurationSettings("Release", targetName, "PRODUCT_NAME", "$(TARGET_NAME)");
            project.AddBuildConfigurationSettings("Release", targetName, "WRAPPER_EXTENSION", "app");
            project.AddBuildConfigurationSettings("Release", targetName, "ALWAYS_SEARCH_USER_PATHS", "NO");
            project.AddBuildConfigurationSettings("Release", targetName, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");

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



            //var frameworks = project.GetFrameworks(targetName);



            project.Save();
        }

		[Test]
		public void TestProjectGeneration002()
		{
		    // Create the dependant project
            string targetName = "MyLibrary002";
		    XcodeProject project1 = new XcodeProject(".", targetName);		
			
			project1.AddTarget (targetName, PBXProductType.LibraryDynamic);
			
            project1.AddBuildConfigurationSettings("Release", null, "ARCHS", "$(ARCHS_STANDARD_32_64_BIT)");
            project1.AddBuildConfigurationSettings("Release", null, "SDKROOT", "macosx");
            project1.AddBuildConfigurationSettings("Release", null, "GCC_VERSION", "com.apple.compilers.llvm.clang.1_0");
            project1.AddBuildConfigurationSettings("Release", null, "MACOSX_DEPLOYMENT_TARGET", "10.6");
            project1.AddBuildConfigurationSettings("Release", null, "GCC_C_LANGUAGE_STANDARD", "gnu99");
            project1.AddBuildConfigurationSettings("Release", null, "GCC_WARN_64_TO_32_BIT_CONVERSION", "YES");
            project1.AddBuildConfigurationSettings("Release", null, "GCC_WARN_ABOUT_RETURN_TYPE", "YES");
            project1.AddBuildConfigurationSettings("Release", null, "GCC_WARN_UNUSED_VARIABLE", "YES");
			
			project1.AddBuildConfigurationSettings ("Release", targetName, "ALWAYS_SEARCH_USER_PATHS", "NO");
			project1.AddBuildConfigurationSettings ("Release", targetName, "COPY_PHASE_STRIP", "YES");
			project1.AddBuildConfigurationSettings ("Release", targetName, "DEBUG_INFORMATION_FORMAT", "dwarf-with-dsym");
			project1.AddBuildConfigurationSettings ("Release", targetName, "DYLIB_COMPATIBILITY_VERSION", "1");
			project1.AddBuildConfigurationSettings ("Release", targetName, "DYLIB_CURRENT_VERSION", "1");
			project1.AddBuildConfigurationSettings ("Release", targetName, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
			project1.AddBuildConfigurationSettings ("Release", targetName, "PRODUCT_NAME", "$(TARGET_NAME)");
			
		    project1.AddFile("Classes", "Classes/AppDelegate.h", targetName);
		    project1.AddFile("Classes", "Classes/Wrong.h", targetName);
		    project1.RemoveFile("Classes", "Classes/Wrong.h", targetName);
		
		    project1.AddFile("Resources", "en.lproj/MainMenu.xib", targetName);
		    project1.AddFile("Resources", "fr.lproj/MainMenu.xib", targetName);
		    project1.RemoveFile("Resources", "fr.lproj/MainMenu.xib", targetName);
		
            project1.AddFramework("Frameworks", "Cocoa", targetName);
            project1.AddFramework("Frameworks", "AddressBook", targetName);
            project1.RemoveFramework("Frameworks", "AddressBook", targetName);
		
		    project1.Save();
		
		    // Create the main project
			targetName = "MyApplication002";
		    XcodeProject project2 = new XcodeProject(".", targetName);
		
			project2.AddTarget (targetName, PBXProductType.Application);
			
            project2.AddBuildConfigurationSettings("Release", null, "ARCHS", "$(ARCHS_STANDARD_32_64_BIT)");
            project2.AddBuildConfigurationSettings("Release", null, "SDKROOT", "macosx");
            project2.AddBuildConfigurationSettings("Release", null, "GCC_VERSION", "com.apple.compilers.llvm.clang.1_0");
            project2.AddBuildConfigurationSettings("Release", null, "MACOSX_DEPLOYMENT_TARGET", "10.6");
            project2.AddBuildConfigurationSettings("Release", null, "GCC_C_LANGUAGE_STANDARD", "gnu99");
            project2.AddBuildConfigurationSettings("Release", null, "GCC_WARN_64_TO_32_BIT_CONVERSION", "YES");
            project2.AddBuildConfigurationSettings("Release", null, "GCC_WARN_ABOUT_RETURN_TYPE", "YES");
            project2.AddBuildConfigurationSettings("Release", null, "GCC_WARN_UNUSED_VARIABLE", "YES");
			
			project2.AddBuildConfigurationSettings ("Release", targetName, "DEBUG_INFORMATION_FORMAT", "dwarf-with-dsym");
			project2.AddBuildConfigurationSettings ("Release", targetName, "COPY_PHASE_STRIP", "YES");
			project2.AddBuildConfigurationSettings ("Release", targetName, "INFOPLIST_FILE", "../../Info.plist");
			project2.AddBuildConfigurationSettings ("Release", targetName, "PRODUCT_NAME", "$(TARGET_NAME)");
			project2.AddBuildConfigurationSettings ("Release", targetName, "WRAPPER_EXTENSION", "app");
			project2.AddBuildConfigurationSettings ("Release", targetName, "ALWAYS_SEARCH_USER_PATHS", "NO");
			project2.AddBuildConfigurationSettings ("Release", targetName, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
			
            project2.AddFile("Files", "MyApplicationAppDelegate.h", targetName);
            project2.AddFile("Files", "MyApplicationAppDelegate.m", targetName);
            project2.AddFile("Files", "Wrong.h", targetName);
            project2.RemoveFile("Files", "Wrong.h", targetName);

            project2.AddFile("Files", "main.m", targetName);
            project2.AddFile("Files", "MyApplication-Info.plist", targetName);

            project2.AddFile("Files", "en.lproj/MainMenu.xib", targetName);
            project2.AddFile("Files", "fr.lproj/MainMenu.xib", targetName);
            project2.RemoveFile("Files", "fr.lproj/MainMenu.xib", targetName);

            project2.AddFramework("Frameworks", "Cocoa", targetName);
            project2.AddFramework("Frameworks", "AddressBook", targetName);
            project2.RemoveFramework("Frameworks", "AddressBook", targetName);
			
		    project2.AddDependantProject(project1, targetName);
		
		    project2.Save();
		}

		[Test]
		public void TestProjectGeneration003()
		{
		    // Create the dependant project
            string targetName = "MyLibrary003";
		    XcodeProject project1 = new XcodeProject(".", targetName);		
			
			project1.AddTarget (targetName, PBXProductType.LibraryDynamic);
			
            project1.AddBuildConfigurationSettings("Release", null, "ARCHS", "$(ARCHS_STANDARD_32_64_BIT)");
            project1.AddBuildConfigurationSettings("Release", null, "SDKROOT", "macosx");
            project1.AddBuildConfigurationSettings("Release", null, "GCC_VERSION", "com.apple.compilers.llvm.clang.1_0");
            project1.AddBuildConfigurationSettings("Release", null, "MACOSX_DEPLOYMENT_TARGET", "10.6");
            project1.AddBuildConfigurationSettings("Release", null, "GCC_C_LANGUAGE_STANDARD", "gnu99");
            project1.AddBuildConfigurationSettings("Release", null, "GCC_WARN_64_TO_32_BIT_CONVERSION", "YES");
            project1.AddBuildConfigurationSettings("Release", null, "GCC_WARN_ABOUT_RETURN_TYPE", "YES");
            project1.AddBuildConfigurationSettings("Release", null, "GCC_WARN_UNUSED_VARIABLE", "YES");
			
			project1.AddBuildConfigurationSettings ("Release", targetName, "ALWAYS_SEARCH_USER_PATHS", "NO");
			project1.AddBuildConfigurationSettings ("Release", targetName, "COPY_PHASE_STRIP", "YES");
			project1.AddBuildConfigurationSettings ("Release", targetName, "DEBUG_INFORMATION_FORMAT", "dwarf-with-dsym");
			project1.AddBuildConfigurationSettings ("Release", targetName, "DYLIB_COMPATIBILITY_VERSION", "1");
			project1.AddBuildConfigurationSettings ("Release", targetName, "DYLIB_CURRENT_VERSION", "1");
			project1.AddBuildConfigurationSettings ("Release", targetName, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
			project1.AddBuildConfigurationSettings ("Release", targetName, "PRODUCT_NAME", "$(TARGET_NAME)");
			
		    project1.AddFile("Classes", "Classes/AppDelegate.h", targetName);
		    project1.AddFile("Classes", "Classes/Wrong.h", targetName);
		    project1.RemoveFile("Classes", "Classes/Wrong.h", targetName);
		
		    project1.AddFile("Resources", "en.lproj/MainMenu.xib", targetName);
		    project1.AddFile("Resources", "fr.lproj/MainMenu.xib", targetName);
		    project1.RemoveFile("Resources", "fr.lproj/MainMenu.xib", targetName);
		
            project1.AddFramework("Frameworks", "Cocoa", targetName);
            project1.AddFramework("Frameworks", "AddressBook", targetName);
            project1.RemoveFramework("Frameworks", "AddressBook", targetName);
		
		    project1.Save();
		
		    // Create the main project
			targetName = "MyApplication003";
		    XcodeProject project2 = new XcodeProject(".", targetName);
		
			project2.AddTarget (targetName, PBXProductType.Application);
			
            project2.AddBuildConfigurationSettings("Release", null, "ARCHS", "$(ARCHS_STANDARD_32_64_BIT)");
            project2.AddBuildConfigurationSettings("Release", null, "SDKROOT", "macosx");
            project2.AddBuildConfigurationSettings("Release", null, "GCC_VERSION", "com.apple.compilers.llvm.clang.1_0");
            project2.AddBuildConfigurationSettings("Release", null, "MACOSX_DEPLOYMENT_TARGET", "10.6");
            project2.AddBuildConfigurationSettings("Release", null, "GCC_C_LANGUAGE_STANDARD", "gnu99");
            project2.AddBuildConfigurationSettings("Release", null, "GCC_WARN_64_TO_32_BIT_CONVERSION", "YES");
            project2.AddBuildConfigurationSettings("Release", null, "GCC_WARN_ABOUT_RETURN_TYPE", "YES");
            project2.AddBuildConfigurationSettings("Release", null, "GCC_WARN_UNUSED_VARIABLE", "YES");
			
			project2.AddBuildConfigurationSettings ("Release", targetName, "DEBUG_INFORMATION_FORMAT", "dwarf-with-dsym");
			project2.AddBuildConfigurationSettings ("Release", targetName, "COPY_PHASE_STRIP", "YES");
			project2.AddBuildConfigurationSettings ("Release", targetName, "INFOPLIST_FILE", "../../Info.plist");
			project2.AddBuildConfigurationSettings ("Release", targetName, "PRODUCT_NAME", "$(TARGET_NAME)");
			project2.AddBuildConfigurationSettings ("Release", targetName, "WRAPPER_EXTENSION", "app");
			project2.AddBuildConfigurationSettings ("Release", targetName, "ALWAYS_SEARCH_USER_PATHS", "NO");
			project2.AddBuildConfigurationSettings ("Release", targetName, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
			
            project2.AddFile("Files", "MyApplicationAppDelegate.h", targetName);
            project2.AddFile("Files", "MyApplicationAppDelegate.m", targetName);
            project2.AddFile("Files", "Wrong.h", targetName);
            project2.RemoveFile("Files", "Wrong.h", targetName);

            project2.AddFile("Files", "main.m", targetName);
            project2.AddFile("Files", "MyApplication-Info.plist", targetName);

            project2.AddFile("Files", "en.lproj/MainMenu.xib", targetName);
            project2.AddFile("Files", "fr.lproj/MainMenu.xib", targetName);
            project2.RemoveFile("Files", "fr.lproj/MainMenu.xib", targetName);

            project2.AddFramework("Frameworks", "Cocoa", targetName);
            project2.AddFramework("Frameworks", "AddressBook", targetName);
            project2.RemoveFramework("Frameworks", "AddressBook", targetName);
			
		    project2.AddDependantProject(project1, targetName);
			
			project2.RemoveDependantProject(project1, targetName);
		
		    project2.Save();
		}
		
        [Test]
        public void TestProjectGeneration004()
        {
            // Create the main project
            string targetName = "MyApplication004";
            XcodeProject project = new XcodeProject(".", targetName);

            project.AddTarget(targetName, PBXProductType.Application);
			
            project.AddBuildConfigurationSettings("Release", null, "ARCHS", "$(ARCHS_STANDARD_32_64_BIT)");
            project.AddBuildConfigurationSettings("Release", null, "SDKROOT", "macosx");

            project.AddBuildConfigurationSettings("Release", targetName, "DEBUG_INFORMATION_FORMAT", "dwarf-with-dsym");
            project.AddBuildConfigurationSettings("Release", targetName, "COPY_PHASE_STRIP", "YES");
			project.AddBuildConfigurationSettings ("Release", targetName, "INFOPLIST_FILE", "../../Info.plist");
            project.AddBuildConfigurationSettings("Release", targetName, "PRODUCT_NAME", "$(TARGET_NAME)");
            project.AddBuildConfigurationSettings("Release", targetName, "WRAPPER_EXTENSION", "app");
            project.AddBuildConfigurationSettings("Release", targetName, "ALWAYS_SEARCH_USER_PATHS", "NO");
            project.AddBuildConfigurationSettings("Release", targetName, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
			
			project.BaseDir = "../..";
			
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

            //var frameworks = project.GetFrameworks(targetName);

            project.Save();
        }
    }
}
