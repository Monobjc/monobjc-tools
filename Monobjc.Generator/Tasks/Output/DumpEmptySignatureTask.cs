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
	public class DumpEmptySignatureTask : BaseTask
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "DumpEmptySignatureTask" /> class.
		/// </summary>
		/// <param name = "name">The name.</param>
		public DumpEmptySignatureTask (String name) : base(name)
		{
		}

		/// <summary>
		///   Executes this instance.
		/// </summary>
		public override void Execute ()
		{
			this.DisplayBanner ();

			foreach (Entry entry in this.Entries) {
				String xmlFile = entry [EntryFolderType.Xml];
				if (!File.Exists (xmlFile)) {
					continue;
				}

				switch (entry.Nature) {
				case TypedEntity.CLASS_NATURE:
					{
						ClassEntity entity = BaseEntity.LoadFrom<ClassEntity> (xmlFile);
						this.DumpEmptySignature (entity);
						break;
					}
				case TypedEntity.PROTOCOL_NATURE:
					{
						ProtocolEntity entity = BaseEntity.LoadFrom<ProtocolEntity> (xmlFile);
						this.DumpEmptySignature (entity);
						break;
					}
				case TypedEntity.TYPE_NATURE:
					{
						TypedEntity entity = BaseEntity.LoadFrom<TypedEntity> (xmlFile);
						this.DumpEmptySignature (entity);
						break;
					}
				default:
					continue;
				}

			}
		}
		
		private void DumpEmptySignature (ClassEntity entity)
		{
			foreach (MethodEntity child in entity.Methods) {
				if (String.IsNullOrEmpty (child.Signature)) {
					Console.WriteLine (entity.Name + " " + child.Name);
				}
			}
			foreach (PropertyEntity child in entity.Properties) {
				if (String.IsNullOrEmpty (child.Getter.Signature)) {
					Console.WriteLine (entity.Name + " " + child.Name);
				}
			}
		}
		
		private void DumpEmptySignature (TypedEntity entity)
		{
			foreach (MethodEntity child in entity.Functions) {
				if (String.IsNullOrEmpty (child.Signature)) {
					Console.WriteLine (entity.Name + " " + child.Name);
				}
			}
		}
	}
}