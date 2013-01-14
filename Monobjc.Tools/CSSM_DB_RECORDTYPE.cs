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
namespace Monobjc.Tools
{
    internal enum CSSM_DB_RECORDTYPE : uint
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
}