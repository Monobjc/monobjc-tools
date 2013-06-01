//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2013 - Laurent Etiemble
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
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Monobjc.MSBuild.Utilities;
using Monobjc.Tools.Generators;
using Monobjc.Tools.Properties;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Monobjc.MSBuild.Tasks
{
	public class CompileXib : Task
	{
		/// <summary>
		/// Gets or sets the xib file.
		/// </summary>
		/// <value>The xib file.</value>
		public ITaskItem XibFile { get; set; }
		
		/// <summary>
		/// Gets or sets the xib files.
		/// </summary>
		/// <value>The xib files.</value>
		public ITaskItem[] XibFiles { get; set; }
		
		/// <summary>
		/// Gets or sets the output dir.
		/// </summary>
		/// <value>The output dir.</value>
		public ITaskItem ToDirectory { get; set; }

		/// <summary>
		/// Gets or sets the output dir.
		/// </summary>
		/// <value>The output dirs.</value>
		public ITaskItem[] ToDirectories { get; set; }

		/// <summary>
		///   Executes the task.
		/// </summary>
		public override bool Execute ()
		{
			List<ITaskItem> files = new List<ITaskItem> ();
			List<ITaskItem> directories = new List<ITaskItem> ();
			
			this.Log.LogMessage ("XibFile       : {0}", "" + this.XibFile);
			this.Log.LogMessage ("XibFiles      : {0}", "" + this.XibFiles);
			this.Log.LogMessage ("ToDirectory   : {0}", "" + this.ToDirectory);
			this.Log.LogMessage ("ToDirectories : {0}", "" + this.ToDirectories);
			
			if (this.XibFile != null && this.ToDirectory != null) {
				files.Add (this.XibFile);
				directories.Add (this.ToDirectory);
			} else if (this.XibFiles != null && this.ToDirectory != null) {
				files.AddRange (this.XibFiles);
				for (int i = 0; i < files.Count; i++) {
					directories.Add (this.ToDirectory);
				}
			} else if (this.XibFiles != null && this.ToDirectories != null) {
				files.AddRange (this.XibFiles);
				directories.AddRange (this.ToDirectories);
			} else if (this.XibFile == null && this.XibFiles == null) {
				return true;
			} else {
				return true;
			}
			
			this.Log.LogMessage ("Processing {0} files to {1} folders", files.Count, directories.Count);
			
			if (files.Count == 0) {
				return true;
			}
			
			if (files.Count != directories.Count) {
				this.Log.LogError (Resources.XibCompilationFailed);
				return false;
			}
			
			XibCompiler compiler = new XibCompiler ();
			compiler.Logger = new ExecutionLogger (this);
			
			for (int i = 0; i < files.Count; i++) {
				String file = files [i].ItemSpec;
				String dest = directories [i].ItemSpec;
				String parent = Path.GetDirectoryName (file);
				dest = Path.Combine (dest, parent);
				
				if (!compiler.Compile (file, dest)) {
					this.Log.LogError (Resources.XibCompilationFailed);
					return false;
				}
			}
			
			return true;
		}
	}
}
