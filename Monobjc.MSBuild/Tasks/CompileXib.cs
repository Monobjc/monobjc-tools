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
using Monobjc.MSBuild.Utilities;
using Monobjc.Tools.Generators;
using Monobjc.Tools.Properties;

namespace Monobjc.MSBuild.Tasks
{
    public class CompileXib : Task
    {
        [Required]
        public String XibFile { get; set; }

        [Required]
        public String ToDirectory { get; set; }

        public override bool Execute()
        {
            XibCompiler compiler = new XibCompiler();
            compiler.Logger = new ExecutionLogger(this);
            if (!compiler.Compile(this.XibFile, this.ToDirectory))
            {
                this.Log.LogError(Resources.XibCompilationFailed);
                return false;
            }
            return true;
        }
    }
}