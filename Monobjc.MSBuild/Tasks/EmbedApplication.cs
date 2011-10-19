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
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Monobjc.MSBuild.Utilities;
using Monobjc.Tools.Generators;
using Monobjc.Tools.Utilities;

namespace Monobjc.MSBuild.Tasks
{
    public class EmbedApplication : Task
    {
        private MacOSVersion targetOSVersion;
        private MacOSArchitecture targetArchitecture;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Monobjc.MSBuild.Tasks.EmbedApplication"/> class.
		/// </summary>
		public EmbedApplication()
		{
			this.targetOSVersion = MacOSVersion.MacOS105;
			this.targetArchitecture = MacOSArchitecture.X86;
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether this instance use Receigen.
		/// </summary>
		/// <value>
		/// <c>true</c> to use Receigen; otherwise, <c>false</c>.
		/// </value>
		public bool UseReceigen { get; set; }
		
        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        [Required]
		public String ApplicationName { get; set; }
		
        /// <summary>
        /// Gets or sets the target OS version.
        /// </summary>
        /// <value>The target OS version.</value>
        public String TargetOSVersion
		{
			get { return this.targetOSVersion.ToString(); }
			set { this.targetOSVersion = (MacOSVersion) Enum.Parse(typeof(MacOSVersion), value); }
		}
		
        /// <summary>
        /// Gets or sets the target architecture.
        /// </summary>
        /// <value>The target architecture.</value>
        public String TargetArchitecture
		{
			get { return this.targetArchitecture.ToString(); }
			set { this.targetArchitecture = (MacOSArchitecture) Enum.Parse(typeof(MacOSArchitecture), value); }
		}
		
        /// <summary>
        /// Gets or sets the main assembly.
        /// </summary>
        /// <value>The main assembly.</value>
        [Required]
        public ITaskItem MainAssembly { get; set; }
		
        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
        [Required]
		public ITaskItem ToDirectory { get; set; }
		
        /// <summary>
        /// Gets or sets the working dir.
        /// </summary>
        /// <value>The working dir.</value>
		public ITaskItem WorkingDirectory { get; set; }
		
        /// <summary>
        ///   Gets or sets the search directories.
        /// </summary>
        /// <value>The search dirs.</value>
        public ITaskItem[] SearchDirectories { get; set; }

        /// <summary>
        ///   Gets or sets the included assemblies.
        /// </summary>
        /// <value>The included assemblies.</value>
        public ITaskItem[] IncludedAssemblies { get; set; }

        /// <summary>
        ///   Gets or sets the excluded assemblies.
        /// </summary>
        /// <value>The excluded assemblies.</value>
        public ITaskItem[] ExcludedAssemblies { get; set; }

        /// <summary>
        ///   Gets or sets the included libraries.
        /// </summary>
        /// <value>The included libraries.</value>
        public ITaskItem[] IncludedLibraries { get; set; }

        /// <summary>
        ///   Gets or sets the machine configuration.
        /// </summary>
        /// <value>The machine configuration.</value>
        public ITaskItem MachineConfiguration { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to compress assemblies.
		/// </summary>
		/// <value>
		/// <c>true</c> to compress; otherwise, <c>false</c>.
		/// </value>
		public bool Compress { get; set; }
		
		/// <summary>
		/// Gets or sets the native compiler.
		/// </summary>
		/// <value>
		/// The native compiler.
		/// </value>
		public String NativeCompiler { get; set; }
		
        /// <summary>
        ///   Executes the task.
        /// </summary>
        public override bool Execute()
        {
			if (this.SearchDirectories == null) {
				this.SearchDirectories = new ITaskItem[0];
			}
			if (this.IncludedAssemblies == null) {
				this.IncludedAssemblies = new ITaskItem[0];
			}
			if (this.ExcludedAssemblies == null) {
				this.ExcludedAssemblies = new ITaskItem[0];
			}
			if (this.IncludedLibraries == null) {
				this.IncludedLibraries = new ITaskItem[0];
			}
			if (this.MachineConfiguration == null) {
				this.MachineConfiguration = new TaskItem("/Library/Frameworks/Mono.framework/Home/etc/mono/4.0/machine.config");
			}
			
			// HACK: We need to put a search directory in order to properly lookup assemblies with no explicit version
			List<ITaskItem> directories = this.SearchDirectories.ToList();
			directories.Insert(0, new TaskItem(FileProvider.GetPath(this.targetOSVersion)));
			this.SearchDirectories = directories.ToArray();
			
			// Set the working directory for the build
			String workingDir;
			if (this.WorkingDirectory == null) {
				workingDir = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
				File.Delete(workingDir);
				this.WorkingDirectory = new TaskItem(workingDir);
			}
            workingDir = this.WorkingDirectory.ItemSpec;
			Directory.CreateDirectory(workingDir);
			
            // Set the output directory for the build
            String outputDir = this.ToDirectory.ItemSpec;
			Directory.CreateDirectory(outputDir);
			
            // Collect all the assemblies
            List<string> assemblies = this.GetAssemblies();

            // Generate the embedded executable
            NativeCodeGenerator codeGenerator = new NativeCodeGenerator();
            codeGenerator.Logger = new ExecutionLogger(this);
            codeGenerator.Assemblies = assemblies;
            codeGenerator.MachineConfiguration = this.MachineConfiguration.ItemSpec;
            codeGenerator.TargetOSVersion = this.targetOSVersion;
            codeGenerator.TargetArchitecture = this.targetArchitecture;
			codeGenerator.Compress = this.Compress;
			codeGenerator.UseReceigen = this.UseReceigen;
			codeGenerator.NativeCompiler = this.NativeCompiler;
            String executableFile = codeGenerator.Generate(workingDir);
            String libraryFile = Path.Combine(workingDir, "libmonobjc.dylib");
			
            // Relocate the libraries
            NativeCodeRelocator relocator = new NativeCodeRelocator();
            relocator.Logger = new ExecutionLogger(this);
            relocator.DependencyPattern = new List<string> {"Mono.framework"};
            relocator.Relocate(executableFile, outputDir);
            relocator.Relocate(libraryFile, outputDir);
			
			// Copy the result files
			File.Copy(executableFile, Path.Combine(outputDir, Path.GetFileName(executableFile)), true);
			File.Copy(libraryFile, Path.Combine(outputDir, Path.GetFileName(libraryFile)), true);
			
			return true;
        }

        /// <summary>
        ///   Gets all assemblies to embed.
        /// </summary>
        private List<String> GetAssemblies()
        {
            List<String> assemblies = new List<String>();

            // Create the collector
            ManagedReferenceCollector collector = new ManagedReferenceCollector();
            collector.Logger = new ExecutionLogger(this);
            collector.SearchDirectories = this.SearchDirectories.Select(f => f.ItemSpec).ToList();

            // Collect the main assembly references
            assemblies.AddRange(collector.Collect(this.MainAssembly.ToString()));

            // Collect the additionnal assemblies references
            foreach (String includeAssembly in this.IncludedAssemblies.Select(f => f.ItemSpec).ToList())
            {
                assemblies.AddRange(collector.Collect(includeAssembly));
            }

            // Remove redundant entries
            assemblies = assemblies.Distinct().ToList();

            // Remove excluded assemblies
            List<String> excludes = this.ExcludedAssemblies.Select(f => Path.GetFileName(f.ItemSpec)).ToList();
            assemblies = assemblies.Where(assembly => !excludes.Contains(Path.GetFileName(assembly))).ToList();

            return assemblies;
        }
	}
}
