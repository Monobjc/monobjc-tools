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
using System.Text;
using Monobjc.Tools.Utilities;
using Monobjc.Tools.PropertyList;
using System.IO;

namespace Monobjc.Tools.External
{
    /// <summary>
    ///   Wrapper class around the <c>codesign</c> command line tool.
    /// </summary>
    public static class Receigen
    {
		private static String executable = "/Applications/Receigen.app/Contents/MacOS/Receigen";
		
		/// <summary>
		/// Generate the specified infoPlist and outputFolder.
		/// </summary>
		/// <param name='infoPlist'>
		/// Info plist.
		/// </param>
		/// <param name='outputFolder'>
		/// Output folder.
		/// </param>
        public static String Generate(String infoPlist, String outputFolder)
        {
			// Load the Info.plist file
			PListDocument document = PListDocument.LoadFromFile(infoPlist);
			if (document == null) {
				return String.Empty;
			}
			if (document.Root == null) {
				return String.Empty;
			}
			if (document.Root.Dict == null) {
				return String.Empty;
			}
			
			// Extract bundle identifier
			PListString identifier = document.Root.Dict["CFBundleIdentifier"] as PListString;
			if (identifier == null || String.IsNullOrWhiteSpace(identifier.Value)) {
				return String.Empty;
			}
			
			// Extract bundle version
			PListString version = document.Root.Dict["CFBundleShortVersionString"] as PListString;
			if (version == null || String.IsNullOrWhiteSpace(version.Value)) {
				return String.Empty;
			}
			
			// Launch the generation
			String file = Path.Combine(outputFolder, "receigen.h");
            StringBuilder arguments = new StringBuilder();
			arguments.AppendFormat(" --identifier {0} ", identifier.Value);
			arguments.AppendFormat(" --version {0} ", version.Value);
			arguments.AppendFormat(" --output \"{0}\" ", file);

            ProcessHelper helper = new ProcessHelper(Executable, arguments.ToString());
            String output = helper.Execute();
            return output;
        }
		
		/// <summary>
		/// Gets or sets the executable.
		/// </summary>
		/// <value>
		/// The executable.
		/// </value>
        public static string Executable
        {
            get { return executable; }
			set { executable = value; }
        }
    }
}