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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Monobjc.Tools.ObjectiveC;

namespace Monobjc.Tools.Utilities
{
    public static class KeyChainAccess
    {
        /// <summary>
        ///   Returns a list of the certificate that can be used for entity signing.
        /// </summary>
        public static IList<String> SigningIdentities
        {
            get
            {
                IList<string> result = new List<String>();
                IntPtr search;

                // Create the keychain search
                OSStatus status = NativeMethods.SecIdentitySearchCreate(IntPtr.Zero, CSSM_KEYUSE.CSSM_KEYUSE_SIGN, out search);
                if (status != OSStatus.errSecSuccess)
                {
                    return result;
                }

                // Create an pool
                ObjCClass cls = ObjCClass.Get("NSAutoreleasePool");
                ObjCId pool = cls.Alloc();
                pool = pool.Init();

                while (true)
                {
                    IntPtr identity;
                    IntPtr certificate;
                    IntPtr commonName;

                    // Extract next search result
                    status = NativeMethods.SecIdentitySearchCopyNext(search, out identity);
                    if (status != OSStatus.errSecSuccess)
                    {
                        break;
                    }

                    // Copy the data as a certificate
                    status = NativeMethods.SecIdentityCopyCertificate(identity, out certificate);
                    NativeMethods.CFRelease(identity);
                    if (status != OSStatus.errSecSuccess)
                    {
                        break;
                    }

                    // Extract the common name
                    status = NativeMethods.SecCertificateCopyCommonName(certificate, out commonName);
                    NativeMethods.CFRelease(certificate);
                    if (status != OSStatus.errSecSuccess)
                    {
                        break;
                    }

                    // Convert CFStringRef into a String object
                    // As CFStringRef and NSString are toll-free bridged, so use NSString method
                    IntPtr utf8String = new ObjCId(commonName).SendMessage("UTF8String");
                    String name = Marshal.PtrToStringAuto(utf8String);
                    NativeMethods.CFRelease(commonName);

                    result.Add(name);
                }

                // Release the pool
                pool.Release();

                return result;
            }
        }
    }
}