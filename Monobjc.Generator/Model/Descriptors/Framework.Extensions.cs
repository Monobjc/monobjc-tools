using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Monobjc.Tools.Generator.Model
{
	public partial class Framework
	{
		public Framework ()
		{
			this.style = PageStyle.Cocoa;
		}

		public Framework (Framework f)
		{
			this.assembly = f.assembly;
			this.name = f.name;
			this.path = f.path;
			this.source = f.source;
			this.usings = f.usings;
		}

		[XmlIgnore]
		public DocSet DocSet { get; set; }

		public String[] UsingList {
			get {
				if (this.usings == null) {
					return new String[0];
				}
				return this.usings.Split (new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
			}
		}

		public void SaveToFile (String path)
		{
			XmlSerializer serializer = new XmlSerializer (typeof(Framework));
			using (StreamWriter writer = new StreamWriter(path)) {
				serializer.Serialize (writer, this);
			}
		}

		public static Framework LoadFromFile (String path)
		{
			XmlSerializer serializer = new XmlSerializer (typeof(Framework));
			using (StreamReader reader = new StreamReader(path)) {
				return (Framework)serializer.Deserialize (reader);
			}
		}

		public String GetPath (String baseFolder, DocumentType documentType)
		{
			String path = null;
			DocSet docSet = this.DocSet;
			
			switch (documentType) {
			case DocumentType.Generated:
				path = Path.Combine (baseFolder, documentType.ToString (), this.assembly);
				Directory.CreateDirectory (Path.GetDirectoryName (path));
				break;
			default:
				break;
			}
			
			return path;
		}

		public override string ToString ()
		{
			return string.Format ("[Framework: name={0}, usings={1}, assembly={2}, source={3}]", name, usings, assembly, source);
		}
	}

	public partial class FrameworkEntity
	{
		public FrameworkEntity ()
		{
		}

		[XmlIgnore]
		public Framework Framework { get; set; }

		public String GetPath (String baseFolder, DocumentType documentType)
		{
			if (this.Framework == null) {
				throw new ArgumentException ("Entity should be part of a framework");
			}

			String path = null;
			Framework framework = this.Framework;
			DocSet docSet = framework.DocSet;

			switch (documentType) {
			case DocumentType.DocSet:
				if (docSet != null && this.File != null) {
					path = Path.Combine (docSet.AbsolutePath, this.File);
				}
				break;
			case DocumentType.Generated:
				path = Path.Combine (baseFolder, documentType.ToString (), framework.assembly, framework.name + "_" + this.type.ToString (), this.name);
				Directory.CreateDirectory (Path.GetDirectoryName (path));
				break;
			default:
				if (docSet != null) {
					path = Path.Combine (baseFolder, docSet.Name, framework.name, documentType.ToString (), this.type.ToString (), this.name);
				} else {
					path = Path.Combine (baseFolder, framework.name, documentType.ToString (), this.type.ToString (), this.name);
				}
				Directory.CreateDirectory (Path.GetDirectoryName (path));
				break;
			}

			String extension = extensions[documentType];
			if (!String.IsNullOrEmpty (extension)) {
				path += extension;
			}

			return path;
		}

		private static readonly Dictionary<DocumentType, String> extensions = new Dictionary<DocumentType, String>() {
			{ DocumentType.DocSet, String.Empty },
			{ DocumentType.Doxygen, String.Empty },
			{ DocumentType.Generated, String.Empty },
			{ DocumentType.Html, ".html" },
			{ DocumentType.Model, ".xml" },
			{ DocumentType.None, String.Empty },
			{ DocumentType.Source, String.Empty },
			{ DocumentType.Xhtml, ".xhtml" },
		};
	}

	public partial class FrameworkType
	{
		public FrameworkType ()
		{
			this.type = FrameworkEntityType.T;
		}

		public override string ToString ()
		{
			return string.Format ("[FrameworkType: type={0}, name={1}, File={2}]", type, name, File);
		}
	}
	
	public partial class FrameworkClass
	{
		public FrameworkClass ()
		{
			this.type = FrameworkEntityType.C;
		}

		public override string ToString ()
		{
			return string.Format ("[FrameworkClass: type={0}, name={1}, File={2}]", type, name, File);
		}
	}
	
	public partial class FrameworkProtocol
	{
		public FrameworkProtocol ()
		{
			this.type = FrameworkEntityType.P;
		}

		public override string ToString ()
		{
			return string.Format ("[FrameworkProtocol: type={0}, name={1}, File={2}]", type, name, File);
		}
	}
	
	public partial class FrameworkStructure
	{
		public FrameworkStructure ()
		{
			this.type = FrameworkEntityType.S;
		}

		public override string ToString ()
		{
			return string.Format ("[FrameworkStructure: type={0}, name={1}, File={2}]", type, name, File);
		}
	}

	public partial class FrameworkEnumeration
	{
		public FrameworkEnumeration ()
		{
			this.type = FrameworkEntityType.E;
		}

		public override string ToString ()
		{
			return string.Format ("[FrameworkEnumeration: type={0}, name={1}, File={2}]", type, name, File);
		}
	}
}
