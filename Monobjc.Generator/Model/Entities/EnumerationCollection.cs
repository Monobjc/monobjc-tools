using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	[Serializable]
	[XmlRoot("Enumerations")]
	public class EnumerationCollection : BaseCollection<EnumerationEntity>
	{
		public EnumerationCollection ()
		{
		}
	}
}
