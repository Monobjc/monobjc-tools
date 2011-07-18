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
using System.IO;
using Monobjc.Tools.External;

namespace Monobjc.Tools.Utilities
{
    public static class FileProvider
    {
        public static String GetPath(MacOSVersion version, String name)
        {
            // Set the base path
            String basedir = "/Library/Frameworks/Mono.framework/Versions/Current/lib/mono";
            String dir = null;

            switch (version)
            {
                case MacOSVersion.MacOS105:
                    dir = Path.Combine(basedir, "monobjc-10.5");
                    break;
                case MacOSVersion.MacOS106:
                    dir = Path.Combine(basedir, "monobjc-10.6");
                    break;
                case MacOSVersion.MacOS107:
                    dir = Path.Combine(basedir, "monobjc-10.7");
                    break;
                default:
                    throw new NotSupportedException("Unsupported version of Mac OS X");
            }

            return Path.Combine(dir, name);
        }

        public static void CopyFile(MacOSVersion version, String name, String destination)
        {
            CopyFile(version, name, destination, null);
        }

        public static void CopyFile(MacOSVersion version, String name, String destination, String permissions)
        {
            // Copy the file
            String file = GetPath(version, name);
            File.Copy(file, destination, true);

            if (!String.IsNullOrEmpty(permissions))
            {
                // Apply permissions
                Chmod.ApplyTo("a+x", destination);
            }
        }
    }
}