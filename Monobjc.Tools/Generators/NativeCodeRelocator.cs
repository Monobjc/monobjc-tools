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
using Monobjc.Tools.External;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Generators
{
    public class NativeCodeRelocator
    {
        private const string PATH_REPLACEMENT = "@executable_path";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "NativeCodeRelocator" /> class.
        /// </summary>
        public NativeCodeRelocator()
        {
            this.Logger = new NullLogger();
        }

        public IExecutionLogger Logger { get; set; }

        /// <summary>
        ///   Gets or sets the dependency pattern.
        /// </summary>
        /// <value>The dependency pattern.</value>
        public IList<String> DependencyPattern { get; set; }

        public void Relocate(String nativeImage, String directory)
        {
            this.DependencyPattern = this.DependencyPattern ?? new List<string> {String.Empty};

            IDictionary<string, string> dependencies = new Dictionary<String, String>();

            // Collect recursivly all the dependencies for the executable file
            this.CollectDependencies(nativeImage, dependencies);

            // Copy all the dependencies next to executable file
            this.CopyDependencies(directory, dependencies);

            // Relocate all the references for the libraries
            this.RelocateFiles(nativeImage, directory, dependencies);
        }

        private void CollectDependencies(String nativeBinary, IDictionary<string, string> dependencies)
        {
            IEnumerable<string> list = OTool.GetDependencies(nativeBinary);
            foreach (string dependency in list)
            {
                // Check if we need this dependency
                if (!this.DependencyPattern.Any(dependency.Contains))
                {
                    continue;
                }

                // Balk if we already have this dependency
                if (dependencies.ContainsKey(dependency))
                {
                    continue;
                }

                this.Logger.LogInfo("Found matching dependency '" + dependency + "'");

                // Add the dependency to the map
                String installName = OTool.GetInstallName(dependency);
                dependencies.Add(dependency, installName);
                this.CollectDependencies(dependency, dependencies);
            }
        }

        private void CopyDependencies(String directory, IDictionary<string, string> dependencies)
        {
            this.Logger.LogInfo("Copying dependencies...");

            foreach (String file in dependencies.Values)
            {
                String library = Path.GetFileName(file);
                String targetFile = Path.Combine(directory, library);

                this.Logger.LogInfo("Copying dependency '" + library + "'");

                File.Copy(file, targetFile, true);
            }
        }

        private void RelocateFiles(String nativeImage, String directory, IDictionary<string, string> dependencies)
        {
            this.Logger.LogInfo("Relocating dependencies...");

            // Relocate all the dependencies
            foreach (String value in dependencies.Values)
            {
                String library = Path.GetFileName(value);
                String installName = Path.Combine(PATH_REPLACEMENT, library);
                String targetFile = Path.Combine(directory, library);

                this.Logger.LogInfo("Relocating dependency '" + library + "'");

                // Change install name
                InstallNameTool.ChangeId(targetFile, installName);

                // Relocate dependencies for library
                foreach (String key in dependencies.Keys)
                {
                    String replacement = Path.Combine(PATH_REPLACEMENT, Path.GetFileName(dependencies[key]));
                    InstallNameTool.ChangeDependency(targetFile, key, replacement);
                }
            }

            this.Logger.LogInfo("Relocating main file...");

            // Relocate libraries for executable
            foreach (String key in dependencies.Keys)
            {
                String replacement = Path.Combine(PATH_REPLACEMENT, Path.GetFileName(dependencies[key]));
                InstallNameTool.ChangeDependency(nativeImage, key, replacement);
            }
        }
    }
}