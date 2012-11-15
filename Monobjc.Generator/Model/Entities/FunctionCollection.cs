using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	[Serializable]
	[XmlRoot("Functions")]
	public class FunctionCollection : BaseCollection<FunctionEntity>
	{
		public FunctionCollection ()
		{
		}
	}
}
