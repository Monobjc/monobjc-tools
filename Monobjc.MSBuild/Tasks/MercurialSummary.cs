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
using Monobjc.Tools.External;
using Monobjc.Tools.Properties;

namespace Monobjc.MSBuild.Tasks
{
    /// <summary>
    ///   This task extracts the latest Mercurial revision from a folder under Mercurial version control.
    /// </summary>
    public class MercurialSummary : Task
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Directory { get; set; }

        public override bool Execute()
        {
            String number = Mercurial.Revision(this.Directory);
            if (number != null)
            {
                Environment.SetEnvironmentVariable(this.Name, number);
                this.Log.LogMessage(Resources.FoundValidRevision, number);
                return true;
            }
            return false;
        }
    }
}