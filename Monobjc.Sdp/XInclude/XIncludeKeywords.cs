#region using

using System;
using System.Xml;

#endregion


namespace Mvp.Xml.XInclude
{
	/// <summary>
	/// XInclude syntax keyword collection.	
	/// </summary>
	/// <author>Oleg Tkachenko, http://www.xmllab.net</author>
	internal class XIncludeKeywords
	{

		#region constructors

		public XIncludeKeywords(XmlNameTable nt)
		{
			nameTable = nt;
			//Preload some keywords
			_XIncludeNamespace = nameTable.Add(s_XIncludeNamespace);
			_OldXIncludeNamespace = nameTable.Add(s_OldXIncludeNamespace);
			_Include = nameTable.Add(s_Include);
			_Href = nameTable.Add(s_Href);
			_Parse = nameTable.Add(s_Parse);
		}

		#endregion

		#region private constants

		//
		// Keyword strings
		private const string s_XIncludeNamespace = "http://www.w3.org/2001/XInclude";
		private const string s_OldXIncludeNamespace = "http://www.w3.org/2003/XInclude";
		private const string s_Include = "include";
		private const string s_Href = "href";
		private const string s_Parse = "parse";
		private const string s_Xml = "xml";
		private const string s_Text = "text";
		private const string s_Xpointer = "xpointer";
		private const string s_Accept = "accept";
		private const string s_AcceptLanguage = "accept-language";
		private const string s_Encoding = "encoding";
		private const string s_Fallback = "fallback";
		private const string s_XmlNamespace = "http://www.w3.org/XML/1998/namespace";
		private const string s_Base = "base";
		private const string s_XmlBase = "xml:base";
		private const string s_Lang = "lang";
		private const string s_XmlLang = "xml:lang";

		#endregion

		#region private fields

		private XmlNameTable nameTable;

		//
		// Properties
		private string _XIncludeNamespace;
		private string _OldXIncludeNamespace;
		private string _Include;
		private string _Href;
		private string _Parse;
		private string _Xml;
		private string _Text;
		private string _Xpointer;
		private string _Accept;
		private string _AcceptLanguage;
		private string _Encoding;
		private string _Fallback;
		private string _XmlNamespace;
		private string _Base;
		private string _XmlBase;
		private string _Lang;
		private string _XmlLang;

		#endregion

		#region public props

		// http://www.w3.org/2003/XInclude
		public string XIncludeNamespace
		{
			get { return _XIncludeNamespace; }
		}

		// http://www.w3.org/2001/XInclude
		public string OldXIncludeNamespace
		{
			get { return _OldXIncludeNamespace; }
		}

		// include
		public string Include
		{
			get { return _Include; }
		}

		// href
		public string Href
		{
			get { return _Href; }
		}

		// parse
		public string Parse
		{
			get { return _Parse; }
		}

		// xml
		public string Xml
		{
			get
			{
				if (_Xml == null)
					_Xml = nameTable.Add(s_Xml);
				return _Xml;
			}
		}

		// text
		public string Text
		{
			get
			{
				if (_Text == null)
					_Text = nameTable.Add(s_Text);
				return _Text;
			}
		}

		// xpointer
		public string Xpointer
		{
			get
			{
				if (_Xpointer == null)
					_Xpointer = nameTable.Add(s_Xpointer);
				return _Xpointer;
			}
		}

		// accept
		public string Accept
		{
			get
			{
				if (_Accept == null)
					_Accept = nameTable.Add(s_Accept);
				return _Accept;
			}
		}

		// accept-language
		public string AcceptLanguage
		{
			get
			{
				if (_AcceptLanguage == null)
					_AcceptLanguage = nameTable.Add(s_AcceptLanguage);
				return _AcceptLanguage;
			}
		}

		// encoding
		public string Encoding
		{
			get
			{
				if (_Encoding == null)
					_Encoding = nameTable.Add(s_Encoding);
				return _Encoding;
			}
		}

		// fallback
		public string Fallback
		{
			get
			{
				if (_Fallback == null)
					_Fallback = nameTable.Add(s_Fallback);
				return _Fallback;
			}
		}

		// Xml namespace
		public string XmlNamespace
		{
			get
			{
				if (_XmlNamespace == null)
					_XmlNamespace = nameTable.Add(s_XmlNamespace);
				return _XmlNamespace;
			}
		}

		// Base
		public string Base
		{
			get
			{
				if (_Base == null)
					_Base = nameTable.Add(s_Base);
				return _Base;
			}
		}

		// xml:base
		public string XmlBase
		{
			get
			{
				if (_XmlBase == null)
					_XmlBase = nameTable.Add(s_XmlBase);
				return _XmlBase;
			}
		}

		// Lang
		public string Lang
		{
			get
			{
				if (_Lang == null)
					_Lang = nameTable.Add(s_Lang);
				return _Lang;
			}
		}

		// xml:lang
		public string XmlLang
		{
			get
			{
				if (_XmlLang == null)
					_XmlLang = nameTable.Add(s_XmlLang);
				return _XmlLang;
			}
		}

		#endregion

		#region public methods

		// Comparison
		public static bool Equals(string keyword1, string keyword2)
		{
			return (object)keyword1 == (object)keyword2;
		}

		#endregion
	}
}
