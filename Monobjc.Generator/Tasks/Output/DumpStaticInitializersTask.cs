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
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.Output
{
    public class DumpStaticInitializersTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DumpStaticInitializersTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public DumpStaticInitializersTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

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
                    case TypedEntity.PROTOCOL_NATURE:
                        if (entry.Nature == TypedEntity.CLASS_NATURE)
                        {
                            classEntity = BaseEntity.LoadFrom<ClassEntity>(xmlFile);
                        }
                        else
                        {
                            classEntity = BaseEntity.LoadFrom<ProtocolEntity>(xmlFile);
                        }
                        break;
                    default:
                        continue;
                }

                this.FindStaticInitializers(classEntity);
            }
        }

        private void FindStaticInitializers(ClassEntity classEntity)
        {
            string[] prefixes = new[] {"AB", "ABV", "NS", "AE", "AX", "ATSU", "CG", "CT", "LS", "PM", "QD", "UT", "AU", "AUMIDI", "CF", "CL", "CV", "DR", "QT", "QL", "SC", "PDF", "Web", "CA"};
            foreach (string prefix in prefixes)
            {
                if (!classEntity.Name.StartsWith(prefix))
                {
                    continue;
                }

                String name = classEntity.Name.Substring(prefix.Length);
                PropertyEntity propertyEntity = classEntity.Properties.Find(p => p.Name.Equals(name) && p.Type == "Id");
                if (propertyEntity != null)
                {
                    Console.WriteLine("Found: {0}.{1}", classEntity.Name, propertyEntity.Name);
                }

                foreach (MethodEntity methodEntity in classEntity.Methods.Where(m => m.Name.StartsWith(name) && m.ReturnType == "Id"))
                {
                    Console.WriteLine("Found: {0}.{1}()", classEntity.Name, methodEntity.Name);
                }
            }
        }
    }
}