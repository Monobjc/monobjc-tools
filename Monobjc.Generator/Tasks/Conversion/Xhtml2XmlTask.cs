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
using System.Collections.Specialized;
using System.Linq;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Parsers.Xhtml;
using Monobjc.Tools.Generator.Parsers.Xhtml.Classic;
using Monobjc.Tools.Generator.Parsers.Xhtml.Cocoa;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Tasks.Conversion
{
    public class Xhtml2XmlTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "Xhtml2XmlTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public Xhtml2XmlTask(String name) : base(name) {}

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
                String xhtmlFile = entry[EntryFolderType.Xhtml];
                String xmlFile = entry[EntryFolderType.Xml];

                switch (entry.Nature)
                {
                    case TypedEntity.TYPE_NATURE:
                        {
                            TypedEntity typedEntity = new TypedEntity();
                            typedEntity.Namespace = entry.Namespace;
                            typedEntity.Name = entry.Name;
                            typedEntity.Nature = entry.Nature;

                            if (this.NeedProcessing(xhtmlFile, xmlFile))
                            {
                                Console.WriteLine("Extracting '{0}'...", entry.Name);

                                IXhtmlTypeParser parser = this.getTypeParser(entry, this.Settings, this.TypeManager);
                                parser.Parse(typedEntity, xhtmlFile);
                                typedEntity.SaveTo(xmlFile);
                            }

                            break;
                        }

                    case TypedEntity.CLASS_NATURE:
                        {
                            ClassEntity classEntity = new ClassEntity();
                            classEntity.Namespace = entry.Namespace;
                            classEntity.Name = entry.Name;
                            classEntity.Nature = entry.Nature;

                            if (this.NeedProcessing(xhtmlFile, xmlFile))
                            {
                                Console.WriteLine("Extracting '{0}'...", entry.Name);

                                IXhtmlClassParser parser = this.getClassParser(entry, this.Settings, this.TypeManager);
                                parser.Parse(classEntity, xhtmlFile);
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

                            if (this.NeedProcessing(xhtmlFile, xmlFile))
                            {
                                Console.WriteLine("Extracting '{0}'...", entry.Name);

                                IXhtmlClassParser parser = this.getProtocolParser(entry, this.Settings, this.TypeManager);
                                parser.Parse(protocolEntity, xhtmlFile);
                                protocolEntity.SaveTo(xmlFile);
                            }

                            break;
                        }

                    case TypedEntity.ENUMERATION_NATURE:
                        {
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

        private IXhtmlTypeParser getTypeParser(Entry entry, NameValueCollection nameValueCollection, TypeManager typeManager)
        {
            switch (entry.PageStyle)
            {
                case PageStyle.Cocoa:
                    return new XhtmlCocoaTypeParser(this.Settings, this.TypeManager);
                case PageStyle.Classic:
                    return new XhtmlClassicTypeParser(this.Settings, this.TypeManager);
                default:
                    throw new NotSupportedException();
            }
        }

        private IXhtmlClassParser getClassParser(Entry entry, NameValueCollection nameValueCollection, TypeManager typeManager)
        {
            switch (entry.PageStyle)
            {
                case PageStyle.Cocoa:
                    return new XhtmlCocoaClassParser(this.Settings, this.TypeManager);
                case PageStyle.Classic:
                    return new XhtmlClassicClassParser(this.Settings, this.TypeManager);
                default:
                    throw new NotSupportedException();
            }
        }

        private IXhtmlClassParser getProtocolParser(Entry entry, NameValueCollection nameValueCollection, TypeManager typeManager)
        {
            switch (entry.PageStyle)
            {
                case PageStyle.Cocoa:
                    return new XhtmlCocoaClassParser(this.Settings, this.TypeManager);
                case PageStyle.Classic:
                    return new XhtmlClassicClassParser(this.Settings, this.TypeManager);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}