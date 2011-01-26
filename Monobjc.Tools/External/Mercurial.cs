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
using System.Text.RegularExpressions;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.External
{
    /// <summary>
    ///   Wrapper class around the <c>hg</c> command line tool.
    /// </summary>
    public static class Mercurial
    {
        private static readonly Regex REGEX = new Regex(@"parent: (?<revision>[0-9]+)", RegexOptions.Multiline | RegexOptions.CultureInvariant);

        /// <summary>
        ///   Get the revision the specified dir under Mercurial version control..
        /// </summary>
        /// <param name = "dir">The dir.</param>
        /// <returns>The revision</returns>
        public static String Revision(String dir)
        {
            ProcessHelper helper = new ProcessHelper("hg", "summary -R " + dir);
            String output = helper.Execute();

            // Apply regular expression
            Match match = REGEX.Match(output);
            if (match != null && match.Success)
            {
                return match.Result("${revision}");
            }

            return null;
        }
    }
}