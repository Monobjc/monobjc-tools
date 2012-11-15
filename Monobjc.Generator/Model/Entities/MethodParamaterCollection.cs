using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	[Serializable]
	[XmlRoot("MethodParamaters")]
	public class MethodParameterCollection : BaseCollection<MethodParameterEntity>
	{
		public MethodParameterCollection ()
		{
		}
	}
}
