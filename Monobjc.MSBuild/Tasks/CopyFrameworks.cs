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
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;
using Monobjc.Tools.Utilities;

namespace Monobjc.MSBuild.Tasks
{
    public class CopyFrameworks : Task
    {
		public CopyFrameworks()
		{
		}
		
        public ITaskItem[] Frameworks { get; set; }
		
        [Required]
		public ITaskItem ToDirectory { get; set; }
		
        public override bool Execute()
        {
			// TODO: I18N
            this.Log.LogMessage("Copying frameworks");
			
			if (this.Frameworks != null && this.Frameworks.Length > 0) {
				Directory.CreateDirectory(this.ToDirectory.ItemSpec);
			}
			
			foreach(ITaskItem item in this.Frameworks) {
				String name = item.ItemSpec;
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
