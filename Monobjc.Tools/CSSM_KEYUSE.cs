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
namespace Monobjc.Tools
{
    internal enum CSSM_KEYUSE : uint
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