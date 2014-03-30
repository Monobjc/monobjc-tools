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
using System.Diagnostics;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;
using Monobjc.MSBuild.Tasks;
using NUnit.Framework;

namespace Monobjc.MSBuild.Tests
{
    [TestFixture]
    public class CodeSigningTests
    {
        public const String IDENTITY = "3rd Party Mac Developer Application: Laurent Etiemble";

        [Test]
        public void TestCodeSignBundle()
        {
            Copy("/Applications/Calculator.app", "/tmp/");

            CodeSigning task = new CodeSigning();
            task.BuildEngine = new ShallowBuildEngine();
            task.Identity = IDENTITY;
            task.Bundle = new TaskItem("/tmp/Calculator.app");
            bool result = task.Execute();
            Assert.IsTrue(result);
        }
        
        [Test]
        public void TestCodeSignLibrary()
        {
            Copy("/usr/lib/libobjc.dylib", "/tmp/");

            CodeSigning task = new CodeSigning();
            task.BuildEngine = new ShallowBuildEngine();
            task.Identity = IDENTITY;
            task.Target = new TaskItem("/tmp/libobjc.dylib");
            bool result = task.Execute();
            Assert.IsTrue(result);
        }
        
        [Test]
        public void TestCodeSignLibraries()
        {
            Copy("/usr/lib/libffi.dylib", "/tmp/");
            Copy("/usr/lib/libobjc.dylib", "/tmp/");

            CodeSigning task = new CodeSigning();
            task.BuildEngine = new ShallowBuildEngine();
            task.Identity = IDENTITY;
            task.Targets = new ITaskItem[] { new TaskItem("/tmp/libffi.dylib"), new TaskItem("/tmp/libobjc.dylib") };
            bool result = task.Execute();
            Assert.IsTrue(result);
        }
        
        [Test]
        public void TestCodeSignFramework()
        {
            Copy("/System/Library/Frameworks/TWAIN.framework", "/tmp/");

            CodeSigning task = new CodeSigning();
            task.BuildEngine = new ShallowBuildEngine();
            task.Identity = IDENTITY;
            task.Target = new TaskItem("/tmp/TWAIN.framework");
            task.Versioned = true;
            bool result = task.Execute();
            Assert.IsTrue(result);
        }
        
        [Test]
        public void TestCodeSignFrameworks()
        {
            Copy("/System/Library/Frameworks/OpenGL.framework", "/tmp/");
            Copy("/System/Library/Frameworks/TWAIN.framework", "/tmp/");

            CodeSigning task = new CodeSigning();
            task.BuildEngine = new ShallowBuildEngine();
            task.Identity = IDENTITY;
            task.Targets = new ITaskItem[] { new TaskItem("/tmp/OpenGL.framework"), new TaskItem("/tmp/TWAIN.framework") };
            task.Versioned = true;
            bool result = task.Execute();
            Assert.IsTrue(result);
        }

        private static void Copy(String source, String destination)
        {
            Exec exec = new Exec();
            exec.BuildEngine = new ShallowBuildEngine();
            exec.Command = String.Format("rsync -rpl \"{0}\" \"{1}\"", source, destination);
            exec.Execute();
        }
    }
}
