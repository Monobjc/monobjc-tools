using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("generate-info")]
	public class GenerateAssemblyInfoTask : BaseTask
	{
		/// <summary>
		/// Gets or sets the template.
		/// </summary>
		[TaskAttribute("template", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public FileInfo Template { get; set; }

		/// <summary>
		/// Executes the task.
		/// </summary>
		protected override void ExecuteTask ()
		{
			String baseFolder = this.CreateBaseDir ();
			DocSet docSet = this.CreateDocSet ();
			IEnumerable<Framework> frameworks = this.CreateFrameworks (docSet);

			foreach (var f in frameworks) {
				this.Log (Level.Verbose, String.Format ("Generation info for '{0}'...", f.name));
				String folder = f.GetPath(baseFolder, DocumentType.Generated);
				String path = Path.Combine(folder, "Properties", "AssemblyInfo.cs");
				Directory.CreateDirectory(Path.GetDirectoryName(path));

				// Set variables
				String assembly = f.assembly;
				String name = assembly.Replace("Monobjc.", String.Empty);
				Guid guid;
				using (MD5 md5 = MD5.Create())
				{
					byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(assembly));
					guid = new Guid(hash);
				}

				// Load the template
				String content = File.ReadAllText(this.Template.ToString());
				content = content.Replace("$name$", name);
				content = content.Replace("$assembly$", assembly);
				content = content.Replace("$guid$", guid.ToString());
				File.WriteAllText (path, content);
			}
		}
	}
}
