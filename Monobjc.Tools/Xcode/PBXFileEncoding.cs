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
namespace Monobjc.Tools.Xcode
{
	/// <summary>
	/// </summary>
	public enum PBXFileEncoding : uint
	{
		/// <summary>
		/// </summary>
		Default = 0,
		/// <summary>
		/// </summary>
		UTF8 = 4,
		/// <summary>
		/// </summary>
		UTF16 = 10,
		/// <summary>
		/// </summary>
		UTF16_BE = 2415919360,
		/// <summary>
		/// </summary>
		UTF16_LE = 2483028224,
		/// <summary>
		/// </summary>
		Western = 30,
		/// <summary>
		/// </summary>
		Japanese = 2147483649,
		/// <summary>
		/// </summary>
		TraditionalChinese = 2147483650,
		/// <summary>
		/// </summary>
		Korean = 2147483651,
		/// <summary>
		/// </summary>
		Arabic = 2147483652,
		/// <summary>
		/// </summary>
		Hebrew = 2147483653,
		/// <summary>
		/// </summary>
		Greek = 2147483654,
		/// <summary>
		/// </summary>
		Cyrillic = 2147483655,
		/// <summary>
		/// </summary>
		SimplifiedChinese = 2147483673,
		/// <summary>
		/// </summary>
		CentralEuropean = 2147483677,
		/// <summary>
		/// </summary>
		Turkish = 2147483683,
		/// <summary>
		/// </summary>
		Icelandic = 2147483685,
	}
}