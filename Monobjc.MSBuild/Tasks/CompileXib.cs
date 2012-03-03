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
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Monobjc.MSBuild.Utilities;
using Monobjc.Tools.Generators;
using Monobjc.Tools.Properties;
using System.IO;

namespace Monobjc.MSBuild.Tasks
{
	public class CompileXib : Task
	{
		/// <summary>
		/// Gets or sets the xib file.
		/// </summary>
		/// <value>
		/// The xib file.
		/// </value>
		public ITaskItem XibFile { get; set; }
		
		/// <summary>
		/// Gets or sets the xib files.
		/// </summary>
		/// <value>
		/// The xib files.
		/// </value>
		public ITaskItem[] XibFiles { get; set; }
		
        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
        [Required]
		public ITaskItem ToDirectory { get; set; }

        /// <summary>
        ///   Executes the task.
        /// </summary>
		public override bool Execute ()
		{
			if (this.XibFile != null) {
				XibCompiler compiler = new XibCompiler ();
				compiler.Logger = new ExecutionLogger (this);
				
				String file = this.XibFile.ItemSpec;
				String dest = this.ToDirectory.ItemSpec;
				String parent = Path.GetDirectoryName(file);
				dest = Path.Combine(dest, parent);
				
				if (!compiler.Compile (file, dest)) {
					this.Log.LogError (Resources.XibCompilationFailed);
					return false;
				}
				return true;
			} else if (this.XibFiles != null) {
				XibCompiler compiler = new XibCompiler ();
				compiler.Logger = new ExecutionLogger (this);
				foreach (ITaskItem item in this.XibFiles) {
					String file = item.ItemSpec;
					String dest = this.ToDirectory.ItemSpec;
					String parent = Path.GetDirectoryName(file);
					dest = Path.Combine(dest, parent);
				
					if (!compiler.Compile (file, dest)) {
						this.Log.LogError (Resources.XibCompilationFailed);
						return false;
					}
				}
				return true;
			} else {

				this.Log.LogError(Resources.XibCompilationFailed);
				return false;
			}
		}
	}
}