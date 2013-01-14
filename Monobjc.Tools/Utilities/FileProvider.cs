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
using Monobjc.Tools.External;
using System.Collections.Generic;

namespace Monobjc.Tools.Utilities
{
	public static class FileProvider
	{
		public static String GetPath (MacOSVersion version)
		{
			// Set the base path
			String basedir = "/Library/Frameworks/Mono.framework/Libraries/mono";

			IDictionary<MacOSVersion, String> map = new Dictionary<MacOSVersion, String> () {
				{ MacOSVersion.MacOS105, "monobjc-10.5" },
				{ MacOSVersion.MacOS106, "monobjc-10.6" },
				{ MacOSVersion.MacOS107, "monobjc-10.7" },
				{ MacOSVersion.MacOS108, "monobjc-10.8" },
			};

			String result = null;
			foreach(var slot in map) {
				String dir = Path.Combine (basedir, slot.Value);
				if (!Directory.Exists (dir)) {
					continue;
				}
				result = dir;
				if (version <= slot.Key) {
					break;
				}
			}

			if (!String.IsNullOrEmpty(result)) {
				return result;
			}

			// TODO: I18N
			throw new NotSupportedException ("Unsupported version of Mac OS X");
		}

		public static String GetPath (MacOSVersion version, String name)
		{
			return Path.Combine (GetPath (version), name);
		}

		public static void CopyFile (MacOSVersion version, String name, String destination)
		{
			CopyFile (version, name, destination, null);
		}

		public static void CopyFile (MacOSVersion version, String name, String destination, String permissions)
		{
			// Copy the file
			String file = GetPath (version, name);
			File.Copy (file, destination, true);

			if (!String.IsNullOrEmpty (permissions)) {
				// Apply permissions
				Chmod.ApplyTo ("a+x", destination);
			}
		}
	}
}