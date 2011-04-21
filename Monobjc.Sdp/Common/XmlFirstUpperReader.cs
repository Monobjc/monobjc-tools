#region using

using System;
using System.IO;
using System.Xml;

#endregion using

namespace Mvp.Xml.Common
{
	/// <summary>
	/// Implements an <see cref="XmlTextReader"/> that turns the 
	/// first letter of incoming elements and attributes into uppercase.
	/// </summary>
	/// <remarks>
	/// To be used in conjunction with <see cref="XmlFirstLowerWriter"/> for 
	/// serialization.
	/// <para>Author: Daniel Cazzulino, <a href="http://clariusconsulting.net/kzu">blog</a></para>
	/// See http://weblogs.asp.net/cazzu/archive/2004/05/10/129106.aspx.
	/// </remarks>
	public class XmlFirstUpperReader : XmlTextReader
	{
		#region Ctors

		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(Stream input) : base(input) { }
		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(TextReader input) : base(input) { }
		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(string url) : base(url) { }
		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(Stream input, XmlNameTable nt) : base(input, nt) { }
		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(TextReader input, XmlNameTable nt) : base(input, nt) { }
		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(string url, Stream input) : base(url, input) { }
		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(string url, TextReader input) : base(url, input) { }
		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(string url, XmlNameTable nt) : base(url, nt) { }
		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context) : base(xmlFragment, fragType, context) { }
		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(string url, Stream input, XmlNameTable nt) : base(url, input, nt) { }
		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(string url, TextReader input, XmlNameTable nt) : base(url, input, nt) { }
		/// <summary>
		/// See <see cref="XmlTextReader"/> constructor overloads.
		/// </summary>
		public XmlFirstUpperReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context) : base(xmlFragment, fragType, context) { }

		#endregion Ctors

		#region Private methods

		private string MakeFirstUpper(string name)
		{
			// Don't process empty strings.
			if (name.Length == 0) return name;
			// If the first is already upper, don't process.
			if (Char.IsUpper(name[0])) return name;
			// If there's just one char, make it lower directly.
			if (name.Length == 1) return name.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
			// Finally, modify and create a string. 
			Char[] letters = name.ToCharArray();
			letters[0] = Char.ToUpper(letters[0], System.Globalization.CultureInfo.CurrentUICulture);
			return NameTable.Add(new string(letters));
		}

		#endregion Private methods

		#region Properties

		/// <summary>See <see cref="XmlReader.this[string, string]"/></summary>
		public override string this[string name, string namespaceURI]
		{
			get
			{
				return base[
					NameTable.Add(XmlFirstLowerWriter.MakeFirstLower(name)), namespaceURI];
			}
		}

		/// <summary>See <see cref="XmlReader.this[string]"/></summary>
		public override string this[string name]
		{
			get { return this[name, String.Empty]; }
		}

		/// <summary>See <see cref="XmlReader.LocalName"/></summary>
		public override string LocalName
		{
			get
			{
				// Capitalize elements and attributes.
				if (base.NodeType == XmlNodeType.Element ||
					base.NodeType == XmlNodeType.EndElement ||
					base.NodeType == XmlNodeType.Attribute)
				{
					return base.NamespaceURI == XmlNamespaces.XmlNs ?
						// Except if the attribute is a namespace declaration
						base.LocalName : MakeFirstUpper(base.LocalName);
				}
				return base.LocalName;
			}
		}

		/// <summary>See <see cref="XmlReader.Name"/></summary>
		public override string Name
		{
			get
			{
				// Again, if this is a NS declaration, pass as-is.
				if (base.NamespaceURI == XmlNamespaces.XmlNs)
					return base.Name;
				// If there's no prefix, capitalize it directly.
				if (base.Name.IndexOf(":") == -1)
					return MakeFirstUpper(base.Name);
				else
				{
					// Turn local name into upper, not the prefix.
					string name = base.Name.Substring(0, base.Name.IndexOf(":") + 1);
					name += MakeFirstUpper(base.Name.Substring(base.Name.IndexOf(":") + 1));
					return NameTable.Add(name);
				}
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>See <see cref="XmlReader.MoveToAttribute(string, string)"/></summary>
		public override bool MoveToAttribute(string name, string ns)
		{
			return base.MoveToAttribute(
				NameTable.Add(XmlFirstLowerWriter.MakeFirstLower(name)), ns);
		}

		#endregion Methods
	}
}
