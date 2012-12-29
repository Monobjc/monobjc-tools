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
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.External
{
	/// <summary>
	///   Wrapper class around the <c>tiffutil</c> command line tool.
	/// </summary>
	public static class TiffUtil
	{
		/// <summary>
		///   Cat multiple images
		/// </summary>
		/// <param name = "mask">The permission mask.</param>
		/// <param name = "file">The file.</param>
		/// <returns>The result of the command.</returns>
		public static void Cat (String outputFile, String[] inputFiles, TextWriter outputWriter = null, TextWriter errorWriter = null)
		{
			if (inputFiles.Length < 2) {
				// TODO: I18N
				throw new ArgumentException("At least 2 image files must be provided", "inputFiles");
			}

			// Check if combination is needed
			bool upToDate = true;
			foreach(String inputFile in inputFiles) {
				upToDate &= FileExtensions.UpToDate(inputFile, outputFile);
			}
			if (upToDate) {
				return;
			}

			// Perform the combination
			String arguments = String.Format (CultureInfo.InvariantCulture, "-cathidpicheck \"{0}\" -out \"{1}\"", String.Join("\" \"", inputFiles), outputFile);
			ProcessHelper helper = new ProcessHelper (Executable, arguments);
			helper.OutputWriter = outputWriter;
			helper.ErrorWriter = errorWriter;
			int exitCode = helper.Execute ();

			if (exitCode == -1) {
				return;
			}

			// Remove sources
			foreach(String inputFile in inputFiles) {
				File.Delete(inputFile);
			}
		}

		private static string Executable {
			get { return "tiffutil"; }
		}
	}
}