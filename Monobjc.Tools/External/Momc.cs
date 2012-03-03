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
using System.Globalization;
using System.IO;
using System.Linq;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.External
{
    /// <summary>
    ///   Wrapper class around the <c>momc</c> command line tool.
    /// </summary>
    public static class Momc
    {
        /// <summary>
        ///   Compiles the specified model into its compiled form.
        /// </summary>
        /// <param name = "modelDir">The model dir.</param>
        public static void Compile(String modelDir)
        {
            String destination = Path.Combine(Path.GetDirectoryName(modelDir), Path.ChangeExtension(modelDir, ".momc"));
            CompileInternal(modelDir, destination);
        }

        /// <summary>
        ///   Compiles the specified model into its compiled form.
        /// </summary>
        /// <param name = "modelDir">The model dir.</param>
        /// <param name = "destinationDir">The destination dir.</param>
        public static String Compile(String modelDir, String destinationDir)
        {
            String destination = Path.Combine(destinationDir, Path.ChangeExtension(modelDir, ".momc"));
            return CompileInternal(modelDir, destination);
        }

        private static String CompileInternal(String modelDir, String destinationDir)
        {
            if (FileExtensions.UpToDate(modelDir, destinationDir))
            {
                return null;
            }
            String arguments = String.Format(CultureInfo.InvariantCulture, "\"{0}\" \"{1}\"", modelDir, destinationDir);
            ProcessHelper helper = new ProcessHelper(Executable, arguments);
            String output = helper.Execute();
            return output;
        }

        private static string Executable
        {
            get
            {
                String[] paths = {
                                     "/Library/Application Support/Apple/Developer Tools/Plug-ins/XDCoreDataModel.xdplugin/Contents/Resources/momc", // XCode 2.4
                                     "/Developer/Library/Xcode/Plug-ins/XDCoreDataModel.xdplugin/Contents/Resources/momc", // XCode 3.0
                                     "/Developer/usr/bin/momc", // XCode 3.1+
                                 };
                foreach (String path in paths.Where(File.Exists))
                {
                    return path;
                }
                return String.Empty;
            }
        }
    }
}