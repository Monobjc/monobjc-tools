using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Parsers.Sgml;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("html-to-xhtml")]
	public class Html2XhtmlTask : BaseTask
	{
		/// <summary>
		/// Executes the task.
		/// </summary>
		protected override void ExecuteTask ()
		{
			String baseFolder = this.CreateBaseDir ();
			DocSet docSet = this.CreateDocSet ();
			IEnumerable<Framework> frameworks = this.CreateFrameworks (docSet);

			foreach (var f in frameworks) {
				foreach (var e in f.GetEntities()) {
					this.Log (Level.Verbose, String.Format ("Processing '{0}'...", e.name));
					
					String sourcePath = e.GetPath (baseFolder, DocumentType.Html);
					String destinationPath = e.GetPath (baseFolder, DocumentType.Xhtml);

					if (sourcePath == null || !File.Exists(sourcePath)) {
						continue;
					}

					if (sourcePath.IsYoungerThan (destinationPath)) {
						this.Log (Level.Info, String.Format ("Converting '{0}'...", e.name));
						Convert (sourcePath, destinationPath);
					} else {
						this.Log (Level.Debug, String.Format ("Skipping '{0}'...", e.name));
					}
				}
			}
		}

		internal static void Convert (String htmlFile, String xhtmlFile)
		{
			using (SgmlReader reader = new SgmlReader()) {
				reader.DocType = "HTML";
				reader.WhitespaceHandling = WhitespaceHandling.None;
				
				using (StreamReader r = new StreamReader(htmlFile)) {
					reader.InputStream = r;
					
					using (XmlTextWriter writer = new XmlTextWriter(xhtmlFile, Encoding.UTF8)) {
						writer.Formatting = Formatting.Indented;
						
						reader.Read ();
						while (!reader.EOF) {
							writer.WriteNode (reader, true);
						}
					}
				}
			}
		}
	}
}
