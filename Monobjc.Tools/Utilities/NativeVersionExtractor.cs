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
using System.Runtime.InteropServices;
using Monobjc.Tools.ObjectiveC;

namespace Monobjc.Tools.Utilities
{
    public static class NativeVersionExtractor
    {
        /// <summary>
        /// Gets the version of the native application.
        /// </summary>
        /// <param name="applicationPath">The application path with or without the ".app" extension.</param>
        /// <returns></returns>
        public static Version GetVersion(String applicationPath)
        {
            String path = applicationPath;
            if (Path.GetExtension(applicationPath) != ".app")
            {
                path += ".app";
            }
            path = Path.Combine(path, "Contents");
            path = Path.Combine(path, "Info.plist");

            // Create an pool
            ObjCClass nsautoreleasepoolClass = ObjCClass.Get("NSAutoreleasePool");
            ObjCClass nsstringClass = ObjCClass.Get("NSString");
            ObjCClass nsdictionaryClass = ObjCClass.Get("NSDictionary");

            ObjCId pool = nsautoreleasepoolClass.Alloc();
            pool = pool.Init();

            // Read the Info.plist file
            ObjCId nsstringPath = new ObjCId(nsstringClass.SendMessage("stringWithUTF8String:", path));
            ObjCId nsstringKey = new ObjCId(nsstringClass.SendMessage("stringWithUTF8String:", "CFBundleShortVersionString"));
            ObjCId nsdictionaryDict = new ObjCId(nsdictionaryClass.SendMessage("dictionaryWithContentsOfFile:", nsstringPath.Pointer));
            ObjCId nsstringVersion = new ObjCId(nsdictionaryDict.SendMessage("valueForKey:", nsstringKey.Pointer));

            // Return the string version
            IntPtr result = nsstringVersion.SendMessage("UTF8String");
            String version = Marshal.PtrToStringAuto(result);

            // Release the pool
            pool.Release();

            return new Version(version);
        }
    }
}