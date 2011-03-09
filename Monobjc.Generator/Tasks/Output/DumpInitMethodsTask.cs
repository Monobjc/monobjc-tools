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
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.Output
{
    public class DumpInitMethodsTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DumpInitMethodsTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public DumpInitMethodsTask(String name) : base(name) {}

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

                Console.WriteLine("Parsing '{0}'...", entry.Name);

                String prefix = classEntity.Name;
                DumpSimilarMethods(classEntity, prefix);
                DumpSimilarProperties(classEntity, prefix);

                prefix = prefix.Substring(2);
                DumpSimilarMethods(classEntity, prefix);
                DumpSimilarProperties(classEntity, prefix);
            }
        }

        private static void DumpSimilarMethods(ClassEntity classEntity, string prefix)
        {
            foreach (MethodEntity methodEntity in classEntity.Methods)
            {
                if (!methodEntity.Name.StartsWith(prefix))
                {
                    continue;
                }
                if (methodEntity.ReturnType != "Id")
                {
                    continue;
                }
                Console.WriteLine("<Patch>Methods[" + methodEntity.Name + "].ReturnType=" + classEntity.Name + "</Patch>");
            }
        }

        private static void DumpSimilarProperties(ClassEntity classEntity, string prefix)
        {
            foreach (PropertyEntity propertyEntity in classEntity.Properties)
            {
                if (!propertyEntity.Name.StartsWith(prefix))
                {
                    continue;
                }
                if (propertyEntity.Type != "Id")
                {
                    continue;
                }
                Console.WriteLine("<Patch>Properties[" + propertyEntity.Name + "].Type=" + classEntity.Name + "</Patch>");
                Console.WriteLine("<Patch>Properties[" + propertyEntity.Name + "].Getter.ReturnType=" + classEntity.Name + "</Patch>");
            }
        }
    }
}