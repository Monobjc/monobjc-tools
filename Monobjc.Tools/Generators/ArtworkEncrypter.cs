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
	public class ArtworkEncrypter
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
				IDictionary<String, String[]> combinations = new Dictionary<String, String[]> ();
				foreach (String extension in this.Extensions.Split(',')) {
					String dotExtension = "." + extension;
					IList<String> files = new List<String> (this.Enumerate (directory, dotExtension));
					
					foreach (var file in files) {
						Encrypt (file, provider);
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

		private byte[] DeriveKey (String encryptionSeed)
		{
			SHA256 sha = SHA256Managed.Create ();
			byte[] result = sha.ComputeHash (Encoding.UTF8.GetBytes (encryptionSeed));
			return result;
		}

		private void Encrypt (String file, Aes aes)
		{
			if (IsEncrypted (file)) {
				return;
			}

			this.Logger.LogDebug (String.Format (CultureInfo.CurrentCulture, "Encrypting {0}...", file));

			using (ICryptoTransform transform = aes.CreateEncryptor ()) {
				using (MemoryStream memoryStream = new MemoryStream()) {
					// Write the magic number
					memoryStream.Write (MAGIC_NUMBER, 0, 4);

					// Write the IV
					memoryStream.Write (aes.IV, 0, 16);

					// Now, write the encrypted file
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write)) {
						// Copy the file into the crypto stream
						using (Stream sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read)) {
							sourceStream.CopyTo (cryptoStream);
							sourceStream.Flush ();
							sourceStream.Close ();
						}
					}

					memoryStream.Flush ();
					memoryStream.Close ();
					byte[] bytes = memoryStream.ToArray ();

					// Write the encrypted content back to file
					File.WriteAllBytes (file, bytes);
				}
			}
		}

		private bool IsEncrypted (String file)
		{
			int read;
			byte[] header = new byte[4];
			using (Stream sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read)) {
				read = sourceStream.Read (header, 0, 4);
				sourceStream.Close ();
			}
			// Something went wrong, so consider the file is encrypted
			if (read < 4) {
				return true;
			}
			bool isEncrypted = true;
			isEncrypted &= (header [0] == MAGIC_NUMBER [0]);
			isEncrypted &= (header [1] == MAGIC_NUMBER [1]);
			isEncrypted &= (header [2] == MAGIC_NUMBER [2]);
			isEncrypted &= (header [3] == MAGIC_NUMBER [3]);
			return isEncrypted;
		}
	}
}
