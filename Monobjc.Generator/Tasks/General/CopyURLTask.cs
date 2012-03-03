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
using System;
using System.IO;
using Monobjc.Tools.Generator.Model.Database;

namespace Monobjc.Tools.Generator.Tasks.General
{
    public class CopyURLTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "CopyURLTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public CopyURLTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            foreach (Entry entry in this.Entries)
            {
                Entry deprecatedEntry = this.Find(entry.Namespace, entry.Nature, entry.Name + ".Deprecated");
                if (deprecatedEntry != null)
                {
                    const string suffix = "DeprecationAppendix/AppendixADeprecatedAPI.html";
                    String url = entry.RemoteUrl;
                    String parent = Path.GetDirectoryName(url);
                    while (!String.IsNullOrEmpty(parent))
                    {
                        if (Path.GetFileName(parent).Contains(entry.Name))
                        {
                            break;
                        }
                        parent = Path.GetDirectoryName(parent);
                    }
                    /*
                    url = url.Replace("Reference/Reference.html", suffix);
                    url = url.Replace("Introduction/Introduction.html", suffix);
                    url = url.Replace("Reference/NSCell.html", suffix);
                    url = url.Replace("Reference/NSCoder.html", suffix);
                    url = url.Replace("Reference/NSString.html", suffix);
                    url = url.Replace("NSArray.html", suffix);
                    url = url.Replace("NSPersistentStoreCoordinator.html", suffix);
                     */
                    url = Path.Combine(parent, suffix);
                    deprecatedEntry.RemoteUrl = url;
                }
            }

            this.Context.Save();
        }
    }
}