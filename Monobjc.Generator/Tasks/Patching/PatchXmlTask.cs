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
using System.Xml.Serialization;
using Monobjc.Tools.Generator.Model.Configuration;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.Patching
{
    public class PatchXmlTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PatchXmlTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public PatchXmlTask(String name) : base(name) {}

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
                    Patch[] patch;

                    if (f.Types != null)
                    {
                        foreach (EntitiesFrameworkType e in f.Types)
                        {
                            string[] parts = e.name.Split(';');
                            entry = this.Find(f.name, TypedEntity.TYPE_NATURE, parts[0]);
                            patch = e.Patch;
                            this.Patch(entry, patch);
                        }
                    }
                    if (f.Classes != null)
                    {
                        foreach (EntitiesFrameworkClass e in f.Classes)
                        {
                            string[] parts = e.name.Split(';');
                            entry = this.Find(f.name, TypedEntity.CLASS_NATURE, parts[0]);
                            patch = e.Patch;
                            this.Patch(entry, patch);
                        }
                    }
                    if (f.Protocols != null)
                    {
                        foreach (EntitiesFrameworkProtocol e in f.Protocols)
                        {
                            string[] parts = e.name.Split(';');
                            entry = this.Find(f.name, TypedEntity.PROTOCOL_NATURE, parts[0]);
                            patch = e.Patch;
                            this.Patch(entry, patch);
                        }
                    }
                    if (f.Enumerations != null)
                    {
                        foreach (EntitiesFrameworkEnumeration e in f.Enumerations)
                        {
                            string[] parts = e.name.Split(';');
                            entry = this.Find(f.name, TypedEntity.ENUMERATION_NATURE, parts[0]);
                            patch = e.Patch;
                            this.Patch(entry, patch);
                        }
                    }
                }
            }
        }

        private void Patch(Entry entry, Patch[] patch)
        {
            if (entry != null && patch != null)
            {
                String xmlFile = entry[EntryFolderType.Xml];
                if (!File.Exists(xmlFile))
                {
                    return;
                }

                BaseEntity baseEntity = null;
                switch (entry.Nature)
                {
                    case TypedEntity.TYPE_NATURE:
                        baseEntity = BaseEntity.LoadFrom<TypedEntity>(xmlFile);
                        break;
                    case TypedEntity.CLASS_NATURE:
                    case TypedEntity.PROTOCOL_NATURE:
                        if (entry.Nature == TypedEntity.CLASS_NATURE)
                        {
                            baseEntity = BaseEntity.LoadFrom<ClassEntity>(xmlFile);
                        }
                        else
                        {
                            baseEntity = BaseEntity.LoadFrom<ProtocolEntity>(xmlFile);
                        }
                        break;
                    case TypedEntity.ENUMERATION_NATURE:
                        baseEntity = BaseEntity.LoadFrom<EnumerationEntity>(xmlFile);
                        break;
                    default:
                        return;
                }

                bool modified = false;
                foreach (Patch query in patch)
                {
                    modified |= baseEntity.SetValue(query.Value);
                }

                if (modified)
                {
                    Console.WriteLine("Patching '{0}'...", entry.Name);
                    baseEntity.SaveTo(xmlFile);
                }
            }
        }
    }
}