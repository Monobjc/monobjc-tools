//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2012 - Laurent Etiemble
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
using Monobjc.NAnt.Utilities;
using Monobjc.Tools.Generators;
using Monobjc.Tools.Utilities;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;

namespace Monobjc.NAnt.Tasks
{
    [TaskName("embed-app")]
    public class EmbedApplicationTask : Task
    {
        /// <summary>
        ///   Gets or sets the name of the executable.
        /// </summary>
        /// <value>The name of the executable.</value>
        [TaskAttribute("name", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public String ApplicationName { get; set; }

        /// <summary>
        ///   Gets or sets the name of the executable.
        /// </summary>
        /// <value>The name of the executable.</value>
        [TaskAttribute("assembly", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public FileInfo MainAssembly { get; set; }

        /// <summary>
        ///   Gets or sets the destination dir.
        /// </summary>
        /// <value>The destination dir.</value>
        [TaskAttribute("todir", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public DirectoryInfo ToDirectory { get; set; }

        /// <summary>
        ///   Gets or sets the target OS version.
        /// </summary>
        /// <value>The target OS version.</value>
        [TaskAttribute("target-os")]
        [StringValidator(AllowEmpty = false)]
        public MacOSVersion TargetOSVersion { get; set; }

        /// <summary>
        ///   Gets or sets the target architecture.
        /// </summary>
        /// <value>The target architecture.</value>
        [TaskAttribute("target-architecture")]
        public MacOSArchitecture TargetArchitecture { get; set; }

        /// <summary>
        ///   Gets or sets the search directories.
        /// </summary>
        /// <value>The search dirs.</value>
        [BuildElementArray("search-in")]
        public FileSet[] SearchDirectories { get; set; }

        /// <summary>
        ///   Gets or sets the included assemblies.
        /// </summary>
        /// <value>The included assemblies.</value>
        [BuildElementArray("include-assembly")]
        public Argument[] IncludedAssemblies { get; set; }

        /// <summary>
        ///   Gets or sets the excluded assemblies.
        /// </summary>
        /// <value>The excluded assemblies.</value>
        [BuildElementArray("exclude-assembly")]
        public Argument[] ExcludedAssemblies { get; set; }

        /// <summary>
        ///   Gets or sets the included libraries.
        /// </summary>
        /// <value>The included libraries.</value>
        [BuildElementArray("include-library")]
        public Argument[] IncludedLibraries { get; set; }

        /// <summary>
        ///   Gets or sets the machine configuration.
        /// </summary>
        /// <value>The machine configuration.</value>
        [TaskAttribute("machine-configuration")]
        public String MachineConfiguration { get; set; }

        /// <summary>
        ///   Executes the task.
        /// </summary>
        protected override void ExecuteTask()
        {
            // Transform fileset into dir list
            if (this.SearchDirectories.Any(f => f.BaseDirectory == null))
            {
                throw new BuildException("Missing 'basedir' attribute on <search-in/> sub-task");
            }

            // Check included assemblies
            if (this.IncludedAssemblies.Any(f => f.File == null))
            {
                throw new BuildException("Missing 'file' attribute on <include-assembly/> sub-task");
            }

            // Check excluded assemblies
            if (this.ExcludedAssemblies.Any(f => f.File == null))
            {
                throw new BuildException("Missing 'file' attribute on <exclude-assembly/> sub-task");
            }

            // Check additionnal libraries
            if (this.IncludedLibraries.Any(f => f.File == null))
            {
                throw new BuildException("Missing 'file' attribute on <include-library/> sub-task");
            }

            // Set the output directory for the build
            String directory = this.ToDirectory.ToString();
			Directory.CreateDirectory(directory);

            // Collect all the assemblies
            List<string> assemblies = this.GetAssemblies();

            // Generate the embedded executable
            NativeCodeGenerator codeGenerator = new NativeCodeGenerator();
            codeGenerator.Logger = new ExecutionLogger(this);
            codeGenerator.Assemblies = assemblies;
            codeGenerator.MachineConfiguration = this.MachineConfiguration;
            codeGenerator.TargetOSVersion = this.TargetOSVersion;
            codeGenerator.TargetArchitecture = this.TargetArchitecture;
            String executableFile = codeGenerator.Generate(directory);
            String libraryFile = Path.Combine(directory, "libmonobjc.dylib");

            // Relocate the libraries
            NativeCodeRelocator relocator = new NativeCodeRelocator();
            relocator.Logger = new ExecutionLogger(this);
            relocator.DependencyPattern = new List<string> {"Mono.framework"};
            relocator.Relocate(executableFile, directory);
            relocator.Relocate(libraryFile, directory);
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
            collector.SearchDirectories = this.SearchDirectories.Select(f => f.BaseDirectory.ToString()).ToList();

            // Collect the main assembly references
            assemblies.AddRange(collector.Collect(this.MainAssembly.ToString()));

            // Collect the additionnal assemblies references
            foreach (String includeAssembly in this.IncludedAssemblies.Select(f => f.File.ToString()).ToList())
            {
                assemblies.AddRange(collector.Collect(includeAssembly));
            }

            // Remove redundant entries
            assemblies = assemblies.Distinct().ToList();

            // Remove excluded assemblies
            List<String> excludes = this.ExcludedAssemblies.Select(f => f.File.Name).ToList();
            assemblies = assemblies.Where(assembly => !excludes.Contains(Path.GetFileName(assembly))).ToList();

            return assemblies;
        }
    }
}