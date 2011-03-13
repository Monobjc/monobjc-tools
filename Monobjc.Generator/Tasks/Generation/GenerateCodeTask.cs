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
using Monobjc.Tools.Generator.Generators;
using Monobjc.Tools.Generator.Model.Configuration;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.Generation
{
    public class GenerateCodeTask : BaseTask
    {
        private Dictionary<String, List<String>> usings;
        private GenerationStatistics statistics;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "GenerateCodeTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public GenerateCodeTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            this.usings = new Dictionary<string, List<string>>();
            this.statistics = new GenerationStatistics();

            IEnumerable<String> names = (from e in this.Entries
                                         where e.Nature == TypedEntity.CLASS_NATURE ||
                                               e.Nature == TypedEntity.PROTOCOL_NATURE
                                         select e.Name);
            this.TypeManager.SetClasses(names);

            this.LoadLicense();
            BaseGenerator.License = GenerationHelper.License;
            BaseGenerator.TypeManager = this.TypeManager;
            this.LoadUsings();
            this.LoadMixedTypes();

            // Generate entry with valid document
            foreach (Entry entry in this.Entries)
            {
                String xmlFile = entry[EntryFolderType.Xml];
                if (!File.Exists(xmlFile))
                {
                    continue;
                }

                switch (entry.Nature)
                {
                    case TypedEntity.TYPE_NATURE:
                        {
                            // Compute generated file
                            String generatedFile = entry[EntryFolderType.Generated];
                            if (this.NeedProcessing(xmlFile, generatedFile + ".cs"))
                            {
                                Console.WriteLine("Generating '{0}'...", entry.Name);

                                TypedEntity typedEntity = BaseEntity.LoadFrom<TypedEntity>(xmlFile);
                                if (!typedEntity.Generate)
                                {
                                    break;
                                }

                                using (StreamWriter writer = new StreamWriter(generatedFile + ".cs"))
                                {
                                    TypedGenerator generator = new TypedGenerator(writer, this.statistics);
                                    generator.Usings = this.usings[entry.Namespace];
                                    generator.Generate(typedEntity);
                                }
                            }
                            break;
                        }

                    case TypedEntity.CLASS_NATURE:
                        {
                            // Compute generated file
                            String generatedFile = entry[EntryFolderType.Generated];
                            if (this.NeedProcessing(xmlFile, generatedFile + ".cs"))
                            {
                                Console.WriteLine("Generating '{0}'...", entry.Name);

                                ClassEntity classEntity = BaseEntity.LoadFrom<ClassEntity>(xmlFile);

                                this.LoadDependentEntitiesForClass(classEntity);

                                if (classEntity.ExtendedEntity != null)
                                {
                                    if (classEntity.Generate)
                                    {
                                        using (StreamWriter writer = new StreamWriter(generatedFile + ".cs"))
                                        {
                                            ClassAdditionsGenerator generator = new ClassAdditionsGenerator(writer, this.statistics);
                                            generator.Usings = this.usings[entry.Namespace];
                                            generator.Generate(classEntity);
                                        }
                                    }

                                    if (classEntity.HasConstants || classEntity.HasEnumerations)
                                    {
                                        using (StreamWriter writer = new StreamWriter(generatedFile + ".Constants.cs"))
                                        {
                                            ClassConstantsGenerator generator = new ClassConstantsGenerator(writer, this.statistics);
                                            generator.Usings = this.usings[entry.Namespace];
                                            generator.Generate(classEntity);
                                        }
                                    }
                                }
                                else
                                {
                                    if (classEntity.Generate)
                                    {
                                        using (StreamWriter writer = new StreamWriter(generatedFile + ".cs"))
                                        {
                                            ClassDefinitionGenerator generator = new ClassDefinitionGenerator(writer, this.statistics);
                                            generator.Usings = this.usings[entry.Namespace];
                                            generator.Generate(classEntity);
                                        }

                                        using (StreamWriter writer = new StreamWriter(generatedFile + ".Class.cs"))
                                        {
                                            ClassMembersGenerator generator = new ClassMembersGenerator(writer, this.statistics);
                                            generator.Usings = this.usings[entry.Namespace];
                                            generator.Generate(classEntity);
                                        }

                                        if (classEntity.HasConstructors)
                                        {
                                            using (StreamWriter writer = new StreamWriter(generatedFile + ".Constructors.cs"))
                                            {
                                                ClassConstructorsGenerator generator = new ClassConstructorsGenerator(writer, this.statistics);
                                                generator.Usings = this.usings[entry.Namespace];
                                                generator.Generate(classEntity);
                                            }
                                        }
                                    }

                                    if (classEntity.HasConstants || classEntity.HasEnumerations)
                                    {
                                        using (StreamWriter writer = new StreamWriter(generatedFile + ".Constants.cs"))
                                        {
                                            ClassConstantsGenerator generator = new ClassConstantsGenerator(writer, this.statistics);
                                            generator.Usings = this.usings[entry.Namespace];
                                            generator.Generate(classEntity);
                                        }
                                    }

                                    if (classEntity.Generate && classEntity.Protocols != null)
                                    {
                                        using (StreamWriter writer = new StreamWriter(generatedFile + ".Protocols.cs"))
                                        {
                                            ClassProtocolsGenerator generator = new ClassProtocolsGenerator(writer, this.statistics);
                                            generator.Usings = this.usings[entry.Namespace];
                                            generator.Generate(classEntity);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case TypedEntity.PROTOCOL_NATURE:
                        {
                            String generatedFile = entry[EntryFolderType.Generated];
                            if (this.NeedProcessing(xmlFile, generatedFile + ".Protocol.cs"))
                            {
                                Console.WriteLine("Generating '{0}'...", entry.Name);

                                ProtocolEntity protocolEntity = BaseEntity.LoadFrom<ProtocolEntity>(xmlFile);

                                this.LoadDependentEntitiesForProtocol(protocolEntity);

                                if (protocolEntity.Generate)
                                {
                                    using (StreamWriter writer = new StreamWriter(generatedFile + ".Protocol.cs"))
                                    {
                                        ProtocolGenerator generator = new ProtocolGenerator(writer, this.statistics);
                                        generator.Usings = this.usings[entry.Namespace];
                                        generator.Generate(protocolEntity);
                                    }

                                    if (protocolEntity.DelegatorEntity != null)
                                    {
                                        String property = protocolEntity.DelegateProperty ?? "Delegate";
                                        String dir = Path.GetDirectoryName(generatedFile);
                                        String file = Path.Combine(dir, protocolEntity.DelegatorEntity.Name + "." + property + ".cs");
                                        using (StreamWriter writer = new StreamWriter(file))
                                        {
                                            ClassDelegateGenerator generator = new ClassDelegateGenerator(writer, this.statistics);
                                            generator.Usings = this.usings[entry.Namespace];
                                            generator.Generate(protocolEntity);
                                        }
                                    }
                                }

                                if (protocolEntity.HasConstants || protocolEntity.HasEnumerations)
                                {
                                    using (StreamWriter writer = new StreamWriter(generatedFile + ".Constants.cs"))
                                    {
                                        ClassConstantsGenerator generator = new ClassConstantsGenerator(writer, this.statistics);
                                        generator.Usings = this.usings[entry.Namespace];
                                        generator.Generate(protocolEntity);
                                    }
                                }
                            }
                            break;
                        }
                    case TypedEntity.ENUMERATION_NATURE:
                        {
                            String generatedFile = entry[EntryFolderType.Generated];
                            if (this.NeedProcessing(xmlFile, generatedFile + ".cs"))
                            {
                                Console.WriteLine("Generating '{0}'...", entry.Name);

                                EnumerationEntity enumerationEntity = BaseEntity.LoadFrom<EnumerationEntity>(xmlFile);
                                if (enumerationEntity.Generate)
                                {
                                    using (StreamWriter writer = new StreamWriter(generatedFile + ".cs"))
                                    {
                                        EnumerationGenerator generator = new EnumerationGenerator(writer, this.statistics);
                                        generator.Usings = this.usings[entry.Namespace];
                                        generator.Generate(enumerationEntity);
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            // Print out statistics
            Console.WriteLine("--- Statistics ---\n{0}", this.statistics);
        }

        private void LoadUsings()
        {
            Console.WriteLine("Loading framework usings...");
            using (StreamReader reader = new StreamReader(this.Settings["Entities"]))
            {
                XmlSerializer serializer = new XmlSerializer(typeof (Entities));
                Entities entities = (Entities) serializer.Deserialize(reader);

                foreach (EntitiesFramework f in entities.Items)
                {
                    if (f.usings != null)
                    {
                        this.usings.Add(f.name, f.usings.Split(',').ToList());
                    }
                    else
                    {
                        this.usings.Add(f.name, new List<string>());
                    }
                }
            }
        }

        private void LoadMixedTypes()
        {
            Console.WriteLine("Loading mixed types...");
            BaseGenerator.MixedTypes.Clear();
            foreach (Entry entry in this.Entries)
            {
                String xmlFile = entry[EntryFolderType.Xml];
                if (!File.Exists(xmlFile))
                {
                    continue;
                }

                switch (entry.Nature)
                {
                    case TypedEntity.TYPE_NATURE:
                        {
                            TypedEntity typedEntity = BaseEntity.LoadFrom<TypedEntity>(xmlFile);
                            if (!String.IsNullOrEmpty(typedEntity.MixedType))
                            {
                                BaseGenerator.MixedTypes[typedEntity.Name] = typedEntity.MixedType;
                            }
                            foreach (EnumerationEntity enumerationEntity in typedEntity.Enumerations)
                            {
                                if (!String.IsNullOrEmpty(enumerationEntity.MixedType))
                                {
                                    BaseGenerator.MixedTypes[enumerationEntity.Name] = enumerationEntity.MixedType;
                                }
                            }
                            break;
                        }
                    case TypedEntity.CLASS_NATURE:
                    case TypedEntity.PROTOCOL_NATURE:
                        {
                            ClassEntity classEntity;
                            if (entry.Nature == TypedEntity.CLASS_NATURE)
                            {
                                classEntity = BaseEntity.LoadFrom<ClassEntity>(xmlFile);
                            }
                            else
                            {
                                classEntity = BaseEntity.LoadFrom<ProtocolEntity>(xmlFile);
                            }
                            if (!String.IsNullOrEmpty(classEntity.MixedType))
                            {
                                BaseGenerator.MixedTypes[classEntity.Name] = classEntity.MixedType;
                            }
                            foreach (EnumerationEntity enumerationEntity in classEntity.Enumerations)
                            {
                                if (!String.IsNullOrEmpty(enumerationEntity.MixedType))
                                {
                                    BaseGenerator.MixedTypes[enumerationEntity.Name] = enumerationEntity.MixedType;
                                }
                            }
                            break;
                        }
                    case TypedEntity.ENUMERATION_NATURE:
                        {
                            EnumerationEntity enumerationEntity = BaseEntity.LoadFrom<EnumerationEntity>(xmlFile);
                            if (!String.IsNullOrEmpty(enumerationEntity.MixedType))
                            {
                                BaseGenerator.MixedTypes[enumerationEntity.Name] = enumerationEntity.MixedType;
                            }
                            break;
                        }
                    case TypedEntity.STRUCTURE_NATURE:
                        {
                            StructureEntity structureEntity = BaseEntity.LoadFrom<StructureEntity>(xmlFile);
                            if (!String.IsNullOrEmpty(structureEntity.MixedType))
                            {
                                BaseGenerator.MixedTypes[structureEntity.Name] = structureEntity.MixedType;
                            }
                            break;
                        }
                }
            }
            Console.WriteLine("Loaded {0} mixed types", BaseGenerator.MixedTypes.Count);
        }

        private void LoadDependentEntitiesForClass(ClassEntity classEntity)
        {
            // Retrieve super class
            Entry entry = this.Find(null, TypedEntity.CLASS_NATURE, classEntity.BaseType);
            if (entry != null)
            {
                ClassEntity entity = BaseEntity.LoadFrom<ClassEntity>(entry[EntryFolderType.Xml]);
                this.LoadDependentEntitiesForClass(entity);
                classEntity.SuperClass = entity;
            }

            // Retrieve protocols
            if (!String.IsNullOrEmpty(classEntity.ConformsTo))
            {
                String[] parts = classEntity.ConformsTo.Split(',');
                List<Entry> results = parts.Select(p => this.Find(null, TypedEntity.PROTOCOL_NATURE, p)).ToList();
                classEntity.Protocols = results.Where(r => r != null).Select(r => BaseEntity.LoadFrom<ProtocolEntity>(r[EntryFolderType.Xml])).ToList();
            }

            // Retrieve extended class
            if (!String.IsNullOrEmpty(classEntity.AdditionFor))
            {
                entry = this.Find(null, TypedEntity.CLASS_NATURE, classEntity.AdditionFor);
                if (entry != null)
                {
                    ClassEntity entity = BaseEntity.LoadFrom<ClassEntity>(entry[EntryFolderType.Xml]);
                    this.LoadDependentEntitiesForClass(entity);
                    classEntity.ExtendedEntity = entity;
                }
            }
        }

        private void LoadDependentEntitiesForProtocol(ProtocolEntity protocolEntity)
        {
            this.LoadDependentEntitiesForClass(protocolEntity);

            // Retrieve delegator class
            if (!String.IsNullOrEmpty(protocolEntity.DelegateFor))
            {
                Entry entry = this.Find(null, TypedEntity.CLASS_NATURE, protocolEntity.DelegateFor);
                if (entry != null)
                {
                    ClassEntity entity = BaseEntity.LoadFrom<ClassEntity>(entry[EntryFolderType.Xml]);
                    this.LoadDependentEntitiesForClass(entity);
                    protocolEntity.DelegatorEntity = entity;
                }
            }
        }
    }
}