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
using System.Text;
using Monobjc.Tools.Utilities;
using Monobjc.Tools.PropertyList;
using System.IO;

namespace Monobjc.Tools.External
{
	/// <summary>
	///   Wrapper class around the <c>codesign</c> command line tool.
	/// </summary>
	public class Receigen
	{
		private String executable = "/Applications/Receigen.app/Contents/MacOS/Receigen";
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Monobjc.Tools.External.Receigen"/> class.
		/// </summary>
		public Receigen ()
		{
			this.Logger = new NullLogger ();
		}
		
		/// <summary>
		///   Gets or sets the logger.
		/// </summary>
		/// <value>The logger.</value>
		public IExecutionLogger Logger { get; set; }
		
		/// <summary>
		/// Generate the specified infoPlist and outputFolder.
		/// </summary>
		/// <param name='infoPlist'>
		/// Info plist.
		/// </param>
		/// <param name='outputFolder'>
		/// Output folder.
		/// </param>
		public String Generate (String infoPlist, String outputFolder)
		{
			// Load the Info.plist file
			PListDocument document = PListDocument.LoadFromFile (infoPlist);
			if (document == null) {
				this.Logger.LogError ("Cannot parse document: " + infoPlist);
				return String.Empty;
			}
			if (document.Root == null) {
				this.Logger.LogError ("Document has no root: " + infoPlist);
				return String.Empty;
			}
			if (document.Root.Dict == null) {
				this.Logger.LogError ("Document has no dict: " + infoPlist);
				return String.Empty;
			}
			
			// Extract bundle identifier
			PListString identifier = document.Root.Dict ["CFBundleIdentifier"] as PListString;
			if (identifier == null || String.IsNullOrWhiteSpace (identifier.Value)) {
				this.Logger.LogError ("Document has no 'CFBundleIdentifier': " + infoPlist);
				return String.Empty;
			}
			
			// Extract bundle version
			PListString version = document.Root.Dict ["CFBundleShortVersionString"] as PListString;
			if (version == null || String.IsNullOrWhiteSpace (version.Value)) {
				this.Logger.LogError ("Document has no 'CFBundleShortVersionString': " + infoPlist);
				return String.Empty;
			}
			
			// Launch the generation
			String file = Path.Combine (outputFolder, "receigen.h");
			StringBuilder arguments = new StringBuilder ();
			arguments.AppendFormat (" --identifier {0} ", identifier.Value);
			arguments.AppendFormat (" --version {0} ", version.Value);
			arguments.AppendFormat (" --output \"{0}\" ", file);
			
			this.Logger.LogDebug ("Calling '" + this.Executable + "' with '" + arguments + "'");
			
			ProcessHelper helper = new ProcessHelper (this.Executable, arguments.ToString ());
			String output = helper.Execute ();
			return output;
		}
		
		/// <summary>
		/// Gets or sets the executable.
		/// </summary>
		/// <value>The executable.</value>
		public string Executable {
			get { return this.executable; }
			set { this.executable = value; }
		}
	}
}