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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Monobjc.Tools.External;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Generators
{
	/// <summary>
	///   Wrapper to handle artwork combination.
	/// </summary>
	public class ArtworkCombiner
	{
		private const String HIPDI_PATTERN = "@2x";
		private const String TIFF_EXTENSION = ".tiff";

		/// <summary>
		///   Initializes a new instance of the <see cref = "ArtworkCombiner" /> class.
		/// </summary>
		public ArtworkCombiner ()
		{
			this.Logger = new NullLogger ();
			this.Extensions = "png,tiff";
		}

		/// <summary>
		///   Gets or sets the logger.
		/// </summary>
		/// <value>The logger.</value>
		public IExecutionLogger Logger { get; set; }

		/// <summary>
		/// Gets or sets the file extensions (comma separated).
		/// </summary>
		/// <value>The file extensions.</value>
		public String Extensions { get; set; }

		/// <summary>
		/// Combine the specified low/high resolution images in the given directory.
		/// </summary>
		public bool Combine (String directory, TextWriter outputWriter, TextWriter errorWriter)
		{
			// TODO: I18N
			this.Logger.LogDebug (String.Format (CultureInfo.CurrentCulture, "Combining artwork in {0}", directory));

			// Perform the combination
			IDictionary<String, String[]> combinations = GetCombinations(directory);
			foreach (var combination in combinations) {
				TiffUtil.Cat(combination.Key, combination.Value, outputWriter, errorWriter);
			}

			return true;
		}

		private IDictionary<String, String[]> GetCombinations(String directory)
		{
			// Get all the files
			IDictionary<String, String[]> combinations = new Dictionary<String, String[]> ();
			foreach (String extension in this.Extensions.Split(',')) {
				String dotExtension = "." + extension;
				IList<String> files = new List<String> (this.Enumerate (directory, dotExtension));
				
				foreach (var file in files) {
					String result = Path.ChangeExtension(file, TIFF_EXTENSION);
					String highRes = file.Replace(dotExtension, HIPDI_PATTERN + dotExtension);
					if (files.Contains (highRes)) {
						this.Logger.LogDebug(String.Format(CultureInfo.CurrentCulture, "Found candidate {0}", file));
						combinations.Add (result, new String[]{file, highRes});
					}
				}
			}
			return combinations;
		}

		private IEnumerable<String> Enumerate (String directory, String suffix)
		{
			this.Logger.LogDebug(String.Format(CultureInfo.CurrentCulture, "Scanning {0}...", directory));
			List<String> result = new List<String> ();
			IEnumerable<FileSystemInfo> infos = new DirectoryInfo (directory).EnumerateFileSystemInfos ("*");
			foreach (var info in infos) {
				if (Directory.Exists (info.FullName)) {
					result.AddRange (Enumerate (info.FullName, suffix));
				}
				if (info is FileInfo && info.FullName.EndsWith (suffix, StringComparison.InvariantCultureIgnoreCase)) {
					result.Add (info.FullName);
				}
			}
			return result;
		}
	}
}