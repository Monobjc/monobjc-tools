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
	/// xpath1() scheme based XPointer pointer part.
	/// </summary>
	internal class XPath1SchemaPointerPart : PointerPart
	{
		#region private members

		private string _xpath;

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

		public static XPath1SchemaPointerPart ParseSchemaData(XPointerLexer lexer)
		{
			XPath1SchemaPointerPart part = new XPath1SchemaPointerPart();
			try
			{
				part._xpath = lexer.ParseEscapedData();
			}
			catch (Exception e)
			{
				throw new XPointerSyntaxException(String.Format(
					CultureInfo.CurrentCulture,
					Monobjc.Tools.Sdp.Properties.Resources.SyntaxErrorInXPath1SchemeData,
					e.Message));
			}
			return part;
		}

		#endregion
	}
}
