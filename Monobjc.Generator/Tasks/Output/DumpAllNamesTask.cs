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
using System.IO;
using System.Linq;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.Output
{
    public class DumpAllNamesTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DumpAllNamesTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public DumpAllNamesTask(String name) : base(name) {}

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

                BaseEntity entity;
                switch (entry.Nature)
                {
                    case TypedEntity.CLASS_NATURE:
                        entity = BaseEntity.LoadFrom<ClassEntity>(xmlFile);
                        break;
                    case TypedEntity.PROTOCOL_NATURE:
                        entity = BaseEntity.LoadFrom<ProtocolEntity>(xmlFile);
                        break;
                    case TypedEntity.ENUMERATION_NATURE:
                        entity = BaseEntity.LoadFrom<EnumerationEntity>(xmlFile);
                        break;
                    case TypedEntity.STRUCTURE_NATURE:
                        entity = BaseEntity.LoadFrom<StructureEntity>(xmlFile);
                        break;
                    case TypedEntity.TYPE_NATURE:
                        entity = BaseEntity.LoadFrom<TypedEntity>(xmlFile);
                        break;
                    default:
                        continue;
                }

                this.DumpNames(entity);
            }
        }

        private void DumpNames(BaseEntity entity)
		{
			Console.WriteLine(entity.Name);
            foreach (BaseEntity child in entity.Children)
            {
				DumpNames(child);
            }
        }
    }
}