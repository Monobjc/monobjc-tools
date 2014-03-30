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
namespace Monobjc.Tools
{
    internal enum OSStatus
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
}