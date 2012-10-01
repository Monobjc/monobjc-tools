using System;
using System.Collections.Generic;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("copy-documents")]
	public class CopyDocumentsTask : BaseTask
	{
		/// <summary>
		/// Gets or sets the document type of the source.
		/// </summary>
		[TaskAttribute("source-type", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public DocumentType SourceType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this task checks source.
		/// </summary>
		[TaskAttribute("check-source", Required = false)]
		public bool CheckSource { get; set; }
		
		/// <summary>
		/// Gets or sets the document type of the destination.
		/// </summary>
		[TaskAttribute("destination-type", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public DocumentType DestinationType { get; set; }

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

					String sourcePath = e.GetPath (baseFolder, this.SourceType);
					String destinationPath = e.GetPath (baseFolder, this.DestinationType);

					if (sourcePath == null) {
						continue;
					}

					if (this.CheckSource && !File.Exists(sourcePath)) {
						this.Log(Level.Error, "File does not exists at " + sourcePath);
					}

					if (sourcePath.IsYoungerThan (destinationPath)) {
						this.Log (Level.Info, String.Format ("Copying '{0}'...", e.name));
						File.Copy (sourcePath, destinationPath, true);
					} else {
						this.Log (Level.Debug, String.Format ("Skipping '{0}'...", e.name));
					}
				}
			}
		}
	}
}
