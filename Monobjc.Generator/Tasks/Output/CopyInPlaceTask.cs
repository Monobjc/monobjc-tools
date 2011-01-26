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
using System.Xml.Serialization;
using Monobjc.Tools.Generator.Model.Configuration;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.Output
{
    public class CopyInPlaceTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "CopyInPlaceTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public CopyInPlaceTask(String name, String targetDir) : base(name)
        {
            this.TargetDirectory = targetDir;
        }

        private string TargetDirectory { get; set; }

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            if (String.IsNullOrEmpty(this.TargetDirectory))
            {
                return;
            }

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
                            this.Copy(f, entry, this.TargetDirectory);
                        }
                    }
                    if (f.Classes != null)
                    {
                        foreach (EntitiesFrameworkClass e in f.Classes)
                        {
                            string[] parts = e.name.Split(';');
                            entry = this.Find(f.name, TypedEntity.CLASS_NATURE, parts[0]);
                            this.Copy(f, entry, this.TargetDirectory);
                        }
                    }
                    if (f.Protocols != null)
                    {
                        foreach (EntitiesFrameworkProtocol e in f.Protocols)
                        {
                            string[] parts = e.name.Split(';');
                            entry = this.Find(f.name, TypedEntity.PROTOCOL_NATURE, parts[0]);
                            this.Copy(f, entry, this.TargetDirectory);
                        }
                    }
                    if (f.Enumerations != null)
                    {
                        foreach (EntitiesFrameworkEnumeration e in f.Enumerations)
                        {
                            string[] parts = e.name.Split(';');
                            entry = this.Find(f.name, TypedEntity.ENUMERATION_NATURE, parts[0]);
                            this.Copy(f, entry, this.TargetDirectory);
                        }
                    }
                }
            }
        }

        private void Copy(EntitiesFramework f, Entry entry, string outputDirectory)
        {
            String baseDir = Path.Combine(outputDirectory, f.assembly);
            baseDir = Path.Combine(baseDir, entry.Namespace + "_" + entry.Nature);
            Directory.CreateDirectory(baseDir);

            String prefix = entry[EntryFolderType.Generated];
            List<String> files = new List<string>();
            switch (entry.Nature)
            {
                case TypedEntity.CLASS_NATURE:
                    files.Add(prefix + ".cs");
                    files.Add(prefix + ".Class.cs");
                    files.Add(prefix + ".Constants.cs");
                    files.Add(prefix + ".Constructors.cs");
                    files.Add(prefix + ".Notifications.cs");
                    files.Add(prefix + ".Protocols.cs");
                    break;
                case TypedEntity.PROTOCOL_NATURE:
                    files.Add(prefix + ".cs");
                    files.Add(prefix + ".Constants.cs");
                    files.Add(prefix + ".Protocol.cs");
                    int pos = prefix.IndexOf("Delegate");
                    if (pos != -1)
                    {
                        files.Add(prefix.Insert(pos, ".") + ".cs");
                    }

                    // Special case NSControlTextEditingDelegate protocol
                    if (String.Equals(Path.GetFileName(prefix), "NSControlTextEditingDelegate"))
                    {
                        files.Add(Path.Combine(Path.GetDirectoryName(prefix), "NSControl.Delegate.cs"));
                    }
                    break;
                case TypedEntity.ENUMERATION_NATURE:
                case TypedEntity.STRUCTURE_NATURE:
                case TypedEntity.TYPE_NATURE:
                    files.Add(prefix + ".cs");
                    break;
                default:
                    break;
            }

            foreach (String file in files)
            {
                String target = Path.Combine(baseDir, Path.GetFileName(file));
                if (this.NeedProcessing(file, target))
                {
                    Console.WriteLine("Write " + target);
                    File.Copy(file, target, true);
                }
            }
        }
    }
}