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
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Monobjc.Tools.Utilities;
using Microsoft.Build.Tasks;
using System.IO;

namespace Monobjc.MSBuild.Tasks
{
    public class CopyRuntime : Task
    {
		public CopyRuntime()
		{
			this.TargetOSVersion = MacOSVersion.MacOS105;
		}
		
        [Required]
		public String ApplicationName { get; set; }
		
        public MacOSVersion TargetOSVersion { get; set; }
		
        [Required]
		public ITaskItem ToDirectory { get; set; }
		
        public override bool Execute()
        {
			// TODO: I18N
            this.Log.LogMessage("Copying runtime");

			String path = Path.Combine(this.ToDirectory.ItemSpec, this.ApplicationName);
			FileProvider.CopyFile(this.TargetOSVersion, "runtime", path, "a+x");
			
			return true;
        }
    }
}
