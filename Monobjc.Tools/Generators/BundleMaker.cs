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
using Monobjc.Tools.External;
using Monobjc.Tools.Properties;
using Monobjc.Tools.PropertyList;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Generators
{
    public class BundleMaker
    {
        private readonly string applicationName;

        private readonly string directory;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "BundleMaker" /> class.
        /// </summary>
        /// <param name = "applicationName">Name of the application.</param>
        /// <param name = "directory">The directory.</param>
        public BundleMaker(String applicationName, String directory)
        {
            this.applicationName = applicationName;
            this.directory = Path.Combine(directory, this.applicationName + ".app");
        }

        public void WriteRuntime(MacOSVersion version)
        {
            switch (version)
            {
                case MacOSVersion.MacOS105:
                    File.WriteAllBytes(this.Runtime, Resources.runtime_10_5);
                    break;
                case MacOSVersion.MacOS106:
                    File.WriteAllBytes(this.Runtime, Resources.runtime_10_6);
                    break;
            }

            // Apply permissions
            Chmod.ApplyTo("a+x", this.Runtime);
        }

        public void WriteInfoPList(PListDocument document)
        {
            String path = Path.Combine(this.ContentsDirectory, "Info.plist");
            document.WriteToFile(path);
        }

        public String ApplicationDirectory
        {
            get
            {
                if (!Directory.Exists(this.directory))
                {
                    Directory.CreateDirectory(this.directory);
                }
                return this.directory;
            }
        }

        public String ContentsDirectory
        {
            get
            {
                String folder = Path.Combine(this.ApplicationDirectory, "Contents");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
        }

        public String ResourcesFolder
        {
            get
            {
                String folder = Path.Combine(this.ContentsDirectory, "Resources");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
        }

        public String MacOSDirectory
        {
            get
            {
                String folder = Path.Combine(this.ContentsDirectory, "MacOS");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
        }

        public String FrameworksDirectory
        {
            get
            {
                String folder = Path.Combine(this.ContentsDirectory, "Frameworks");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
        }

        public String Runtime
        {
            get
            {
                String file = Path.Combine(this.MacOSDirectory, this.applicationName);
                return file;
            }
        }

        public void CopyTo(String file, String directory)
        {
            String destinationFile = Path.Combine(directory, Path.GetFileName(file));
            File.Copy(file, destinationFile, true);
        }
    }
}