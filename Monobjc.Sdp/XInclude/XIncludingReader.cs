#region using

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.IO;
using System.Net;
using System.Text;
using System.Security;
using System.Collections.Generic;
using System.Reflection;

using Mvp.Xml.Common;
using Mvp.Xml.Common.XPath;
using Mvp.Xml.XPointer;
using System.Globalization;

#endregion

namespace Mvp.Xml.XInclude
{
	/// <summary>
	/// <c>XIncludingReader</c> class implements streamable subset of the
	/// XInclude 1.0 in a fast, non-caching, forward-only fashion.
	/// To put it another way <c>XIncludingReader</c> is XML Base and XInclude 1.0 aware
	/// <see cref="XmlReader"/>.
	/// </summary>
    /// <remarks><para>See <a href="http://www.codeplex.com/MVPXML/Wiki/View.aspx?title=XInclude.NET">XInclude.NET homepage</a> for more info.</para>    
	/// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
	/// </remarks>
	public class XIncludingReader : XmlReader, IXmlLineInfo
	{
		#region Private fields
		//XInclude keywords
		private XIncludeKeywords _keywords;
		//Current reader
		private XmlReader _reader;
		//Stack of readers
		private Stack<XmlReader> _readers;
		//Top base URI
		private Uri _topBaseUri;
		//Top-level included item flag
		private bool _topLevel;
		//A top-level included element has been included already
		private bool _gotTopIncludedElem;
		//At least one element has been returned by the reader
		private bool _gotElement;
		//Internal state
		private XIncludingReaderState _state;
		//Name table
		private XmlNameTable _nameTable;
		//Whitespace handling
		private WhitespaceHandling _whiteSpaceHandling;
		//Emit relative xml:base URIs
		private bool _relativeBaseUri = true;
		//Current fallback state
		private FallbackState _fallbackState;
		//Previous fallback state (imagine enclosed deep xi:fallback/xi:include tree)
		private FallbackState _prevFallbackState;
		//XmlResolver to resolve URIs
		XmlResolver _xmlResolver;
		//Expose text inclusions as CDATA
		private bool _exposeTextAsCDATA;
		//Top-level included item has different xml:lang
		private bool _differentLang = false;
		//Acquired infosets cache
		private static IDictionary<string, WeakReference> _cache;
        //Real xml:base attribute index
        int _realXmlBaseIndex = -1;        
		#endregion

		#region Constructors
		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified underlying <c>XmlReader</c> reader.
		/// </summary>
		/// <param name="reader">Underlying reader to read from</param>        
		public XIncludingReader(XmlReader reader)
		{            
			XmlTextReader xtr = reader as XmlTextReader;
			if (xtr != null)
			{
//#pragma warning disable 0618
                //XmlValidatingReader vr = new XmlValidatingReader(reader);
                //vr.ValidationType = ValidationType.None;
                //vr.EntityHandling = EntityHandling.ExpandEntities;
                //vr.ValidationEventHandler += new ValidationEventHandler(
                //    ValidationCallback);
                //_whiteSpaceHandling = xtr.WhitespaceHandling;
                //_reader = vr;                                
                XmlReaderSettings s = new XmlReaderSettings();
                s.ProhibitDtd = false;
                s.ValidationType = ValidationType.None;
                s.ValidationEventHandler += new ValidationEventHandler(
                    ValidationCallback);
                if (xtr.WhitespaceHandling == WhitespaceHandling.Significant) 
                {
                    s.IgnoreWhitespace = true;
                }
                _reader = XmlReader.Create(reader, s);
//#pragma warning restore 0618
			}
			else
			{
				_reader = reader;
			}

			_nameTable = reader.NameTable;
			_keywords = new XIncludeKeywords(NameTable);
			if (_reader.BaseURI != "")
				_topBaseUri = new Uri(_reader.BaseURI);
			else
			{
				_relativeBaseUri = false;
				_topBaseUri = new Uri(Assembly.GetExecutingAssembly().Location);
			}
			_readers = new Stack<XmlReader>();
			_state = XIncludingReaderState.Default;
		}

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified URL.
		/// </summary>
		/// <param name="url">Document location.</param>
		public XIncludingReader(string url)
			: this(new XmlBaseAwareXmlReader(url)) { }

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified URL and resolver.
		/// </summary>
		/// <param name="url">Document location.</param>
		/// <param name="resolver">Resolver to acquire external resources.</param>
		public XIncludingReader(string url, XmlResolver resolver)
			: this(new XmlBaseAwareXmlReader(url, resolver)) { }

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified URL and nametable.
		/// </summary>
		/// <param name="url">Document location.</param>
		/// <param name="nt">Name table.</param>
		public XIncludingReader(string url, XmlNameTable nt)
			:
			this(new XmlBaseAwareXmlReader(url, nt)) { }

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified <c>TextReader</c> reader.
		/// </summary>
		/// <param name="reader"><c>TextReader</c>.</param>
		public XIncludingReader(TextReader reader)
			: this(new XmlBaseAwareXmlReader(reader)) { }

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified URL and <c>TextReader</c> reader.
		/// </summary>
		/// <param name="reader"><c>TextReader</c>.</param>
		/// <param name="url">Source document's URL</param>
		public XIncludingReader(string url, TextReader reader)
			: this(new XmlBaseAwareXmlReader(url, reader)) { }

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified <c>TextReader</c> reader and nametable.
		/// </summary>
		/// <param name="reader"><c>TextReader</c>.</param>
		/// <param name="nt">Nametable.</param>
		public XIncludingReader(TextReader reader, XmlNameTable nt)
			:
			this(new XmlBaseAwareXmlReader(reader, nt)) { }

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified URL, <c>TextReader</c> reader and nametable.
		/// </summary>
		/// <param name="reader"><c>TextReader</c>.</param>
		/// <param name="nt">Nametable.</param>
		/// <param name="url">Source document's URI</param>
		public XIncludingReader(string url, TextReader reader, XmlNameTable nt)
			:
			this(new XmlBaseAwareXmlReader(url, reader, nt)) { }

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified <c>Stream</c>.
		/// </summary>
		/// <param name="input"><c>Stream</c>.</param>
		public XIncludingReader(Stream input)
			: this(new XmlBaseAwareXmlReader(input)) { }

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified URL and <c>Stream</c>.
		/// </summary>
		/// <param name="input"><c>Stream</c>.</param>
		/// <param name="url">Source document's URL</param>
		public XIncludingReader(string url, Stream input)
			: this(new XmlBaseAwareXmlReader(url, input)) { }

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified URL, <c>Stream</c> and resolver.
		/// </summary>
		/// <param name="input"><c>Stream</c>.</param>
		/// <param name="url">Source document's URL</param>
		/// <param name="resolver">Resolver to acquire external resources.</param>
		public XIncludingReader(string url, Stream input, XmlResolver resolver)
			: this(new XmlBaseAwareXmlReader(url, input, resolver)) { }

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified <c>Stream</c> and nametable.
		/// </summary>
		/// <param name="input"><c>Stream</c>.</param>
		/// <param name="nt">Nametable</param>
		public XIncludingReader(Stream input, XmlNameTable nt)
			:
			this(new XmlBaseAwareXmlReader(input, nt)) { }

		/// <summary>
		/// Creates new instance of <c>XIncludingReader</c> class with
		/// specified URL, <c>Stream</c> and nametable.
		/// </summary>
		/// <param name="input"><c>Stream</c>.</param>
		/// <param name="nt">Nametable</param>
		/// <param name="url">Source document's URL</param>
		public XIncludingReader(string url, Stream input, XmlNameTable nt)
			:
			this(new XmlBaseAwareXmlReader(url, input, nt)) { }

		#endregion

		#region XmlReader's overriden members

		/// <summary>See <see cref="XmlReader.AttributeCount"/></summary>
		public override int AttributeCount
		{
			get
			{
				if (_topLevel)
				{
					int ac = _reader.AttributeCount;
					if (_reader.GetAttribute(_keywords.XmlBase) == null)
						ac++;
					if (_differentLang)
						ac++;
					return ac;
				}
				else
					return _reader.AttributeCount;
			}
		}

		/// <summary>See <see cref="XmlReader.BaseURI"/></summary>
		public override string BaseURI
		{
			get { return _reader.BaseURI; }
		}

		/// <summary>See <see cref="XmlReader.HasValue"/></summary>
		public override bool HasValue
		{
			get
			{
				if (_state == XIncludingReaderState.Default)
					return _reader.HasValue;
				else
					return true;
			}
		}

		/// <summary>See <see cref="XmlReader.IsDefault"/></summary>
		public override bool IsDefault
		{
			get
			{
				if (_state == XIncludingReaderState.Default)
					return _reader.IsDefault;
				else
					return false;
			}
		}

		/// <summary>See <see cref="XmlReader.Name"/></summary>
		public override string Name
		{
			get
			{
				switch (_state)
				{
					case XIncludingReaderState.ExposingXmlBaseAttr:
						return _keywords.XmlBase;
					case XIncludingReaderState.ExposingXmlBaseAttrValue:
					case XIncludingReaderState.ExposingXmlLangAttrValue:
						return String.Empty;
					case XIncludingReaderState.ExposingXmlLangAttr:
						return _keywords.XmlLang;
					default:
						return _reader.Name;
				}
			}
		}

		/// <summary>See <see cref="XmlReader.LocalName"/></summary>
		public override string LocalName
		{
			get
			{
				switch (_state)
				{
					case XIncludingReaderState.ExposingXmlBaseAttr:
						return _keywords.Base;
					case XIncludingReaderState.ExposingXmlBaseAttrValue:
					case XIncludingReaderState.ExposingXmlLangAttrValue:
						return String.Empty;
					case XIncludingReaderState.ExposingXmlLangAttr:
						return _keywords.Lang;
					default:
						return _reader.LocalName;
				}
			}
		}

		/// <summary>See <see cref="XmlReader.NamespaceURI"/></summary>
		public override string NamespaceURI
		{
			get
			{
				switch (_state)
				{
					case XIncludingReaderState.ExposingXmlBaseAttr:
					case XIncludingReaderState.ExposingXmlLangAttr:
						return _keywords.XmlNamespace;
					case XIncludingReaderState.ExposingXmlBaseAttrValue:
					case XIncludingReaderState.ExposingXmlLangAttrValue:
						return String.Empty;
					default:
						return _reader.NamespaceURI;
				}
			}
		}

		/// <summary>See <see cref="XmlReader.NameTable"/></summary>
		public override XmlNameTable NameTable
		{
			get { return _nameTable; }
		}

		/// <summary>See <see cref="XmlReader.NodeType"/></summary>
		public override XmlNodeType NodeType
		{
			get
			{
				switch (_state)
				{
					case XIncludingReaderState.ExposingXmlBaseAttr:
					case XIncludingReaderState.ExposingXmlLangAttr:
						return XmlNodeType.Attribute;
					case XIncludingReaderState.ExposingXmlBaseAttrValue:
					case XIncludingReaderState.ExposingXmlLangAttrValue:
						return XmlNodeType.Text;
					default:
						return _reader.NodeType;
				}
			}
		}

		/// <summary>See <see cref="XmlReader.Prefix"/></summary>
		public override string Prefix
		{
			get
			{
				switch (_state)
				{
					case XIncludingReaderState.ExposingXmlBaseAttr:
					case XIncludingReaderState.ExposingXmlLangAttr:
						return _keywords.Xml;
					case XIncludingReaderState.ExposingXmlBaseAttrValue:
					case XIncludingReaderState.ExposingXmlLangAttrValue:
						return String.Empty;
					default:
						return _reader.Prefix;
				}
			}
		}

		/// <summary>See <see cref="XmlReader.QuoteChar"/></summary>
		public override char QuoteChar
		{
			get
			{
				switch (_state)
				{
					case XIncludingReaderState.ExposingXmlBaseAttr:
					case XIncludingReaderState.ExposingXmlLangAttr:
						return '"';
					default:
						return _reader.QuoteChar;
				}
			}
		}

		/// <summary>See <see cref="XmlReader.Close"/></summary>
		public override void Close()
		{
			if (_reader != null)
				_reader.Close();
			//Close all readers in the stack
			while (_readers.Count > 0)
			{
				_reader = (XmlReader)_readers.Pop();
				if (_reader != null)
					_reader.Close();
			}
		}

		/// <summary>See <see cref="XmlReader.Depth"/></summary>
		public override int Depth
		{
			get
			{
				if (_readers.Count == 0)
					return _reader.Depth;
				else
					//TODO: that might be way ineffective
					return ((XmlReader)_readers.Peek()).Depth + _reader.Depth;
			}
		}

		/// <summary>See <see cref="XmlReader.EOF"/></summary>
		public override bool EOF
		{
			get { return _reader.EOF; }
		}

		/// <summary>See <see cref="XmlReader.GetAttribute(int)"/></summary>
		public override string GetAttribute(int i)
		{
            if (_topLevel)
            {
                int ac = _reader.AttributeCount;
                if (i < ac)
                {
                    if (i == _realXmlBaseIndex)
                    {
                        //case 1: it's real xml:base
                        return GetBaseUri();
                    }
                    else
                    {
                        //case 2: it's real attribute and it's not xml:base
                        return _reader.GetAttribute(i);                     
                    }
                }
                else
                {
                    if (i == ac)
                    {
                        //case 3: it's virtual xml:base - it comes first
                        return GetBaseUri();
                    }
                    else
                    {
                        //case 4: it's virtual xml:lang - it comes last
                        return _reader.XmlLang;
                    }
                }                                                               
            }
            else
            {
                return _reader.GetAttribute(i);
            }
		}       

		/// <summary>See <see cref="XmlReader.GetAttribute(string)"/></summary>
		public override string GetAttribute(string name)
		{
			if (_topLevel)
			{
                if (XIncludeKeywords.Equals(name, _keywords.XmlBase))
                    return GetBaseUri();
                else if (XIncludeKeywords.Equals(name, _keywords.XmlLang))
                    return _reader.XmlLang;
			}
			return _reader.GetAttribute(name);
		}

		/// <summary>See <see cref="XmlReader.GetAttribute(string, string)"/></summary>
		public override string GetAttribute(string name, string namespaceURI)
		{
			if (_topLevel)
			{
                if (XIncludeKeywords.Equals(name, _keywords.Base) &&
                    XIncludeKeywords.Equals(namespaceURI, _keywords.XmlNamespace))
                    return GetBaseUri();
                else if (XIncludeKeywords.Equals(name, _keywords.Lang) &&
                    XIncludeKeywords.Equals(namespaceURI, _keywords.XmlNamespace))
                    return _reader.XmlLang;
			}
			return _reader.GetAttribute(name, namespaceURI);
		}

		/// <summary>See <see cref="XmlReader.IsEmptyElement"/></summary>
		public override bool IsEmptyElement
		{
			get { return _reader.IsEmptyElement; }
		}

		/// <summary>See <see cref="XmlReader.LookupNamespace"/></summary>
		public override String LookupNamespace(String prefix)
		{
			return _reader.LookupNamespace(prefix);
		}

		/// <summary>See <see cref="XmlReader.MoveToAttribute(int)"/></summary>
		public override void MoveToAttribute(int i)
		{
            if (_topLevel)
            {                
                if (i >= _reader.AttributeCount || i == _realXmlBaseIndex)
                {
                    _state = XIncludingReaderState.ExposingXmlBaseAttr;
                }
                else
                {
                    _reader.MoveToAttribute(i);
                }                
            }
            else
            {
                _reader.MoveToAttribute(i);
            }
		}

		/// <summary>See <see cref="XmlReader.MoveToAttribute(string)"/></summary>
		public override bool MoveToAttribute(string name)
		{
			if (_topLevel)
			{
				if (XIncludeKeywords.Equals(name, _keywords.XmlBase))
				{
					_state = XIncludingReaderState.ExposingXmlBaseAttr;
					return true;
				}
				else if (XIncludeKeywords.Equals(name, _keywords.XmlLang))
				{
					_state = XIncludingReaderState.ExposingXmlLangAttr;
					return true;
				}
			}
			return _reader.MoveToAttribute(name);
		}

		/// <summary>See <see cref="XmlReader.MoveToAttribute(string, string)"/></summary>
		public override bool MoveToAttribute(string name, string ns)
		{
			if (_topLevel)
			{
				if (XIncludeKeywords.Equals(name, _keywords.Base) &&
					XIncludeKeywords.Equals(ns, _keywords.XmlNamespace))
				{
					_state = XIncludingReaderState.ExposingXmlBaseAttr;
					return true;
				}
				else if (XIncludeKeywords.Equals(name, _keywords.Lang) &&
					XIncludeKeywords.Equals(ns, _keywords.XmlNamespace))
				{
					_state = XIncludingReaderState.ExposingXmlLangAttr;
					return true;
				}
			}
			return _reader.MoveToAttribute(name, ns);
		}

		/// <summary>See <see cref="XmlReader.MoveToElement"/></summary>
		public override bool MoveToElement()
		{
			return _reader.MoveToElement();
		}

		/// <summary>See <see cref="XmlReader.MoveToFirstAttribute"/></summary>
		public override bool MoveToFirstAttribute()
		{
			if (_topLevel)
			{
				bool res = _reader.MoveToFirstAttribute();
				if (res)
				{
					//it might be xml:base or xml:lang
					if (_reader.Name == _keywords.XmlBase ||
						_reader.Name == _keywords.XmlLang)
						//omit them - we expose virtual ones at the end
						return MoveToNextAttribute();
					else
						return res;
				}
				else
				{
					//No attrs? Expose xml:base
					_state = XIncludingReaderState.ExposingXmlBaseAttr;
					return true;
				}

			}
			else
				return _reader.MoveToFirstAttribute();
		}

		/// <summary>See <see cref="XmlReader.MoveToNextAttribute"/></summary>
		public override bool MoveToNextAttribute()
		{
			if (_topLevel)
			{
				switch (_state)
				{
					case XIncludingReaderState.ExposingXmlBaseAttr:
					case XIncludingReaderState.ExposingXmlBaseAttrValue:
						//Exposing xml:base already - switch to xml:lang                                                                            
						if (_differentLang)
						{
							_state = XIncludingReaderState.ExposingXmlLangAttr;
							return true;
						}
						else
						{
							//No need for xml:lang, stop
							_state = XIncludingReaderState.Default;
							return false;
						}
					case XIncludingReaderState.ExposingXmlLangAttr:
					case XIncludingReaderState.ExposingXmlLangAttrValue:
						//Exposing xml:lang already - that's a last one
						_state = XIncludingReaderState.Default;
						return false;
					default:
						//1+ attrs, default mode
						bool res = _reader.MoveToNextAttribute();
						if (res)
						{
							//Still real attributes - it might be xml:base or xml:lang
							if (_reader.Name == _keywords.XmlBase ||
								_reader.Name == _keywords.XmlLang)
								//omit them - we expose virtual ones at the end
								return MoveToNextAttribute();
							else
								return res;
						}
						else
						{
							//No more attrs - expose virtual xml:base                                
							_state = XIncludingReaderState.ExposingXmlBaseAttr;
							return true;
						}
				}

			}
			else
				return _reader.MoveToNextAttribute();
		}

		/// <summary>See <see cref="XmlReader.ReadAttributeValue"/></summary>
		public override bool ReadAttributeValue()
		{
			switch (_state)
			{
				case XIncludingReaderState.ExposingXmlBaseAttr:
					_state = XIncludingReaderState.ExposingXmlBaseAttrValue;
					return true;
				case XIncludingReaderState.ExposingXmlBaseAttrValue:
					return false;
				case XIncludingReaderState.ExposingXmlLangAttr:
					_state = XIncludingReaderState.ExposingXmlLangAttrValue;
					return true;
				case XIncludingReaderState.ExposingXmlLangAttrValue:
					return false;
				default:
					return _reader.ReadAttributeValue();
			}
		}

		/// <summary>See <see cref="XmlReader.ReadState"/></summary>
		public override ReadState ReadState
		{
			get { return _reader.ReadState; }
		}

		/// <summary>See <see cref="XmlReader.this[int]"/></summary>
		public override String this[int i]
		{
			get { return GetAttribute(i); }
		}

		/// <summary>See <see cref="XmlReader.this[string]"/></summary>
		public override string this[string name]
		{
			get { return GetAttribute(name); }
		}

		/// <summary>See <see cref="XmlReader.this[string, string]"/></summary>
		public override string this[string name, string namespaceURI]
		{
			get { return GetAttribute(name, namespaceURI); }
		}

		/// <summary>See <see cref="XmlReader.ResolveEntity"/></summary>
		public override void ResolveEntity()
		{
			_reader.ResolveEntity();
		}

		/// <summary>See <see cref="XmlReader.XmlLang"/></summary>
		public override string XmlLang
		{
			get { return _reader.XmlLang; }
		}

		/// <summary>See <see cref="XmlReader.XmlSpace"/></summary>
		public override XmlSpace XmlSpace
		{
			get { return _reader.XmlSpace; }
		}

		/// <summary>See <see cref="XmlReader.Value"/></summary>
		public override string Value
		{
			get
			{
				switch (_state)
				{
					case XIncludingReaderState.ExposingXmlBaseAttr:
					case XIncludingReaderState.ExposingXmlBaseAttrValue:
                        return GetBaseUri();
					case XIncludingReaderState.ExposingXmlLangAttr:
					case XIncludingReaderState.ExposingXmlLangAttrValue:
						return _reader.XmlLang;
					default:
						return _reader.Value;
				}
			}
		}

		/// <summary>See <see cref="XmlReader.ReadInnerXml"/></summary>
		public override string ReadInnerXml()
		{
			switch (_state)
			{
				case XIncludingReaderState.ExposingXmlBaseAttr:
                    return GetBaseUri();
				case XIncludingReaderState.ExposingXmlBaseAttrValue:
					return String.Empty;
				case XIncludingReaderState.ExposingXmlLangAttr:
					return _reader.XmlLang;
				case XIncludingReaderState.ExposingXmlLangAttrValue:
					return String.Empty;
				default:
					if (NodeType == XmlNodeType.Element)
					{
						int depth = Depth;
						if (Read())
						{
							StringWriter sw = new StringWriter();
							XmlTextWriter xw = new XmlTextWriter(sw);
							while (Depth > depth)
								xw.WriteNode(this, false);
							xw.Close();
							return sw.ToString();
						}
						else
							return String.Empty;
					}
					else if (NodeType == XmlNodeType.Attribute)
					{
						return Value;
					}
					else
						return String.Empty;
			}
		}

		/// <summary>See <see cref="XmlReader.ReadOuterXml"/></summary>
		public override string ReadOuterXml()
		{
			switch (_state)
			{
				case XIncludingReaderState.ExposingXmlBaseAttr:
					return @"xml:base="" + _reader.BaseURI + @""";
				case XIncludingReaderState.ExposingXmlBaseAttrValue:
					return String.Empty;
				case XIncludingReaderState.ExposingXmlLangAttr:
					return @"xml:lang="" + _reader.XmlLang + @""";
				case XIncludingReaderState.ExposingXmlLangAttrValue:
					return String.Empty;
				default:
					if (NodeType == XmlNodeType.Element)
					{
						StringWriter sw = new StringWriter();
						XmlTextWriter xw = new XmlTextWriter(sw);
						xw.WriteNode(this, false);
						xw.Close();
						return sw.ToString();
					}
					else if (NodeType == XmlNodeType.Attribute)
					{
						return String.Format("{0=\"{1}\"}", Name, Value);
					}
					else
						return String.Empty;
			}
		}

		/// <summary>See <see cref="XmlReader.ReadString"/></summary>
		public override string ReadString()
		{
			switch (_state)
			{
				case XIncludingReaderState.ExposingXmlBaseAttr:
					return String.Empty;
				case XIncludingReaderState.ExposingXmlBaseAttrValue:
                    return GetBaseUri();
				case XIncludingReaderState.ExposingXmlLangAttr:
					return String.Empty;
				case XIncludingReaderState.ExposingXmlLangAttrValue:
					return _reader.XmlLang;
				default:
					return _reader.ReadString();
			}
		}

		/// <summary>See <see cref="XmlReader.Read"/></summary>
		public override bool Read()
		{
            _state = XIncludingReaderState.Default;
			//Read internal reader
			bool baseRead = _reader.Read();
			if (baseRead)
			{
				//If we are including and including reader is at 0 depth - 
				//we are at a top level included item
				_topLevel = (_readers.Count > 0 && _reader.Depth == 0) ? true : false;				                
				if (_topLevel && _reader.NodeType == XmlNodeType.Attribute)
					//Attempt to include an attribute or namespace node
					throw new AttributeOrNamespaceInIncludeLocationError(Monobjc.Tools.Sdp.Properties.Resources.AttributeOrNamespaceInIncludeLocationError);

				if (_topLevel && ((XmlReader)_readers.Peek()).Depth == 0 &&
					_reader.NodeType == XmlNodeType.Element)
				{
					if (_gotTopIncludedElem)
						//Attempt to include more than one element at the top level
						throw new MalformedXInclusionResultError(Monobjc.Tools.Sdp.Properties.Resources.MalformedXInclusionResult);
					else
						_gotTopIncludedElem = true;
				}
                if (_topLevel)
                {
                    //Check if included item has different language
                    _differentLang = AreDifferentLangs(_reader.XmlLang, ((XmlReader)_readers.Peek()).XmlLang);
                    if (_reader.NodeType == XmlNodeType.Element)
                    {
                        //Record real xml:base index
                        _realXmlBaseIndex = -1;
                        int i = 0;
                        while (_reader.MoveToNextAttribute())
                        {
                            if (_reader.Name == _keywords.XmlBase)
                            {
                                _realXmlBaseIndex = i;
                                break;
                            }
                            i++;
                        }
                        _reader.MoveToElement();
                    }
                }
				switch (_reader.NodeType)
				{
					case XmlNodeType.XmlDeclaration:
					case XmlNodeType.Document:
					case XmlNodeType.DocumentType:
					case XmlNodeType.DocumentFragment:
						//This stuff should not be included into resulting infoset,
						//but should be inclused into acquired infoset                        
						return _readers.Count > 0 ? Read() : baseRead;
					case XmlNodeType.Element:
						//Check for xi:include
						if (IsIncludeElement(_reader))
						{
							//xi:include element found
							//Save current reader to possible fallback processing
							XmlReader current = _reader;
							try
							{
								return ProcessIncludeElement();
							}
							catch (FatalException fe)
							{
								throw fe;
							}
							catch (Exception e)
							{
								//Let's be liberal - any exceptions other than fatal one 
								//should be treated as resource error
								//Console.WriteLine("Resource error has been detected: " + e.Message);
								//Start fallback processing
								if (!current.Equals(_reader))
								{
									_reader.Close();
									_reader = current;
								}
								_prevFallbackState = _fallbackState;
								return ProcessFallback(_reader.Depth, e);
							}
							//No, it's not xi:include, check it for xi:fallback    
						}
						else if (IsFallbackElement(_reader))
						{
							//Found xi:fallback not child of xi:include
							IXmlLineInfo li = _reader as IXmlLineInfo;
							if (li != null && li.HasLineInfo())
							{
								throw new XIncludeSyntaxError(String.Format(
									CultureInfo.CurrentCulture,
									Monobjc.Tools.Sdp.Properties.Resources.FallbackNotChildOfIncludeLong,
									_reader.BaseURI.ToString(), li.LineNumber,
									li.LinePosition));
							}
							else
								throw new XIncludeSyntaxError(String.Format(
									CultureInfo.CurrentCulture,
									Monobjc.Tools.Sdp.Properties.Resources.FallbackNotChildOfInclude,
									_reader.BaseURI.ToString()));
						}
						else
						{
							_gotElement = true;
							goto default;
						}
					case XmlNodeType.EndElement:
						//Looking for end of xi:fallback
						if (_fallbackState.Fallbacking &&
							_reader.Depth == _fallbackState.FallbackDepth &&
							IsFallbackElement(_reader))
						{
							//End of fallback processing
							_fallbackState.FallbackProcessed = true;
							//Now read other ignored content till </xi:fallback>
							return ProcessFallback(_reader.Depth - 1, null);
						}
						else
							goto default;
					default:
						return baseRead;
				}
			}
			else
			{
				//No more input - finish possible xi:include processing
				if (_topLevel)
					_topLevel = false;
				if (_readers.Count > 0)
				{
					_reader.Close();
					//Pop previous reader
					_reader = (XmlReader)_readers.Pop();
					//Successful include - skip xi:include content
					if (!_reader.IsEmptyElement)
						CheckAndSkipContent();
					return Read();
				}
				else
				{
					if (!_gotElement)
						throw new MalformedXInclusionResultError(Monobjc.Tools.Sdp.Properties.Resources.MalformedXInclusionResult);
					//That's all, folks
					return false;
				}
			}
		} // Read()

		#endregion

        #region IXmlLineInfo impl 

        /// <summary>
        /// Gets a value indicating whether the class can return line information.
        /// See <see cref="IXmlLineInfo.HasLineInfo"/>.
        /// </summary>        
        public bool HasLineInfo()
        {
            IXmlLineInfo core = _reader as IXmlLineInfo;
            if (core != null)
            {
                return core.HasLineInfo();
            }
            return false;
        }

        /// <summary>
        /// Gets the current line number.
        /// See <see cref="IXmlLineInfo.LineNumber "/>.
        /// </summary>
        public int LineNumber
        {
            get
            {
                IXmlLineInfo core = _reader as IXmlLineInfo;
                if (core != null)
                {
                    return core.LineNumber;
                }
                return 0;
            }
        }

        /// <summary>
        ///   	Gets the current line position.
        /// See <see cref="IXmlLineInfo.LinePosition "/>.
        /// </summary>
        public int LinePosition
        {
            get
            {
                IXmlLineInfo core = _reader as IXmlLineInfo;
                if (core != null)
                {
                    return core.LinePosition;
                }
                return 0;
            }
        }

        #endregion

        #region Public members

        /// <summary>
		/// See <see cref="XmlTextReader.WhitespaceHandling"/>.
		/// </summary>
		public WhitespaceHandling WhitespaceHandling
		{
			get { return _whiteSpaceHandling; }
			set { _whiteSpaceHandling = value; }
		}

		/// <summary>
		/// XmlResolver to resolve external URI references
		/// </summary>
		public XmlResolver XmlResolver
		{
			set
			{
				_xmlResolver = value;
			}
		}

        /// <summary>
        /// Gets the encoding of the document.
        /// </summary>
        /// <remarks>If underlying XmlReader doesn't support Encoding property, null is returned.</remarks>
        public Encoding Encoding
        {
            get
            {
                XmlTextReader xtr = _reader as XmlTextReader;
                if (xtr != null)
                {
                    return xtr.Encoding;
                }
                XIncludingReader xir = _reader as XIncludingReader;
                if (xir != null)
                {
                    return xir.Encoding;
                }
                return null;
            }
        }

		/// <summary>
		/// Flag indicating whether to emit <c>xml:base</c> as relative URI.
		/// True by default.
		/// </summary>
		public bool MakeRelativeBaseUri
		{
			get { return _relativeBaseUri; }
			set { _relativeBaseUri = value; }
		}

		/// <summary>
		/// Flag indicating whether expose text inclusions
		/// as CDATA or as Text. By default it's Text.
		/// </summary>
		public bool ExposeTextInclusionsAsCDATA
		{
			get { return _exposeTextAsCDATA; }
			set { _exposeTextAsCDATA = value; }
		}

		#endregion

		#region Private methods

		//Dummy validation even handler
		private static void ValidationCallback(object sender, ValidationEventArgs args)
		{
			//do nothing
		}

		/// <summary>
		/// Checks if given reader is positioned on a xi:include element.
		/// </summary>        
		private bool IsIncludeElement(XmlReader r)
		{
			if (
				(
				XIncludeKeywords.Equals(_reader.NamespaceURI, _keywords.XIncludeNamespace) ||
				XIncludeKeywords.Equals(_reader.NamespaceURI, _keywords.OldXIncludeNamespace)
				) &&
				XIncludeKeywords.Equals(_reader.LocalName, _keywords.Include))
				return true;
			else
				return false;
		}

		/// <summary>
		/// /// Checks if given reader is positioned on a xi:fallback element.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		private bool IsFallbackElement(XmlReader r)
		{
			if (
				(
				XIncludeKeywords.Equals(_reader.NamespaceURI, _keywords.XIncludeNamespace) ||
				XIncludeKeywords.Equals(_reader.NamespaceURI, _keywords.OldXIncludeNamespace)
				) &&
				XIncludeKeywords.Equals(_reader.LocalName, _keywords.Fallback))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Fetches resource by URI.
		/// </summary>        
		internal static Stream GetResource(string includeLocation,
			string accept, string acceptLanguage, out WebResponse response)
		{
			WebRequest wReq;
			try
			{
				wReq = WebRequest.Create(includeLocation);
			}
			catch (NotSupportedException nse)
			{
				throw new ResourceException(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.URISchemaNotSupported, includeLocation), nse);
			}
			catch (SecurityException se)
			{
				throw new ResourceException(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.SecurityException, includeLocation), se);
			}
			//Add accept headers if this is HTTP request
			HttpWebRequest httpReq = wReq as HttpWebRequest;
			if (httpReq != null)
			{
				if (accept != null)
				{
					TextUtils.CheckAcceptValue(accept);
					if (httpReq.Accept == null || httpReq.Accept == String.Empty)
						httpReq.Accept = accept;
					else
						httpReq.Accept += "," + accept;
				}
				if (acceptLanguage != null)
				{
					if (httpReq.Headers["Accept-Language"] == null)
						httpReq.Headers.Add("Accept-Language", acceptLanguage);
					else
						httpReq.Headers["Accept-Language"] += "," + acceptLanguage;
				}
			}
			try
			{
				response = wReq.GetResponse();
			}
			catch (WebException we)
			{
				throw new ResourceException(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.ResourceError, includeLocation), we);
			}
			return response.GetResponseStream();
		}

		/// <summary>
		/// Processes xi:include element.
		/// </summary>		
		private bool ProcessIncludeElement()
		{
			string href = _reader.GetAttribute(_keywords.Href);
			string xpointer = _reader.GetAttribute(_keywords.Xpointer);
			string parse = _reader.GetAttribute(_keywords.Parse);

			if (href == null || href == String.Empty)
			{
				//Intra-document inclusion                                                
				if (parse == null || parse.Equals(_keywords.Xml))
				{
					if (xpointer == null)
					{
						//Both href and xpointer attributes are absent in xml mode, 
						// => critical error
						IXmlLineInfo li = _reader as IXmlLineInfo;
						if (li != null && li.HasLineInfo())
						{
							throw new XIncludeSyntaxError(String.Format(
								CultureInfo.CurrentCulture,
								Monobjc.Tools.Sdp.Properties.Resources.MissingHrefAndXpointerExceptionLong,
								_reader.BaseURI.ToString(),
								li.LineNumber, li.LinePosition));
						}
						else
							throw new XIncludeSyntaxError(String.Format(
								CultureInfo.CurrentCulture,
								Monobjc.Tools.Sdp.Properties.Resources.MissingHrefAndXpointerException,
								_reader.BaseURI.ToString()));
					}
					//No support for intra-document refs                    
					throw new InvalidOperationException(Monobjc.Tools.Sdp.Properties.Resources.IntradocumentReferencesNotSupported);
				}
				else if (parse.Equals(_keywords.Text))
				{
					//No support for intra-document refs                    
					throw new InvalidOperationException(Monobjc.Tools.Sdp.Properties.Resources.IntradocumentReferencesNotSupported);
				}
			}
			else
			{
				//Inter-document inclusion
				if (parse == null || parse.Equals(_keywords.Xml))
					return ProcessInterDocXMLInclusion(href, xpointer);
				else if (parse.Equals(_keywords.Text))
					return ProcessInterDocTextInclusion(href);
			}

			//Unknown "parse" attribute value, critical error
			IXmlLineInfo li2 = _reader as IXmlLineInfo;
			if (li2 != null && li2.HasLineInfo())
			{
				throw new XIncludeSyntaxError(String.Format(
					CultureInfo.CurrentCulture,
					Monobjc.Tools.Sdp.Properties.Resources.UnknownParseAttrValueLong,
					parse,
					_reader.BaseURI.ToString(),
					li2.LineNumber, li2.LinePosition));
			}
			else
				throw new XIncludeSyntaxError(String.Format(
					CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.UnknownParseAttrValue, parse));
		}

		/// <summary>
		/// Resolves include location.
		/// </summary>
		/// <param name="href">href value</param>
		/// <returns>Include location.</returns>
		private Uri ResolveHref(string href)
		{
			Uri includeLocation;
			try
			{
				Uri baseURI = _reader.BaseURI == "" ? _topBaseUri : new Uri(_reader.BaseURI);
				if (_xmlResolver == null)
					includeLocation = new Uri(baseURI, href);
				else
					includeLocation = _xmlResolver.ResolveUri(baseURI, href);
			}
			catch (UriFormatException ufe)
			{
				throw new ResourceException(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.InvalidURI, href), ufe);
			}
			catch (Exception e)
			{
				throw new ResourceException(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.UnresolvableURI, href), e);
			}
			return includeLocation;
		}

		/// <summary>
		/// Skips content of an element using directly current reader's methods.
		/// </summary>
		private void SkipContent()
		{
			if (!_reader.IsEmptyElement)
			{
				int depth = _reader.Depth;
				while (_reader.Read() && depth < _reader.Depth) ;
			}
		}

		/// <summary>
		/// Fallback processing.
		/// </summary>
		/// <param name="depth"><c>xi:include</c> depth level.</param>    
		/// <param name="e">Resource error, which caused this processing.</param>
		/// <remarks>When inluding fails due to any resource error, <c>xi:inlcude</c> 
		/// element content is processed as follows: each child <c>xi:include</c> - 
		/// fatal error, more than one child <c>xi:fallback</c> - fatal error. No 
		/// <c>xi:fallback</c> - the resource error results in a fatal error.
		/// Content of first <c>xi:fallback</c> element is included in a usual way.</remarks>
		private bool ProcessFallback(int depth, Exception e)
		{
			//Read to the xi:include end tag
			while (_reader.Read() && depth < _reader.Depth)
			{
				switch (_reader.NodeType)
				{
					case XmlNodeType.Element:
						if (IsIncludeElement(_reader))
						{
							//xi:include child of xi:include - fatal error
							IXmlLineInfo li = _reader as IXmlLineInfo;
							if (li != null && li.HasLineInfo())
							{
								throw new XIncludeSyntaxError(String.Format(
									CultureInfo.CurrentCulture,
									Monobjc.Tools.Sdp.Properties.Resources.IncludeChildOfIncludeLong,
									BaseURI.ToString(),
									li.LineNumber, li.LinePosition));
							}
							else
								throw new XIncludeSyntaxError(String.Format(
									CultureInfo.CurrentCulture,
									Monobjc.Tools.Sdp.Properties.Resources.IncludeChildOfInclude,
									BaseURI.ToString()));
						}
						if (IsFallbackElement(_reader))
						{
							//Found xi:fallback
							if (_fallbackState.FallbackProcessed)
							{
								IXmlLineInfo li = _reader as IXmlLineInfo;
								if (li != null && li.HasLineInfo())
								{
									//Two xi:fallback                                 
									throw new XIncludeSyntaxError(String.Format(
										CultureInfo.CurrentCulture,
										Monobjc.Tools.Sdp.Properties.Resources.TwoFallbacksLong,
										BaseURI.ToString(),
										li.LineNumber, li.LinePosition));
								}
								else
									throw new XIncludeSyntaxError(String.Format(
										CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.TwoFallbacks, BaseURI.ToString()));
							}
							if (_reader.IsEmptyElement)
							{
								//Empty xi:fallback - nothing to include
								_fallbackState.FallbackProcessed = true;
								break;
							}
							_fallbackState.Fallbacking = true;
							_fallbackState.FallbackDepth = _reader.Depth;
							return Read();
						}
						else
							//Ignore anything else along with its content
							SkipContent();
						break;
					default:
						break;
				}
			}
			//xi:include content is read
			if (!_fallbackState.FallbackProcessed)
				//No xi:fallback, fatal error
				throw new FatalResourceException(e);
			else
			{
				//End of xi:include content processing, reset and go forth
				_fallbackState = _prevFallbackState;
				return Read();
			}
		}

		/// <summary>
		/// Skips xi:include element's content, while checking XInclude syntax (no 
		/// xi:include, no more than one xi:fallback).
		/// </summary>
		private void CheckAndSkipContent()
		{
			int depth = _reader.Depth;
			bool fallbackElem = false;
			while (_reader.Read() && depth < _reader.Depth)
			{
				switch (_reader.NodeType)
				{
					case XmlNodeType.Element:
						if (IsIncludeElement(_reader))
						{
							//xi:include child of xi:include - fatal error
							IXmlLineInfo li = _reader as IXmlLineInfo;
							if (li != null && li.HasLineInfo())
							{
								throw new XIncludeSyntaxError(String.Format(
									CultureInfo.CurrentCulture,
									Monobjc.Tools.Sdp.Properties.Resources.IncludeChildOfIncludeLong,
									_reader.BaseURI.ToString(),
									li.LineNumber, li.LinePosition));
							}
							else
								throw new XIncludeSyntaxError(String.Format(
									CultureInfo.CurrentCulture,
									Monobjc.Tools.Sdp.Properties.Resources.IncludeChildOfInclude,
									_reader.BaseURI.ToString()));
						}
						else if (IsFallbackElement(_reader))
						{
							//Found xi:fallback
							if (fallbackElem)
							{
								//More than one xi:fallback
								IXmlLineInfo li = _reader as IXmlLineInfo;
								if (li != null && li.HasLineInfo())
								{
									throw new XIncludeSyntaxError(String.Format(
										CultureInfo.CurrentCulture,
										Monobjc.Tools.Sdp.Properties.Resources.TwoFallbacksLong,
										_reader.BaseURI.ToString(),
										li.LineNumber, li.LinePosition));
								}
								else
									throw new XIncludeSyntaxError(String.Format(
										CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.TwoFallbacks, _reader.BaseURI.ToString()));
							}
							else
							{
								fallbackElem = true;
								SkipContent();
							}
						}
						//Check anything else in XInclude namespace
						else if (XIncludeKeywords.Equals(_reader.NamespaceURI, _keywords.XIncludeNamespace))
						{
							throw new XIncludeSyntaxError(String.Format(
								CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.UnknownXIncludeElement, _reader.Name));
						}
						else
							//Ignore everything else
							SkipContent();
						break;
					default:
						break;
				}
			}
		} // CheckAndSkipContent()

		/// <summary>
		/// Throws CircularInclusionException.
		/// </summary>        
		private void ThrowCircularInclusionError(XmlReader reader, Uri url)
		{
			IXmlLineInfo li = reader as IXmlLineInfo;
			if (li != null && li.HasLineInfo())
			{
				throw new CircularInclusionException(url,
					BaseURI.ToString(),
					li.LineNumber, li.LinePosition);
			}
			else
				throw new CircularInclusionException(url);
		}

		/// <summary>
		/// Compares two languages as per IETF RFC 3066.
		/// </summary>        
		private bool AreDifferentLangs(string lang1, string lang2)
		{
			return lang1.ToLower() != lang2.ToLower();
		}

		/// <summary>
		/// Creates acquired infoset.
		/// </summary>        
		private string CreateAcquiredInfoset(Uri includeLocation)
		{
			if (_cache == null)
				_cache = new Dictionary<string, WeakReference>();
			WeakReference wr;
			if (_cache.TryGetValue(includeLocation.AbsoluteUri, out wr) && wr.IsAlive)
			{
				return (string)wr.Target;
			}
			else
			{
				//Not cached or GCollected
				WebResponse wRes;
				Stream stream = GetResource(includeLocation.AbsoluteUri,
					_reader.GetAttribute(_keywords.Accept),
					_reader.GetAttribute(_keywords.AcceptLanguage), out wRes);
				XIncludingReader xir = new XIncludingReader(wRes.ResponseUri.AbsoluteUri, stream, _nameTable);
				xir.WhitespaceHandling = _whiteSpaceHandling;
				StringWriter sw = new StringWriter();
				XmlTextWriter w = new XmlTextWriter(sw);
				try
				{
					while (xir.Read())
						w.WriteNode(xir, false);
				}
				finally
				{
					if (xir != null)
						xir.Close();
					if (w != null)
						w.Close();
				}
				string content = sw.ToString();
				lock (_cache)
				{
					if (!_cache.ContainsKey(includeLocation.AbsoluteUri))
						_cache.Add(includeLocation.AbsoluteUri, new WeakReference(content));
				}
				return content;
			}
		}

		/// <summary>
		/// Creates acquired infoset.
		/// </summary>
		/// <param name="reader">Source reader</param>
		/// <param name="includeLocation">Base URI</param>
		private string CreateAcquiredInfoset(Uri includeLocation, TextReader reader)
		{
			return CreateAcquiredInfoset(
				new XmlBaseAwareXmlReader(includeLocation.AbsoluteUri, reader, _nameTable));
		}

		/// <summary>
		/// Creates acquired infoset.
		/// </summary>
		/// <param name="reader">Source reader</param>
		private string CreateAcquiredInfoset(XmlReader reader)
		{
			//TODO: Try to stream out this stuff                                    
			XIncludingReader xir = new XIncludingReader(reader);
			xir.XmlResolver = this._xmlResolver;
			StringWriter sw = new StringWriter();
			XmlTextWriter w = new XmlTextWriter(sw);
			try
			{
				while (xir.Read())
					w.WriteNode(xir, false);
			}
			finally
			{
				if (xir != null)
					xir.Close();
				if (w != null)
					w.Close();
			}
			return sw.ToString();
		}

		/// <summary>
		/// Processes inter-document inclusion (xml mode).
		/// </summary>
		/// <param name="href">'href' attr value</param>
		/// <param name="xpointer">'xpointer attr value'</param>
		private bool ProcessInterDocXMLInclusion(string href, string xpointer)
		{
			//Include document as XML                                
			Uri includeLocation = ResolveHref(href);
			if (includeLocation.Fragment != String.Empty)
				throw new XIncludeSyntaxError(Monobjc.Tools.Sdp.Properties.Resources.FragmentIDInHref);
			CheckLoops(includeLocation, xpointer);
			if (_xmlResolver == null)
			{
				//No custom resolver
				if (xpointer != null)
				{
					//Push current reader to the stack
					_readers.Push(_reader);
					//XPointers should be resolved against the acquired infoset, 
					//not the source infoset                                                                                          
					_reader = new XPointerReader(includeLocation.AbsoluteUri,
						CreateAcquiredInfoset(includeLocation),
						xpointer);
				}
				else
				{
					WebResponse wRes;
					Stream stream = GetResource(includeLocation.AbsoluteUri,
						_reader.GetAttribute(_keywords.Accept),
						_reader.GetAttribute(_keywords.AcceptLanguage), out wRes);
					//Push current reader to the stack
					_readers.Push(_reader);
					XmlReaderSettings settings = new XmlReaderSettings();
					settings.XmlResolver = _xmlResolver;
					settings.IgnoreWhitespace = (_whiteSpaceHandling == WhitespaceHandling.None);
					XmlReader r = new XmlBaseAwareXmlReader(wRes.ResponseUri.AbsoluteUri, stream, _nameTable);
					_reader = r;
				}
				return Read();
			}
			else
			{
				//Custom resolver provided, let's ask him
				object resource;
				try
				{
					resource = _xmlResolver.GetEntity(includeLocation, null, null);
				}
				catch (Exception e)
				{
					throw new ResourceException(Monobjc.Tools.Sdp.Properties.Resources.CustomXmlResolverError, e);
				}
				if (resource == null)
					throw new ResourceException(Monobjc.Tools.Sdp.Properties.Resources.CustomXmlResolverReturnedNull);

				//Push current reader to the stack
				_readers.Push(_reader);

				//Ok, we accept Stream, TextReader and XmlReader only                    
				if (resource is Stream)
					resource = new StreamReader((Stream)resource);
				if (xpointer != null)
				{
					if (resource is TextReader)
					{
						//XPointers should be resolved against the acquired infoset, 
						//not the source infoset                                     
						_reader = new XPointerReader(includeLocation.AbsoluteUri,
							CreateAcquiredInfoset(includeLocation, (TextReader)resource),
							xpointer);
					}
					else if (resource is XmlReader)
					{
						XmlReader r = (XmlReader)resource;
						_reader = new XPointerReader(r.BaseURI,
							CreateAcquiredInfoset(r), xpointer);
					}
					else
					{
						//Unsupported type
						throw new ResourceException(String.Format(
							CultureInfo.CurrentCulture,
							Monobjc.Tools.Sdp.Properties.Resources.CustomXmlResolverReturnedUnsupportedType,
							resource.GetType().ToString()));
					}
				}
				else
				{
					//No XPointer   
					if (resource is TextReader)
						_reader = new XmlBaseAwareXmlReader(includeLocation.AbsoluteUri, (TextReader)resource, _nameTable);
					else if (resource is XmlReader)
						_reader = (XmlReader)resource;
					else
					{
						//Unsupported type
						throw new ResourceException(String.Format(
							CultureInfo.CurrentCulture,
							Monobjc.Tools.Sdp.Properties.Resources.CustomXmlResolverReturnedUnsupportedType,
							resource.GetType().ToString()));
					}
				}

				return Read();
			}
		}

		/// <summary>
		/// Process inter-document inclusion as text.
		/// </summary>
		/// <param name="href">'href' attr value</param>        
		private bool ProcessInterDocTextInclusion(string href)
		{
			//Include document as text                            
			string encoding = GetAttribute(_keywords.Encoding);
			Uri includeLocation = ResolveHref(href);
			//No need to check loops when including as text
			//Push current reader to the stack
			_readers.Push(_reader);
			_reader = new TextIncludingReader(includeLocation, encoding,
				_reader.GetAttribute(_keywords.Accept),
				_reader.GetAttribute(_keywords.AcceptLanguage),
				_exposeTextAsCDATA);
			return Read();
		}


		/// <summary>
		/// Checks for inclusion loops.
		/// </summary>        
		private void CheckLoops(Uri url, string xpointer)
		{
			//Check circular inclusion  
			Uri baseUri = _reader.BaseURI == "" ? _topBaseUri : new Uri(_reader.BaseURI);
			if (baseUri.Equals(url))
				ThrowCircularInclusionError(_reader, url);
			foreach (XmlReader r in _readers)
			{
				baseUri = r.BaseURI == "" ? _topBaseUri : new Uri(r.BaseURI);
				if (baseUri.Equals(url))
					ThrowCircularInclusionError(_reader, url);
			}
		}

        private string GetBaseUri()
        {
            if (_reader.BaseURI == String.Empty)
            {
                return String.Empty;
            }
            if (_relativeBaseUri)
            {
                Uri baseUri = new Uri(_reader.BaseURI);
                return _topBaseUri.MakeRelativeUri(baseUri).ToString();
            }
            else
                return _reader.BaseURI;   
        }

		#endregion
	}
}
