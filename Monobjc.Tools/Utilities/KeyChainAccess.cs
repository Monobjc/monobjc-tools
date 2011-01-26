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

namespace Monobjc.Tools.Utilities
{
    public static class KeyChainAccess
    {
        private const string SecurityFramework = "/System/Library/Frameworks/Security.framework/Security";
        private const string FoundationFramework = "/System/Library/Frameworks/Foundation.framework/Foundation";

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
                OSStatus status = SecIdentitySearchCreate(IntPtr.Zero, CSSM_KEYUSE.CSSM_KEYUSE_SIGN, out search);
                if (status != OSStatus.errSecSuccess)
                {
                    return result;
                }

                // Create an pool
                IntPtr pool = alloc(objc_getClass("NSAutoreleasePool"), selector("alloc"));
                pool = init(pool, selector("init"));

                while (true)
                {
                    IntPtr identity;
                    IntPtr certificate;
                    IntPtr commonName;

                    // Extract next search result
                    status = SecIdentitySearchCopyNext(search, out identity);
                    if (status != OSStatus.errSecSuccess)
                    {
                        break;
                    }

                    // Copy the data as a certificate
                    status = SecIdentityCopyCertificate(identity, out certificate);
                    CFRelease(identity);
                    if (status != OSStatus.errSecSuccess)
                    {
                        break;
                    }

                    // Extract the common name
                    status = SecCertificateCopyCommonName(certificate, out commonName);
                    CFRelease(certificate);
                    if (status != OSStatus.errSecSuccess)
                    {
                        break;
                    }

                    // Convert CFStringRef into a String object
                    // As CFStringRef and NSString are toll-free bridged, so use NSString method
                    IntPtr utf8String = UTF8String(commonName, selector("UTF8String"));
                    String name = Marshal.PtrToStringAuto(utf8String);
                    CFRelease(commonName);

                    result.Add(name);
                }

                // Release the pool
                release(pool, selector("release"));

                return result;
            }
        }

        [DllImport(FoundationFramework, EntryPoint = "objc_getClass", CharSet = CharSet.Ansi)]
        private static extern IntPtr objc_getClass([MarshalAs(UnmanagedType.LPStr)] String str);

        [DllImport(FoundationFramework, EntryPoint = "sel_registerName", CharSet = CharSet.Ansi)]
        private static extern IntPtr selector([MarshalAs(UnmanagedType.LPStr)] String str);

        [DllImport(FoundationFramework, EntryPoint = "objc_msgSend")]
        private static extern IntPtr alloc(IntPtr receiver, IntPtr selector);

        [DllImport(FoundationFramework, EntryPoint = "objc_msgSend")]
        private static extern IntPtr init(IntPtr receiver, IntPtr selector);

        [DllImport(FoundationFramework, EntryPoint = "objc_msgSend")]
        private static extern void release(IntPtr receiver, IntPtr selector);

        [DllImport(FoundationFramework, EntryPoint = "objc_msgSend")]
        private static extern IntPtr UTF8String(IntPtr receiver, IntPtr selector);

        [DllImport(FoundationFramework, EntryPoint = "CFRelease")]
        private static extern void CFRelease(IntPtr cfRef);

        [DllImport(SecurityFramework, EntryPoint = "SecIdentitySearchCreate")]
        private static extern OSStatus SecIdentitySearchCreate(IntPtr keychainOrArray, CSSM_KEYUSE keyUsage, out IntPtr searchRef);

        [DllImport(SecurityFramework, EntryPoint = "SecIdentitySearchCopyNext")]
        private static extern OSStatus SecIdentitySearchCopyNext(IntPtr searchRef, out IntPtr identity);

        [DllImport(SecurityFramework, EntryPoint = "SecIdentityCopyCertificate")]
        private static extern OSStatus SecIdentityCopyCertificate(IntPtr identityRef, out IntPtr certificateRef);

        [DllImport(SecurityFramework, EntryPoint = "SecCertificateCopyCommonName")]
        private static extern OSStatus SecCertificateCopyCommonName(IntPtr certificate, out IntPtr commonName);

        private enum OSStatus
        {
            errSecSuccess = 0, /* No error. */
            errSecUnimplemented = -4, /* Function or operation not implemented. */
            errSecParam = -50, /* One or more parameters passed to a function were not valid. */
            errSecAllocate = -108, /* Failed to allocate memory. */
            errSecNotAvailable = -25291, /* No keychain is available. You may need to restart your computer. */
            errSecReadOnly = -25292, /* This keychain cannot be modified. */
            errSecAuthFailed = -25293, /* The user name or passphrase you entered is not correct. */
            errSecNoSuchKeychain = -25294, /* The specified keychain could not be found. */
            errSecInvalidKeychain = -25295, /* The specified keychain is not a valid keychain file. */
            errSecDuplicateKeychain = -25296, /* A keychain with the same name already exists. */
            errSecDuplicateCallback = -25297, /* The specified callback function is already installed. */
            errSecInvalidCallback = -25298, /* The specified callback function is not valid. */
            errSecDuplicateItem = -25299, /* The specified item already exists in the keychain. */
            errSecItemNotFound = -25300, /* The specified item could not be found in the keychain. */
            errSecBufferTooSmall = -25301, /* There is not enough memory available to use the specified item. */
            errSecDataTooLarge = -25302, /* This item contains information which is too large or in a format that cannot be displayed. */
            errSecNoSuchAttr = -25303, /* The specified attribute does not exist. */
            errSecInvalidItemRef = -25304, /* The specified item is no longer valid. It may have been deleted from the keychain. */
            errSecInvalidSearchRef = -25305, /* Unable to search the current keychain. */
            errSecNoSuchClass = -25306, /* The specified item does not appear to be a valid keychain item. */
            errSecNoDefaultKeychain = -25307, /* A default keychain could not be found. */
            errSecInteractionNotAllowed = -25308, /* User interaction is not allowed. */
            errSecReadOnlyAttr = -25309, /* The specified attribute could not be modified. */
            errSecWrongSecVersion = -25310, /* This keychain was created by a different version of the system software and cannot be opened. */
            errSecKeySizeNotAllowed = -25311, /* This item specifies a key size which is too large. */
            errSecNoStorageModule = -25312, /* A required component (data storage module) could not be loaded. You may need to restart your computer. */
            errSecNoCertificateModule = -25313, /* A required component (certificate module) could not be loaded. You may need to restart your computer. */
            errSecNoPolicyModule = -25314, /* A required component (policy module) could not be loaded. You may need to restart your computer. */
            errSecInteractionRequired = -25315, /* User interaction is required, but is currently not allowed. */
            errSecDataNotAvailable = -25316, /* The contents of this item cannot be retrieved. */
            errSecDataNotModifiable = -25317, /* The contents of this item cannot be modified. */
            errSecCreateChainFailed = -25318, /* One or more certificates required to validate this certificate cannot be found. */
            errSecInvalidPrefsDomain = -25319, /* The specified preferences domain is not valid. */
            errSecACLNotSimple = -25240, /* The specified access control list is not in standard (simple) form. */
            errSecPolicyNotFound = -25241, /* The specified policy cannot be found. */
            errSecInvalidTrustSetting = -25242, /* The specified trust setting is invalid. */
            errSecNoAccessForItem = -25243, /* The specified item has no access control. */
            errSecInvalidOwnerEdit = -25244, /* Invalid attempt to change the owner of this item. */
            errSecTrustNotAvailable = -25245, /* No trust results are available. */
            errSecUnsupportedFormat = -25256, /* Import/Export format unsupported. */
            errSecUnknownFormat = -25257, /* Unknown format in import. */
            errSecKeyIsSensitive = -25258, /* Key material must be wrapped for export. */
            errSecMultiplePrivKeys = -25259, /* An attempt was made to import multiple private keys. */
            errSecPassphraseRequired = -25260, /* Passphrase is required for import/export. */
            errSecInvalidPasswordRef = -25261, /* The password reference was invalid. */
            errSecInvalidTrustSettings = -25262, /* The Trust Settings Record was corrupted. */
            errSecNoTrustSettings = -25263, /* No Trust Settings were found. */
            errSecPkcs12VerifyFailure = -25264, /* MAC verification failed during PKCS12 Import. */
            errSecDecode = -26275, /* Unable to decode the provided data. */
        }

        private enum CSSM_DB_RECORDTYPE : uint
        {
            /* Schema Management Name Space Range Definition*/
            CSSM_DB_RECORDTYPE_SCHEMA_START = 0x00000000,
            CSSM_DB_RECORDTYPE_SCHEMA_END = CSSM_DB_RECORDTYPE_SCHEMA_START + 4,
            /* Open Group Application Name Space Range Definition*/
            CSSM_DB_RECORDTYPE_OPEN_GROUP_START = 0x0000000A,
            CSSM_DB_RECORDTYPE_OPEN_GROUP_END = CSSM_DB_RECORDTYPE_OPEN_GROUP_START + 8,
            /* Industry At Large Application Name Space Range Definition */
            CSSM_DB_RECORDTYPE_APP_DEFINED_START = 0x80000000,
            CSSM_DB_RECORDTYPE_APP_DEFINED_END = 0xffffffff,
            /* Record Types defined in the Schema Management Name Space */
            CSSM_DL_DB_SCHEMA_INFO = CSSM_DB_RECORDTYPE_SCHEMA_START + 0,
            CSSM_DL_DB_SCHEMA_INDEXES = CSSM_DB_RECORDTYPE_SCHEMA_START + 1,
            CSSM_DL_DB_SCHEMA_ATTRIBUTES = CSSM_DB_RECORDTYPE_SCHEMA_START + 2,
            CSSM_DL_DB_SCHEMA_PARSING_MODULE = CSSM_DB_RECORDTYPE_SCHEMA_START + 3,
            /* Record Types defined in the Open Group Application Name Space */
            CSSM_DL_DB_RECORD_ANY = CSSM_DB_RECORDTYPE_OPEN_GROUP_START + 0,
            CSSM_DL_DB_RECORD_CERT = CSSM_DB_RECORDTYPE_OPEN_GROUP_START + 1,
            CSSM_DL_DB_RECORD_CRL = CSSM_DB_RECORDTYPE_OPEN_GROUP_START + 2,
            CSSM_DL_DB_RECORD_POLICY = CSSM_DB_RECORDTYPE_OPEN_GROUP_START + 3,
            CSSM_DL_DB_RECORD_GENERIC = CSSM_DB_RECORDTYPE_OPEN_GROUP_START + 4,
            CSSM_DL_DB_RECORD_PUBLIC_KEY = CSSM_DB_RECORDTYPE_OPEN_GROUP_START + 5,
            CSSM_DL_DB_RECORD_PRIVATE_KEY = CSSM_DB_RECORDTYPE_OPEN_GROUP_START + 6,
            CSSM_DL_DB_RECORD_SYMMETRIC_KEY = CSSM_DB_RECORDTYPE_OPEN_GROUP_START + 7,
            CSSM_DL_DB_RECORD_ALL_KEYS = CSSM_DB_RECORDTYPE_OPEN_GROUP_START + 8
        }

        private enum CSSM_DL_DB_RECORD : uint
        {
            CSSM_DL_DB_RECORD_GENERIC_PASSWORD = CSSM_DB_RECORDTYPE.CSSM_DB_RECORDTYPE_APP_DEFINED_START + 0,
            CSSM_DL_DB_RECORD_INTERNET_PASSWORD = CSSM_DB_RECORDTYPE.CSSM_DB_RECORDTYPE_APP_DEFINED_START + 1,
            CSSM_DL_DB_RECORD_APPLESHARE_PASSWORD = CSSM_DB_RECORDTYPE.CSSM_DB_RECORDTYPE_APP_DEFINED_START + 2,
            CSSM_DL_DB_RECORD_X509_CERTIFICATE = CSSM_DB_RECORDTYPE.CSSM_DB_RECORDTYPE_APP_DEFINED_START + 0x1000,
            CSSM_DL_DB_RECORD_USER_TRUST,
            CSSM_DL_DB_RECORD_X509_CRL,
            CSSM_DL_DB_RECORD_UNLOCK_REFERRAL,
            CSSM_DL_DB_RECORD_EXTENDED_ATTRIBUTE,
            CSSM_DL_DB_RECORD_METADATA = CSSM_DB_RECORDTYPE.CSSM_DB_RECORDTYPE_APP_DEFINED_START + 0x8000
        }

        private enum SecItemClass : uint
        {
            kSecInternetPasswordItemClass = 0x696E6574, // 'inet'
            kSecGenericPasswordItemClass = 0x67656E70, // 'genp'
            kSecAppleSharePasswordItemClass = 0x61736870, // 'ashp'
            kSecCertificateItemClass = CSSM_DL_DB_RECORD.CSSM_DL_DB_RECORD_X509_CERTIFICATE,
            kSecPublicKeyItemClass = CSSM_DB_RECORDTYPE.CSSM_DL_DB_RECORD_PUBLIC_KEY,
            kSecPrivateKeyItemClass = CSSM_DB_RECORDTYPE.CSSM_DL_DB_RECORD_PRIVATE_KEY,
            kSecSymmetricKeyItemClass = CSSM_DB_RECORDTYPE.CSSM_DL_DB_RECORD_SYMMETRIC_KEY
        }

        private enum CSSM_KEYUSE : uint
        {
            CSSM_KEYUSE_ANY = 0x80000000,
            CSSM_KEYUSE_ENCRYPT = 0x00000001,
            CSSM_KEYUSE_DECRYPT = 0x00000002,
            CSSM_KEYUSE_SIGN = 0x00000004,
            CSSM_KEYUSE_VERIFY = 0x00000008,
            CSSM_KEYUSE_SIGN_RECOVER = 0x00000010,
            CSSM_KEYUSE_VERIFY_RECOVER = 0x00000020,
            CSSM_KEYUSE_WRAP = 0x00000040,
            CSSM_KEYUSE_UNWRAP = 0x00000080,
            CSSM_KEYUSE_DERIVE = 0x00000100
        }
    }
}