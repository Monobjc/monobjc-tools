using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	[Serializable]
	[XmlRoot("Propertys")]
	public class PropertyCollection : BaseCollection<PropertyEntity>
	{
		public PropertyCollection ()
		{
		}
	}
}
