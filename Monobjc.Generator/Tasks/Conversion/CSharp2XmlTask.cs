﻿//
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
using Monobjc.Tools.Generator.Parsers.CodeDom;

namespace Monobjc.Tools.Generator.Tasks.Conversion
{
    public class CSharp2XmlTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "CSharp2XmlTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public CSharp2XmlTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            // Prepare the TypeManager
            this.TypeManager.SetMappings(this.Settings["Mappings"]);
            IEnumerable<String> names = (from e in this.Entries
                                         where e.Nature == TypedEntity.CLASS_NATURE ||
                                               e.Nature == TypedEntity.PROTOCOL_NATURE
                                         select e.Name);
            this.TypeManager.SetClasses(names);

            // Clean entry with valid document
            foreach (Entry entry in this.Entries)
            {
                String codeFile = entry[EntryFolderType.CSharp] + ".cs";
                String xmlFile = entry[EntryFolderType.Xml];

                switch (entry.Nature)
                {
                    case TypedEntity.TYPE_NATURE:
                        {
                            break;
                        }

                    case TypedEntity.CLASS_NATURE:
                        {
                            ClassEntity classEntity = new ClassEntity();
                            classEntity.Namespace = entry.Namespace;
                            classEntity.Name = entry.Name;
                            classEntity.Nature = entry.Nature;

                            if (this.NeedProcessing(codeFile, xmlFile))
                            {
                                Console.WriteLine("Extracting '{0}'...", entry.Name);

                                CodeDomClassParser parser = new CodeDomClassParser(this.Settings, this.TypeManager);
                                parser.Parse(classEntity, codeFile);

                                codeFile = entry[EntryFolderType.CSharp] + ".Class.cs";
                                if (File.Exists(codeFile))
                                {
                                    parser.Parse(classEntity, codeFile);
                                }

                                codeFile = entry[EntryFolderType.CSharp] + ".Notifications.cs";
                                if (File.Exists(codeFile))
                                {
                                    parser.Parse(classEntity, codeFile);
                                }

                                codeFile = entry[EntryFolderType.CSharp] + ".Constants.cs";
                                if (File.Exists(codeFile))
                                {
                                    parser.Parse(classEntity, codeFile);
                                }

                                classEntity.SaveTo(xmlFile);
                            }

                            break;
                        }

                    case TypedEntity.PROTOCOL_NATURE:
                        {
                            ProtocolEntity protocolEntity = new ProtocolEntity();
                            protocolEntity.Namespace = entry.Namespace;
                            protocolEntity.Name = entry.Name;
                            protocolEntity.Nature = entry.Nature;

                            codeFile = entry[EntryFolderType.CSharp] + ".Protocol.cs";
                            if (this.NeedProcessing(codeFile, xmlFile))
                            {
                                Console.WriteLine("Extracting '{0}'...", entry.Name);

                                CodeDomProtocolParser parser = new CodeDomProtocolParser(this.Settings, this.TypeManager);
                                parser.Parse(protocolEntity, codeFile);
                                protocolEntity.SaveTo(xmlFile);
                            }

                            break;
                        }

                    case TypedEntity.ENUMERATION_NATURE:
                        {
                            EnumerationEntity enumerationEntity = new EnumerationEntity();
                            enumerationEntity.Namespace = entry.Namespace;
                            enumerationEntity.Name = entry.Name;

                            if (this.NeedProcessing(codeFile, xmlFile))
                            {
                                Console.WriteLine("Extracting '{0}'...", entry.Name);

                                CodeDomEnumParser parser = new CodeDomEnumParser(this.Settings, this.TypeManager);
                                parser.Parse(enumerationEntity, codeFile);
                                enumerationEntity.SaveTo(xmlFile);
                            }

                            break;
                        }

                    case TypedEntity.STRUCTURE_NATURE:
                        {
                            break;
                        }

                    default:
                        throw new Exception("Unexpected type: " + entry.Nature);
                }
            }
        }
    }
}