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
using Monobjc.NAnt.Types;
using Monobjc.Tools.External;
using Monobjc.Tools.Properties;
using Monobjc.Tools.Utilities;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Tasks;
using NAnt.Core.Types;

namespace Monobjc.NAnt.Tasks
{
    [TaskName("bundle-app")]
    public class BundleApplicationTask : Task
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "BundleApplicationTask" /> class.
        /// </summary>
        public BundleApplicationTask()
        {
            this.TargetOSVersion = MacOSVersion.MacOS105;
        }

        /// <summary>
        ///   Gets or sets the application name.
        /// </summary>
        /// <value>The name dir.</value>
        [TaskAttribute("name", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string ApplicationName { get; set; }

        /// <summary>
        ///   Gets or sets the info.plist file.
        /// </summary>
        /// <value>The info.plist file.</value>
        [TaskAttribute("plist", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public FileInfo InfoPList { get; set; }

        /// <summary>
        ///   Gets or sets the destination dir.
        /// </summary>
        /// <value>The destination dir.</value>
        [TaskAttribute("todir", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public DirectoryInfo ToDirectory { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the generation is for a native .NET application.
        /// </summary>
        /// <value><c>true</c> if native; otherwise, <c>false</c>.</value>
        [TaskAttribute("native")]
        [BooleanValidator]
        public bool Native { get; set; }

        /// <summary>
        ///   Gets or sets the mininum required OS version.
        /// </summary>
        /// <value>The mininum required OS version.</value>
        [TaskAttribute("target-os")]
        [StringValidator(AllowEmpty = false)]
        public MacOSVersion TargetOSVersion { get; set; }

        /// <summary>
        ///   Gets or sets the files to copy in Contents folder.
        /// </summary>
        /// <value>The files.</value>
        [BuildElementArray("copy-in-contents")]
        public FileSet[] CopyInContents { get; set; }

        /// <summary>
        ///   Gets or sets the files to copy in MacOS folder.
        /// </summary>
        /// <value>The files.</value>
        [BuildElementArray("copy-in-macos")]
        public FileSet[] CopyInMacOS { get; set; }

        /// <summary>
        ///   Gets or sets the files to copy in Resources folder.
        /// </summary>
        /// <value>The files.</value>
        [BuildElementArray("copy-in-resources")]
        public FileSet[] CopyInResources { get; set; }

        /// <summary>
        ///   Gets or sets the framework to copy in Frameworks folder.
        /// </summary>
        /// <value>The files.</value>
        [BuildElementArray("copy-in-frameworks")]
        public FrameworkDirSet[] CopyInFrameworks { get; set; }

        /// <summary>
        ///   Executes the task.
        /// </summary>
        protected override void ExecuteTask()
        {
            this.Log(Level.Info, "Building application '{0}'", this.ApplicationName);

            String bundleName = this.ApplicationName + ".app";
            String bundleBase = Path.Combine(this.ToDirectory.ToString(), bundleName);
            String bundleContents = Path.Combine(bundleBase, "Contents");
            String bundleMacOS = Path.Combine(bundleContents, "MacOS");
            String bundleResources = Path.Combine(bundleContents, "Resources");
            String bundleFrameworks = Path.Combine(bundleContents, "Frameworks");

            //
            // Create the bundle structure
            //
            // + <Name>.app
            // |
            // +-+ Contents
            //   |
            //   +-+ Frameworks
            //   |
            //   +-+ MacoOS
            //   |
            //   +-+ Resources
            //
            this.Log(Level.Info, "Creating bundle structure...");
            Directory.CreateDirectory(bundleBase);
            Directory.CreateDirectory(bundleContents);
            Directory.CreateDirectory(bundleMacOS);
            Directory.CreateDirectory(bundleResources);

            // Copy or create the Info.plist file
            this.Log(Level.Error, "Copying Info.plist...");
            this.CopyFile(this.InfoPList, bundleContents);

            // Create the launcher
            if (!this.Native)
            {
                this.Log(Level.Info, "Copying custom runtime...");

                byte[] runtime;
                switch (this.TargetOSVersion)
                {
                    case MacOSVersion.MacOS105:
                        runtime = Resources.runtime_10_5;
                        break;
                    case MacOSVersion.MacOS106:
                        runtime = Resources.runtime_10_6;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                String path = Path.Combine(bundleMacOS, this.ApplicationName);
                File.WriteAllBytes(path, runtime);
                Chmod.ApplyTo("a+rx", path);
            }

            // Copy files in Contents if any
            if (this.CopyInContents != null && this.CopyInContents.Length > 0)
            {
                this.Log(Level.Info, "Copying files in 'Contents' folder...");
                foreach (FileSet fileSet in this.CopyInContents)
                {
                    this.CopyFileSet(fileSet, bundleContents);
                }
            }

            // Copy files in MacOS if any
            if (this.CopyInMacOS != null && this.CopyInMacOS.Length > 0)
            {
                this.Log(Level.Info, "Copying files in 'MacOS' folder...");
                foreach (FileSet fileSet in this.CopyInMacOS)
                {
                    this.CopyFileSet(fileSet, bundleMacOS);
                }
            }

            // Copy files in Resources if any
            if (this.CopyInResources != null && this.CopyInResources.Length > 0)
            {
                this.Log(Level.Info, "Copying files in 'Resources' folder...");
                foreach (FileSet fileSet in this.CopyInResources)
                {
                    this.CopyFileSet(fileSet, bundleResources);
                }
            }

            // Copy folder in Frameworks if any
            if (this.CopyInFrameworks != null && this.CopyInFrameworks.Length > 0)
            {
                Directory.CreateDirectory(bundleFrameworks);
                this.Log(Level.Info, "Copying frameworks in 'Frameworks' folder...");
                foreach (FrameworkDirSet dirSet in this.CopyInFrameworks)
                {
                    CopyFrameworkDirSet(dirSet, bundleFrameworks);
                }
            }
        }

        private void CopyFile(FileInfo fileInfo, String toDir)
        {
            CopyTask task = new CopyTask();
            task.Project = this.Project;
            task.InitializeTaskConfiguration();
            task.ToDirectory = new DirectoryInfo(toDir);
            task.SourceFile = fileInfo;
            task.Execute();
        }

        private void CopyFileSet(FileSet fileSet, String toDir)
        {
            CopyTask task = new CopyTask();
            task.Project = this.Project;
            task.InitializeTaskConfiguration();
            task.ToDirectory = new DirectoryInfo(toDir);
            task.CopyFileSet = fileSet;
            task.Execute();
        }

        private static void CopyFrameworkDirSet(FrameworkDirSet dirSet, String toDir)
        {
            String target = Path.Combine(toDir, dirSet.FrameworkName);
            DirectoryInfo sourceInfo = dirSet.BaseDirectory;

            bool exists = Directory.Exists(target);
            if (exists && (sourceInfo.LastWriteTime.CompareTo(new DirectoryInfo(target).LastWriteTime) <= 0))
            {
                return;
            }
            if (exists)
            {
                // Remove old directory
                Directory.Delete(target, true);
            }

            Copy.Recursivly(sourceInfo.ToString(), toDir);
        }
    }
}