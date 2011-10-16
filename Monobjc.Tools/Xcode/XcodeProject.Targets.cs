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
    /// <summary>
    ///   Wraps a Xcode project.
    /// </summary>
    public partial class XcodeProject
    {
        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="targetName">Name of the target.</param>
        /// <returns>A list of files.</returns>
        public IEnumerable<PBXBuildFile> GetFiles(String pattern, String targetName)
        {
            PBXTarget target = this.GetTarget(targetName);
            if (target == null)
            {
                yield break;
            }

            PBXBuildPhase phase = GetTargetPhase(target, pattern);
            if (phase == null)
            {
                yield break;
            }

            foreach (PBXBuildFile buildFile in phase.Files)
            {
                yield return buildFile;
            }
        }

        /// <summary>
        ///   Adds the file.
        /// </summary>
        /// <param name = "groups">The groups.</param>
        /// <param name = "file">The file.</param>
        /// <param name = "targetName">Name of the target.</param>
        /// <returns></returns>
        public PBXBuildFile AddFile(String groups, String file, String targetName)
        {
            lock (this.syncRoot)
            {
                PBXFileElement fileElement = this.AddFile(groups, file);

                PBXTarget target = this.GetTarget(targetName);
                if (target == null)
                {
                    return null;
                }

                PBXBuildPhase phase = GetTargetPhase(target, file);
                if (phase == null)
                {
                    return null;
                }

                PBXBuildFile buildFile = phase.FindFile(fileElement);
                if (buildFile == null)
                {
                    buildFile = new PBXBuildFile(fileElement);
                    phase.AddFile(buildFile);
                }

                return buildFile;
            }
        }

        /// <summary>
        ///   Removes the file.
        /// </summary>
        /// <param name = "groups">The groups.</param>
        /// <param name = "file">The file.</param>
        /// <param name = "targetName">Name of the target.</param>
        /// <returns></returns>
        public PBXBuildFile RemoveFile(String groups, String file, String targetName)
        {
            lock (this.syncRoot)
            {
                PBXFileElement fileElement = this.RemoveFile(groups, file);
                if (fileElement == null)
                {
                    return null;
                }

                PBXTarget target = this.GetTarget(targetName);
                if (target == null)
                {
                    return null;
                }

                PBXBuildPhase phase = GetTargetPhase(target, file);
                if (phase == null)
                {
                    return null;
                }

                PBXBuildFile buildFile = phase.FindFile(fileElement);
                if (buildFile != null)
                {
                    phase.RemoveFile(buildFile);
                }
                return buildFile;
            }
        }

        /// <summary>
        /// Gets the libraries.
        /// </summary>
        /// <param name="targetName">Name of the target.</param>
        /// <returns>A list of libraries.</returns>
        public IEnumerable<String> GetLibraries(String targetName)
        {
            PBXTarget target = this.GetTarget(targetName);
            if (target == null)
            {
                yield break;
            }

            PBXBuildPhase phase = GetTargetPhase<PBXFrameworksBuildPhase>(target);
            if (phase == null)
            {
                yield break;
            }

            foreach (PBXBuildFile buildFile in phase.Files)
            {
				String extension = Path.GetExtension(buildFile.FileRef.Name);
				if (extension.Equals(".a") || extension.Equals(".dylib")) {
	                yield return Path.GetFileNameWithoutExtension(buildFile.FileRef.Name);
				}
            }
        }

        /// <summary>
        ///   Adds the library.
        /// </summary>
        /// <param name = "groups">The groups.</param>
        /// <param name = "library">The library.</param>
        /// <param name = "targetName">Name of the target.</param>
        /// <returns></returns>
        public PBXBuildFile AddLibrary(String groups, String library, String targetName)
        {
            lock (this.syncRoot)
            {
                PBXTarget target = this.GetTarget(targetName);
                PBXBuildPhase phase = GetTargetPhase<PBXFrameworksBuildPhase>(target);
                PBXFileElement fileElement = this.AddFile(groups, library, PBXSourceTree.Group);
                PBXBuildFile buildFile = phase.FindFile(fileElement);
                if (buildFile == null)
                {
                    buildFile = new PBXBuildFile(fileElement);
                    phase.AddFile(buildFile);
                }
                return buildFile;
            }
        }

        /// <summary>
        ///   Removes the framework.
        /// </summary>
        /// <param name = "groups">The groups.</param>
        /// <param name = "framework">The framework.</param>
        /// <param name = "targetName">Name of the target.</param>
        /// <returns></returns>
        public PBXBuildFile RemoveLibrary(String groups, String library, String targetName)
        {
            return this.RemoveFile(groups, Path.GetFileName(library), targetName);
        }

        /// <summary>
        /// Gets the frameworks.
        /// </summary>
        /// <param name="targetName">Name of the target.</param>
        /// <returns>A list of frameworks.</returns>
        public IEnumerable<String> GetFrameworks(String targetName)
        {
            PBXTarget target = this.GetTarget(targetName);
            if (target == null)
            {
                yield break;
            }

            PBXBuildPhase phase = GetTargetPhase<PBXFrameworksBuildPhase>(target);
            if (phase == null)
            {
                yield break;
            }

            foreach (PBXBuildFile buildFile in phase.Files)
            {
				String extension = Path.GetExtension(buildFile.FileRef.Name);
				if (extension.Equals(".framework")) {
	                yield return Path.GetFileNameWithoutExtension(buildFile.FileRef.Name);
				}
            }
        }

        /// <summary>
        ///   Adds the framework.
        /// </summary>
        /// <param name = "groups">The groups.</param>
        /// <param name = "framework">The framework.</param>
        /// <param name = "targetName">Name of the target.</param>
        /// <returns></returns>
        public PBXBuildFile AddFramework(String groups, String framework, String targetName)
        {
            lock (this.syncRoot)
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

                // Fallback: Assume it is a system framework
                path = String.Format(CultureInfo.CurrentCulture, "/System/Library/Frameworks/{0}.framework/{0}", framework);

                bail:
                String file = Path.GetDirectoryName(path);
                PBXTarget target = this.GetTarget(targetName);
                PBXBuildPhase phase = GetTargetPhase<PBXFrameworksBuildPhase>(target);
                PBXFileElement fileElement = this.AddFile(groups, file, PBXSourceTree.Absolute);
                PBXBuildFile buildFile = phase.FindFile(fileElement);
                if (buildFile == null)
                {
                    buildFile = new PBXBuildFile(fileElement);
                    phase.AddFile(buildFile);
                }
                return buildFile;
            }
        }

        /// <summary>
        ///   Removes the framework.
        /// </summary>
        /// <param name = "groups">The groups.</param>
        /// <param name = "framework">The framework.</param>
        /// <param name = "targetName">Name of the target.</param>
        /// <returns></returns>
        public PBXBuildFile RemoveFramework(String groups, String framework, String targetName)
        {
            return this.RemoveFile(groups, framework + ".framework", targetName);
        }

        /// <summary>
        ///   Adds the target.
        /// </summary>
        /// <param name = "targetName">Name of the target.</param>
        /// <param name = "type">The type.</param>
        /// <returns></returns>
        public PBXTarget AddTarget(String targetName, PBXProductType type)
        {
            lock (this.syncRoot)
            {
                PBXTarget target = this.GetTarget(targetName);
                if (target == null)
                {
                    switch (type)
                    {
                        case PBXProductType.Application:
                            {
                                this.Project.ProductRefGroup = this.AddGroup("Products");

                                PBXFileReference fileReference = new PBXFileReference();
                                fileReference.ExplicitFileType = PBXFileType.WrapperApplication;
                                fileReference.IncludeInIndex = 0;
                                fileReference.Path = targetName + ".app";
                                fileReference.SourceTree = PBXSourceTree.BuildProductDir;
                                this.Project.ProductRefGroup.AddChild(fileReference);

                                PBXNativeTarget nativeTarget = new PBXNativeTarget();
                                nativeTarget.AddBuildPhase(new PBXResourcesBuildPhase());
                                nativeTarget.AddBuildPhase(new PBXSourcesBuildPhase());
                                nativeTarget.AddBuildPhase(new PBXFrameworksBuildPhase());
                                nativeTarget.Name = targetName;
                                nativeTarget.ProductInstallPath = "$(HOME)/Applications";
                                nativeTarget.ProductName = targetName;
                                nativeTarget.ProductReference = fileReference;
                                nativeTarget.ProductType = type;
                                target = nativeTarget;

                                break;
                            }
                        case PBXProductType.LibraryDynamic:
                            {
                                this.Project.ProductRefGroup = this.AddGroup("Products");

                                PBXFileReference fileReference = new PBXFileReference();
                                fileReference.ExplicitFileType = PBXFileType.CompiledMachODylib;
                                fileReference.IncludeInIndex = 0;
                                fileReference.Path = targetName + ".dylib";
                                fileReference.SourceTree = PBXSourceTree.BuildProductDir;
                                this.Project.ProductRefGroup.AddChild(fileReference);

                                PBXNativeTarget nativeTarget = new PBXNativeTarget();
                                nativeTarget.AddBuildPhase(new PBXHeadersBuildPhase());
                                nativeTarget.AddBuildPhase(new PBXResourcesBuildPhase());
                                nativeTarget.AddBuildPhase(new PBXSourcesBuildPhase());
                                nativeTarget.AddBuildPhase(new PBXFrameworksBuildPhase());
                                nativeTarget.Name = targetName;
                                nativeTarget.ProductInstallPath = "/usr/lib";
                                nativeTarget.ProductName = targetName;
                                nativeTarget.ProductReference = fileReference;
                                nativeTarget.ProductType = type;
                                target = nativeTarget;
                                break;
                            }
                        default:
                            throw new NotSupportedException();
                    }
                    this.Project.AddTarget(target);
                }
                return target;
            }
        }

        /// <summary>
        ///   Removes the target.
        /// </summary>
        /// <param name = "targetName">Name of the target.</param>
        public void RemoveTarget(String targetName)
        {
            lock (this.syncRoot)
            {
                PBXTarget target = this.GetTarget(targetName);
                if (target != null)
                {
                    this.Project.RemoveTarget(target);
                    PBXNativeTarget nativeTarget = target as PBXNativeTarget;
                    if (nativeTarget != null)
                    {
                        switch (nativeTarget.ProductType)
                        {
                            case PBXProductType.Application:
                                this.RemoveFile("Products", targetName + ".app");
                                break;
                            case PBXProductType.LibraryDynamic:
                                this.RemoveFile("Products", targetName + ".dylib");
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                    }
                }
            }
        }

        /// <summary>
        ///   Adds the build configuration settings.
        /// </summary>
        /// <param name = "configurationName">Name of the configuration.</param>
        /// <param name = "targetName">Name of the target.</param>
        /// <param name = "key">The key.</param>
        /// <param name = "value">The value.</param>
        public void AddBuildConfigurationSettings(String configurationName, String targetName, String key, Object value)
        {
            lock (this.syncRoot)
            {
                PBXTarget target = this.GetTarget(targetName);
                XCBuildConfiguration buildConfiguration = this.GetBuildConfiguration(configurationName, target);
                buildConfiguration.BuildSettings.Add(new KeyValuePair<String, Object>(key, value));
            }
        }

        /// <summary>
        ///   Removes the build configuration settings.
        /// </summary>
        /// <param name = "configurationName">Name of the configuration.</param>
        /// <param name = "targetName">Name of the target.</param>
        /// <param name = "key">The key.</param>
        public void RemoveBuildConfigurationSettings(String configurationName, String targetName, String key)
        {
            lock (this.syncRoot)
            {
                PBXTarget target = this.GetTarget(targetName);
                XCBuildConfiguration buildConfiguration = this.GetBuildConfiguration(configurationName, target);
                buildConfiguration.BuildSettings.Remove(key);
            }
        }
		
        /// <summary>
        /// Clears the dependant projects.
        /// </summary>
        /// <param name="targetName">Name of the target.</param>
        /// <returns>A list of files.</returns>
        public void ClearDependantProjects(String targetName)
        {
			IEnumerable<PBXFileReference> fileReferences = this.Project.MainGroup.Children.OfType<PBXFileReference>().Where(e => e.LastKnownFileType == PBXFileType.WrapperPBProject);
			foreach(PBXFileReference fileReference in fileReferences) {
				// TODO: Refactor
                this.Project.MainGroup.RemoveChild(fileReference);
                this.Project.RemoveProjectReference(fileReference);
                // TODO: Remove project output from target
			}
        }
		
        /// <summary>
        ///   Adds the dependant project.
        /// </summary>
        /// <param name = "project">The project.</param>
        /// <param name = "targetName">Name of the target.</param>
        public void AddDependantProject(XcodeProject project, String targetName)
        {
            lock (this.syncRoot)
            {
                PBXGroup group = this.AddGroup("Products");
                PBXFileReference fileReference = this.AddFile(String.Empty, project.ProjectFolder) as PBXFileReference;
                this.Project.AddProjectReference(group, fileReference);
                // TODO: Add project output to target
            }
        }

        /// <summary>
        ///   Removes the dependant project.
        /// </summary>
        /// <param name = "project">The project.</param>
        /// <param name = "targetName">Name of the target.</param>
        public void RemoveDependantProject(XcodeProject project, String targetName)
        {
            lock (this.syncRoot)
            {
                PBXFileReference fileReference = this.Project.MainGroup.FindFileReference(project.ProjectFolder);
                if (fileReference == null)
                {
                    return;
                }
				// TODO: Refactor
                this.Project.MainGroup.RemoveChild(fileReference);
                this.Project.RemoveProjectReference(fileReference);
                // TODO: Remove project output from target
            }
        }

        /// <summary>
        ///   Gets the target.
        /// </summary>
        private PBXTarget GetTarget(String targetName)
        {
            if (String.IsNullOrEmpty(targetName))
            {
                return null;
            }
            return this.Project.Targets.FirstOrDefault(t => String.Equals(t.Name, targetName));
        }

        /// <summary>
        ///   Gets the target phase.
        /// </summary>
        private static T GetTargetPhase<T>(PBXTarget target) where T : PBXBuildPhase
        {
            T phase = target.BuildPhases.FirstOrDefault(p => p is T) as T;
            return phase;
        }

        /// <summary>
        ///   Gets the target phase.
        /// </summary>
        private static PBXBuildPhase GetTargetPhase(PBXTarget target, String file)
        {
            PBXFileType fileType = GetFileType(file);
            PBXBuildPhase phase;

            switch (fileType)
            {
                case PBXFileType.SourcecodeC:
                case PBXFileType.SourcecodeCppCpp:
                case PBXFileType.SourcecodeCObjc:
                    phase = GetTargetPhase<PBXSourcesBuildPhase>(target);
                    break;
                case PBXFileType.SourcecodeCH:
                case PBXFileType.SourcecodeCppH:
                    phase = GetTargetPhase<PBXHeadersBuildPhase>(target);
                    break;
                case PBXFileType.FileXib:
                case PBXFileType.TextPlist:
                case PBXFileType.TextPlistInfo:
                    phase = GetTargetPhase<PBXResourcesBuildPhase>(target);
                    break;
                case PBXFileType.WrapperFramework:
                    phase = GetTargetPhase<PBXFrameworksBuildPhase>(target);
                    break;
                default:
                    return null;
            }

            return phase;
        }

        private XCBuildConfiguration GetBuildConfiguration(String configurationName, PBXTarget target)
        {
            XCConfigurationList configurationList = target != null ? target.BuildConfigurationList : this.Project.BuildConfigurationList;
            XCBuildConfiguration buildConfiguration = configurationList.FindConfiguration(configurationName);
            if (buildConfiguration == null)
            {
                buildConfiguration = new XCBuildConfiguration();
                buildConfiguration.Name = configurationName;
                configurationList.AddBuildConfiguration(buildConfiguration);
                if (configurationList.BuildConfigurations.Count() == 1)
                {
                    configurationList.DefaultConfigurationName = configurationName;
                }
            }
            return buildConfiguration;
        }
    }
}