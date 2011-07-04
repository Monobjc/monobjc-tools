#region using

using System;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Globalization;

#endregion using

namespace Mvp.Xml.Common.XPath
{
	/// <summary>
	/// An <see cref="XPathNodeIterator"/> that allows 
	/// arbitrary addition of the <see cref="XPathNavigator"/> 
	/// nodes that belong to the set.
	/// </summary>
	/// <remarks>
	/// <para>Author: Daniel Cazzulino, <a href="http://clariusconsulting.net/kzu">blog</a></para>
	/// <para>Contributors: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
	/// </remarks>
	public class XPathNavigatorIterator : XPathNodeIterator
	{
		#region Fields & Ctors

		private List<XPathNavigator> _navigators;
		private int _position = -1;

		/// <summary>
		/// Initializes a new instance of the <see cref="XPathNavigatorIterator"/>.
		/// </summary>
		public XPathNavigatorIterator()
		{
			_navigators = new List<XPathNavigator>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XPathNavigatorIterator"/>
		/// with given initial capacity.
		/// </summary>
		public XPathNavigatorIterator(int capacity)
		{
			_navigators = new List<XPathNavigator>(capacity);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XPathNavigatorIterator"/>, 
		/// using the received navigator as the initial item in the set. 
		/// </summary>
		public XPathNavigatorIterator(XPathNavigator navigator)
			: this()
		{
			_navigators.Add(navigator);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XPathNavigatorIterator"/>
		/// using given list of navigators.
		/// </summary>
		public XPathNavigatorIterator(XPathNodeIterator iterator)
			: this(iterator, false) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="XPathNavigatorIterator"/>
		/// using given list of navigators.
		/// </summary>
		public XPathNavigatorIterator(XPathNodeIterator iterator, bool removeDuplicates)
			: this()
		{
			XPathNodeIterator it = iterator.Clone();

			while (it.MoveNext())
			{
				if (removeDuplicates)
				{
					if (this.Contains(it.Current))
					{
						continue;
					}
				}

				this.Add(it.Current.Clone());
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XPathNavigatorIterator"/>
		/// using given list of navigators.
		/// </summary>
		public XPathNavigatorIterator(List<XPathNavigator> navigators)
		{
			_navigators = navigators;
		}

		#endregion Fields & Ctors

		#region Public Methods

		/// <summary>
		/// Adds a <see cref="XPathNavigator"/> to the set.
		/// </summary>
		/// <param name="navigator">The navigator to add. It's cloned automatically.</param>
		public void Add(XPathNavigator navigator)
		{
			if (_position != -1)
				throw new InvalidOperationException(Monobjc.Tools.Sdp.Properties.Resources.XPathNavigatorIterator_CantAddAfterMove);

			_navigators.Add(navigator.Clone());
		}

		/// <summary>
		/// Adds a <see cref="XPathNodeIterator"/> containing a set of navigators to add.
		/// </summary>
		/// <param name="iterator">The set of navigators to add. Each one is cloned automatically.</param>
		public void Add(XPathNodeIterator iterator)
		{
			if (_position != -1)
				throw new InvalidOperationException(
					Monobjc.Tools.Sdp.Properties.Resources.XPathNavigatorIterator_CantAddAfterMove);

			while (iterator.MoveNext())
			{
				_navigators.Add(iterator.Current.Clone());
			}
		}

		/// <summary>
		/// Adds a <see cref="IEnumerable&lt;XPathNavigator&gt;"/> containing a set of navigators to add.
		/// </summary>
		/// <param name="navigators">The set of navigators to add. Each one is cloned automatically.</param>
		public void Add(IEnumerable<XPathNavigator> navigators)
		{
			if (_position != -1)
				throw new InvalidOperationException(
					Monobjc.Tools.Sdp.Properties.Resources.XPathNavigatorIterator_CantAddAfterMove);

			foreach (XPathNavigator navigator in navigators)
			{
				_navigators.Add(navigator.Clone());
			}
		}

		/// <summary>
		/// Determines whether the list contains a navigator positioned at the same 
		/// location as the specified XPathNavigator. This 
		/// method relies on the IsSamePositon() method of the XPathNavightor. 
		/// </summary>
		/// <param name="value">The object to locate in the list.</param>
		/// <returns>true if the object is found in the list; otherwise, false.</returns>
		public bool Contains(XPathNavigator value)
		{
			foreach (XPathNavigator nav in _navigators)
			{
				if (nav.IsSamePosition(value))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Determines whether the list contains a navigator whose Value property matches
		/// the target value
		/// </summary>
		/// <param name="value">The value to locate in the list.</param>
		/// <returns>true if the value is found in the list; otherwise, false.</returns>
		public bool ContainsValue(string value)
		{

			foreach (XPathNavigator nav in _navigators)
			{
				if (nav.Value.Equals(value))
				{
					return true;
				}
			}
			return false;
		}


		/// <summary>
		/// Gets or sets the element at the specified index
		/// </summary>
		public XPathNavigator this[int index]
		{
			get { return _navigators[index]; }
			set { _navigators[index] = value; }
		}

		/// <summary>
		/// Removes the list item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		public void RemoveAt(int index)
		{
			_navigators.RemoveAt(index);
		}


		/// <summary>
		/// Resets the iterator.
		/// </summary>
		public void Reset()
		{
			_position = -1;
		}

		#endregion Public Methods

		#region XPathNodeIterator Overrides

		/// <summary>
		/// See <see cref="XPathNodeIterator.Clone"/>.
		/// </summary>
		public override XPathNodeIterator Clone()
		{
			return new XPathNavigatorIterator(
				new List<XPathNavigator>(_navigators));
		}

		/// <summary>
		/// See <see cref="XPathNodeIterator.Count"/>.
		/// </summary>
		public override int Count
		{
			get { return _navigators.Count; }
		}

		/// <summary>
		/// See <see cref="XPathNodeIterator.Current"/>.
		/// </summary>
		public override XPathNavigator Current
		{
			get { return _position == -1 ? null : _navigators[_position]; }
		}

		/// <summary>
		/// See <see cref="XPathNodeIterator.CurrentPosition"/>.
		/// </summary>
		public override int CurrentPosition
		{
			get { return _position + 1; }
		}

		/// <summary>
		/// See <see cref="XPathNodeIterator.MoveNext"/>.
		/// </summary>
		public override bool MoveNext()
		{
			if (_navigators.Count == 0) return false;

			_position++;
			if (_position < _navigators.Count) return true;

			return false;
		}

		#endregion XPathNodeIterator Overrides
	}
}
