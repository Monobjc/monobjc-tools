#region using

using System;
using System.Xml;

#endregion using

namespace Mvp.Xml.Common
{
	/// <summary>
	/// Represents a mapping between a prefix and a namespace.
	/// </summary>
	public class XmlPrefix
	{
		/// <summary>
		/// Creates the prefix mapping.
		/// </summary>
		/// <param name="prefix">Prefix associated with the namespace.</param>
		/// <param name="ns">Namespace to associate with the prefix.</param>
		public XmlPrefix(string prefix, string ns)
		{
			_prefix = prefix;
			_ns = ns;
		}

		/// <summary>
		/// Creates the prefix mapping, using atomized strings from the 
		/// <paramref name="nameTable"/> for faster lookups and comparisons.
		/// </summary>
		/// <param name="prefix">Prefix associated with the namespace.</param>
		/// <param name="ns">Namespace to associate with the prefix.</param>
		/// <param name="nameTable">The name table to use to atomize strings.</param>
		/// <remarks>
		/// This is the recommended way to construct this class, as it uses the 
		/// best approach to handling strings in XML.
		/// </remarks>
		public XmlPrefix(string prefix, string ns, XmlNameTable nameTable)
		{
			_prefix = nameTable.Add(prefix);
			_ns = nameTable.Add(ns);
		}

		/// <summary>
		/// Gets the prefix associated with the <see cref="NamespaceURI"/>.
		/// </summary>
		public string Prefix
		{
			get { return _prefix; }
		} string _prefix;

		/// <summary>
		/// Gets the namespace associated with the <see cref="Prefix"/>.
		/// </summary>
		public string NamespaceURI
		{
			get { return _ns; }
		} string _ns;
	}
}
