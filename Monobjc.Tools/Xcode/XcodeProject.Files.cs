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

namespace Monobjc.Tools.Xcode
{
	/// <summary>
	///   Wraps a Xcode project.
	/// </summary>
	public partial class XcodeProject
	{
		/// <summary>
		///   Adds the group.
		/// </summary>
		/// <param name = "groups">The group path.</param>
		/// <returns></returns>
		public PBXGroup AddGroup (String groups)
		{
			lock (this.syncRoot) {
				// Split the group paths
				List<string> parts = groups.Split (new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).ToList ();
				PBXGroup group = this.Document.Project.MainGroup;
				foreach (String part in parts) {
					PBXGroup g = group.FindGroup (part);
					if (g == null) {
						// For each group not found, create it
						g = new PBXGroup (part);
						group.AddChild (g);
					}
					group = g;
				}
				return group;
			}
		}

		/// <summary>
		///   Removes the group.
		/// </summary>
		/// <param name = "groups">The groups.</param>
		/// <returns></returns>
		public PBXGroup RemoveGroup (String groups)
		{
			lock (this.syncRoot) {
				// Only keep the n-1 groups
				List<String> parts = groups.Split (new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).ToList ();
				String last = parts.Last ();
				parts.RemoveAt (parts.Count - 1);

				// Go to the parent group
				PBXGroup g, group = this.Document.Project.MainGroup;
				foreach (String part in parts) {
					g = group.FindGroup (part);
					if (g == null) {
						// For each group not found, create it
						g = new PBXGroup (part);
						group.AddChild (g);
					}
					group = g;
				}

				// If the group to delete exists, remove it
				g = group.FindGroup (last);
				if (g != null) {
					group.RemoveChild (g);
				}

				return g;
			}
		}

		/// <summary>
		///   Adds the file.
		/// </summary>
		/// <param name = "groups">The groups.</param>
		/// <param name = "file">The file.</param>
		/// <returns></returns>
		public PBXFileElement AddFile (String groups, String file)
		{
			return this.AddFile (groups, file, PBXSourceTree.None);
		}

		/// <summary>
		///   Adds the file.
		/// </summary>
		/// <param name = "groups">The groups.</param>
		/// <param name = "file">The file.</param>
		/// <param name = "sourceTree">The source tree.</param>
		/// <returns></returns>
		public PBXFileElement AddFile (String groups, String file, PBXSourceTree sourceTree)
		{
			lock (this.syncRoot) {
				// Prepare the group that will contain the file
				PBXGroup group = this.AddGroup (groups);
				PBXFileReference fileReference = null;
				PBXFileElement result = null;

				// Extract information
				String name = Path.GetFileName (file);
				String path = Path.GetFullPath (file);
				String rootDir = Path.GetFullPath (this.Dir);
				if (!String.IsNullOrEmpty(this.BaseDir)) {
					rootDir = Path.Combine(rootDir, this.BaseDir);
					rootDir = Path.GetFullPath (rootDir);
				}
				String parentDir = Path.GetDirectoryName (file);

				// If the file is localized, then add it to a variant group
				if (Path.GetExtension (parentDir).Equals (".lproj")) {
					// The variant group may exists to search for it
					PBXVariantGroup variantGroup = group.FindVariantGroup (name);
					if (variantGroup == null) {
						variantGroup = new PBXVariantGroup (name);
						group.AddChild (variantGroup);
					}

					// The file is named like the language
					name = Path.GetFileNameWithoutExtension (parentDir);
					group = variantGroup;
					result = variantGroup;
				}

				// Check if the file already exists
				fileReference = group.FindFileReference (name);
				if (fileReference == null) {
					// Create a file reference
					fileReference = new PBXFileReference (name);

					// Set the source tree if none specified
					if (sourceTree != PBXSourceTree.None) {
						fileReference.SourceTree = sourceTree;
					} else {
						if (path.StartsWith (rootDir)) {
							path = path.Substring (rootDir.Length + 1);
							fileReference.SourceTree = PBXSourceTree.Group;
						} else {
							fileReference.SourceTree = PBXSourceTree.Absolute;
						}
					}
					fileReference.Path = path;
					fileReference.LastKnownFileType = GetFileType (file);

					// Add it to the group
					group.AddChild (fileReference);
				}

				return result ?? fileReference;
			}
		}

		/// <summary>
		///   Removes the file.
		/// </summary>
		/// <param name = "groups">The groups.</param>
		/// <param name = "file">The file.</param>
		/// <returns></returns>
		public PBXFileReference RemoveFile (String groups, String file)
		{
			lock (this.syncRoot) {
				// Prepare the group that contains the file
				PBXGroup group = this.AddGroup (groups);
				PBXFileReference fileReference = null;
				PBXFileElement result = null;

				// Extract information
				String name = Path.GetFileName (file);
				String parentDir = Path.GetDirectoryName (file);

				// If the file is localized, search for the variant group
				if (Path.GetExtension (parentDir).Equals (".lproj")) {
					PBXVariantGroup variantGroup = group.FindVariantGroup (name);
					if (variantGroup != null) {
						// The file is named like the language
						name = Path.GetFileNameWithoutExtension (parentDir);
					}
					group = variantGroup;
					result = variantGroup;
				}

				if (group != null) {
					// Search for the file and remove it
					fileReference = group.FindFileReference (name);
					if (fileReference != null) {
						group.RemoveChild (fileReference);
					}
				}

				if (result == null) {
					result = fileReference;
				}
				return fileReference;
			}
		}

		/// <summary>
		///   Gets the type of the file.
		/// </summary>
		private static PBXFileType GetFileType (String file)
		{
			String extension = Path.GetExtension (file);
			switch (extension) {
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