using System;
using System.Xml;
using System.Xml.XPath;

namespace Mvp.Xml.Common
{
	/// <summary>
	/// Wraps an <see cref="XmlReader"/> which was created to read 
	/// fragments (using <see cref="ConformanceLevel.Fragment"/> for 
	/// its <see cref="XmlReaderSettings"/>) with a "virtual" root node 
	/// so that it can be loaded into an <see cref="XmlDocument"/> or 
	/// <see cref="XPathDocument"/>.
	/// </summary>
	public class XmlFragmentReader : XmlWrappingReader
	{
		private XmlQualifiedName rootName;
		private bool isRoot = false;
		private ReadState state = ReadState.Initial;
		private XmlNodeType nodeType;

		/// <summary>
		/// Instantiates the reader using the given <paramref name="rootElementName"/> 
		/// for the virtual root node.
		/// </summary>
		/// <param name="baseReader">XML fragment reader.</param>
		/// <param name="rootElementName">Name of the virtual root element.</param>
		public XmlFragmentReader(string rootElementName, XmlReader baseReader)
			: base(baseReader)
		{
			Guard.ArgumentNotNullOrEmptyString(rootElementName, "rootElementName");

			Initialize(new XmlQualifiedName(rootElementName, String.Empty));
		}

		/// <summary>
		/// Instantiates the reader using the given <paramref name="rootElementName"/> and 
		/// <paramref name="rootXmlNamespace"/> for the virtual root node.
		/// </summary>
		/// <param name="baseReader">XML fragment reader.</param>
		/// <param name="rootElementName">Name of the virtual root element.</param>
		/// <param name="rootXmlNamespace">XML namespace for the virtual root element.</param>
		public XmlFragmentReader(string rootElementName, string rootXmlNamespace, XmlReader baseReader)
			: base(baseReader)
		{
			Guard.ArgumentNotNullOrEmptyString(rootElementName, "rootElementName");
			Guard.ArgumentNotNull(rootXmlNamespace, "rootXmlNamespace");

			Initialize(new XmlQualifiedName(rootElementName, rootXmlNamespace));
		}

		/// <summary>
		/// Instantiates the reader using the given <paramref name="rootName"/> for the virtual root node.
		/// </summary>
		/// <param name="baseReader">XML fragment reader.</param>
		/// <param name="rootName">Qualified name of the virtual root element.</param>
		public XmlFragmentReader(XmlQualifiedName rootName, XmlReader baseReader)
			: base(baseReader)
		{
			Guard.ArgumentNotNull(rootName, "rootName");

			Initialize(rootName);
		}

		private void Initialize(XmlQualifiedName rootName)
		{
			this.rootName = rootName;
		}

		/// <summary>
		/// See <see cref="XmlReader.Read"/>.
		/// </summary>
		public override bool Read()
		{
			if (state == ReadState.Initial)
			{
				state = ReadState.Interactive;
				isRoot = true;
				nodeType = XmlNodeType.Element;
				return true;
			}
			else if (state == ReadState.EndOfFile)
			{
				return false;
			}

			bool read = base.Read();

			if (isRoot)
			{
				if (!read)
				{
					isRoot = false;
					nodeType = XmlNodeType.None;
					state = ReadState.EndOfFile;
				}
				else
				{
					isRoot = false;
				}
			}
			else
			{
				if (!read)
				{
					isRoot = true;
					nodeType = XmlNodeType.EndElement;
					return true;
				}
			}

			return read;
		}

		/// <summary>
		/// See <see cref="XmlReader.ReadState"/>.
		/// </summary>
		public override ReadState ReadState
		{
			get
			{
				ReadState baseState = base.ReadState;

				if (baseState == ReadState.Initial ||
					baseState == ReadState.EndOfFile)
					return state;

				return baseState;
			}
		}

		/// <summary>
		/// See <see cref="XmlReader.NodeType"/>.
		/// </summary>
		public override XmlNodeType NodeType
		{
			get
			{
				if (isRoot)
					return nodeType;
				else
					return base.NodeType;
			}
		}

		/// <summary>
		/// See <see cref="XmlReader.Depth"/>.
		/// </summary>
		public override int Depth
		{
			get { return base.Depth + 1; }
		}

		/// <summary>
		/// See <see cref="XmlReader.LocalName"/>.
		/// </summary>
		public override string LocalName
		{
			get
			{
				if (isRoot)
					return rootName.Name;

				return base.LocalName;
			}
		}

		/// <summary>
		/// See <see cref="XmlReader.NamespaceURI"/>.
		/// </summary>
		public override string NamespaceURI
		{
			get
			{
				if (isRoot)
					return rootName.Namespace;

				return base.NamespaceURI;
			}
		}

		/// <summary>
		/// See <see cref="XmlReader.Prefix"/>.
		/// </summary>
		public override string Prefix
		{
			get
			{
				// TODO: add support for specifying prefix.
				if (isRoot)
					return String.Empty;
				else
					return base.Prefix;
			}
		}

		/// <summary>
		/// See <see cref="XmlReader.Name"/>.
		/// </summary>
		public override string Name
		{
			get
			{
				if (isRoot)
				{
					if (this.Prefix.Length == 0)
					{
						return this.LocalName;
					}
					else
					{
						return this.NameTable.Add(this.Prefix + ":" + this.LocalName);
					}
				}
				else
				{
					return base.Name;
				}
			}
		}
	}
}
