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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Monobjc.Tools.Generator.Model.Configuration;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.Output
{
    public class DumpMissingTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DumpMissingTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public DumpMissingTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            IList<String> input = new List<String>();
            IList<String> output = new List<String>();

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
                            input.Add(String.Format("{0}:{1}:{2}", f.name, TypedEntity.TYPE_NATURE, parts[0]));
                        }
                    }
                    if (f.Classes != null)
                    {
                        foreach (EntitiesFrameworkClass e in f.Classes)
                        {
                            string[] parts = e.name.Split(';');
                            input.Add(String.Format("{0}:{1}:{2}", f.name, TypedEntity.CLASS_NATURE, parts[0]));
                        }
                    }
                    if (f.Protocols != null)
                    {
                        foreach (EntitiesFrameworkProtocol e in f.Protocols)
                        {
                            string[] parts = e.name.Split(';');
                            input.Add(String.Format("{0}:{1}:{2}", f.name, TypedEntity.PROTOCOL_NATURE, parts[0]));
                        }
                    }
                    if (f.Enumerations != null)
                    {
                        foreach (EntitiesFrameworkEnumeration e in f.Enumerations)
                        {
                            string[] parts = e.name.Split(';');
                            input.Add(String.Format("{0}:{1}:{2}", f.name, TypedEntity.ENUMERATION_NATURE, parts[0]));
                        }
                    }
                    if (f.Structures != null)
                    {
                        foreach (EntitiesFrameworkStructure e in f.Structures)
                        {
                            string[] parts = e.name.Split(';');
                            input.Add(String.Format("{0}:{1}:{2}", f.name, TypedEntity.STRUCTURE_NATURE, parts[0]));
                        }
                    }
                }
            }

            foreach (Entry entry in this.Entries)
            {
                output.Add(String.Format("{0}:{1}:{2}", entry.Namespace, entry.Nature, entry.Name));
            }

            IEnumerable<String> notEntries = output.Except(input);
            foreach (String notEntry in notEntries)
            {
                string[] parts = notEntry.Split(';');
                if (notEntry.Contains(";C;"))
                {
                    Console.WriteLine("<Class name=\"{0}\"></Class>", parts[2]);
                }
                else if (notEntry.Contains(";P;"))
                {
                    Console.WriteLine("<Protocol name=\"{0}\"></Protocol>", parts[2]);
                }
                else
                {
                    Console.WriteLine(notEntry);
                }
            }
        }
    }
}