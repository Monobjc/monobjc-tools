using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using NAnt.Core;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("patch-model")]
	public class PatchModelTask : BaseTask
	{
		protected override void ExecuteTask ()
		{
			String baseFolder = this.CreateBaseDir ();
			DocSet docSet = this.CreateDocSet ();
			IEnumerable<Framework> frameworks = this.CreateFrameworks (docSet);

			foreach (var f in frameworks) {
				foreach (var e in f.GetEntities()) {
					if (e.Patch == null) {
						continue;
					}

					var changes = e.Patch.Where (p => p.type == DocumentType.Model).SelectMany (p => p.Change);
					if (changes == null) {
						continue;
					}

					this.Change (baseFolder, e, changes);
				}
			}
		}

		protected void Change (String baseFolder, FrameworkEntity e, IEnumerable<String> changes)
		{
			String file = e.GetPath (baseFolder, DocumentType.Model);
			if (file == null || !File.Exists (file)) {
				return;
			}

			this.Log (Level.Info, String.Format ("Patching {0} for '{1}'...", DocumentType.Model, e.name));

			BaseEntity baseEntity = null;
			switch (e.type) {
			case FrameworkEntityType.T:
				baseEntity = LoadAndChange<TypedEntity>(file, changes);
				break;
			case FrameworkEntityType.C:
				baseEntity = LoadAndChange<ClassEntity>(file, changes);
				break;
			case FrameworkEntityType.P:
				baseEntity = LoadAndChange<ProtocolEntity>(file, changes);
				break;
			case FrameworkEntityType.S:
				baseEntity = LoadAndChange<StructureEntity>(file, changes);
				break;
			case FrameworkEntityType.E:
				baseEntity = LoadAndChange<EnumerationEntity>(file, changes);
				break;
			}

			baseEntity.SaveTo (file);
		}
		
		private BaseEntity LoadAndChange<T>(String file, IEnumerable<String> changes) where T : BaseEntity
		{
			T entity = BaseEntity.LoadFrom<T> (file);
			BaseEntity.Change<T> (entity, changes);
			return entity;
		}
	}
}
