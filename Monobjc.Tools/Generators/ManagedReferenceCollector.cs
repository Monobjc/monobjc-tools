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
using System.IO;
using System.Linq;
using System.Reflection;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Generators
{
    public class ManagedReferenceCollector
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ManagedReferenceCollector" /> class.
        /// </summary>
        public ManagedReferenceCollector()
        {
            this.Logger = new NullLogger();
        }

        /// <summary>
        ///   Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public IExecutionLogger Logger { get; set; }

        /// <summary>
        ///   Gets or sets the search directories.
        /// </summary>
        /// <value>The search directories.</value>
        public IList<String> SearchDirectories { get; set; }

        /// <summary>
        ///   Collect the dependencies of the main assembly.
        /// </summary>
        /// <returns>A collection of all the managed assemblies.</returns>
        public IEnumerable<String> Collect(String assembly)
        {
            IDictionary<string, string> assemblies = new Dictionary<string, string>();

            // Complete the search directories
            IEnumerable<String> directories = this.ComputeSearchDirectories(assembly);

            // Load the assembly and collect its dependencies
            if (!this.Collect(assembly, assemblies, directories))
            {
                return null;
            }

            // Only return files
            return assemblies.Values;
        }

        /// <summary>
        ///   Compute the search directories. Append the main assembly directory and the GAC folder.
        /// </summary>
        private IEnumerable<String> ComputeSearchDirectories(String assembly)
        {
            this.Logger.LogInfo("Setting up the search directories...");

            // Create the search directories list
            List<String> directories = new List<String>();
            if (this.SearchDirectories != null)
            {
                directories.AddRange(this.SearchDirectories);
            }

            // First add the main assembly directory
            String folder = Path.GetDirectoryName(assembly);
            if (!directories.Contains(folder))
            {
                directories.Add(folder);
            }

            // Then add the GAC folder
            String corlibFolder = Path.GetDirectoryName(new Uri(typeof (Object).Assembly.CodeBase).AbsolutePath);
            if (!directories.Contains(corlibFolder))
            {
                directories.Add(corlibFolder);
            }

            return directories.Distinct();
        }

        /// <summary>
        ///   Collects the dependencies of the specified assembly.
        /// </summary>
        /// <param name = "assembly">The assembly.</param>
        /// <param name = "assemblies">The assemblies.</param>
        /// <param name = "directories">The directories.</param>
        /// <returns>
        ///   <code>true</code> if the assembly file is found, <code>false</code> otherwise
        /// </returns>
        private bool Collect(String assembly, IDictionary<string, string> assemblies, IEnumerable<String> directories)
        {
            this.Logger.LogInfo(string.Format(CultureInfo.CurrentCulture, "Collecting dependencies for '{0}'...", assembly));

            // Load the assembly
            Assembly a = this.LoadAssembly(assembly, directories);
            if (a == null)
            {
                this.Logger.LogInfo(String.Format(CultureInfo.CurrentCulture, "Assembly '{0}' not found", assembly));
                return false;
            }

            // Collect the dependencies if needed
            if (!assemblies.ContainsKey(a.GetName().Name))
            {
                assemblies.Add(a.GetName().Name, assembly);
                Collect(a, assemblies, directories);
            }

            return true;
        }

        /// <summary>
        ///   Collects the dependencies of the specified assembly.
        /// </summary>
        /// <param name = "assembly">The assembly.</param>
        /// <param name = "assemblies">The assemblies.</param>
        /// <param name = "directories">The directories.</param>
        private void Collect(Assembly assembly, IDictionary<string, string> assemblies, IEnumerable<String> directories)
        {
            this.Logger.LogDebug(string.Format(CultureInfo.CurrentCulture, "Collecting dependencies of '{0}'...", assembly.GetName()));

            IEnumerable<AssemblyName> dependencies = GetDependencies(assembly);
            foreach (AssemblyName assemblyName in dependencies)
            {
                // Skip if we already have this assembly
                if (assemblies.ContainsKey(assemblyName.Name))
                {
                    continue;
                }

                this.Logger.LogInfo(string.Format(CultureInfo.CurrentCulture, "Found '{0}'", assemblyName));

                // Load it by its name
                Assembly a = this.LoadAssembly(assemblyName.Name, directories);
                assemblies.Add(assemblyName.Name, new Uri(a.CodeBase).AbsolutePath);

                // Recurse for each asssembly found
                Collect(a, assemblies, directories);
            }
        }

        /// <summary>
        ///   Try to load the assembly in every search directories.
        /// </summary>
        /// <param name = "nameOrFile">The name or file.</param>
        /// <param name = "directories">The directories.</param>
        /// <returns>An assembly if found, null otherwise.</returns>
        private Assembly LoadAssembly(String nameOrFile, IEnumerable<String> directories)
        {
            this.Logger.LogInfo(String.Format(CultureInfo.CurrentCulture, "Probing assembly '{0}'...", nameOrFile));

            // Make sure the name has an extension
            String file = nameOrFile.EndsWith(".dll") || nameOrFile.EndsWith(".exe") ? nameOrFile : nameOrFile + ".dll";

            // First try a direct load
            if (File.Exists(file))
            {
                return Assembly.LoadFrom(file);
            }

            // Probe each directory)
            foreach (string searchDirectory in directories)
            {
                String name = Path.GetFileName(file);
                String path = Path.Combine(searchDirectory, name);
                if (!File.Exists(path))
                {
                    continue;
                }
                Assembly a = Assembly.LoadFrom(path);
                if (a != null)
                {
                    return a;
                }
            }
            return null;
        }

        /// <summary>
        ///   Gets the dependencies of the assembly.
        /// </summary>
        /// <param name = "assembly">The assembly.</param>
        /// <returns>A collection of dependencies.</returns>
        private static IEnumerable<AssemblyName> GetDependencies(Assembly assembly)
        {
            return new List<AssemblyName>(assembly.GetReferencedAssemblies());
        }
    }
}