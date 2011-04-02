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

namespace Monobjc.Tools.Xcode
{
    public class XcodeProject
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "XcodeProject" /> class.
        /// </summary>
        /// <param name = "dir">The dir.</param>
        /// <param name = "name">The name.</param>
        public XcodeProject(String dir, string name)
        {
            this.Dir = dir;
            this.Name = name;
            this.Document = new PBXDocument();
        }

        /// <summary>
        ///   Gets or sets the dir.
        /// </summary>
        /// <value>The dir.</value>
        public String Dir { get; private set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public String Name { get; private set; }

        /// <summary>
        ///   Gets or sets the document.
        /// </summary>
        /// <value>The document.</value>
        public PBXDocument Document { get; private set; }

        /// <summary>
        ///   Gets or sets the project folder with the xcodeproj extension.
        /// </summary>
        /// <value>The folder.</value>
        public String ProjectFolder
        {
            get
            {
                String folder = Path.Combine(this.Dir, this.Name + ".xcodeproj");
                folder = Path.GetFullPath(folder);
                Directory.CreateDirectory(folder);
                return folder;
            }
        }

        /// <summary>
        ///   Gets or sets the project file with the pbxproj extension.
        /// </summary>
        /// <value>The file.</value>
        public String ProjectFile
        {
            get
            {
                String file = Path.Combine(this.ProjectFolder, "project.pbxproj");
                file = Path.GetFullPath(file);
                return file;
            }
        }

        /// <summary>
        ///   Adds a group.
        /// </summary>
        /// <param name = "groups">The group paths.</param>
        /// <returns>The created instance.</returns>
        public PBXGroup AddGroup(String groups)
        {
            // Split the group paths
            List<string> parts = groups.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            PBXGroup group = this.Document.Project.MainGroup;
            foreach (string part in parts)
            {
                PBXGroup g = group.FindGroup(part);
                if (g == null)
                {
                    // For each group not found, create it
                    g = new PBXGroup(part);
                    group.AddChild(g);
                }
                group = g;
            }
            return group;
        }

        /// <summary>
        ///   Removes a group.
        /// </summary>
        /// <param name = "groups">The group paths.</param>
        public void RemoveGroup(String groups)
        {
            // Only keep the n-1 groups
            List<string> parts = groups.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            String last = parts.Last();
            parts.RemoveAt(parts.Count - 1);

            // Go to the parent group
            PBXGroup g, group = this.Document.Project.MainGroup;
            foreach (string part in parts)
            {
                g = group.FindGroup(part);
                if (g == null)
                {
                    // For each group not found, create it
                    g = new PBXGroup(part);
                    group.AddChild(g);
                }
                group = g;
            }

            // If the group to delete exists, remove it
            g = group.FindGroup(last);
            if (g != null)
            {
                group.RemoveChild(g);
            }
        }

        /// <summary>
        ///   Adds a file.
        /// </summary>
        /// <param name = "groups">The group paths.</param>
        /// <param name = "file">The file.</param>
        /// <returns>The created instance.</returns>
        public PBXFileReference AddFile(String groups, String file)
        {
            // Prepare the group that will contain the file
            PBXGroup group = this.AddGroup(groups);

            // Extract information
            String name = Path.GetFileName(file);
            String path = Path.GetFullPath(file);
            String baseDir = Path.GetFullPath(this.Dir);
            String parentDir = Path.GetDirectoryName(file);

            // If the file is localized, then add it to a variant group
            if (Path.GetExtension(parentDir).Equals(".lproj"))
            {
                // The variant group may exists to search for it
                PBXVariantGroup variantGroup = group.FindVariantGroup(name);
                if (variantGroup == null)
                {
                    variantGroup = new PBXVariantGroup(name);
                    group.AddChild(variantGroup);
                }

                // The file is named like the language
                name = Path.GetFileNameWithoutExtension(parentDir);
                group = variantGroup;
            }

            // Check if the file already exists
            PBXFileReference fileReference = group.FindFileReference(name);
            if (fileReference != null)
            {
                return fileReference;
            }

            // Create a file reference
            fileReference = new PBXFileReference(name);
            if (path.StartsWith(baseDir))
            {
                path = path.Substring(baseDir.Length + 1);
                fileReference.SourceTree = PBXSourceTree.Group;
            }
            else
            {
                fileReference.SourceTree = PBXSourceTree.Absolute;
            }
            fileReference.Path = path;
            fileReference.LastKnownFileType = GetFileType(file);

            // Add it to the group
            group.AddChild(fileReference);

            return fileReference;
        }

        /// <summary>
        ///   Removes a file.
        /// </summary>
        /// <param name = "groups">The group paths.</param>
        /// <param name = "file">The file.</param>
        public void RemoveFile(String groups, String file)
        {
            // Prepare the group that contains the file
            PBXGroup group = this.AddGroup(groups);

            // Extract information
            String name = Path.GetFileName(file);
            String parentDir = Path.GetDirectoryName(file);

            // If the file is localized, search for the variant group
            if (Path.GetExtension(parentDir).Equals(".lproj"))
            {
                PBXVariantGroup variantGroup = group.FindVariantGroup(name);
                if (variantGroup != null)
                {
                    // The file is named like the language
                    name = Path.GetFileNameWithoutExtension(parentDir);
                }
                group = variantGroup;
            }

            if (group != null)
            {
                // Search for the file and remove it
                PBXFileReference fileReference = group.FindFileReference(name);
                if (fileReference != null)
                {
                    group.RemoveChild(fileReference);
                }
            }
        }

        /// <summary>
        ///   Adds a framework.
        /// </summary>
        /// <param name = "groups">The group paths.</param>
        /// <param name = "framework">The framework.</param>
        /// <returns>The created instance.</returns>
        public PBXFileReference AddFramework(String groups, String framework)
        {
            // Test for presence in System
            String path = String.Format(CultureInfo.CurrentCulture, "/System/Library/Frameworks/{0}.framework/{0}", framework);
            if (File.Exists(path))
            {
                goto bail;
            }

            // Test for presence in Library
            path = String.Format(CultureInfo.CurrentCulture, "/Library/Frameworks/{0}.framework/{0}", framework);
            if (File.Exists(path))
            {
                goto bail;
            }

            // HACK: Assume it is a system framework
            path = String.Format(CultureInfo.CurrentCulture, "/System/Library/Frameworks/{0}.framework/{0}", framework);

            bail:
            path = Path.GetDirectoryName(path);
            PBXFileReference fileReference = this.AddFile(groups, path);
            fileReference.SourceTree = PBXSourceTree.SdkRoot;
            return fileReference;
        }

        /// <summary>
        ///   Removes a framework.
        /// </summary>
        /// <param name = "groups">The group paths.</param>
        /// <param name = "framework">The framework.</param>
        public void RemoveFramework(String groups, String framework)
        {
            this.RemoveFile(groups, framework + ".framework");
        }

        /// <summary>
        ///   Adds a build configuration.
        /// </summary>
        /// <param name = "buildConfiguration">The build configuration.</param>
        /// <param name = "targetName">Name of the target or null to add it to the project only.</param>
        public void AddBuildConfiguration(XCBuildConfiguration buildConfiguration, String targetName)
        {
            // Add the configuration to the project
            this.Document.Project.BuildConfigurationList.AddBuildConfiguration(buildConfiguration);
            if (String.IsNullOrEmpty(targetName))
            {
                return;
            }

            // If the target exists, add the configuration to the target
            PBXTarget target = this.Document.Project.Targets.FirstOrDefault(t => String.Equals(t.Name, targetName));
            if (target != null)
            {
                target.BuildConfigurationList.AddBuildConfiguration(buildConfiguration);
            }
        }

        /// <summary>
        ///   Adds the dependant project.
        /// </summary>
        /// <param name = "project">The project.</param>
        public void AddDependantProject(XcodeProject project)
        {
            PBXGroup group = this.AddGroup("Products");
            PBXFileReference fileReference = this.AddFile(String.Empty, project.ProjectFolder);
            this.Document.Project.AddProjectReference(group, fileReference);
        }

        /// <summary>
        ///   Removes the dependant project.
        /// </summary>
        /// <param name = "project">The project.</param>
        public void RemoveDependantProject(XcodeProject project)
        {
            PBXFileReference fileReference = this.Project.MainGroup.FindFileReference(project.ProjectFolder);
            if (fileReference == null)
            {
                return;
            }
            this.Project.MainGroup.RemoveChild(fileReference);
            this.Document.Project.RemoveProjectReference(fileReference);
        }

        /// <summary>
        ///   Saves the project; create the xcodeproj bundle and write the project file.
        /// </summary>
        public void Save()
        {
            this.Document.WriteToFile(this.ProjectFile);
        }

        /// <summary>
        ///   Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        private PBXProject Project
        {
            get { return this.Document.Project; }
        }

        /// <summary>
        ///   Gets the project type of the file.
        /// </summary>
        private static PBXFileType GetFileType(String file)
        {
            String extension = Path.GetExtension(file);
            switch (extension)
            {
                case ".c":
                    return PBXFileType.SourcecodeC;
                case ".cpp":
                case ".cxx":
                    return PBXFileType.SourcecodeCppCpp;
                case ".framework":
                    return PBXFileType.WrapperFramework;
                case ".h":
                    return PBXFileType.SourcecodeCH;
                case ".hpp":
                case ".hxx":
                    return PBXFileType.SourcecodeCppH;
                case ".m":
                    return PBXFileType.SourcecodeCObjc;
                case ".xcodeproj":
                    return PBXFileType.WrapperPBProject;
                case ".xib":
                    return PBXFileType.FileXib;
            }
            return PBXFileType.None;
        }
    }
}