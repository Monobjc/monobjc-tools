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
using System.Linq;
using Monobjc.Tools.PropertyList;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.External
{
    /// <summary>
    ///   Wrapper class around the <c>ibtool</c> command line tool.
    /// </summary>
    public static class XibTool
    {
        public const string DOCUMENT_NOTICES = "com.apple.ibtool.document.notices";
        public const string DOCUMENT_WARNINGS = "com.apple.ibtool.document.warnings";
        public const string DOCUMENT_ERRORS = "com.apple.ibtool.document.errors";
        public const string ERRORS = "com.apple.ibtool.errors";
        public const string KEY_DESCRIPTION = "description";
        public const string KEY_MESSAGE = "message";
        public const string KEY_TYPE = "type";

        /// <summary>
        ///   Compiles the specified xib file into a nib file.
        /// </summary>
        /// <param name = "xibFile">The xib file.</param>
        /// <returns>The <see cref = "PListDocument" /> that contains the result.</returns>
        public static PListDocument Compile(String xibFile)
        {
            String destination = Path.ChangeExtension(xibFile, ".nib");
            return CompileInternal(xibFile, destination);
        }

        /// <summary>
        ///   Compiles the specified xib file into a nib file.
        /// </summary>
        /// <param name = "xibFile">The xib file.</param>
        /// <param name = "destinationDir">The destination dir.</param>
        /// <returns>The <see cref = "PListDocument" /> that contains the result.</returns>
        public static PListDocument Compile(String xibFile, String destinationDir)
        {
            Directory.CreateDirectory(destinationDir);
            String destination = Path.Combine(destinationDir, Path.GetFileNameWithoutExtension(xibFile) + ".nib");
            return CompileInternal(xibFile, destination);
        }

        private static PListDocument CompileInternal(String xibFile, String destination)
        {
            if (FileExtensions.UpToDate(xibFile, destination))
            {
                return null;
            }
            String arguments = String.Format(CultureInfo.InvariantCulture, "--errors --warnings --notices --compile \"{0}\" \"{1}\"", destination, xibFile);
            ProcessHelper helper = new ProcessHelper(Executable, arguments);
            String output = helper.Execute();
            try
            {
				output = output.Trim();
                PListDocument document = PListDocument.LoadFromXml(output);
                return document;
            }
            catch (Exception)
            {
                Console.WriteLine("XibTool cannot parse output:");
                Console.WriteLine("-----");
                Console.WriteLine(output);
                Console.WriteLine("-----");
                return null;
            }
        }

		/// <summary>
		/// Gets or sets the executable.
		/// </summary>
		/// <value>The executable.</value>
        private static string Executable
        {
            get
            {
                String[] paths = {
                                     "/usr/bin/nibtool", // XCode 2.5
                                     "/usr/bin/ibtool", // XCode 3.0+
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