using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using NAnt.Core;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("mark-deprecation")]
	public class MarkDeprecationTask : BaseTask
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
					if (!e.name.EndsWith(".Deprecated")) {
						continue;
					}
					
					this.MarkDeprecate(baseFolder, e, writer);
				}
			}

			if (this.Report != null) {
				writer.Close ();
				writer.Dispose ();
			}
		}

		private void MarkDeprecate (String baseFolder, FrameworkEntity e, TextWriter writer)
		{
			String file = e.GetPath (baseFolder, DocumentType.Model);
			if (file == null || !File.Exists (file)) {
				return;
			}

			this.Log (Level.Info, String.Format ("Marking {0} for '{1}'...", DocumentType.Model, e.name));

			Action<TypedEntity> markFunctions = (t) => Mark<FunctionEntity>(t.Functions);
			Action<ClassEntity> markMethods = (c) => Mark<MethodEntity>(c.Methods);
			Action<ClassEntity> markProperties = (c) => Mark<PropertyEntity>(c.Properties);

			switch (e.type) {
			case FrameworkEntityType.T:
				LoadAndMark<TypedEntity> (e.name, file, writer, markFunctions);
				break;
			case FrameworkEntityType.C:
				LoadAndMark<ClassEntity> (e.name, file, writer, markFunctions, markMethods, markProperties);
				break;
			case FrameworkEntityType.P:
				LoadAndMark<ProtocolEntity> (e.name, file, writer, markFunctions, markMethods, markProperties);
				break;
			default:
				throw new NotSupportedException ("Unsupported type: " + e.type);
			}
		}

		private void LoadAndMark<T> (String name, String file, TextWriter writer, params Object[] actions) where T : BaseEntity
		{
			T entity = BaseEntity.LoadFrom<T> (file);

			int hash = entity.GetHashValue();
			foreach(var a in actions) {
				Action<T> action = (Action<T>) a;
				action(entity);
			}
			int newHash = entity.GetHashValue();

			if (writer != null) {
				writer.WriteLine("{0} {1}", (hash != newHash) ? "MODIFIED " : "UNTOUCHED", entity.Name);
			}

			if (hash != newHash) {
				entity.SaveTo (file);
			}
		}

		private void Mark<T>(IEnumerable<T> entities) where T : BaseEntity {
			foreach(T entity in entities) {
				List<String> lines = entity.Summary;
				String line = lines.FirstOrDefault();
				if (String.IsNullOrEmpty(line)) {
					continue;
				}
				AvailabilityHelper.AddMention(entity, line);
			}
		}
	}
}
