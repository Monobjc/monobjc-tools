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
using System.IO;
using System.Text;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.External
{
    /// <summary>
    ///   Wrapper class around the <c>productbuild</c> command line tool.
    /// </summary>
    public static class ProductBuild
    {
        /// <summary>
        ///   Archive the application bundle and sign the result with the following identity.
        /// </summary>
        /// <param name = "bundle">The path to the application bundle.</param>
        /// <param name = "identity">The signing identity.</param>
        /// <param name = "productDefinition">The product definition.</param>
        /// <returns>The result of the command.</returns>
        public static String ArchiveApplication(String bundle, String identity, String productDefinition)
        {
            String package = Path.ChangeExtension(bundle, ".pkg");

            StringBuilder arguments = new StringBuilder();
            arguments.AppendFormat(" --component \"{0}\" /Applications ", bundle);
            if (identity != null)
            {
                arguments.AppendFormat(" --sign \"{0}\" ", identity);
            }
            if (productDefinition != null)
            {
                arguments.AppendFormat(" --product \"{0}\" ", productDefinition);
            }
            arguments.AppendFormat(" \"{0}\" ", package);

            ProcessHelper helper = new ProcessHelper(Executable, arguments.ToString());
            String output = helper.Execute();
            return output;
        }

        private static string Executable
        {
            get { return "/usr/bin/productbuild"; }
        }
    }
}