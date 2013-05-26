using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Tasks;
using Monobjc.Tools.Generator.Model;
using System.Linq;

namespace Monobjc.Tools.Generator.NAnt
{
	[TaskName("doxygen-exec")]
	public class DoxygenExecTask : BaseTask
	{
		[TaskAttribute("doxygen", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public FileInfo DoxygenBinary { get; set; }

		/// <summary>
		/// Executes the task.
		/// </summary>
		protected override void ExecuteTask ()
		{
			if (!this.DoxygenBinary.Exists) {
				this.Log (Level.Error, "Doxygen cannot be found.");
				return;
			}

			String baseFolder = this.CreateBaseDir ();
			DocSet docSet = this.CreateDocSet ();
			IEnumerable<Framework> frameworks = this.CreateFrameworks (docSet);

			foreach (var f in frameworks) {
				if (f.source != DocumentOrigin.Doxygen) {
					continue;
				}

				String configuration;
				if (docSet != null) {
					configuration = Path.Combine (baseFolder, docSet.Name, f.name, "doxygen.cfg");
				} else {
					configuration = Path.Combine (baseFolder, f.name, "doxygen.cfg");
				}

				if (File.Exists (configuration)) {
					continue;
				}

				this.Log (Level.Info, "Generating Doxygen files for '{0}'...", f.name);

				ExportConfiguration (configuration);

				ExecTask execTask = new ExecTask ();
				execTask.Project = this.Project;
				execTask.FailOnError = true;
				execTask.FileName = this.DoxygenBinary.ToString ();
				execTask.CommandLineArguments = Path.GetFileName (configuration);
				execTask.WorkingDirectory = new DirectoryInfo (Path.GetDirectoryName (configuration));

				execTask.Execute ();

				GenerateDescriptor (baseFolder, f);
			}
		}

		private static void ExportConfiguration (String path)
		{
			Stream inputStream = Assembly.GetExecutingAssembly ().GetManifestResourceStream ("Monobjc.Tools.Generator.NAnt.Resources.doxygen.cfg");
			if (inputStream == null) {
				throw new ArgumentException ("Cannot extract Doxygen configuration");
			}
			using (FileStream outputStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write)) {
				inputStream.CopyTo (outputStream);
			}
		}

		/// <summary>
		///   Generate an XML fragment ready to be included in the entities list of the generator.
		/// </summary>
		private void GenerateDescriptor (String baseFolder, Framework f)
		{
			this.Log (Level.Info, "Placing Doxygen files for '{0}'...", f.name);

			// Set file names
			String doxygenFolder = Path.Combine (baseFolder, f.name, DocumentType.Doxygen.ToString ());
			String indexPath = Path.Combine (doxygenFolder, "index.xml");
			String descriptorPath = Path.Combine (baseFolder, f.name, f.name + ".xml");

			// Deserialize Doxygen index
			DoxygenType index;
			XmlSerializer serializer = new XmlSerializer (typeof(DoxygenType));
			using (StreamReader reader = new StreamReader(indexPath)) {
				index = (DoxygenType)serializer.Deserialize (reader);
			}

			// Collect entities
			List<FrameworkType> types = new List<FrameworkType> ();
			List<FrameworkClass> classes = new List<FrameworkClass> ();
			List<FrameworkProtocol> protocols = new List<FrameworkProtocol> ();
			List<FrameworkStructure> structures = new List<FrameworkStructure> ();
			List<FrameworkEnumeration> enumerations = new List<FrameworkEnumeration> ();

			foreach (CompoundType compound in index.compound.Where(c => c.kind == CompoundKind.@class)) {
				String name = compound.name;
				String categoryName = name.Replace ('(', '_').Trim (')');
				int pos = name.IndexOf ("(");
				String className = (pos != -1) ? name.Substring (0, pos) : name;

				FrameworkClass entity = new FrameworkClass ();
				entity.Framework = f;
				entity.name = categoryName;
				if (className != categoryName) {
					Patch patch = new Patch ();
					patch.type = DocumentType.Model;
					patch.Change = new String[]{ "<![CDATA[AdditionFor=\"" + className + "\"]]>"};
					entity.Patch = new Patch[]{ patch};
				}
				classes.Add (entity);

				// Place the xml file in the right folder
				String inputFile = Path.Combine (doxygenFolder, compound.refid + ".xml");
				String outputFile = entity.GetPath (baseFolder, DocumentType.Xhtml);
				File.Copy (inputFile, outputFile, true);
			}

			foreach (CompoundType compound in index.compound.Where(c => c.kind == CompoundKind.category)) {
				String name = compound.name;
				String categoryName = name.Replace ('(', '_').Trim (')');
				String className = name.Substring (0, name.IndexOf ("("));

				FrameworkClass entity = new FrameworkClass ();
				entity.Framework = f;
				entity.name = categoryName;
				Patch patch = new Patch ();
				patch.type = DocumentType.Model;
                patch.Change = new String[]{ "<![CDATA[AdditionFor=\"" + className + "\"]]>"};
				entity.Patch = new Patch[]{ patch};
				classes.Add (entity);
				
				// Place the xml file in the right folder
				String inputFile = Path.Combine (doxygenFolder, compound.refid + ".xml");
				String outputFile = entity.GetPath (baseFolder, DocumentType.Xhtml);
				File.Copy (inputFile, outputFile, true);
			}

			foreach (CompoundType compound in index.compound.Where(c => c.kind == CompoundKind.file)) {
				String name = compound.name;
				if (!name.EndsWith (".h")) {
					continue;
				}
				name = name.Replace ("+", "_").Replace (".h", String.Empty);
				String className = name + "_Definitions";

				FrameworkClass entity = new FrameworkClass ();
				entity.Framework = f;
				entity.name = className;
				Patch patch = new Patch ();
				patch.type = DocumentType.Model;
                patch.Change = new String[]{ "<![CDATA[AdditionFor=\"" + className + "\"]]>"};
				entity.Patch = new Patch[]{ patch};
				classes.Add (entity);

				// Place the xml file in the right folder
				String inputFile = Path.Combine (doxygenFolder, compound.refid + ".xml");
				String outputFile = entity.GetPath (baseFolder, DocumentType.Xhtml);
				File.Copy (inputFile, outputFile, true);
			}

			foreach (CompoundType compound in index.compound.Where(c => c.kind == CompoundKind.protocol)) {
				String name = compound.name;
				if (name.EndsWith ("-p")) {
					name = name.Replace ("-p", String.Empty);
				}

				FrameworkProtocol entity = new FrameworkProtocol ();
				entity.Framework = f;
				entity.name = name;
				protocols.Add (entity);

				// Place the xml file in the right folder
				String inputFile = Path.Combine (doxygenFolder, compound.refid + ".xml");
				String outputFile = entity.GetPath (baseFolder, DocumentType.Xhtml);
				File.Copy (inputFile, outputFile, true);
			}

			foreach (CompoundType compound in index.compound.Where(c => c.kind == CompoundKind.@struct)) {
				String name = compound.name;
				name = name.TrimStart ('_');

				FrameworkStructure entity = new FrameworkStructure ();
				entity.Framework = f;
				entity.name = name;
				structures.Add (entity);

				// Place the xml file in the right folder
				String inputFile = Path.Combine (doxygenFolder, compound.refid + ".xml");
				String outputFile = entity.GetPath (baseFolder, DocumentType.Xhtml);
				File.Copy (inputFile, outputFile, true);
			}

			// Save the framework descriptor
			Framework framework = new Framework (f);
			framework.Types = types.ToArray ();
			framework.Classes = classes.ToArray ();
			framework.Protocols = protocols.ToArray ();
			framework.Structures = structures.ToArray ();
			framework.Enumerations = enumerations.ToArray ();
			framework.SaveToFile (descriptorPath);
		}
	}
}
