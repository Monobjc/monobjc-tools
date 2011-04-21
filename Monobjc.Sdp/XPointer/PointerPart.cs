#region using

using System;
using System.Xml;
using System.Xml.XPath;

#endregion

namespace Mvp.Xml.XPointer
{
	/// <summary>
	/// Part of SchemaBased <see cref="XPointer"/> pointer.
	/// </summary>
	internal abstract class PointerPart
	{

		#region public methods

		/// <summary>
		/// Evaluates <see cref="XPointer"/> pointer part and returns pointed nodes.
		/// </summary>
		/// <param name="doc">Document to evaluate pointer part on</param>
		/// <param name="nm">Namespace manager</param>
		/// <returns>Pointed nodes</returns>		    
		public abstract XPathNodeIterator Evaluate(XPathNavigator doc, XmlNamespaceManager nm);

		#endregion
	}
}
