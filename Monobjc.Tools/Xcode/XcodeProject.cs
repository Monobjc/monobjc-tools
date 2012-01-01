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
using System.IO;

namespace Monobjc.Tools.Xcode
{
	/// <summary>
	///   Wraps a Xcode project.
	/// </summary>
	public partial class XcodeProject
	{
		private Object syncRoot = new Object ();

		/// <summary>
		///   Initializes a new instance of the <see cref = "XcodeProject" /> class.
		/// </summary>
		/// <param name = "dir">The dir.</param>
		/// <param name = "name">The name.</param>
		public XcodeProject (String dir, string name)
		{
			this.Dir = dir;
			this.Name = name;
			this.Document = new PBXDocument ();
		}

		/// <summary>
		///   Gets or sets the dir.
		/// </summary>
		/// <value>The dir.</value>
		public String Dir { get; set; }

		/// <summary>
		///   Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public String Name { get; set; }

		/// <summary>
		/// Gets or sets the base dir.
		/// </summary>
		/// <value>
		/// The base dir.
		/// </value>
		public String BaseDir { 
			get { return this.Project.ProjectDirPath; } 
			set { this.Project.ProjectDirPath = value; }
		}

		/// <summary>
		///   Gets or sets the document.
		/// </summary>
		/// <value>The document.</value>
		public PBXDocument Document { get; private set; }

		/// <summary>
		///   Gets or sets the project folder with the xcodeproj extension.
		/// </summary>
		/// <value>The folder.</value>
		public String ProjectFolder {
			get {
				String folder = Path.Combine (this.Dir, this.Name + ".xcodeproj");
				folder = Path.GetFullPath (folder);
				Directory.CreateDirectory (folder);
				return folder;
			}
		}

		/// <summary>
		///   Gets or sets the project file with the pbxproj extension.
		/// </summary>
		/// <value>The file.</value>
		public String ProjectFile {
			get {
				String file = Path.Combine (this.ProjectFolder, "project.pbxproj");
				file = Path.GetFullPath (file);
				return file;
			}
		}

		/// <summary>
		///   Saves the project; create the xcodeproj bundle and write the project file.
		/// </summary>
		public void Save ()
		{
			lock (this.syncRoot) {
				this.Document.WriteToFile (this.ProjectFile);
			}
		}

		/// <summary>
		///   Delete the xcodeproj bundle and project file.
		/// </summary>
		public void Delete ()
		{
			lock (this.syncRoot) {
				Directory.Delete (this.ProjectFolder, true);
			}
		}

		/// <summary>
		///   Gets or sets the project.
		/// </summary>
		/// <value>The project.</value>
		private PBXProject Project {
			get { return this.Document.Project; }
		}
	}
}