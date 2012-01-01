//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2012 - Laurent Etiemble
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
    internal enum CSSM_DL_DB_RECORD : uint
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
}