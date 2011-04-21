#region using

using System;
using System.Xml;
using System.Xml.XPath;

#endregion using

namespace Mvp.Xml.Common.XPath
{
	/// <summary>	
	/// Allows to navigate a subtree of an <see cref="IXPathNavigable"/> source, 
	/// by limiting the scope of the navigator to that received 
	/// at construction time.
	/// </summary>
	/// <remarks>Author: Daniel Cazzulino, <a href="http://clariusconsulting.net/kzu">blog</a>
	/// <para>See http://weblogs.asp.net/cazzu/archive/2004/06/24/164243.aspx</para>
	/// </remarks>
	public class SubtreeXPathNavigator : XPathNavigator
	{
		#region Fields & Ctors

		// The current location.
		XPathNavigator _navigator;
		// The node that limits the scope.
		XPathNavigator _root;
		// Whether we're at the root node (parent of the first child).
		bool _atroot = true;
		// Whether XML fragment navigation is enabled.
		bool _fragment;

		/// <summary>
		/// Creates SubtreeXPathNavigator over specified XPathNavigator.
		/// </summary>
		/// <param name="navigator">Navigator that determines scope.</param>
		/// <remarks>The incoming navigator is cloned upon construction, 
		/// which isolates the calling code from movements to the 
		/// <see cref="SubtreeXPathNavigator"/>.</remarks>
		public SubtreeXPathNavigator(XPathNavigator navigator)
			: this(navigator, false)
		{
		}

		/// <summary>
		/// Creates SubtreeXPathNavigator over specified XPathNavigator.
		/// </summary>
		/// <param name="navigator">Navigator that determines scope.</param>
		/// <param name="enableFragment">Whether the navigator should be able to 
		/// move among all siblings of the <paramref name="navigator"/> defining the 
		/// scope.</param>
		/// <remarks>The incoming navigator is cloned upon construction, 
		/// which isolates the calling code from movements to the 
		/// <see cref="SubtreeXPathNavigator"/>.</remarks>
		public SubtreeXPathNavigator(XPathNavigator navigator, bool enableFragment)
		{
			_navigator = navigator.Clone();
			_root = navigator.Clone();
			_fragment = enableFragment;
		}

		private SubtreeXPathNavigator(XPathNavigator root, XPathNavigator current,
			bool atRoot, bool enableFragment)
		{
			_root = root.Clone();
			_navigator = current.Clone();
			_atroot = atRoot;
			_fragment = enableFragment;
		}

		#endregion Fields & Ctors

		#region Private Members

		/// <summary>
		/// Determines whether the navigator is on the root node (before the first child).
		/// </summary>
		private bool AtRoot
		{
			get { return _atroot; }
		}

		/// <summary>
		/// Determines whether the navigator is at the same position as the "document element".
		/// </summary>
		private bool IsTop
		{
			get { return _navigator.IsSamePosition(_root); }
		}

		#endregion Private Members

		#region XPathNavigator Overrides

		#region Properties

		/// <summary>
		/// See <see cref="XPathNavigator.BaseURI"/>.
		/// </summary>
		public override String BaseURI
		{
			get { return AtRoot ? String.Empty : _navigator.BaseURI; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.HasAttributes"/>.
		/// </summary>
		public override bool HasAttributes
		{
			get { return AtRoot ? false : _navigator.HasAttributes; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.HasChildren"/>.
		/// </summary>
		public override bool HasChildren
		{
			get { return AtRoot ? true : _navigator.HasChildren; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.IsEmptyElement"/>.
		/// </summary>
		public override bool IsEmptyElement
		{
			get { return AtRoot ? false : _navigator.IsEmptyElement; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.LocalName"/>.
		/// </summary>
		public override string LocalName
		{
			get { return AtRoot ? String.Empty : _navigator.LocalName; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.Name"/>.
		/// </summary>
		public override string Name
		{
			get { return AtRoot ? String.Empty : _navigator.Name; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.NamespaceURI"/>.
		/// </summary>
		public override string NamespaceURI
		{
			get { return AtRoot ? String.Empty : _navigator.NamespaceURI; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.NameTable"/>.
		/// </summary>
		public override XmlNameTable NameTable
		{
			get { return _navigator.NameTable; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.NodeType"/>.
		/// </summary>
		public override XPathNodeType NodeType
		{
			get { return AtRoot ? XPathNodeType.Root : _navigator.NodeType; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.Prefix"/>.
		/// </summary>
		public override string Prefix
		{
			get { return AtRoot ? String.Empty : _navigator.Prefix; }
		}

		/// <summary>
		/// See <see cref="XPathItem.Value"/>.
		/// </summary>
		public override string Value
		{
			get { return AtRoot ? String.Empty : _navigator.Value; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.XmlLang"/>.
		/// </summary>
		public override string XmlLang
		{
			get { return AtRoot ? String.Empty : _navigator.XmlLang; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Creates new cloned version of the <see cref="SubtreeXPathNavigator"/>.
		/// </summary>
		/// <returns>Cloned copy of the <see cref="SubtreeXPathNavigator"/>.</returns>
		public override XPathNavigator Clone()
		{
			return new SubtreeXPathNavigator(_root, _navigator, _atroot, _fragment);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.IsSamePosition"/>.
		/// </summary>
		public override bool IsSamePosition(XPathNavigator other)
		{
			if (other == null || !(other is SubtreeXPathNavigator))
				return false;

			SubtreeXPathNavigator nav = (SubtreeXPathNavigator)other;
			return nav._atroot == this._atroot &&
				nav._navigator.IsSamePosition(this._navigator) &&
				nav._root.IsSamePosition(this._root);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToId"/>.
		/// </summary>
		public override bool MoveToId(string id)
		{
			return _navigator.MoveToId(id);
		}

		#region Element methods

		/// <summary>
		/// See <see cref="XPathNavigator.MoveTo"/>.
		/// </summary>
		public override bool MoveTo(XPathNavigator other)
		{
			if (other == null || !(other is SubtreeXPathNavigator))
				return false;

			return _navigator.MoveTo(((SubtreeXPathNavigator)other)._navigator);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToFirst"/>.
		/// </summary>
		public override bool MoveToFirst()
		{
			if (AtRoot) return false;
			if (IsTop)
			{
				if (!_fragment)
				{
					return false;
				}
				else if (_root.MoveToFirst())
				{
					_navigator.MoveToFirst();
					return true;
				}
			}

			return _navigator.MoveToNext();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToFirstChild"/>.
		/// </summary>
		public override bool MoveToFirstChild()
		{
			if (AtRoot)
			{
				_atroot = false;
				return true;
			}

			return _navigator.MoveToFirstChild();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToNext()"/>.
		/// </summary>
		public override bool MoveToNext()
		{
			if (AtRoot) return false;
			if (IsTop)
			{
				if (!_fragment)
				{
					return false;
				}
				else if (_root.MoveToNext())
				{
					_navigator.MoveToNext();
					return true;
				}
			}

			return _navigator.MoveToNext();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToParent"/>.
		/// </summary>
		public override bool MoveToParent()
		{
			if (AtRoot) return false;

			if (IsTop)
			{
				_atroot = true;
				return true;
			}

			return _navigator.MoveToParent();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToPrevious"/>.
		/// </summary>
		public override bool MoveToPrevious()
		{
			if (AtRoot) return false;
			if (IsTop)
			{
				if (!_fragment)
				{
					return false;
				}
				else if (_root.MoveToPrevious())
				{
					_navigator.MoveToPrevious();
					return true;
				}
			}

			return _navigator.MoveToPrevious();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToRoot"/>.
		/// </summary>
		public override void MoveToRoot()
		{
			_navigator = _root.Clone();
			_atroot = true;
		}

		#endregion Element methods

		#region Attribute methods

		/// <summary>
		/// See <see cref="XPathNavigator.GetAttribute"/>.
		/// </summary>
		public override string GetAttribute(string localName, string namespaceURI)
		{
			return AtRoot ? String.Empty : _navigator.GetAttribute(localName, namespaceURI);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToAttribute"/>.
		/// </summary>
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			return AtRoot ? false : _navigator.MoveToAttribute(localName, namespaceURI);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToFirstAttribute"/>.
		/// </summary>
		public override bool MoveToFirstAttribute()
		{
			return AtRoot ? false : _navigator.MoveToFirstAttribute();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToNextAttribute"/>.
		/// </summary>
		public override bool MoveToNextAttribute()
		{
			return AtRoot ? false : _navigator.MoveToNextAttribute();
		}

		#endregion Attribute methods

		#region Namespace methods

		/// <summary>
		/// See <see cref="XPathNavigator.GetNamespace"/>.
		/// </summary>
		public override string GetNamespace(string localName)
		{
			return AtRoot ? String.Empty : _navigator.GetNamespace(localName);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToNamespace"/>.
		/// </summary>
		public override bool MoveToNamespace(string @namespace)
		{
			return AtRoot ? false : _navigator.MoveToNamespace(@namespace);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToFirstNamespace(XPathNamespaceScope)"/>.
		/// </summary>
		public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{
			return AtRoot ? false : _navigator.MoveToFirstNamespace(namespaceScope);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToNextNamespace(XPathNamespaceScope)"/>.
		/// </summary>
		public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			return AtRoot ? false : _navigator.MoveToNextNamespace(namespaceScope);
		}

		#endregion Namespace methods

		#endregion Methods

		#endregion XPathNavigator Overrides
	}
}