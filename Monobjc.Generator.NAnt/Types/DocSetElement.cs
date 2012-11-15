using System;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.NAnt
{
	[Serializable]
	[ElementName ("docset")]
	public class DocSetElement : DataTypeBase
	{
		public DocSetElement () : base()
		{
			this.IfDefined = true;
		}

		[BooleanValidator, TaskAttribute ("if", Required = false)]
		public bool IfDefined {
			get;
			set;
		}
		
		[BooleanValidator, TaskAttribute ("unless", Required = false)]
		public bool UnlessDefined {
			get;
			set;
		}
		
		[StringValidator (AllowEmpty = false), TaskAttribute ("name", Required = true)]
		public String DocSetName {
			get;
			set;
		}

		[StringValidator (AllowEmpty = false), TaskAttribute ("path", Required = true)]
		public String DocSetPath {
			get;
			set;
		}

		internal bool Enabled {
			get {
				return this.IfDefined && !this.UnlessDefined;
			}
		}

		internal DocSet CreateDocSet ()
		{
			return new DocSet (this.DocSetName ?? this.ID, this.DocSetPath);
		}
	}
}
