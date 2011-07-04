#region using

using System;
using System.Xml;
using System.Xml.XPath;

using Mvp.Xml.Common.XPath;
using System.Globalization;

#endregion

namespace Mvp.Xml.XPointer
{
	/// <summary>
	/// xpointer() scheme based XPointer pointer part.
	/// </summary>
	internal class XPointerSchemaPointerPart : PointerPart
	{

		#region private members

		private string _xpath;

		#endregion

		#region ctors
		public XPointerSchemaPointerPart(string xpath)
		{
			_xpath = xpath;
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
			try
			{
				return XPathCache.Select(_xpath, doc, nm);
			}
			catch
			{
				return null;
			}
		}

		#endregion

		#region parser

		public static XPointerSchemaPointerPart ParseSchemaData(XPointerLexer lexer)
		{
			try
			{
				return new XPointerSchemaPointerPart(lexer.ParseEscapedData());
			}
			catch (Exception e)
			{
				throw new XPointerSyntaxException(String.Format(CultureInfo.CurrentCulture, Monobjc.Tools.Sdp.Properties.Resources.SyntaxErrorInXPointerSchemeData, e.Message));
			}
		}

		#endregion
	}
}
