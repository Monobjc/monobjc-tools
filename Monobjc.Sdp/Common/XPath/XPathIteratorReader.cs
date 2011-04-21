#region using

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

#endregion using

namespace Mvp.Xml.Common.XPath
{
	/// <summary>
	/// Provides an <see cref="XmlReader"/> over an 
	/// <see cref="XPathNodeIterator"/>.
	/// </summary>
	/// <remarks>
	/// The reader exposes a new root element enclosing all navigators from the 
	/// iterator. This root node is configured in the constructor, by 
	/// passing the desired name and optional namespace for it.
	/// <para>Author: Daniel Cazzulino, <a href="http://clariusconsulting.net/kzu">blog</a></para>
	/// See: http://weblogs.asp.net/cazzu/archive/2004/04/26/120684.aspx
	/// </remarks>
	public class XPathIteratorReader : XmlTextReader, IXmlSerializable
	{
		#region Fields

		// Holds the current child being read.
		XmlReader _current;

		// Holds the iterator passed to the ctor. 
		XPathNodeIterator _iterator;

		// The name for the root element.
		XmlQualifiedName _rootname;

		#endregion Fields

		#region Ctor

		/// <summary>
		/// Parameterless constructor for XML serialization.
		/// </summary>
		/// <remarks>Supports the .NET serialization infrastructure. Don't use this 
		/// constructor in your regular application.</remarks>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public XPathIteratorReader()
		{
		}

		/// <summary>
		/// Initializes the reader, using the default &lt;root&gt; element.
		/// </summary>
		/// <param name="iterator">The iterator to expose as a single reader.</param>
		public XPathIteratorReader(XPathNodeIterator iterator)
			: this(iterator, "root", String.Empty)
		{
		}

		/// <summary>
		/// Initializes the reader.
		/// </summary>
		/// <param name="iterator">The iterator to expose as a single reader.</param>
		/// <param name="rootName">The name to use for the enclosing root element.</param>
		public XPathIteratorReader(XPathNodeIterator iterator, string rootName)
			: this(iterator, rootName, String.Empty)
		{
		}

		/// <summary>
		/// Initializes the reader.
		/// </summary>
		/// <param name="iterator">The iterator to expose as a single reader.</param>
		/// <param name="rootName">The name to use for the enclosing root element.</param>
		/// <param name="ns">The namespace URI of the root element.</param>
		public XPathIteratorReader(XPathNodeIterator iterator, string rootName, string ns)
			: base(new StringReader(String.Empty))
		{
			_iterator = iterator.Clone();
			_current = new FakedRootReader(rootName, ns, XmlNodeType.Element);
			_rootname = new XmlQualifiedName(rootName, ns);
		}

		#endregion Ctor

		#region Private Members

		/// <summary>
		/// Returns the XML representation of the current node and all its children.
		/// </summary>
		private string Serialize()
		{
			StringWriter sw = new StringWriter(System.Globalization.CultureInfo.CurrentCulture);
			XmlTextWriter tw = new XmlTextWriter(sw);
			tw.WriteNode(this, false);

			sw.Flush();
			return sw.ToString();
		}

		#endregion Private Members

		#region Properties

		/// <summary>See <see cref="XmlReader.AttributeCount"/></summary>
		public override int AttributeCount
		{
			get { return _current.AttributeCount; }
		}

		/// <summary>See <see cref="XmlReader.BaseURI"/></summary>
		public override string BaseURI
		{
			get { return _current.BaseURI; }
		}

		/// <summary>See <see cref="XmlReader.Depth"/></summary>
		public override int Depth
		{
			get { return _current.Depth + 1; }
		}

		/// <summary>See <see cref="XmlReader.EOF"/></summary>
		public override bool EOF
		{
			get { return _current.ReadState == ReadState.EndOfFile || _current.ReadState == ReadState.Closed; }
		}

		/// <summary>See <see cref="XmlReader.HasValue"/></summary>
		public override bool HasValue
		{
			get { return _current.HasValue; }
		}

		/// <summary>See <see cref="XmlReader.IsDefault"/></summary>
		public override bool IsDefault
		{
			get { return false; }
		}

		/// <summary>See <see cref="XmlReader.IsDefault"/></summary>
		public override bool IsEmptyElement
		{
			get { return _current.IsEmptyElement; }
		}

		/// <summary>See <see cref="XmlReader.this[string, string]"/></summary>
		public override string this[string name, string ns]
		{
			get { return _current[name, ns]; }
		}

		/// <summary>See <see cref="XmlReader.this[string]"/></summary>
		public override string this[string name]
		{
			get { return _current[name, String.Empty]; }
		}

		/// <summary>See <see cref="XmlReader.this[int]"/></summary>
		public override string this[int i]
		{
			get { return _current[i]; }
		}

		/// <summary>See <see cref="XmlReader.LocalName"/></summary>
		public override string LocalName
		{
			get { return _current.LocalName; }
		}

		/// <summary>See <see cref="XmlReader.Name"/></summary>
		public override string Name
		{
			get { return _current.Name; }
		}

		/// <summary>See <see cref="XmlReader.NamespaceURI"/></summary>
		public override string NamespaceURI
		{
			get { return _current.NamespaceURI; }
		}

		/// <summary>See <see cref="XmlReader.NameTable"/></summary>
		public override XmlNameTable NameTable
		{
			get { return _current.NameTable; }
		}

		/// <summary>See <see cref="XmlReader.NodeType"/></summary>
		public override XmlNodeType NodeType
		{
			get { return _current.NodeType; }

		}

		/// <summary>See <see cref="XmlReader.Prefix"/></summary>
		public override string Prefix
		{
			get { return _current.Prefix; }
		}

		/// <summary>See <see cref="XmlReader.QuoteChar"/></summary>
		public override char QuoteChar
		{
			get { return _current.QuoteChar; }
		}

		/// <summary>See <see cref="XmlReader.ReadState"/></summary>
		public override ReadState ReadState
		{
			get { return _current.ReadState; }
		}

		/// <summary>See <see cref="XmlReader.Value"/></summary>
		public override string Value
		{
			get { return _current.Value; }
		}

		/// <summary>See <see cref="XmlReader.XmlLang"/></summary>
		public override string XmlLang
		{
			get { return _current.XmlLang; }
		}

		/// <summary>See <see cref="XmlReader.XmlSpace"/></summary>
		public override XmlSpace XmlSpace
		{
			get { return XmlSpace.Default; }
		}

		#endregion Properties

		#region Methods

		/// <summary>See <see cref="XmlReader.Close"/></summary>
		public override void Close()
		{
			_current.Close();
		}

		/// <summary>See <see cref="XmlReader.GetAttribute(string, string)"/></summary>
		public override string GetAttribute(string name, string ns)
		{
			return _current.GetAttribute(name, ns);
		}

		/// <summary>See <see cref="XmlReader.GetAttribute(string)"/></summary>
		public override string GetAttribute(string name)
		{
			return _current.GetAttribute(name);
		}

		/// <summary>See <see cref="XmlReader.GetAttribute(int)"/></summary>
		public override string GetAttribute(int i)
		{
			return _current.GetAttribute(i);
		}

		/// <summary>See <see cref="XmlReader.LookupNamespace"/></summary>
		public override string LookupNamespace(string prefix)
		{
			return _current.LookupNamespace(prefix);
		}

		/// <summary>See <see cref="XmlReader.MoveToAttribute(string, string)"/></summary>
		public override bool MoveToAttribute(string name, string ns)
		{
			return _current.MoveToAttribute(name, ns);
		}

		/// <summary>See <see cref="XmlReader.MoveToAttribute(string)"/></summary>
		public override bool MoveToAttribute(string name)
		{
			return _current.MoveToAttribute(name);
		}

		/// <summary>See <see cref="XmlReader.MoveToAttribute(int)"/></summary>
		public override void MoveToAttribute(int i)
		{
			_current.MoveToAttribute(i);
		}

		/// <summary>See <see cref="XmlReader.MoveToContent"/></summary>
		public override XmlNodeType MoveToContent()
		{
			return base.MoveToContent();
		}

		/// <summary>See <see cref="XmlReader.MoveToElement"/></summary>
		public override bool MoveToElement()
		{
			return _current.MoveToElement();
		}

		/// <summary>See <see cref="XmlReader.MoveToFirstAttribute"/></summary>
		public override bool MoveToFirstAttribute()
		{
			return _current.MoveToFirstAttribute();
		}

		/// <summary>See <see cref="XmlReader.MoveToNextAttribute"/></summary>
		public override bool MoveToNextAttribute()
		{
			return _current.MoveToNextAttribute();
		}

		/// <summary>See <see cref="XmlReader.Read"/></summary>
		public override bool Read()
		{
			// Return fast if state is no appropriate.
			if (_current.ReadState == ReadState.Closed || _current.ReadState == ReadState.EndOfFile)
				return false;

			bool read = _current.Read();
			if (!read)
			{
				read = _iterator.MoveNext();
				if (read)
				{
					// Just move to the next node and create the reader.
					_current = new SubtreeXPathNavigator(_iterator.Current).ReadSubtree();
					return _current.Read();
				}
				else
				{
					if (_current is FakedRootReader && _current.NodeType == XmlNodeType.EndElement)
					{
						// We're done!
						return false;
					}
					else
					{
						// We read all nodes in the iterator. Return to faked root end element.
						_current = new FakedRootReader(_rootname.Name, _rootname.Namespace, XmlNodeType.EndElement);
						return true;
					}
				}
			}

			return read;
		}

		/// <summary>See <see cref="XmlReader.ReadAttributeValue"/></summary>
		public override bool ReadAttributeValue()
		{
			return _current.ReadAttributeValue();
		}

		/// <summary>See <see cref="XmlReader.ReadInnerXml"/></summary>
		public override string ReadInnerXml()
		{
			if (this.Read()) return Serialize();
			return String.Empty;
		}

		/// <summary>See <see cref="XmlReader.ReadOuterXml"/></summary>
		public override string ReadOuterXml()
		{
			if (_current.ReadState != ReadState.Interactive) return String.Empty;
			return Serialize();
		}

		/// <summary>See <see cref="XmlReader.Read"/></summary>
		public override void ResolveEntity()
		{
			// Not supported.
		}

		#endregion Methods

		#region IXmlSerializable Members

		/// <summary>
		/// See <see cref="IXmlSerializable.WriteXml"/>.
		/// </summary>
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteNode(this, false);
		}

		/// <summary>
		/// See <see cref="IXmlSerializable.GetSchema"/>.
		/// </summary>
		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>
		/// See <see cref="IXmlSerializable.ReadXml"/>.
		/// </summary>
		public void ReadXml(XmlReader reader)
		{
			XPathDocument doc = new XPathDocument(reader);
			XPathNavigator nav = doc.CreateNavigator();

			// Pull the faked root out.
			nav.MoveToFirstChild();
			_rootname = new XmlQualifiedName(nav.LocalName, nav.NamespaceURI);

			// Get iterator for all child nodes.
			_iterator = nav.SelectChildren(XPathNodeType.All);
		}

		#endregion

		#region Internal Classes

		#region FakedRootReader

		private class FakedRootReader : XmlReader
		{
			public FakedRootReader(string name, string ns,
				XmlNodeType nodeType)
			{
				_name = name;
				_namespace = ns;
				_nodetype = nodeType;
				_state = nodeType == XmlNodeType.Element ?
					ReadState.Initial : ReadState.Interactive;
			}

			#region Properties

			/// <summary>See <see cref="XmlReader.AttributeCount"/></summary>
			public override int AttributeCount
			{
				get { return 0; }
			}

			/// <summary>See <see cref="XmlReader.BaseURI"/></summary>
			public override string BaseURI
			{
				get { return String.Empty; }
			}

			/// <summary>See <see cref="XmlReader.Depth"/></summary>
			public override int Depth
			{
				// Undo the depth increment of the outer reader.
				get { return -1; }
			}

			/// <summary>See <see cref="XmlReader.EOF"/></summary>
			public override bool EOF
			{
				get { return _state == ReadState.EndOfFile; }
			}

			/// <summary>See <see cref="XmlReader.HasValue"/></summary>
			public override bool HasValue
			{
				get { return false; }
			}

			/// <summary>See <see cref="XmlReader.IsDefault"/></summary>
			public override bool IsDefault
			{
				get { return false; }
			}

			/// <summary>See <see cref="XmlReader.IsDefault"/></summary>
			public override bool IsEmptyElement
			{
				get { return false; }
			}

			/// <summary>See <see cref="XmlReader.this[string, string]"/></summary>
			public override string this[string name, string ns]
			{
				get { return null; }
			}

			/// <summary>See <see cref="XmlReader.this[string]"/></summary>
			public override string this[string name]
			{
				get { return null; }
			}

			/// <summary>See <see cref="XmlReader.this[int]"/></summary>
			public override string this[int i]
			{
				get { return null; }
			}

			/// <summary>See <see cref="XmlReader.LocalName"/></summary>
			public override string LocalName
			{
				get { return _name; }
			} string _name;

			/// <summary>See <see cref="XmlReader.Name"/></summary>
			public override string Name
			{
				get { return _name; }
			}

			/// <summary>See <see cref="XmlReader.NamespaceURI"/></summary>
			public override string NamespaceURI
			{
				get { return _namespace; }
			} string _namespace;

			/// <summary>See <see cref="XmlReader.NameTable"/></summary>
			public override XmlNameTable NameTable
			{
				get { return null; }
			}

			/// <summary>See <see cref="XmlReader.NodeType"/></summary>
			public override XmlNodeType NodeType
			{
				get { return _state == ReadState.Initial ? XmlNodeType.None : _nodetype; }
			} XmlNodeType _nodetype;

			/// <summary>See <see cref="XmlReader.Prefix"/></summary>
			public override string Prefix
			{
				get { return String.Empty; }
			}

			/// <summary>See <see cref="XmlReader.QuoteChar"/></summary>
			public override char QuoteChar
			{
				get { return '"'; }
			}

			/// <summary>See <see cref="XmlReader.ReadState"/></summary>
			public override ReadState ReadState
			{
				get { return _state; }
			} ReadState _state;

			/// <summary>See <see cref="XmlReader.Value"/></summary>
			public override string Value
			{
				get { return String.Empty; }
			}

			/// <summary>See <see cref="XmlReader.XmlLang"/></summary>
			public override string XmlLang
			{
				get { return String.Empty; }
			}

			/// <summary>See <see cref="XmlReader.XmlSpace"/></summary>
			public override XmlSpace XmlSpace
			{
				get { return XmlSpace.Default; }
			}

			#endregion Properties

			#region Methods

			/// <summary>See <see cref="XmlReader.Close"/></summary>
			public override void Close()
			{
				_state = ReadState.Closed;
			}

			/// <summary>See <see cref="XmlReader.GetAttribute(string, string)"/></summary>
			public override string GetAttribute(string name, string ns)
			{
				return null;
			}

			/// <summary>See <see cref="XmlReader.GetAttribute(string)"/></summary>
			public override string GetAttribute(string name)
			{
				return null;
			}

			/// <summary>See <see cref="XmlReader.GetAttribute(int)"/></summary>
			public override string GetAttribute(int i)
			{
				return null;
			}

			/// <summary>See <see cref="XmlReader.LookupNamespace"/></summary>
			public override string LookupNamespace(string prefix)
			{
				return null;
			}

			/// <summary>See <see cref="XmlReader.MoveToAttribute(string, string)"/></summary>
			public override bool MoveToAttribute(string name, string ns)
			{
				return false;
			}

			/// <summary>See <see cref="XmlReader.MoveToAttribute(string)"/></summary>
			public override bool MoveToAttribute(string name)
			{
				return false;
			}

			/// <summary>See <see cref="XmlReader.MoveToAttribute(int)"/></summary>
			public override void MoveToAttribute(int i)
			{
			}

			public override XmlNodeType MoveToContent()
			{
				if (_state == ReadState.Initial)
					_state = ReadState.Interactive;
				return _nodetype;
			}


			/// <summary>See <see cref="XmlReader.MoveToElement"/></summary>
			public override bool MoveToElement()
			{
				return false;
			}

			/// <summary>See <see cref="XmlReader.MoveToFirstAttribute"/></summary>
			public override bool MoveToFirstAttribute()
			{
				return false;
			}

			/// <summary>See <see cref="XmlReader.MoveToNextAttribute"/></summary>
			public override bool MoveToNextAttribute()
			{
				return false;
			}

			/// <summary>See <see cref="XmlReader.Read"/></summary>
			public override bool Read()
			{
				if (_state == ReadState.Initial)
				{
					_state = ReadState.Interactive;
					return true;
				}
				if (_state == ReadState.Interactive && _nodetype == XmlNodeType.EndElement)
				{
					_state = ReadState.EndOfFile;
					return false;
				}

				return false;
			}

			/// <summary>See <see cref="XmlReader.ReadAttributeValue"/></summary>
			public override bool ReadAttributeValue()
			{
				return false;
			}

			/// <summary>See <see cref="XmlReader.ReadInnerXml"/></summary>
			public override string ReadInnerXml()
			{
				return String.Empty;
			}

			/// <summary>See <see cref="XmlReader.ReadOuterXml"/></summary>
			public override string ReadOuterXml()
			{
				return String.Empty;
			}

			/// <summary>See <see cref="XmlReader.Read"/></summary>
			public override void ResolveEntity()
			{
				// Not supported.
			}

			#endregion Methods
		}

		#endregion FakedRootReader

		#endregion Internal Classes
	}
}
