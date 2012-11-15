using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace Monobjc.Tools.Generator.Model
{
	[Serializable]
	[XmlRoot("Constants")]
	public class ConstantCollection : BaseCollection<ConstantEntity>
	{
		public ConstantCollection ()
		{
		}
	}
}
