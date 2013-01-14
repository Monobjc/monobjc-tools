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
	[TaskName("find-deprecation")]
	public class FindDeprecationTask : BaseTask
	{
		private static readonly Regex PATTERN_DEPRECATED = new Regex("Deprecated");
		private static readonly Regex PATTERN_URL = new Regex("href=\\\"(.*DeprecationAppendix/AppendixADeprecatedAPI\\.html#)");

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

					// Skip deprecated entities
					if (e.name.EndsWith(".Deprecated")) {
						continue;
					}

					// Skip existing entities
					String name = e.name + ".Deprecated";
					if (entities.Any(v => v.name == name)) {
						continue;
					}

					String path = e.GetPath (baseFolder, DocumentType.DocSet);
					if (String.IsNullOrEmpty(path)) {
						continue;
					}

					String content = File.ReadAllText (path);

					Match m = PATTERN_DEPRECATED.Match(content);
					if (!m.Success) {
						continue;
					}

					m = PATTERN_URL.Match(content);
					if (!m.Success) {
						continue;
					}

					String url = m.Groups[1].Value;
					url = url.Substring(0, url.IndexOf("#"));

					String dir = Path.GetDirectoryName(path);
					String deprecatedPath = Path.Combine(dir, url);
					deprecatedPath = deprecatedPath.Substring(docSet.AbsolutePath.Length + 1);

					if (writer != null) {
						switch (e.type) {
						case FrameworkEntityType.T:
							writer.WriteLine ("<Type name=\"{0}.Deprecated\">", e.name);
							writer.WriteLine ("<File>{0}</File>", deprecatedPath);
							writer.WriteLine ("</Type>");
							writer.WriteLine ();
							break;
						case FrameworkEntityType.C:
							writer.WriteLine ("<Class name=\"{0}.Deprecated\">", e.name);
							writer.WriteLine ("<File>{0}</File>", deprecatedPath);
							writer.WriteLine ("<Patch type=\"Model\">");
							writer.WriteLine ("<Change><![CDATA[AdditionFor=\"{0}\"]]></Change>", e.name);
							writer.WriteLine ("</Patch>");
							writer.WriteLine ("</Class>");
							writer.WriteLine ();
							break;
						case FrameworkEntityType.P:
							writer.WriteLine ("<Protocol name=\"{0}.Deprecated\">", e.name);
							writer.WriteLine ("<File>{0}</File>", deprecatedPath);
							writer.WriteLine ("<Patch type=\"Model\">");
							writer.WriteLine ("<Change><![CDATA[AdditionFor=\"{0}\"]]></Change>", e.name);
							writer.WriteLine ("</Patch>");
							writer.WriteLine ("</Protocol>");
							writer.WriteLine ();
							break;
						default:
							break;
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
