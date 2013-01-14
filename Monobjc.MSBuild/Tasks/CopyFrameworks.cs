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
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;
using Monobjc.Tools.Utilities;

namespace Monobjc.MSBuild.Tasks
{
    public class CopyFrameworks : Task
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="Monobjc.MSBuild.Tasks.CopyFrameworks"/> class.
		/// </summary>
		public CopyFrameworks()
		{
		}
		
		/// <summary>
		/// Gets or sets the frameworks.
		/// </summary>
		/// <value>
		/// The frameworks.
		/// </value>
        public String Frameworks { get; set; }
		
        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
        [Required]
		public ITaskItem ToDirectory { get; set; }
		
        public override bool Execute()
        {
			if (this.Frameworks == null) {
				return true;
			}
			
			// TODO: I18N
            this.Log.LogMessage("Copying frameworks");
			
			foreach(String name in this.Frameworks.Split(';')) {
				String path;
				
				// Probe system location
				path = String.Format("/System/Library/Frameworks/{0}.framework", name);
				if (Directory.Exists(path)) {
					goto copy;
				}
				path = String.Format("/Library/Frameworks/{0}.framework", name);
				if (Directory.Exists(path)) {
					goto copy;
				}
				path = String.Format("~/Library/Frameworks/{0}.framework", name);
				if (Directory.Exists(path)) {
					goto copy;
				}
				
				// TODO: I18N
				this.Log.LogError("Framework {0} not found", name);
				continue;
			copy:
				Copy copy = new Copy();
				copy.BuildEngine = this.BuildEngine;
				copy.SourceFiles = new TaskItem[] { new TaskItem(path) };
				copy.DestinationFolder = this.ToDirectory;
				copy.SkipUnchangedFiles = true;
				copy.Execute();
			}
			
			return true;
        }
    }
}
