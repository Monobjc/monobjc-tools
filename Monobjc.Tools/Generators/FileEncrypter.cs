//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2014 - Laurent Etiemble
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

namespace Monobjc.Tools.Generators
{
	/// <summary>
	///   Wrapper to handle file encryption.
	/// </summary>
	public partial class FileEncrypter
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "ArtworkEncrypter" /> class.
		/// </summary>
		public FileEncrypter ()
		{
			this.Logger = new NullLogger ();
		}

		/// <summary>
		///   Gets or sets the logger.
		/// </summary>
		/// <value>The logger.</value>
		public IExecutionLogger Logger { get; set; }

		/// <summary>
		/// Encrypt the specified files into the directory with the given encryptionSeed.
		/// </summary>
		public bool Encrypt (IEnumerable<String> files, String directory, String encryptionSeed)
		{
			try {
				Aes provider = new AesCryptoServiceProvider ();
				provider.Key = DeriveKey (encryptionSeed);
				
				return Encrypt (files, directory, provider);
			} catch (Exception ex) {
				this.Logger.LogError (ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Encrypt the specified files into the directory with the given provider.
		/// </summary>
		public bool Encrypt (IEnumerable<String> files, String directory, Aes provider)
		{
			try {
				foreach (String file in files) {
					String destination = Path.Combine (directory, Path.GetFileName (file));
					Encrypt (file, destination, provider);
				}
				return true;
			} catch (Exception ex) {
				this.Logger.LogError (ex.Message);
				return false;
			}
		}

		public void Encrypt (String inputFile, String outputFile, Aes aes)
		{
			if (IsEncrypted (inputFile)) {
				return;
			}
			
			this.Logger.LogDebug (String.Format (CultureInfo.CurrentCulture, "Encrypting {0}...", inputFile));
			
			byte[] data = File.ReadAllBytes (inputFile);
			byte[] output = Encrypt (data, aes);
			File.WriteAllBytes (outputFile, output);
		}

		public void Decrypt (String inputFile, String outputFile, Aes aes)
		{
			if (!IsEncrypted (inputFile)) {
				return;
			}
			
			this.Logger.LogDebug (String.Format (CultureInfo.CurrentCulture, "Decrypting {0}...", inputFile));
			
			byte[] data = File.ReadAllBytes (inputFile);
			byte[] output = Decrypt (data, aes);
			File.WriteAllBytes (outputFile, output);
		}
	}
}
