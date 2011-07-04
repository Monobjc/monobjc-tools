#region using

using System;
using System.Xml.XPath;

#endregion

namespace Mvp.Xml.Common.XPath
{
	/// <summary>
	/// <see cref="XPathNodeIterator"/> over a single node. Can be used to return a single
	/// node out of an XSLT or XPath extension function.
	/// </summary>
	/// <remarks>
	/// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
	/// </remarks>
	public class SingletonXPathNodeIterator : XPathNodeIterator
	{
		private XPathNavigator navigator;
		private int position;

		#region ctors
		/// <summary>
		/// Creates new instance of SingletonXPathNodeIterator over
		/// given node.
		/// </summary>
		public SingletonXPathNodeIterator(XPathNavigator nav)
		{
			this.navigator = nav;
		}

		#endregion

		#region XPathNodeIterator impl

		/// <summary>
		/// See <see cref="XPathNodeIterator.Clone()"/>
		/// </summary>
		public override XPathNodeIterator Clone()
		{
			return new SingletonXPathNodeIterator(navigator.Clone());
		}

		/// <summary>
		/// Always 1. See <see cref="XPathNodeIterator.Count"/>
		/// </summary>
		public override int Count
		{
			get
			{
				return 1;
			}
		}

		/// <summary>
		/// See <see cref="XPathNodeIterator.Current"/>
		/// </summary>
		public override XPathNavigator Current
		{
			get
			{
				return navigator;
			}
		}

		/// <summary>
		/// See <see cref="XPathNodeIterator.CurrentPosition"/>
		/// </summary>
		public override int CurrentPosition
		{
			get
			{
				return position;
			}
		}

		/// <summary>
		/// See <see cref="XPathNodeIterator.MoveNext()"/>
		/// </summary>
		public override bool MoveNext()
		{
			if (position == 0)
			{
				position = 1;
				return true;
			}
			return false;
		}
		#endregion

	}
}
