using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.NAnt
{
	public abstract class BaseTask : Task
	{
		private MRUDictionary<String, BaseEntity> cache = new MRUDictionary<String, BaseEntity> (10);
		
		/// <summary>
		/// Gets or sets the base dir.
		/// </summary>
		[TaskAttribute("dir", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public DirectoryInfo BaseDir { get; set; }

		/// <summary>
		/// Gets or sets the document set.
		/// </summary>
		[BuildElement("docset", Required = false)]
		public DocSetElement DocSet { get; set; }

		/// <summary>
		/// Gets or sets the framework set.
		/// </summary>
		[BuildElement("framework-set", Required = true)]
		public FrameworkSetElement FrameworkSet { get; set; }

		protected String CreateBaseDir()
		{
			return this.BaseDir.ToString();
		}

		protected DocSet CreateDocSet()
		{
			if (this.DocSet != null) {
				return this.DocSet.CreateDocSet();
			}
			return null;
		}
		
		protected IEnumerable<Framework> CreateFrameworks(DocSet docSet)
		{
			List<Framework> frameworks = this.FrameworkSet.CreateFrameworks().ToList();
			foreach(var f in frameworks.Where(f => f.source == DocumentOrigin.DocSet)) {
				f.DocSet = docSet;
			}
			return frameworks;
		}

		protected T FindAndLoad<T>(String baseFolder, IList<FrameworkEntity> entities, FrameworkEntityType type, String name, bool useCache = true) where T : BaseEntity
		{
			if (String.IsNullOrEmpty (name)) {
				return null;
			}
			
			// Try the cache first
			String key = type + ":" + name;
			BaseEntity result = null;
			if (useCache && !cache.TryGetValue (key, out result)) {
				//this.Log (Level.Verbose, "Cache miss for '" + type + "' and name '" + name + "'");
				
				var candidates = entities.Where (e => e.type == type && e.name == name).ToList ();
				FrameworkEntity candidate = candidates.FirstOrDefault();
				
				// Print a warning if multiple
				if (candidates.Count > 1) {
					this.Log (Level.Warning, "Found several entities with the type '" + type + "' and name '" + name + "'");
				}
				
				if (candidate != null) {
					String file = candidate.GetPath (baseFolder, DocumentType.Model);
					result = BaseEntity.LoadFrom<T> (file);
				}
			} else {
				//this.Log (Level.Verbose, "Cache hit for '" + type + "' and name '" + name + "'");
			}
			
			// Add to MRU
			if (useCache && result != null) {
				cache.Add (key, result);
			}
			
			return result as T;
		}
	}
}
