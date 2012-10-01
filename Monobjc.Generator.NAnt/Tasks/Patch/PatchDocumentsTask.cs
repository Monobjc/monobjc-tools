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

			if (this.Type == DocumentType.Generated) {
				path += ".cs";
			}

			if (!File.Exists(path)) {
				return;
			}

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
				this.Log (Level.Info, String.Format ("Patching {0} for '{1}'...", this.Type, entity.name));
				File.WriteAllText (path, contents);
			} else {
				this.Log (Level.Debug, String.Format ("Skipping {0} for '{1}'...", this.Type, entity.name));
			}
		}
	}
}
