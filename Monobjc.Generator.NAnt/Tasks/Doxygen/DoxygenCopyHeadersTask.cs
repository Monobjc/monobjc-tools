using System;
using System.Collections.Generic;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Filters;
using NAnt.Core.Tasks;
using NAnt.Core.Types;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("doxygen-copy-headers")]
	public class DoxygenCopyHeadersTask : BaseTask
	{
		/// <summary>
		/// Gets or sets the filters.
		/// </summary>
		[BuildElement ("filterchain")]
		public FilterChain Filters { get; set; }

		/// <summary>
		/// Executes the task.
		/// </summary>
		protected override void ExecuteTask ()
		{
			String baseFolder = this.CreateBaseDir ();
			DocSet docSet = this.CreateDocSet ();
			IEnumerable<Framework> frameworks = this.CreateFrameworks (docSet);

			foreach (var f in frameworks) {
				if (f.source != DocumentOrigin.Doxygen) {
					continue;
				}

				this.Log(Level.Info, "Copying header files for '{0}'...", f.name);

				FileSet fileSet = new FileSet();
				fileSet.BaseDirectory = new DirectoryInfo(f.path);
				fileSet.Includes.Add("**/*.h");

				CopyTask copyTask = new CopyTask();
				copyTask.Project = this.Project;
				copyTask.FailOnError = true;
				if (docSet != null) {
					copyTask.ToDirectory = new DirectoryInfo(Path.Combine(baseFolder, docSet.Name, f.name, DocumentType.Source.ToString()));
				} else {
					copyTask.ToDirectory = new DirectoryInfo(Path.Combine(baseFolder, f.name, DocumentType.Source.ToString()));
				}
				copyTask.CopyFileSet = fileSet;
				copyTask.Filters = this.Filters;
				copyTask.Execute();
			}
		}
	}
}
