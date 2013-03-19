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
using Monobjc.MSBuild.Tasks;
using NUnit.Framework;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Utilities;

namespace Monobjc.MSBuild.Tests
{
    [TestFixture]
	public class CombineArtworkTests
    {
        public void TestTask()
        {
			CombineArtwork task = new CombineArtwork();
            task.BuildEngine = new ShallowBuildEngine();
			task.DoCombine = true;
            task.Directory = new TaskItem(".");
            bool result = task.Execute();
            Assert.IsTrue(result);
        }
    }
}
