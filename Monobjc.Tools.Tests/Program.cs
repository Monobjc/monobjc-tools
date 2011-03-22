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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monobjc.Tools
{
    public class Program
    {
        public static void Main(String[] args)
        {
            //PBXProjectGenerationTests pBXProjectGenerationTests = new PBXProjectGenerationTests();
            //pBXProjectGenerationTests.TestProjectGeneration001();

            XcodeProjectGenerationTests xcodeProjectGenerationTests = new XcodeProjectGenerationTests();
            xcodeProjectGenerationTests.TestProjectGeneration001();

            //XIBLoadTests xibLoadTests = new XIBLoadTests();
            //xibLoadTests.TestMainMenuReading001();
            //xibLoadTests.TestMainMenuReading002();
            //xibLoadTests.TestMainMenuReading003();
            //xibLoadTests.TestMainMenuReading004();
            //xibLoadTests.TestMainMenuReading005();
            //xibLoadTests.TestMainMenuReading006();
            //xibLoadTests.TestMainMenuReading007();
            //xibLoadTests.TestMainMenuReading008();
            //xibLoadTests.TestMainMenuReading010();
            //xibLoadTests.TestMyDocumentReading005();

            //PListLoadTests pListLoadTests = new PListLoadTests();
            //pListLoadTests.TestPListReading001();
            //pListLoadTests.TestPListReading002();
            //pListLoadTests.TestPListReading004();
            //pListLoadTests.TestPListReading005();
            //pListLoadTests.TestPListReading010();
        }
    }
}
