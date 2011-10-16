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
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Monobjc.MSBuild.Utilities;
using Monobjc.Tools.Generators;
using Monobjc.Tools.Properties;
using Monobjc.Tools.Xcode;
using Monobjc.Tools.Utilities;
using System.IO;
using System.Collections.Generic;

namespace Monobjc.MSBuild.Tasks
{
	public class ExportToXcode : Task
	{
		private const String GROUP_SOURCES = "Sources";
		private const String GROUP_RESOURCES = "Resources";
		private const String GROUP_LIBRARIES = "Libraries";
		private const String GROUP_FRAMEWORKS = "Frameworks";
		private const String CONFIGURATION_RELEASE = "Release";
		private MacOSVersion targetOSVersion;
		private MacOSArchitecture targetArchitecture;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Monobjc.MSBuild.Tasks.ExportToXcode"/> class.
		/// </summary>
		public ExportToXcode ()
		{
			this.targetOSVersion = MacOSVersion.MacOS105;
			this.targetArchitecture = MacOSArchitecture.X86;
		}
		
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[Required]
		public String Name { get; set; }
		
		/// <summary>
		/// Gets or sets the Info.plist file.
		/// </summary>
		/// <value>
		/// The Info.plist file.
		/// </value>
		public ITaskItem InfoPList { get; set; }
		
		/// <summary>
		/// Gets or sets the xib files.
		/// </summary>
		/// <value>
		/// The xib files.
		/// </value>
		public ITaskItem[] XibFiles { get; set; }
		
		/// <summary>
		/// Gets or sets the resources.
		/// </summary>
		/// <value>
		/// The resources.
		/// </value>
		public ITaskItem[] Resources { get; set; }
		
		/// <summary>
		/// Gets or sets the frameworks.
		/// </summary>
		/// <value>
		/// The frameworks.
		/// </value>
		public String Frameworks { get; set; }
		
		/// <summary>
		///   Gets or sets the signing identity.
		/// </summary>
		/// <value>The identity.</value>
		public String Identity { get; set; }

		/// <summary>
		///   Gets or sets the entitlements.
		/// </summary>
		/// <value>The entitlements.</value>
		public ITaskItem Entitlements { get; set; }
		
		/// <summary>
		/// Gets or sets the target OS version.
		/// </summary>
		/// <value>The target OS version.</value>
		public String TargetOSVersion {
			get { return this.targetOSVersion.ToString (); }
			set { this.targetOSVersion = (MacOSVersion)Enum.Parse (typeof(MacOSVersion), value); }
		}
		
		/// <summary>
		/// Gets or sets the target architecture.
		/// </summary>
		/// <value>The target architecture.</value>
		public String TargetArchitecture {
			get { return this.targetArchitecture.ToString (); }
			set { this.targetArchitecture = (MacOSArchitecture)Enum.Parse (typeof(MacOSArchitecture), value); }
		}
		
		/// <summary>
		/// Gets or sets the base directory.
		/// </summary>
		/// <value>
		/// The base directory.
		/// </value>
		[Required]
		public ITaskItem BaseDirectory { get; set; }
		
		/// <summary>
		/// Gets or sets the native directory.
		/// </summary>
		/// <value>
		/// The native directory.
		/// </value>
		[Required]
		public ITaskItem NativeDirectory { get; set; }
		
		/// <summary>
		///   Executes the task.
		/// </summary>
		public override bool Execute ()
		{
			String baseFolder = this.BaseDirectory.ItemSpec;
			String nativeFolder = this.NativeDirectory.ItemSpec;
			String targetName = this.Name;
			
			// TODO: Move all above into Tools
			
			XcodeProject xcodeProject = new XcodeProject (baseFolder, this.Name);
			xcodeProject.Document.Project.ProjectRoot = String.Empty;

			xcodeProject.AddGroup (GROUP_SOURCES);
			xcodeProject.AddGroup (GROUP_RESOURCES);
			xcodeProject.AddGroup (GROUP_FRAMEWORKS);
			xcodeProject.AddGroup (GROUP_LIBRARIES);

			// Set default settings
			// TODO: Use specific architecture
			// ARCHS_STANDARD_32_BIT
			// ARCHS_STANDARD_32_64_BIT
			// ARCHS_STANDARD_64_BIT
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, null, "ARCHS", "$(ARCHS_STANDARD_32_BIT)");
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, null, "SDKROOT", "macosx");
			// TODO: Use specific version
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, null, "MACOSX_DEPLOYMENT_TARGET", "10.5");
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, null, "GCC_WARN_64_TO_32_BIT_CONVERSION", "YES");
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, null, "GCC_WARN_ABOUT_RETURN_TYPE", "YES");
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, null, "GCC_WARN_UNUSED_VARIABLE", "YES");
			
			xcodeProject.AddTarget (targetName, PBXProductType.Application);
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, targetName, "DEBUG_INFORMATION_FORMAT", "dwarf-with-dsym");
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, targetName, "COPY_PHASE_STRIP", "YES");
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, targetName, "INFOPLIST_FILE", "Info.plist");
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, targetName, "PRODUCT_NAME", "$(TARGET_NAME)");
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, targetName, "WRAPPER_EXTENSION", "app");
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, targetName, "ALWAYS_SEARCH_USER_PATHS", "NO");
			xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, targetName, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
			
			// Add specific files and options
			xcodeProject.AddFile(GROUP_RESOURCES, "Info.plist");
			if (this.Identity != null) {
				xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, targetName, "CODE_SIGN_IDENTITY", this.Identity);
			}
			if (this.Entitlements != null) {
				PBXFileReference file = (PBXFileReference) xcodeProject.AddFile(GROUP_RESOURCES, this.Entitlements.ItemSpec);
				xcodeProject.AddBuildConfigurationSettings (CONFIGURATION_RELEASE, targetName, "CODE_SIGN_ENTITLEMENTS", file.Path);
			}
			
			// Add source files
			xcodeProject.AddFile(GROUP_SOURCES, Path.Combine(nativeFolder, "main.c"), targetName);
			xcodeProject.AddFile(GROUP_SOURCES, Path.Combine(nativeFolder, "monobjc.h"), targetName);
			
			// Add XIB files
			if (this.XibFiles != null) {
				foreach (ITaskItem item in this.XibFiles) {
					xcodeProject.AddFile (GROUP_RESOURCES, item.ItemSpec, targetName);
				}
			}
			
			// Add resources
			if (this.Resources != null) {
				foreach (ITaskItem item in this.Resources) {
					xcodeProject.AddFile (GROUP_RESOURCES, item.ItemSpec, targetName);
				}
			}
			
			// Add frameworks
			if (this.Frameworks != null) {
				foreach(String name in this.Frameworks.Split(';')) {
					xcodeProject.AddFramework (GROUP_FRAMEWORKS, name, targetName);
				}
				if (!this.Frameworks.Contains("Security")) {
					xcodeProject.AddFramework (GROUP_FRAMEWORKS, "Security", targetName);
				}
				if (!this.Frameworks.Contains("IOKit")) {
					xcodeProject.AddFramework (GROUP_FRAMEWORKS, "IOKit", targetName);
				}
			}
			
			// Add libraries
			IEnumerable<String> libraries = Directory.EnumerateFiles(nativeFolder, "lib*.*");
			foreach (String library in libraries) {
				this.Log.LogMessage("Library file " + library);
				xcodeProject.AddLibrary (GROUP_LIBRARIES, library, targetName);
			}
			xcodeProject.AddLibrary (GROUP_LIBRARIES, "/usr/lib/libz.dylib", targetName);
			
			xcodeProject.Save();
			
			return true;
		}
	}
}
