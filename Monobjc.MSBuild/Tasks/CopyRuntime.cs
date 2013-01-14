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
using Monobjc.Tools.Utilities;
using Microsoft.Build.Tasks;
using System.IO;

namespace Monobjc.MSBuild.Tasks
{
    public class CopyRuntime : Task
    {
        private MacOSVersion targetOSVersion;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Monobjc.MSBuild.Tasks.CopyRuntime"/> class.
		/// </summary>
		public CopyRuntime()
		{
			this.targetOSVersion = MacOSVersion.MacOS105;
		}
		
		/// <summary>
		/// Gets or sets the name of the application.
		/// </summary>
		/// <value>
		/// The name of the application.
		/// </value>
        [Required]
		public String ApplicationName { get; set; }
		
		/// <summary>
		/// Gets or sets the target OS version.
		/// </summary>
		/// <value>
		/// The target OS version.
		/// </value>
        public String TargetOSVersion
		{
			get { return this.targetOSVersion.ToString(); }
			set { this.targetOSVersion = (MacOSVersion) Enum.Parse(typeof(MacOSVersion), value); }
		}
		
        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
        [Required]
		public ITaskItem ToDirectory { get; set; }
		
        public override bool Execute()
        {
			// TODO: I18N
            this.Log.LogMessage("Copying runtime");

			String path = Path.Combine(this.ToDirectory.ItemSpec, this.ApplicationName);
			FileProvider.CopyFile(this.targetOSVersion, "runtime", path, "a+x");
			
			return true;
        }
    }
}
