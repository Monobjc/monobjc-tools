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
using System.Collections.Generic;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Tasks.General
{
    public class UpdateTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "UpdateTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public UpdateTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            // Update entry with no URL
            foreach (Entry entry in this.EntriesWithEmptyUrls)
            {
                Console.WriteLine("Updating '{0}'...", entry.Name);

                Update(entry);
            }

            this.Context.Save();

            // Update entry with invalid URL
            foreach (Entry entry in this.EntriesWithUrls)
            {
                Console.WriteLine("Updating '{0}'...", entry.Name);

                if (String.Equals(entry.RemoteUrl, "N/A"))
                {
                    continue;
                }

                if (DownloadHelper.Validate(entry.RemoteUrl))
                {
                    continue;
                }

                Update(entry);
            }

            this.Context.Save();
        }

        private static void Update(Entry entry)
        {
            IEnumerable<string> urls = UrlGenerator.GenerateURL(entry.Nature, entry.Name, entry.RemoteFile);
            foreach (string url in urls)
            {
                if (!DownloadHelper.Validate(url))
                {
                    continue;
                }
                Console.WriteLine("Found a valid URL for '{0}'...", entry.Name);
                entry.RemoteUrl = url;
                break;
            }
        }
    }
}