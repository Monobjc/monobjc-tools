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
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Monobjc.Tools.Generator.Model.Configuration;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.Patching
{
    public class ReplaceTextTask : BaseTask
    {
        protected EntryFolderType Type { get; private set; }
        protected String Extension { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ReplaceTextTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        /// <param name = "type">The type.</param>
        /// <param name = "extension">The extension.</param>
        public ReplaceTextTask(String name, EntryFolderType type, String extension = null) : base(name)
        {
            this.Type = type;
            this.Extension = extension;
        }

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            using (StreamReader reader = new StreamReader(this.Settings["Entities"]))
            {
                XmlSerializer serializer = new XmlSerializer(typeof (Entities));
                Entities entities = (Entities) serializer.Deserialize(reader);

                foreach (EntitiesFramework f in entities.Items)
                {
                    Entry entry;

                    if (f.Types != null)
                    {
                        foreach (EntitiesFrameworkType e in f.Types)
                        {
                            string[] parts = e.name.Split(';');
                            entry = this.Find(f.name, TypedEntity.TYPE_NATURE, parts[0]);
                            if (e.Replace == null)
                            {
                                continue;
                            }
                            IEnumerable<Replace> replacements = from r in e.Replace where r.type == this.Type.ToString() select r;
                            this.Patch(entry, replacements);
                        }
                    }
                    if (f.Classes != null)
                    {
                        foreach (EntitiesFrameworkClass e in f.Classes)
                        {
                            string[] parts = e.name.Split(';');
                            entry = this.Find(f.name, TypedEntity.CLASS_NATURE, parts[0]);
                            if (e.Replace == null)
                            {
                                continue;
                            }
                            IEnumerable<Replace> replacements = from r in e.Replace where r.type == this.Type.ToString() select r;
                            this.Patch(entry, replacements);
                        }
                    }
                    if (f.Protocols != null)
                    {
                        foreach (EntitiesFrameworkProtocol e in f.Protocols)
                        {
                            string[] parts = e.name.Split(';');
                            entry = this.Find(f.name, TypedEntity.PROTOCOL_NATURE, parts[0]);
                            if (e.Replace == null)
                            {
                                continue;
                            }
                            IEnumerable<Replace> replacements = from r in e.Replace where r.type == this.Type.ToString() select r;
                            this.Patch(entry, replacements);
                        }
                    }
                    if (f.Enumerations != null)
                    {
                        foreach (EntitiesFrameworkEnumeration e in f.Enumerations)
                        {
                            string[] parts = e.name.Split(';');
                            entry = this.Find(f.name, TypedEntity.ENUMERATION_NATURE, parts[0]);
                            if (e.Replace == null)
                            {
                                continue;
                            }
                            IEnumerable<Replace> replacements = from r in e.Replace where r.type == this.Type.ToString() select r;
                            this.Patch(entry, replacements);
                        }
                    }
                }
            }
        }

        protected void Patch(Entry entry, IEnumerable<Replace> replacements)
        {
            if (entry != null && replacements != null)
            {
                String file = entry[this.Type];
                if (!String.IsNullOrEmpty(this.Extension))
                {
                    file += this.Extension;
                }
                if (!File.Exists(file))
                {
                    return;
                }

                String contents = File.ReadAllText(file);
                bool modified = false;
                foreach (Replace replacement in replacements)
                {
                    String source = replacement.Source;
                    String with = replacement.With;
                    if (contents.Contains(source))
                    {
                        contents = contents.Replace(source, with);
                        modified |= true;
                    }
                }

                if (modified)
                {
                    Console.WriteLine("Patching '{0}'...", entry.Name);
                    File.WriteAllText(file, contents);
                }
            }
        }
    }
}