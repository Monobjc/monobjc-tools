using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	[Serializable]
	[XmlRoot("Methods")]
	public class MethodCollection : BaseCollection<MethodEntity>
	{
		public MethodCollection ()
		{
		}
	}
}
