#region using

using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Globalization;

#endregion

namespace Mvp.Xml.XPointer
{
	/// <summary>
	/// xmlns() scheme based <see cref="XPointer"/> pointer part.
	/// </summary>
	internal class XmlnsSchemaPointerPart : PointerPart
	{

		#region private members

		private string _prefix, _uri;

		#endregion

		#region constructors

		/// <summary>
		/// Creates xmlns() scheme pointer part with given
		/// namespace prefix and namespace URI. 
		/// </summary>
		/// <param name="prefix">Namespace prefix</param>
		/// <param name="uri">Namespace URI</param>
		public XmlnsSchemaPointerPart(string prefix, string uri)
		{
			_prefix = prefix;
			_uri = uri;
		}

		#endregion


		#region properties
		public string Prefix
		{
			get { return _prefix; }
			set { _prefix = value; }
		}
		public string Uri
		{
			get { return _uri; }
			set { _uri = value; }
		}
		#endregion


		#region PointerPart overrides

		/// <summary>
		/// Evaluates <see cref="XPointer"/> pointer part and returns pointed nodes.
		/// </summary>
		/// <param name="doc">Document to evaluate pointer part on</param>
		/// <param name="nm">Namespace manager</param>
		/// <returns>Pointed nodes</returns>
		public override XPathNodeIterator Evaluate(XPathNavigator doc, XmlNamespaceManager nm)
		{
			nm.AddNamespace(_prefix, _uri);
			return null;
		}

		#endregion

		#region parser

		public static XmlnsSchemaPointerPart ParseSchemaData(XPointerLexer lexer)
		{
			//[1]   	XmlnsSchemeData	   ::=   	 NCName S? '=' S? EscapedNamespaceName
			//[2]   	EscapedNamespaceName	   ::=   	EscapedData*                      	                    
			//Read prefix as NCName
			lexer.NextLexeme();
			if (lexer.Kind != XPointerLexer.LexKind.NCName)
			{
				Debug.WriteLine(Monobjc.Tools.Sdp.Properties.Resources.InvalidTokenInXmlnsSchemeWhileNCNameExpected);
				return null;
			}
			string prefix = lexer.NCName;
			lexer.SkipWhiteSpace();
			lexer.NextLexeme();
			if (lexer.Kind != XPointerLexer.LexKind.Eq)
			{				
				Debug.WriteLine(Monobjc.Tools.Sdp.Properties.Resources.InvalidTokenInXmlnsSchemeWhileEqualsSignExpected);
				return null;
			}
			lexer.SkipWhiteSpace();
			string nsURI;
			try
			{
				nsURI = lexer.ParseEscapedData();
			}
			catch (Exception e)
			{
				throw new XPointerSyntaxException(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.SyntaxErrorInXmlnsSchemeData, e.Message));
			}
			return new XmlnsSchemaPointerPart(prefix, nsURI);
		}
		#endregion
	}
}
