//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2013 - Laurent Etiemble
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
using Monobjc.Tools.Utilities;
using System.Security.Cryptography;
using System.Text;

namespace Monobjc.Tools.Generators
{
	/// <summary>
	///   Wrapper to handle artwork combination.
	/// </summary>
	public partial class ArtworkEncrypter
	{
		private static byte[] MAGIC_NUMBER = new byte[]{ 0x50 , 0x4B, 0x03, 0x04 };

		/// <summary>
		///   Initializes a new instance of the <see cref = "ArtworkEncrypter" /> class.
		/// </summary>
		public ArtworkEncrypter ()
		{
			this.Logger = new NullLogger ();
			this.Extensions = "png,tiff,jpg,jpeg";
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
		public bool Encrypt (String directory, String encryptionSeed)
		{
			// TODO: I18N
			this.Logger.LogDebug (String.Format (CultureInfo.CurrentCulture, "Encrypting artwork in {0}", directory));

			try {
				AesCryptoServiceProvider provider = new AesCryptoServiceProvider ();
				provider.Key = DeriveKey (encryptionSeed);

				// Perform the encryption
				foreach (String extension in this.Extensions.Split(',')) {
					String dotExtension = "." + extension;
					IList<String> files = new List<String> (this.Enumerate (directory, dotExtension));
					
					foreach (var file in files) {
						Encrypt (file, file, provider);
					}
				}

				return true;
			} catch (Exception ex) {
				this.Logger.LogError (ex.Message);
				return false;
			}
		}

		private IEnumerable<String> Enumerate (String directory, String suffix)
		{
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

		private void Encrypt (String inputFile, String outputFile, Aes aes)
		{
			if (IsEncrypted (inputFile)) {
				return;
			}
			
			this.Logger.LogDebug (String.Format (CultureInfo.CurrentCulture, "Encrypting {0}...", inputFile));
			
			byte[] data = File.ReadAllBytes(inputFile);
			byte[] output = Encrypt(data, aes);
			File.WriteAllBytes (outputFile, output);
		}

		private void Decrypt (String inputFile, String outputFile, Aes aes)
		{
			if (!IsEncrypted (inputFile)) {
				return;
			}
			
			this.Logger.LogDebug (String.Format (CultureInfo.CurrentCulture, "Decrypting {0}...", inputFile));
			
			byte[] data = File.ReadAllBytes(inputFile);
			byte[] output = Decrypt(data, aes);
			File.WriteAllBytes (outputFile, output);
		}
	}
}
