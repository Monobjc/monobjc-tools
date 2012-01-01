//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2012 - Laurent Etiemble
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
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.External
{
    /// <summary>
    ///   Wrapper class around the <c>otool</c> command line tool.
    /// </summary>
    public static class OTool
    {
        private static readonly Regex OTOOL_ID_REGEX = new Regex(@"(?<library>.*\.dylib)[^:]", RegexOptions.Multiline | RegexOptions.CultureInvariant);
        private static readonly Regex OTOOL_LIST_REGEX = new Regex(@"\s+(?<library>.*\.dylib)", RegexOptions.Multiline | RegexOptions.CultureInvariant);

        /// <summary>
        ///   Gets the name of the install of the library.
        /// </summary>
        /// <param name = "nativeBinary">The native binary.</param>
        /// <returns>The install name.</returns>
        public static String GetInstallName(String nativeBinary)
        {
            String arguments = String.Format(CultureInfo.InvariantCulture, "-D \"{0}\"", nativeBinary);
            ProcessHelper helper = new ProcessHelper(Executable, arguments);
            String output = helper.Execute();

            String installName = String.Empty;
            Match match = OTOOL_ID_REGEX.Match(output);
            if (match != null && match.Success)
            {
                installName = match.Result("${library}");
            }
            return installName;
        }

        /// <summary>
        ///   Gets the dependencies of the native binary.
        /// </summary>
        /// <param name = "nativeBinary">The native binary.</param>
        /// <returns>The dependencies list</returns>
        public static IEnumerable<string> GetDependencies(String nativeBinary)
        {
            String arguments = String.Format(CultureInfo.InvariantCulture, "-L \"{0}\"", nativeBinary);
            ProcessHelper helper = new ProcessHelper(Executable, arguments);
            String output = helper.Execute();

            List<String> dependencies = new List<String>();
            MatchCollection matches = OTOOL_LIST_REGEX.Matches(output);
            if (matches.Count > 0)
            {
                dependencies.AddRange(from Match match in matches select match.Result("${library}"));
            }
            return dependencies;
        }

        private static string Executable
        {
            get { return "/usr/bin/otool"; }
        }
    }
}