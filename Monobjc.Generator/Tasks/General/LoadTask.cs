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
using System.Linq;
using System.Xml.Serialization;
using Monobjc.Tools.Generator.Model.Configuration;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.General
{
    public class LoadTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "LoadTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public LoadTask(String name) : base(name) {}

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
                    if (f.Types != null)
                    {
                        foreach (EntitiesFrameworkType e in f.Types)
                        {
                            string[] parts = e.name.Split(';');
                            this.AddEntry(f.name, TypedEntity.TYPE_NATURE, parts);
                        }
                    }
                    if (f.Classes != null)
                    {
                        foreach (EntitiesFrameworkClass e in f.Classes)
                        {
                            string[] parts = e.name.Split(';');
                            this.AddEntry(f.name, TypedEntity.CLASS_NATURE, parts);
                        }
                    }
                    if (f.Protocols != null)
                    {
                        foreach (EntitiesFrameworkProtocol e in f.Protocols)
                        {
                            string[] parts = e.name.Split(';');
                            this.AddEntry(f.name, TypedEntity.PROTOCOL_NATURE, parts);
                        }
                    }
                    if (f.Enumerations != null)
                    {
                        foreach (EntitiesFrameworkEnumeration e in f.Enumerations)
                        {
                            string[] parts = e.name.Split(';');
                            Entry entry = this.AddEntry(f.name, TypedEntity.ENUMERATION_NATURE, parts);
                            entry.RemoteUrl = "N/A";
                        }
                    }
                    if (f.Structures != null)
                    {
                        foreach (EntitiesFrameworkStructure e in f.Structures)
                        {
                            string[] parts = e.name.Split(';');
                            Entry entry = this.AddEntry(f.name, TypedEntity.STRUCTURE_NATURE, parts);
                            entry.RemoteUrl = "N/A";
                        }
                    }
                }
            }

            this.Context.Save();
        }

        private Entry AddEntry(String framework, String nature, String[] parts)
        {
            String name = parts[0];
            String remoteFile = parts.Length > 1 ? parts[1] : name;

            Entry entry = this.Entries.SingleOrDefault(en => en.Nature.Equals(nature) && en.Name.Equals(name));
            if (entry == null)
            {
                entry = new Entry();
                entry.Name = name;
                this.Entries.Add(entry);
            }

            Console.WriteLine("Adding '{0}'...", entry.Name);

            entry.Namespace = framework;
            entry.Nature = nature;
            entry.RemoteFile = remoteFile;

            return entry;
        }
    }
}