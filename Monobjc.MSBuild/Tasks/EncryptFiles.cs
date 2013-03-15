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
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Monobjc.MSBuild.Utilities;
using Monobjc.Tools.Generators;
using System.Security.Cryptography;

namespace Monobjc.MSBuild.Tasks
{
	public class EncryptFiles : Task
	{
		/// <summary>
		/// Gets or sets the source files.
		/// </summary>
		[Required]
		public ITaskItem[] SourceFiles { get; set; }

		/// <summary>
		/// Gets or sets the destination files.
		/// </summary>
		public ITaskItem[] DestinationFiles { get; set; }

		/// <summary>
		/// Gets or sets the destination folder.
		/// </summary>
		public ITaskItem DestinationFolder { get; set; }

		/// <summary>
		/// Gets or sets the encryption seed.
		/// </summary>
		/// <value>The seed from which to derive the key.</value>
		[Required]
		public String EncryptionSeed { get; set; }

		public override bool Execute ()
		{
			if (this.SourceFiles.Length == 0) {
				return true;
			}

			if (this.SourceFiles != null && this.DestinationFiles != null && this.SourceFiles.Length != this.DestinationFiles.Length) {
				this.Log.LogError ("Number of source files is different than number of destination files.");
				return false;
			}

			if (this.DestinationFiles != null && this.DestinationFolder != null) {
				this.Log.LogError ("You must specify only one attribute from DestinationFiles and DestinationFolder");
				return false;
			}

			FileEncrypter encrypter = new FileEncrypter ();
			encrypter.Logger = new ExecutionLogger (this);

			Aes provider = new AesCryptoServiceProvider ();
			provider.Key = FileEncrypter.DeriveKey (this.EncryptionSeed);

			if (this.DestinationFiles != null && this.DestinationFiles.Length > 0) {
				for (int i = 0; i < this.SourceFiles.Length; i++) {
					ITaskItem sourceItem = this.SourceFiles [i];
					ITaskItem destinationItem = this.DestinationFiles [i];
					String sourcePath = sourceItem.GetMetadata ("FullPath");
					String destinationPath = destinationItem.GetMetadata ("FullPath");
					if (!File.Exists (sourcePath)) {
						this.Log.LogError ("Cannot encrypt {0} to {1}, as the source file doesn't exist.", new object[] {
							sourcePath,
							destinationPath
						});
					} else {
						encrypter.Encrypt (sourcePath, destinationPath, provider);
					}
				}
				return true;
			} 

			if (this.DestinationFolder == null) {
				this.Log.LogError ("You must specify DestinationFolder or DestinationFiles attribute.");
				return false;
			}

			String destinationFolder = this.DestinationFolder.GetMetadata ("FullPath");
			for (int i = 0; i < this.SourceFiles.Length; i++) {
				ITaskItem sourceItem = this.SourceFiles [i];
				String sourcePath = sourceItem.GetMetadata ("FullPath");
				String path = sourceItem.GetMetadata ("Filename") + sourceItem.GetMetadata ("Extension");
				String destinationPath = Path.Combine (destinationFolder, path);
				if (!File.Exists (sourcePath)) {
					this.Log.LogError ("Cannot encrypt {0} to {1}, as the source file doesn't exist.", new object[] {
						sourcePath,
						destinationPath
					});
				} else {
					encrypter.Encrypt (sourcePath, destinationPath, provider);
				}
				return true;
			}

			return true;
		}
	}
}
