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
using System.Runtime.InteropServices;

namespace Monobjc.Tools
{
    /// <summary>
    ///   <para>Exports native methods exposed in <c>libobjc.dylib</c> shared library.</para>
    ///   <para>Thanks to .NET P/Invoke system, most of the marshalling work is automatic.</para>
    ///   <para>The following methods are safe for use on both Mac OS X 10.4 and later.</para>
    /// </summary>
    internal static class NativeMethods
    {
        private const string SecurityFramework = "/System/Library/Frameworks/Security.framework/Security";
        private const string FoundationFramework = "/System/Library/Frameworks/Foundation.framework/Foundation";

        [DllImport(FoundationFramework, EntryPoint = "objc_getClass", CharSet = CharSet.Auto)]
        internal static extern IntPtr objc_getClass([MarshalAs(UnmanagedType.LPStr)] String str);

        [DllImport(FoundationFramework, EntryPoint = "sel_registerName", CharSet = CharSet.Ansi)]
        internal static extern IntPtr register_selector([MarshalAs(UnmanagedType.LPStr)] String str);

        [DllImport(FoundationFramework, EntryPoint = "objc_msgSend")]
        internal static extern IntPtr objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector);

        [DllImport(FoundationFramework, EntryPoint = "objc_msgSend")]
        internal static extern void objc_msgSend_Void(IntPtr receiver, IntPtr selector);

        [DllImport(FoundationFramework, EntryPoint = "objc_msgSend")]
        internal static extern IntPtr objc_msgSend_IntPtr_IntPtr(IntPtr receiver, IntPtr selector, IntPtr parameter1);

        [DllImport(FoundationFramework, EntryPoint = "objc_msgSend")]
        internal static extern IntPtr objc_msgSend_IntPtr_String(IntPtr receiver, IntPtr selector, String parameter1);

        [DllImport(FoundationFramework, EntryPoint = "CFRelease")]
        internal static extern void CFRelease(IntPtr cfRef);

        [DllImport(SecurityFramework, EntryPoint = "SecIdentitySearchCreate")]
        internal static extern OSStatus SecIdentitySearchCreate(IntPtr keychainOrArray, CSSM_KEYUSE keyUsage, out IntPtr searchRef);

        [DllImport(SecurityFramework, EntryPoint = "SecIdentitySearchCopyNext")]
        internal static extern OSStatus SecIdentitySearchCopyNext(IntPtr searchRef, out IntPtr identity);

        [DllImport(SecurityFramework, EntryPoint = "SecIdentityCopyCertificate")]
        internal static extern OSStatus SecIdentityCopyCertificate(IntPtr identityRef, out IntPtr certificateRef);

        [DllImport(SecurityFramework, EntryPoint = "SecCertificateCopyCommonName")]
        internal static extern OSStatus SecCertificateCopyCommonName(IntPtr certificate, out IntPtr commonName);
    }
}