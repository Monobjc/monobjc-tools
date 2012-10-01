using System;
using System.Collections.Generic;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;
using System.Linq;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("gather-mixed-types")]
	public class GatherMixedTypesTask : BaseTask
	{
		/// <summary>
		/// Gets or sets the mixed types file.
		/// </summary>
		[TaskAttribute("mixed-types", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public FileInfo MixedTypesFile { get; set; }

		/// <summary>
		/// Executes the task.
		/// </summary>
		protected override void ExecuteTask ()
		{
			String baseFolder = this.CreateBaseDir ();
			DocSet docSet = this.CreateDocSet ();
			IEnumerable<Framework> frameworks = this.CreateFrameworks (docSet);
			IList<FrameworkEntity> entities = frameworks.SelectMany (f => f.GetEntities ()).ToList ();

			String mixedTypesFile = this.MixedTypesFile.ToString ();
			Dictionary<String, String> table = new Dictionary<String, String> ();
			this.LoadMixedTypes(mixedTypesFile, table);

			foreach (var e in entities) {
				String sourcePath = e.GetPath (baseFolder, DocumentType.Model);
				if (sourcePath == null || !File.Exists (sourcePath)) {
					continue;
				}

				if (sourcePath.IsOlderThan (mixedTypesFile)) {
					continue;
				}

				this.Log (Level.Verbose, String.Format ("Scanning '{0}' for mixed types...", e.name));
				
				switch (e.type) {
				case FrameworkEntityType.T:
				{
					TypedEntity entity = BaseEntity.LoadFrom<TypedEntity> (sourcePath);
					this.AddMixedType (table, entity.Name, entity.MixedType);
					foreach (EnumerationEntity enumerationEntity in entity.Enumerations) {
						this.AddMixedType (table, enumerationEntity.Name, enumerationEntity.MixedType);
					}
				}
					break;
				case FrameworkEntityType.C:
				{
					ClassEntity entity = BaseEntity.LoadFrom<ClassEntity> (sourcePath);
					this.AddMixedType (table, entity.Name, entity.MixedType);
					foreach (EnumerationEntity enumerationEntity in entity.Enumerations) {
						this.AddMixedType (table, enumerationEntity.Name, enumerationEntity.MixedType);
					}
				}
					break;
				case FrameworkEntityType.P:
				{
					ProtocolEntity entity = BaseEntity.LoadFrom<ProtocolEntity> (sourcePath);
					this.AddMixedType (table, entity.Name, entity.MixedType);
					foreach (EnumerationEntity enumerationEntity in entity.Enumerations) {
						this.AddMixedType (table, enumerationEntity.Name, enumerationEntity.MixedType);
					}
				}
					break;
				case FrameworkEntityType.S:
				{
					StructureEntity entity = BaseEntity.LoadFrom<StructureEntity> (sourcePath);
					this.AddMixedType (table, entity.Name, entity.MixedType);
				}
					break;
				case FrameworkEntityType.E:
				{
					EnumerationEntity entity = BaseEntity.LoadFrom<EnumerationEntity> (sourcePath);
					this.AddMixedType (table, entity.Name, entity.MixedType);
				}
					break;
				default:
					throw new NotSupportedException ("Entity type not support: " + e.type);
				}
			}

			this.SaveMixedTypes(mixedTypesFile, table);
		}

		private void LoadMixedTypes(String file, Dictionary<String, String> table)
		{
			if (!File.Exists(file)) {
				return;
			}

			String[] lines = File.ReadAllLines(file);
			foreach(String line in lines) {
				String[] parts = line.Split('=');
				if (parts.Length != 2) {
					continue;
				}
				this.AddMixedType(table, parts[0], parts[1]);
			}
		}

		private void SaveMixedTypes(String file, Dictionary<String, String> table)
		{
			List<String> lines = new List<String>();
			foreach(String key in table.Keys) {
				lines.Add(String.Format("{0}={1}", key, table[key]));
			}
			File.WriteAllLines(file, lines.ToArray());
		}

		private void AddMixedType (Dictionary<String, String> table, String name, String mixedType)
		{
			if (String.IsNullOrWhiteSpace (mixedType)) {
				return;
			}
			this.Log (Level.Info, String.Format ("Adding MixedType {0}={1}", name, mixedType));
			table [name] = mixedType;
		}
	}
}
