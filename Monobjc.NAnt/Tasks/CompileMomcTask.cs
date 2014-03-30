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
using System.IO;
using Monobjc.Tools.External;
using Monobjc.Tools.Properties;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Monobjc.NAnt.Tasks
{
    [TaskName("compile-momc")]
    public class CompileMomcTask : Task
    {
        /// <summary>
        ///   Gets or sets the directory.
        /// </summary>
        /// <value>The directory.</value>
        [TaskAttribute("model", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public DirectoryInfo Model { get; set; }

        /// <summary>
        ///   Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
        [TaskAttribute("todir", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public DirectoryInfo ToDirectory { get; set; }

        protected override void ExecuteTask()
        {
            String output = Momc.Compile(this.Model.ToString(), this.ToDirectory.ToString());
            if (output == null)
            {
                this.Log(Level.Info, Resources.MomcModelUpToDate, this.Model);
                return;
            }
            this.Log(Level.Info, Resources.CommandReturned, output);
        }
    }
}