using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	[Serializable]
	[XmlRoot("EnumerationValues")]
	public class EnumerationValueCollection : BaseCollection<EnumerationValueEntity>
	{
		public EnumerationValueCollection ()
		{
		}
	}
}
