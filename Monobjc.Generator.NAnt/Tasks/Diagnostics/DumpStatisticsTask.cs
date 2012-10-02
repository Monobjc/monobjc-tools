using System;
using System.Collections.Generic;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Parsers;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("dump-statistics")]
	public class DumpStatisticsTask : BaseTask
	{
		/// <summary>
		/// Gets or sets the mapping file.
		/// </summary>
		[TaskAttribute("report", Required = false)]
		[StringValidator(AllowEmpty = false)]
		public FileInfo Report { get; set; }

		/// <summary>
		/// Executes the task.
		/// </summary>
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
					this.Log (Level.Verbose, String.Format ("Processing '{0}'...", e.name));

					// TODO
				}
			}

			if (this.Report != null) {
				writer.Close ();
				writer.Dispose ();
			}
		}
	}
}
