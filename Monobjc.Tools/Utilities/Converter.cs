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
using System;

namespace Monobjc.Tools.Utilities
{
    /// <summary>
    ///   Utility class to perform special convertions.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        ///   Extract a byte array from the given base 64 encoded string.
        /// </summary>
        /// <param name = "content">The content to decode.</param>
        /// <returns>A byte array</returns>
        public static byte[] FromBase64(String content)
        {
            // Strip ALL kinds of whitespace that can be in Base 64 String to get an accurate count.
            String realContent = content.
                Replace("\r", String.Empty).
                Replace("\n", String.Empty).
                Replace("\t", String.Empty).
                Replace(" ", String.Empty);

            int remainder = realContent.Length%4;
            if (remainder > 0)
            {
                // Pad to multiple of 4 with characters System.Convert.ToBase64String() would use.
                realContent += "A==".Substring(remainder - 1);
            }

            return Convert.FromBase64String(realContent);
        }
    }
}