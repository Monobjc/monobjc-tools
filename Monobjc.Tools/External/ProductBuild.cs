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
using System.IO;
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
            String arguments;
            if (productDefinition != null)
            {
                arguments = String.Format(CultureInfo.InvariantCulture, "--component \"{0}\" /Applications --sign \"{1}\" --product \"{2}\" \"{3}\"", bundle, identity, productDefinition, package);
            }
            else
            {
                arguments = String.Format(CultureInfo.InvariantCulture, "--component \"{0}\" /Applications --sign \"{1}\" \"{2}\"", bundle, identity, package);
            }
            ProcessHelper helper = new ProcessHelper(Executable, arguments);
            String output = helper.Execute();
            return output;
        }

        private static string Executable
        {
            get { return "/usr/bin/productbuild"; }
        }
    }
}