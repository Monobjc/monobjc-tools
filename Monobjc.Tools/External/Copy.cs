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
    ///   Wrapper class around the <c>cp</c> command line tool.
    /// </summary>
    public static class Copy
    {
        /// <summary>
        ///   Copy recursively the source into the destination
        /// </summary>
        /// <param name = "source">The source file or folder.</param>
        /// <param name = "destination">The destination file or folder.</param>
        /// <returns>The result of the command.</returns>
        public static String Recursivly(String source, String destination)
        {
            ProcessHelper helper = new ProcessHelper("cp", string.Format(CultureInfo.InvariantCulture, "-R {0} {1}", source, destination));
            String output = helper.Execute();
            return output;
        }
    }
}