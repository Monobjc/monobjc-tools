using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;
using System.Collections.Specialized;
using Monobjc.Tools.Generator.Parsers.Xhtml;
using Monobjc.Tools.Generator.Parsers.Xhtml.Cocoa;
using Monobjc.Tools.Generator.Parsers.Xhtml.Classic;
using Monobjc.Tools.Generator.Parsers.Xhtml.Doxygen;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("xhtml-to-model")]
	public class Xhtml2ModelTask : BaseTask
	{
		private TypeManager typeManager;
		private NameValueCollection settings;

		/// <summary>
		/// Gets or sets the mapping file.
		/// </summary>
		[TaskAttribute("mappings", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public FileInfo MappingFile { get; set; }

		/// <summary>
		/// Gets or sets the method names that aren't getter.
		/// </summary>
		[TaskAttribute("methods", Required = false)]
		[StringValidator(AllowEmpty = false)]
		public String NoGetterMethods { get; set; }

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

			this.typeManager = new TypeManager ();
			this.typeManager.SetMappings (this.MappingFile.ToString ());
			Func<FrameworkEntity, bool> selector = delegate(FrameworkEntity e) {
				return e.type == FrameworkEntityType.C || e.type == FrameworkEntityType.P;
			};
			var names = frameworks.SelectMany (f => f.GetEntities ().Where (selector).Select (e => e.name));
			this.typeManager.SetClasses (names);

			// Prepare the settings
			this.settings = this.CreateSettings ();

			TextWriter writer = null;
			if (this.Report != null) {
				writer = new StreamWriter (this.Report.ToString ());
			}

			foreach (var f in frameworks) {
				if (writer != null) {
					writer.WriteLine ("Framework " + f.name);
				}
				foreach (var e in f.GetEntities()) {
					if (writer != null) {
						writer.WriteLine ("Entity " + e.name);
					}
					this.Log (Level.Verbose, String.Format ("Processing '{0}'...", e.name));
					
					String sourcePath = e.GetPath (baseFolder, DocumentType.Xhtml);
					String destinationPath = e.GetPath (baseFolder, DocumentType.Model);
					
					if (sourcePath == null || !File.Exists (sourcePath)) {
						continue;
					}

					if (sourcePath.IsYoungerThan (destinationPath)) {
						this.Log (Level.Info, String.Format ("Converting '{0}'...", e.name));
						Convert (f, e, sourcePath, destinationPath, writer);
					} else {
						this.Log (Level.Debug, String.Format ("Skipping '{0}'...", e.name));
					}
				}
			}

			if (this.Report != null) {
				writer.Close ();
				writer.Dispose ();
			}
		}

		private NameValueCollection CreateSettings ()
		{
			NameValueCollection settings = new NameValueCollection ();
			settings.Set ("NotGetterMethods", this.NoGetterMethods ?? "");
			return settings;
		}

		private void Convert (Framework f, FrameworkEntity e, String sourcePath, String destinationPath, TextWriter writer = null)
		{
			switch (e.type) {
			case FrameworkEntityType.T:
				{
					IXhtmlTypeParser parser = this.GetTypeParser (f, e, writer);

					TypedEntity entity = new TypedEntity ();
					entity.Namespace = f.name;
					entity.Name = e.name;
					entity.Nature = e.type;

					parser.Parse (entity, sourcePath);
					entity.SaveTo (destinationPath);
				}
				break;
			case FrameworkEntityType.C:
				{
					IXhtmlClassParser parser = this.GetClassParser (f, e, writer);
				
					ClassEntity entity = new ClassEntity ();
					entity.Namespace = f.name;
					entity.Name = e.name;
					entity.Nature = e.type;

					parser.Parse (entity, sourcePath);
					entity.SaveTo (destinationPath);
				}
				break;
			case FrameworkEntityType.P:
				{
					IXhtmlClassParser parser = this.GetProtocolParser (f, e, writer);
				
					ProtocolEntity entity = new ProtocolEntity ();
					entity.Namespace = f.name;
					entity.Name = e.name;
					entity.Nature = e.type;

					parser.Parse (entity, sourcePath);
					entity.SaveTo (destinationPath);
				}
				break;
			default:
				break;
			}
		}

		private IXhtmlTypeParser GetTypeParser (Framework f, FrameworkEntity e, TextWriter writer)
		{
			switch (f.style) {
			case PageStyle.Cocoa:
				return new XhtmlCocoaTypeParser (this.settings, this.typeManager, writer);
			case PageStyle.Classic:
				return new XhtmlClassicTypeParser (this.settings, this.typeManager, writer);
			case PageStyle.Doxygen:
				return new XhtmlDoxygenTypeParser (this.settings, this.typeManager, writer);
			default:
				throw new NotSupportedException ("No style defined for " + f.name);
			}
		}
		
		private IXhtmlClassParser GetClassParser (Framework f, FrameworkEntity e, TextWriter writer)
		{
			switch (f.style) {
			case PageStyle.Cocoa:
				return new XhtmlCocoaClassParser (this.settings, this.typeManager, writer);
			case PageStyle.Classic:
				return new XhtmlClassicClassParser (this.settings, this.typeManager, writer);
			case PageStyle.Doxygen:
				return new XhtmlDoxygenClassParser (this.settings, this.typeManager, writer);
			default:
				throw new NotSupportedException ("No style defined for " + f.name);
			}
		}
		
		private IXhtmlClassParser GetProtocolParser (Framework f, FrameworkEntity e, TextWriter writer)
		{
			switch (f.style) {
			case PageStyle.Cocoa:
				return new XhtmlCocoaClassParser (this.settings, this.typeManager, writer);
			case PageStyle.Classic:
				return new XhtmlClassicClassParser (this.settings, this.typeManager, writer);
			case PageStyle.Doxygen:
				return new XhtmlDoxygenClassParser (this.settings, this.typeManager, writer);
			default:
				throw new NotSupportedException ("No style defined for " + f.name);
			}
		}
	}
}
