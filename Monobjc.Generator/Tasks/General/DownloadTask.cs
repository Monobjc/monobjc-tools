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
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Tasks.General
{
    public class DownloadTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DownloadTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public DownloadTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            // Download entry with valid URL
            foreach (Entry entry in this.EntriesWithUrls)
            {
                if (String.Equals(entry.RemoteUrl, "N/A"))
                {
                    continue;
                }

                String htmlFile = entry[EntryFolderType.Html];
                if (!this.Context.Force && File.Exists(htmlFile))
                {
                    continue;
                }

                Console.WriteLine("Downloading '{0}'...", entry.Name);
                DownloadHelper.Download(entry.RemoteUrl, htmlFile);
            }
        }
    }
}