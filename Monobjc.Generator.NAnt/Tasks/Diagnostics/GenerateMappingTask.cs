using System;
using System.Collections.Generic;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("generate-mapping")]
	public class GenerateMappingTask : BaseTask
	{
		/// <summary>
		/// Gets or sets the mapping file.
		/// </summary>
		[TaskAttribute("mappings", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public FileInfo MappingFile { get; set; }

		/// <summary>
		/// Gets or sets the mapping report file.
		/// </summary>
		[TaskAttribute("mapping-report", Required = false)]
		[StringValidator(AllowEmpty = false)]
		public FileInfo MappingReport { get; set; }

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
			IDictionary<String, String> mixedTypes = this.LoadMixedTypes ();
			IDictionary<String, String> unmapped = this.LoadUnMapped ();

			// Create the list of unmapped types and remove known ones
			List<String> types = new List<String> (unmapped.Keys);
			types.RemoveAll (t => mixedTypes.ContainsKey (t));
			types.Sort ();

			Mappings result = new Mappings ();
			result.Items = new MappingsMapping[types.Count];
			for (int i = 0; i < types.Count; i++) {
				result.Items [i] = new MappingsMapping () { 
					type=types[i], 
					Value = unmapped[types[i]] 
				};
			}

			using (StreamWriter writer = new StreamWriter(this.MappingFile.ToString())) {
				XmlSerializer serializer = new XmlSerializer (typeof(Mappings));
				serializer.Serialize(writer, result);
			}
		}

		private IDictionary<String, String> LoadMixedTypes ()
		{
			this.Log (Level.Info, "Loading mixed types...");
			String file = this.MixedTypesFile.ToString ();
			Dictionary<String, String> table = new Dictionary<String, String> ();
			
			String[] lines = File.ReadAllLines (file);
			foreach (String line in lines) {
				String[] parts = line.Split ('=');
				if (parts.Length != 2) {
					continue;
				}
				this.AddMixedType (table, parts [0], parts [1]);
			}
			this.Log (Level.Info, "Loaded {0} mixed types", table.Count);
			
			return table;
		}
		
		private void AddMixedType (Dictionary<String, String> table, String name, String mixedType)
		{
			if (String.IsNullOrWhiteSpace (mixedType)) {
				return;
			}
			this.Log (Level.Info, String.Format ("Adding MixedType {0}={1}", name, mixedType));
			table [name] = mixedType;
		}

		private IDictionary<String, String> LoadUnMapped ()
		{
			this.Log (Level.Info, "Loading unmapped types...");
			String file = this.MappingReport.ToString ();
			Dictionary<String, String> table = new Dictionary<String, String> ();
			
			String[] lines = File.ReadAllLines (file);
			foreach (String line in lines) {
				String[] parts = line.Split ('=');
				if (parts.Length != 2) {
					continue;
				}
				table.Add (parts [0], parts [1]);
			}

			return table;
		}
		
	}
}
