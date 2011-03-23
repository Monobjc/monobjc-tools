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
using Monobjc.Tools.Properties;
using Monobjc.Tools.PropertyList;
using NUnit.Framework;
using Monobjc.Tools.Xcode;

namespace Monobjc.Tools
{
    [TestFixture]
    [Category("Xcode")]
    [Category("Generation")]
    public class XcodeProjectGenerationTests
    {
        [Test]
        public void TestProjectGeneration001()
        {
            // Create the document
            XcodeProject project = new XcodeProject(".", "MyApplication");
            project.AddGroup("Classes");
            project.AddFile("Classes", "Classes/AppDelegate.h");
            project.AddGroup("Resources");
            project.AddFile("Resources", "en.lproj/MainMenu.xib");
            project.AddFile("Resources", "fr.lproj/MainMenu.xib");
            project.AddGroup("Frameworks");
            project.AddFramework("Frameworks", "Cocoa");
            project.AddGroup("Products");

            project.Save();
        }
    }
}
