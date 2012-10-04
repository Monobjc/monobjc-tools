using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using NAnt.Core;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("merge-deprecated")]
	public class MergeDeprecatedTask : BaseTask
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
			IList<FrameworkEntity> entities = frameworks.SelectMany (f => f.GetEntities ()).ToList ();

			TextWriter writer = null;
			if (this.Report != null) {
				writer = new StreamWriter (this.Report.ToString ());
			}
			
			foreach (var f in frameworks) {
				foreach (var e in f.GetEntities()) {
					if (!e.name.EndsWith(".Deprecated")) {
						continue;
					}

					this.Log (Level.Info, String.Format ("Merging {0}...", e.name));
					
					String name = e.name.Split('.')[0];
					IEnumerable<FrameworkEntity> list = entities.Where(v => (v.type == e.type) && (v.name.Split('.')[0] == name)).OrderBy(v => v.name);
					this.Merge (baseFolder, list, writer);
				}
			}
			
			if (this.Report != null) {
				writer.Close ();
				writer.Dispose ();
			}
		}

		private void Merge(String baseFolder, IEnumerable<FrameworkEntity> list, TextWriter writer) {
			if (list.Count() < 2) {
				this.Log(Level.Error, "Cannot merge less than 2 entities");
			}

			IEnumerable<FrameworkEntityType> types = list.Select(e => e.type).Distinct();
			if (types.Count() > 1) {
				this.Log(Level.Error, "Cannot merge different type of entities");
			}

			IEnumerable<String> files = list.Select(e => e.GetPath(baseFolder, DocumentType.Model));
			if (files.Count() < list.Count()) {
				this.Log(Level.Error, "Cannot merge because some entities have no path.");
			}

			Action<TypedEntity, TypedEntity> mergeFunctions = (t1, t2) => Merge<FunctionEntity>(t1.Functions, t2.Functions, new FunctionComparer());
			Action<ClassEntity, ClassEntity> mergeMethods = (c1, c2) => Merge<MethodEntity>(c1.Methods, c2.Methods, new MethodComparer());
			Action<ClassEntity, ClassEntity> mergeProperties = (c1, c2) => Merge<PropertyEntity>(c1.Properties, c2.Properties, new PropertyComparer());

			if (writer != null) {
				foreach(var e in list) {
					writer.WriteLine("    Merging {0}", e.name);
				}
			}

			FrameworkEntityType type = types.First();
			switch(type) {
			case FrameworkEntityType.T:
				LoadAndMerge<TypedEntity>(files, writer, mergeFunctions);
				break;
			case FrameworkEntityType.C:
				LoadAndMerge<ClassEntity>(files, writer, mergeFunctions, mergeMethods, mergeProperties);
				break;
			case FrameworkEntityType.P:
				LoadAndMerge<ProtocolEntity>(files, writer, mergeFunctions, mergeMethods, mergeProperties);
				break;
			}
		}

		private void LoadAndMerge<T>(IEnumerable<String> files, TextWriter writer, params Object[] actions) where T : BaseEntity {
			IEnumerable<T> entities = files.Select(f => BaseEntity.LoadFrom<T>(f));
			T target = entities.First();
			int hash = target.GetHashValue();

			foreach(T entity in entities.Skip(1)) {
				foreach(var a in actions) {
					Action<T, T> action = (Action<T, T>) a;
					action(target, entity);
				}
			}

			int newHash = target.GetHashValue();
			
			if (writer != null) {
				writer.WriteLine("{0} {1}", (hash != newHash) ? "MODIFIED " : "UNTOUCHED", target.Name);
			}
			
			if (hash != newHash) {
				target.SaveTo (files.First());
			}
		}

		private void Merge<T>(IList<T> target, IEnumerable<T> values, IEqualityComparer<T> comparer) where T : BaseEntity {
			IList<T> newValues = values.Except(target, comparer).ToList();
			foreach(T value in newValues) {
				target.Add(value);
			}
		}
	}
}
