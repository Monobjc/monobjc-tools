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
		/// <value>
		/// The path.
		/// </value>
        public ITaskItem Path { get; set; }
		
		/// <summary>
		/// Gets or sets the info P list.
		/// </summary>
		/// <value>
		/// The info P list.
		/// </value>
        [Required]
        public ITaskItem InfoPList { get; set; }
		
		/// <summary>
		/// Gets or sets to directory.
		/// </summary>
		/// <value>
		/// To directory.
		/// </value>
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
				receigen.Executable = this.Path.ItemSpec;
			}
			
			String plist = System.IO.Path.GetFullPath(this.InfoPList.ItemSpec);
			String directory = System.IO.Path.GetFullPath(this.ToDirectory.ItemSpec);
			
			String result = receigen.Generate(plist, directory);
			this.Log.LogMessage(result);
			
			return true;
        }
    }
}