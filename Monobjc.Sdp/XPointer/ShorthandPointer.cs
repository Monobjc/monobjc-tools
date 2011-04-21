#region using

using System;
using System.Xml;
using System.Xml.XPath;

using Mvp.Xml.Common.XPath;
using System.Globalization;

#endregion

namespace Mvp.Xml.XPointer
{
	/// <summary>
	/// Shorthand XPointer pointer.
	/// </summary>
	internal class ShorthandPointer : Pointer
	{
		#region private members

		private string _NCName;

		#endregion

		#region constructors

		/// <summary>
		/// Creates shorthand XPointer given bare name.
		/// </summary>
		/// <param name="n">Shorthand (bare name)</param>
		public ShorthandPointer(string n)
		{
			_NCName = n;
		}

		#endregion

		#region Pointer overrides

		/// <summary>
		/// Evaluates <see cref="XPointer"/> pointer and returns 
		/// iterator over pointed nodes.
		/// </summary>
		/// <remarks>Note, that returned XPathNodeIterator is already moved once.</remarks>
		/// <param name="nav">XPathNavigator to evaluate the 
		/// <see cref="XPointer"/> on.</param>
		/// <returns><see cref="XPathNodeIterator"/> over pointed nodes</returns>	    					
		public override XPathNodeIterator Evaluate(XPathNavigator nav)
		{
			XPathNodeIterator result = XPathCache.Select("id('" + _NCName + "')", nav, (XmlNamespaceManager)null);
			if (result != null && result.MoveNext())
			{
				return result;
			}
			else
				throw new NoSubresourcesIdentifiedException(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.NoSubresourcesIdentifiedException, _NCName));
		}
		#endregion
	}
}
