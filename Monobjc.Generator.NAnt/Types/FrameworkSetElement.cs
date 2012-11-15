using System;
using System.Collections.Generic;
using NAnt.Core;
using NAnt.Core.Attributes;
using Monobjc.Tools.Generator.Model;

namespace Monobjc.Tools.Generator.NAnt
{
	[Serializable]
	[ElementName ("framework-set")]
	public class FrameworkSetElement : DataTypeBase
	{
		public FrameworkSetElement () : base()
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

		[BuildElementArray ("framework")]
		public FrameworkElement[] Frameworks {
			get;
			set;
		}
		
		[BuildElementArray ("framework-set")]
		public FrameworkSetElement[] FrameworkSets {
			get;
			set;
		}

		internal bool Enabled {
			get {
				return this.IfDefined && !this.UnlessDefined;
			}
		}

		internal IEnumerable<Framework> CreateFrameworks ()
		{
			List<Framework> frameworks = new List<Framework>();
			foreach (var fs in this.FrameworkSets) {
				frameworks.AddRange(fs.CreateFrameworks());
			}
			foreach (var f in this.Frameworks) {
				frameworks.Add(Framework.LoadFromFile (f.Descriptor.ToString ()));
			}
			return frameworks;
		}
	}
}
