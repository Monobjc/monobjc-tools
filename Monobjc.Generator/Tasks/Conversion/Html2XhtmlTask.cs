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

namespace Monobjc.Tools.Generator.Tasks.Conversion
{
    public class Html2XhtmlTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "Html2XhtmlTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public Html2XhtmlTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            // Clean entry with valid document
            foreach (Entry entry in this.Entries)
            {
                String htmlFile = entry[EntryFolderType.Html];
                if (!File.Exists(htmlFile))
                {
                    continue;
                }

                String xhtmlFile = entry[EntryFolderType.Xhtml];
                if (!this.Context.Force && xhtmlFile.IsYoungerThan(htmlFile))
                {
                    continue;
                }

                Console.WriteLine("Cleaning '{0}'...", entry.Name);
                HtmlCleaner.HtmlToXhtml(htmlFile, xhtmlFile);
            }
        }
    }
}