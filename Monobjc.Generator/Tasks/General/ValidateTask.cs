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

namespace Monobjc.Tools.Generator.Tasks.General
{
    public class ValidateTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ValidateTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public ValidateTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            // Print entry with no URL
            foreach (Entry entry in this.EntriesWithEmptyUrls)
            {
                Console.WriteLine("No URL for '{0}'", entry.Name);
            }

            // Print entry with invalid URL
            foreach (Entry entry in this.EntriesWithUrls)
            {
                //Console.WriteLine("Validating '{0}'...", entry.Name);

                if (String.Equals(entry.RemoteUrl, "N/A"))
                {
                    continue;
                }
                String path = entry.GetRemoteUrl();
                if (File.Exists(path))
                {
                    continue;
                }
                path = Path.ChangeExtension(path, ".htm");
                if (File.Exists(path))
                {
                    continue;
                }

                Console.WriteLine("Invalid URL for '{0}'", entry.Name);
            }
        }
    }
}