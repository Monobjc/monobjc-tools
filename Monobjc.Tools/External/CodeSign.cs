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
using System.Text;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.External
{
    /// <summary>
    ///   Wrapper class around the <c>codesign</c> command line tool.
    /// </summary>
    public static class CodeSign
    {
        /// <summary>
        ///   Sign the target with the following identity.
        /// </summary>
        /// <param name = "target">The path to the target.</param>
        /// <param name = "identity">The signing identity.</param>
        /// <param name = "identity">The entitlements.</param>
        /// <returns>The result of the command.</returns>
        public static void PerformSigning(String target, String identity, String entitlements, TextWriter outputWriter = null, TextWriter errorWriter = null)
        {
            StringBuilder arguments = new StringBuilder(" --verbose --force ");
            if (identity != null)
            {
                arguments.AppendFormat(" --sign \"{0}\" ", identity);
            }
            if (entitlements != null)
            {
                arguments.AppendFormat(" --entitlements \"{0}\" ", entitlements);
            }
            arguments.AppendFormat(" \"{0}\" ", target);

            ProcessHelper helper = new ProcessHelper(Executable, arguments.ToString());
			helper.OutputWriter = outputWriter;
			helper.ErrorWriter = errorWriter;
			helper.Execute ();
        }

        /// <summary>
        ///   Sign the target with the following identity.
        /// </summary>
        /// <param name = "bundle">The path to the application bundle.</param>
        /// <param name = "identity">The signing identity.</param>
        /// <returns>The result of the command.</returns>
        public static void PerformSigning(String bundle, String identity, TextWriter outputWriter = null, TextWriter errorWriter = null)
        {
			PerformSigning(bundle, identity, null, outputWriter, errorWriter);
        }

		/// <summary>
		/// Gets or sets the executable.
		/// </summary>
		/// <value>The executable.</value>
        private static string Executable
        {
            get { return "/usr/bin/codesign"; }
        }
    }
}