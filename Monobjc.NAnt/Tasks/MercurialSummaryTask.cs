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
using Monobjc.Tools.External;
using Monobjc.Tools.Properties;
using Monobjc.Tools.Utilities;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Monobjc.NAnt.Tasks
{
    /// <summary>
    ///   This task extracts the latest Mercurial revision from a folder under Mercurial version control.
    /// </summary>
    [TaskName("hg-sum")]
    public class MercurialSummaryTask : Task
    {
        /// <summary>
        ///   Gets or sets the folder to use.
        /// </summary>
        /// <value>The revision.</value>
        [TaskAttribute("dir", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public DirectoryInfo Directory { get; set; }

        /// <summary>
        ///   Gets or sets the revision property that will hold the result.
        /// </summary>
        /// <value>The revision.</value>
        [TaskAttribute("revision", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string Revision { get; set; }

        /// <summary>
        ///   Executes the task.
        /// </summary>
        protected override void ExecuteTask()
        {
            String number = Mercurial.Revision(this.Directory.ToString());
            if (number != null)
            {
                this.Properties[this.Revision] = number;
                this.Log(Level.Info, Resources.FoundValidRevision, number);
            }
        }
    }
}