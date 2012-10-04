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
		/// <summary>
		/// Gets or sets the report file.
		/// </summary>
		[TaskAttribute("report", Required = false)]
		[StringValidator(AllowEmpty = false)]
		public FileInfo Report { get; set; }
		
		protected override void ExecuteTask ()
		{
			String baseFolder = this.CreateBaseDir ();
			DocSet docSet = this.CreateDocSet ();
			IEnumerable<Framework> frameworks = this.CreateFrameworks (docSet);

			TextWriter writer = null;
			if (this.Report != null) {
				writer = new StreamWriter (this.Report.ToString ());
			}
			
			foreach (var f in frameworks) {
				foreach (var e in f.GetEntities()) {
					if (e.Patch == null) {
						continue;
					}

					var changes = e.Patch.Where (p => p.type == DocumentType.Model).SelectMany (p => p.Change);
					if (changes == null) {
						continue;
					}

					this.Change (baseFolder, e, changes, writer);
				}
			}

			if (this.Report != null) {
				writer.Close ();
				writer.Dispose ();
			}
		}

		protected void Change (String baseFolder, FrameworkEntity e, IEnumerable<String> changes, TextWriter writer)
		{
			String file = e.GetPath (baseFolder, DocumentType.Model);
			if (file == null || !File.Exists (file)) {
				return;
			}

			this.Log (Level.Info, String.Format ("Patching {0} for '{1}'...", DocumentType.Model, e.name));

			switch (e.type) {
			case FrameworkEntityType.T:
				LoadAndChange<TypedEntity> (e.name, file, changes, writer);
				break;
			case FrameworkEntityType.C:
				LoadAndChange<ClassEntity> (e.name, file, changes, writer);
				break;
			case FrameworkEntityType.P:
				LoadAndChange<ProtocolEntity> (e.name, file, changes, writer);
				break;
			case FrameworkEntityType.S:
				LoadAndChange<StructureEntity> (e.name, file, changes, writer);
				break;
			case FrameworkEntityType.E:
				LoadAndChange<EnumerationEntity> (e.name, file, changes, writer);
				break;
			default:
				throw new NotSupportedException ("Unsupported type: " + e.type);
			}
		}
		
		private void LoadAndChange<T> (String name, String file, IEnumerable<String> changes, TextWriter writer) where T : BaseEntity
		{
			T entity = BaseEntity.LoadFrom<T> (file);
			bool modified = false;
			foreach (var change in changes) {
				this.Log(Level.Verbose, "Patching {0} with '{1}'", name, change);

				int hash = entity.GetHashValue();
				BaseEntity.Change<T> (entity, change);
				int newHash = entity.GetHashValue();

				modified |= (hash != newHash);
				if (writer != null) {
					writer.WriteLine("{0} {1} with '{2}'", (hash != newHash) ? "MODIFIED " : "UNTOUCHED", name, change);
				}
			}

			if (modified) {
				entity.SaveTo (file);
			}
		}
	}
}
