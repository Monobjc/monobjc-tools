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
using Microsoft.Build.Utilities;
using Monobjc.MSBuild.Utilities;
using Monobjc.Tools.Generators;

namespace Monobjc.MSBuild.Tasks
{
    public class CombineArtwork : Task
    {
		/// <summary>
		/// Gets or sets a value indicating whether to do artwork combination.
		/// </summary>
		/// <value><c>true</c> to do artwork combination; otherwise, <c>false</c>.</value>
		public bool DoCombine { get; set; }

        /// <summary>
        /// Gets or sets the dir.
        /// </summary>
        /// <value>The dir.</value>
        [Required]
		public ITaskItem Directory { get; set; }
		
        public override bool Execute()
        {
			// TODO: I18N
            this.Log.LogMessage("Combining artwork...");

			using (StringWriter outputWriter = new StringWriter()) {
				using (StringWriter errorWriter = new StringWriter()) {
					ArtworkCombiner combiner = new ArtworkCombiner();
					combiner.Logger = new ExecutionLogger(this);
					combiner.Combine(this.Directory.GetMetadata("FullPath"), outputWriter, errorWriter);
					String outputLog = outputWriter.ToString ();
					String errorLog = errorWriter.ToString ();
					this.Log.LogMessage (outputLog);
					this.Log.LogMessage (errorLog);
				}
			}

			return true;
        }
    }
}
