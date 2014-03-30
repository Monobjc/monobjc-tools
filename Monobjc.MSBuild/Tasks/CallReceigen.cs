//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2014 - Laurent Etiemble
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
using Monobjc.Tools.External;

namespace Monobjc.MSBuild.Tasks
{
    /// <summary>
    ///   This task can generates receipt validation code.
    /// </summary>
    public class CallReceigen : Task
    {
		/// <summary>
		/// Gets or sets the path.
		/// </summary>
        public ITaskItem Path { get; set; }
		
		/// <summary>
		/// Gets or sets the info.plist path.
		/// </summary>
        [Required]
        public ITaskItem InfoPList { get; set; }
		
		/// <summary>
		/// Gets or sets the output directory.
		/// </summary>
        [Required]
        public ITaskItem ToDirectory { get; set; }

        /// <summary>
        ///   Executes the task.
        /// </summary>
		public override bool Execute ()
		{
			Receigen receigen = new Receigen();
            receigen.Logger = new ExecutionLogger(this);
			
			if (this.Path != null) {
				receigen.Executable = this.Path.GetMetadata("FullPath");
			}
			
			String plist = this.InfoPList.GetMetadata("FullPath");
			String directory = this.ToDirectory.GetMetadata("FullPath");
			
			String result = receigen.Generate(plist, directory);
			this.Log.LogMessage(result);
			
			return true;
        }
    }
}
