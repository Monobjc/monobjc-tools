using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using NAnt.Core;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("patch-documents")]
	public class PatchDocumentsTask : BaseTask
	{
		[TaskAttribute("type", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public DocumentType Type { get; set; }

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

					var replacements = e.Patch.Where (p => p.type == this.Type).SelectMany (p => p.Replace);
					if (replacements.Count () == 0) {
						continue;
					}

					this.Patch (baseFolder, e, replacements);
				}
			}
		}

		protected void Patch (String baseFolder, FrameworkEntity entity, IEnumerable<PatchReplace> replacements)
		{
			String path = entity.GetPath (baseFolder, this.Type);
			if (path == null) {
				return;
			}

			IList<String> paths = new List<String>();
			switch(this.Type) {
			case DocumentType.Generated:
				paths.Add(path + ".cs");
				paths.Add(path + ".Class.cs");
				paths.Add(path + ".Constants.cs");
				paths.Add(path + ".Constructors.cs");
				paths.Add(path + ".Protocol.cs");
				paths.Add(path + ".Protocols.cs");
				break;
			default:
				paths.Add(path);
				break;
			}

			this.Patch(paths, entity, replacements);
		}

		protected void Patch (IEnumerable<String> paths, FrameworkEntity e, IEnumerable<PatchReplace> replacements)
		{
			foreach (var path in paths) {
				if (!File.Exists (path)) {
					continue;
				}

				this.Log (Level.Verbose, String.Format ("Probing {0} for '{1}'...", Path.GetFileName(path), e.name));

				String contents = File.ReadAllText (path);
				bool modified = false;
				foreach (PatchReplace replacement in replacements) {
					String source = replacement.Source;
					String with = replacement.With;
					if (contents.Contains (source)) {
						contents = contents.Replace (source, with);
						modified |= true;
					}
				}
			
				if (modified) {
					this.Log (Level.Info, String.Format ("Patching {0} for '{1}'...", this.Type, e.name));
					File.WriteAllText (path, contents);
				} else {
					this.Log (Level.Verbose, String.Format ("Skipping {0} for '{1}'...", this.Type, e.name));
				}
			}
		}
	}
}
