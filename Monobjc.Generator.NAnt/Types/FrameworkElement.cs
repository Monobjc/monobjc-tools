using System;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.NAnt
{
	[ElementName ("framework")]
	[Serializable]
	public class FrameworkElement : DataTypeBase
	{
		public FrameworkElement () : base()
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

		[StringValidator (AllowEmpty = false), TaskAttribute ("descriptor", Required = true)]
		public FileInfo Descriptor {
			get;
			set;
		}

		internal bool Enabled {
			get {
				return this.IfDefined && !this.UnlessDefined;
			}
		}

		internal Framework Framework {
			get {
				return Framework.LoadFromFile (this.Descriptor.ToString ());
			}
		}
	}
}
