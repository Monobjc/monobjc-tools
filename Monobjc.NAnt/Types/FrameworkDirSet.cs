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
using System.Globalization;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;

namespace Monobjc.NAnt.Types
{
    /// <summary>
    ///   Specialized <see cref = "DirSet" /> to handle Mac OS X framework.
    /// </summary>
    [ElementName("dirset")]
    public class FrameworkDirSet : DirSet
    {
        private const String SHARED_SCOPE = "/Library/Frameworks/{0}.framework/{0}";
        private const String SYSTEM_SCOPE = "/System/Library/Frameworks/{0}.framework/{0}";
        private const String USER_SCOPE = "~/Library/Frameworks/{0}.framework/{0}";

        /// <summary>
        ///   Gets or sets the name of the framework.
        /// </summary>
        /// <value>The name of the framework.</value>
        [TaskAttribute("framework")]
        public String FrameworkName
        {
            get { return Path.GetFileName(this.BaseDirectory.ToString()); }
            set
            {
                // Search for system scope framework
                string path = String.Format(CultureInfo.CurrentCulture, SYSTEM_SCOPE, value);
                if (File.Exists(path))
                {
                    this.BaseDirectory = new DirectoryInfo(Path.GetDirectoryName(path));
                    return;
                }

                // Search for machine scope framework
                path = String.Format(CultureInfo.CurrentCulture, SHARED_SCOPE, value);
                if (File.Exists(path))
                {
                    this.BaseDirectory = new DirectoryInfo(Path.GetDirectoryName(path));
                    return;
                }

                // Search for user scope framework
                path = String.Format(CultureInfo.CurrentCulture, USER_SCOPE, value);
                if (File.Exists(path))
                {
                    this.BaseDirectory = new DirectoryInfo(Path.GetDirectoryName(path));
                    return;
                }

                throw new BuildException("Error creating FrameworkDirSet. Cannot locate framework '" + value + "'", this.Location);
            }
        }
    }
}