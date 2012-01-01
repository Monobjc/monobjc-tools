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
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.Output
{
    public class DumpDelegateMethodsTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DumpDelegateMethodsTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public DumpDelegateMethodsTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            List<String> names = new List<string>();
            foreach (Entry entry in this.Entries)
            {
                String xmlFile = entry[EntryFolderType.Xml];
                if (!File.Exists(xmlFile))
                {
                    continue;
                }

                ClassEntity classEntity;
                switch (entry.Nature)
                {
                    case TypedEntity.CLASS_NATURE:
                        classEntity = BaseEntity.LoadFrom<ClassEntity>(xmlFile);
                        break;
                    default:
                        continue;
                }

                if (classEntity.DelegateMethods.Count > 0)
                {
                    names.Add(classEntity.Name);

                    ProtocolEntity protocolEntity = new ProtocolEntity();
                    protocolEntity.Name = classEntity.Name + "Delegate";
                    protocolEntity.Namespace = classEntity.Namespace;
                    protocolEntity.MinAvailability = classEntity.MinAvailability;
                    protocolEntity.MaxAvailability = classEntity.MaxAvailability;

                    xmlFile = Path.Combine("Analysis", Path.Combine(protocolEntity.Namespace, protocolEntity.Name));
                    Console.WriteLine("Saving " + xmlFile);
                    protocolEntity.SaveTo(xmlFile);
                }
            }

            Console.WriteLine("Class that have delegate methods");

            names.Sort();
            foreach (String name in names.Distinct())
            {
                Console.WriteLine(name);
            }
        }
    }
}