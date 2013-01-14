using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("find-delegate")]
	public class FindDelegateTask : BaseTask
	{
		private static readonly Regex PATTERN_DELEGATE = new Regex ("Delegate Methods");

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
			IList<FrameworkEntity> entities = frameworks.SelectMany (f => f.GetEntities ()).ToList ();

			TextWriter writer = null;
			if (this.Report != null) {
				writer = new StreamWriter (this.Report.ToString ());
			}

			foreach (var f in frameworks) {
				if (writer != null) {
					writer.WriteLine ();
					writer.WriteLine ("Framework " + f.name);
					writer.WriteLine ();
				}
				foreach (var e in f.GetEntities()) {
					this.Log (Level.Verbose, String.Format ("Processing '{0}'...", e.name));

					String path = e.GetPath (baseFolder, DocumentType.DocSet);
					if (String.IsNullOrEmpty (path)) {
						continue;
					}

					if (e.type == FrameworkEntityType.C) {
						String content = File.ReadAllText (path);

						Match m = PATTERN_DELEGATE.Match (content);
						if (!m.Success) {
							continue;
						}

						String name = e.name + "Delegate";
						FrameworkEntity delegateEntity = entities.FirstOrDefault (v => v.name == name); 
						if (delegateEntity == null) {
							continue;
						}

						if (writer != null) {
							writer.WriteLine ();
							writer.WriteLine ("Entity " + delegateEntity.name);
							writer.WriteLine ();
							writer.WriteLine ("<Patch type=\"Model\">");
							writer.WriteLine ("<Change><![CDATA[DelegateFor=\"{0}\"]]></Change>", e.name);
							writer.WriteLine ("<Change><![CDATA[DelegateProperty=\"{0}\"]]></Change>", "Delegate");
							writer.WriteLine ("</Patch>");
						}
					}
					if (e.type == FrameworkEntityType.P && e.name.EndsWith("Delegate")) {
						String name = e.name.Replace("Delegate", String.Empty);
						FrameworkEntity classEntity = entities.FirstOrDefault (v => v.name == name); 
						if (classEntity == null) {
							continue;
						}

						if (writer != null) {
							writer.WriteLine ();
							writer.WriteLine ("Entity " + e.name);
							writer.WriteLine ();
							writer.WriteLine ("<Patch type=\"Model\">");
							writer.WriteLine ("<Change><![CDATA[DelegateFor=\"{0}\"]]></Change>", classEntity.name);
							writer.WriteLine ("<Change><![CDATA[DelegateProperty=\"{0}\"]]></Change>", "Delegate");
							writer.WriteLine ("</Patch>");
						}
					}
				}
			}

			if (this.Report != null) {
				writer.Close ();
				writer.Dispose ();
			}
		}
	}
}
