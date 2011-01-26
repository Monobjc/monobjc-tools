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

namespace Monobjc.Tools.Utilities
{
    /// <summary>
    ///   Utiltiy class for File releated operations.
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        ///   Returns whether the destination is younger than the source.
        /// </summary>
        public static bool UpToDate(String source, String destination)
        {
            if (!File.Exists(destination))
            {
                return false;
            }

            DateTime sourceDateTime = File.GetLastWriteTime(source);
            DateTime destinationDateTime = File.GetLastWriteTime(destination);
            return destinationDateTime.CompareTo(sourceDateTime) > 0;
        }
    }
}