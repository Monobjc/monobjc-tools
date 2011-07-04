#region using

using System;
using System.Xml.XPath;

#endregion

namespace Mvp.Xml.Common.XPath
{
	/// <summary>
	/// Empty <see cref="XPathNodeIterator"/>, used to represent empty
	/// node sequence. Can be used to return empty nodeset out of an XSLT or XPath extension function.
	/// Implemented as a singleton.
	/// </summary>
	/// <remarks>
	/// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
	/// </remarks>
	public class EmptyXPathNodeIterator : XPathNodeIterator
	{
		/// <summary>
		/// EmptyXPathNodeIterator instance.
		/// </summary>
		public static EmptyXPathNodeIterator Instance = new EmptyXPathNodeIterator();

		/// <summary>
		/// This is a singleton, get an instance instead.
		/// </summary>
		private EmptyXPathNodeIterator() { }

		#region XPathNodeIterator implementation
		/// <summary>
		/// See <see cref="XPathNodeIterator.Clone()"/>
		/// </summary>        
		public override XPathNodeIterator Clone()
		{
			return this;
		}

		/// <summary>
		/// Always 0. See <see cref="XPathNodeIterator.Count"/>
		/// </summary>
		public override int Count
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Always null. See <see cref="XPathNodeIterator.Current"/>
		/// </summary>
		public override XPathNavigator Current
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Always 0. See <see cref="XPathNodeIterator.CurrentPosition"/>
		/// </summary>
		public override int CurrentPosition
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Always false. See <see cref="XPathNodeIterator.MoveNext()"/>
		/// </summary>
		public override bool MoveNext()
		{
			return false;
		}

		#endregion
	}
}
