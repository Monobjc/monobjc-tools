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
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.Output
{
    public class DumpDeprecatedTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DumpDeprecatedTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public DumpDeprecatedTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            foreach (Entry entry in this.Entries)
            {
                if (entry.Name.EndsWith(".Deprecated"))
                {
                    continue;
                }

                String xhtmlFile = entry[EntryFolderType.Xhtml];
                if (!File.Exists(xhtmlFile))
                {
                    continue;
                }

                String content = File.ReadAllText(xhtmlFile);
                if (content.Contains("DeprecationAppendix"))
                {
                    switch (entry.Nature)
                    {
                        case TypedEntity.CLASS_NATURE:
                            Console.WriteLine("<Class name=\"{0}.Deprecated\"><Patch>AdditionFor={0}</Patch></Class>", entry.Name);
                            break;
                        default:
                            Console.WriteLine("{0} has deprecated API", entry.Name);
                            break;
                    }
                }
            }
        }
    }
}