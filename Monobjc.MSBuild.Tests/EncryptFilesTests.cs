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
using Monobjc.MSBuild.Tasks;
using NUnit.Framework;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace Monobjc.MSBuild.Tests
{
	[TestFixture]
	public class EncryptFilesTests
	{
		[Test]
		public void TestNoSourceFiles ()
		{
			EncryptFiles task = new EncryptFiles ();
			task.BuildEngine = new ShallowBuildEngine ();
			task.SourceFiles = new ITaskItem[0];
			task.EncryptionSeed = "monobjc";
			bool result = task.Execute ();
			Assert.IsTrue (result);
		}
		
		[Test]
		public void TestDifferentSourceAndDestination ()
		{
			EncryptFiles task = new EncryptFiles ();
			task.BuildEngine = new ShallowBuildEngine ();
			task.SourceFiles = new ITaskItem[] { null, null };
			task.DestinationFiles = new ITaskItem[] { null };
			task.EncryptionSeed = "monobjc";
			bool result = task.Execute ();
			Assert.IsFalse (result);
		}
		
		[Test]
		public void TestDestinationFilesAndFolder ()
		{
			EncryptFiles task = new EncryptFiles ();
			task.BuildEngine = new ShallowBuildEngine ();
			task.SourceFiles = new ITaskItem[] { null };
			task.DestinationFiles = new ITaskItem[] { null };
			task.DestinationFolder = new TaskItem ("./encrypted");
			task.EncryptionSeed = "monobjc";
			bool result = task.Execute ();
			Assert.IsFalse (result);
		}
		
		[Test]
		public void TestNoDestinationFolder ()
		{
			EncryptFiles task = new EncryptFiles ();
			task.BuildEngine = new ShallowBuildEngine ();
			task.SourceFiles = new ITaskItem[] { null };
			task.EncryptionSeed = "monobjc";
			bool result = task.Execute ();
			Assert.IsFalse (result);
		}
		
		[Test]
		public void TestFilesToFolder ()
		{
			EncryptFiles task = new EncryptFiles ();
			task.BuildEngine = new ShallowBuildEngine ();
			task.SourceFiles = new ITaskItem[]{ new TaskItem ("CallReceigenTests_Info.plist") };
			task.DestinationFolder = new TaskItem ("./encrypted");
			task.EncryptionSeed = "monobjc";
			bool result = task.Execute ();
			Assert.IsTrue (result);
		}
		
		[Test]
		public void TestFilesToFiles ()
		{
			EncryptFiles task = new EncryptFiles ();
			task.BuildEngine = new ShallowBuildEngine ();
			task.SourceFiles = new ITaskItem[]{ new TaskItem ("CallReceigenTests_Info.plist") };
			task.DestinationFiles = new ITaskItem[]{ new TaskItem ("./encrypted/CallReceigenTests_Info.plist") };
			task.EncryptionSeed = "monobjc";
			bool result = task.Execute ();
			Assert.IsTrue (result);
		}
	}
}
