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
using System.Security.Cryptography;
using Monobjc.Tools.Generators;
using NUnit.Framework;

namespace Monobjc.Tools
{
	[TestFixture]
	[Category("Images")]
	[Category("Encryption")]
	public class FileEncrypterTests
	{
		private static readonly String KEY = "123";

		[Test]
		public void TestImageEncryption ()
		{
			File.Copy ("Embedded/application-sidebar-list.nopng", "dummy.png", true);
			
			FileEncrypter encrypter = new FileEncrypter ();
			encrypter.Encrypt (new String[]{"dummy.png"}, ".", KEY);
		}

		[Test]
		public void TestImageDecryption ()
		{
			Aes provider = FileEncrypter.GetProvider (KEY);

			{
				byte[] clean = File.ReadAllBytes ("Embedded/application-sidebar-list.nopng");
				byte [] content = FileEncrypter.Encrypt (clean, provider);
				
				byte[] output = FileEncrypter.Decrypt (content, provider);
				File.WriteAllBytes ("dummy2.png", output);
			}
			{
				byte [] content = File.ReadAllBytes ("dummy.png");
				byte[] output = FileEncrypter.Decrypt (content, provider);
				File.WriteAllBytes ("dummy3.png", output);
			}
		}
	}
}
