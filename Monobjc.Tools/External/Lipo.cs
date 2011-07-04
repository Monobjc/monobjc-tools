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
using System.Globalization;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.External
{
    /// <summary>
    ///   Wrapper class around the <c>lipo</c> command line tool.
    /// </summary>
    public static class Lipo
    {
        /// <summary>
        ///   Returns the architectures for the given file.
        /// </summary>
        /// <param name = "file">The file.</param>
        /// <returns>The result of the command.</returns>
        public static MacOSArchitecture GetArchitecture(String file)
        {
            String arguments = String.Format(CultureInfo.InvariantCulture, "-info \"{0}\"", file);
            ProcessHelper helper = new ProcessHelper(Executable, arguments);
            String output = helper.Execute();

            MacOSArchitecture architecture = MacOSArchitecture.None;
            if (output.Contains("i386"))
            {
                architecture |= MacOSArchitecture.X86;
            }
            if (output.Contains("x86_64"))
            {
                architecture |= MacOSArchitecture.X8664;
            }
            if (output.Contains("ppc7400"))
            {
                architecture |= MacOSArchitecture.PPC;
            }

            return architecture;
        }

        private static string Executable
        {
            get { return "lipo"; }
        }
    }
}