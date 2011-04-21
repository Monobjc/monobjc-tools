#region using

using System;
using System.IO;
using System.Text;
using System.Xml;

#endregion using

namespace Mvp.Xml.Common
{
	/// <summary>
	/// Implements an <see cref="XmlWriter"/> that turns the 
	/// first letter of outgoing elements and attributes into lowercase.
	/// </summary>
	/// <remarks>
	/// To be used in conjunction with <see cref="XmlFirstUpperReader"/>.
	/// <para>Author: Daniel Cazzulino, <a href="http://clariusconsulting.net/kzu">blog</a></para>
	/// See http://weblogs.asp.net/cazzu/archive/2004/05/10/129106.aspx.
	/// </remarks>
	public class XmlFirstLowerWriter : XmlTextWriter
	{
		#region Fields & Ctor

		/// <summary>
		/// See <see cref="XmlTextWriter"/> ctors.
		/// </summary>
		public XmlFirstLowerWriter(TextWriter w)
			: base(w)
		{
		}

		/// <summary>
		/// See <see cref="XmlTextWriter"/> ctors.
		/// </summary>
		public XmlFirstLowerWriter(Stream w, Encoding encoding)
			: base(w, encoding)
		{
		}

		/// <summary>
		/// See <see cref="XmlTextWriter"/> ctors.
		/// </summary>
		public XmlFirstLowerWriter(string filename, Encoding encoding)
			: base(filename, encoding)
		{
		}

		#endregion Fields & Ctor

		#region MakeFirstLower

		internal static string MakeFirstLower(string name)
		{
			// Don't process empty strings.
			if (name.Length == 0) return name;
			// If the first is already lower, don't process.
			if (Char.IsLower(name[0])) return name;
			// If there's just one char, make it lower directly.
			if (name.Length == 1) return name.ToLower(System.Globalization.CultureInfo.CurrentCulture);
			// Finally, modify and create a string. 
			Char[] letters = name.ToCharArray();
			letters[0] = Char.ToLower(letters[0], System.Globalization.CultureInfo.CurrentCulture);
			return new string(letters);
		}

		#endregion MakeFirstUpper

		#region Methods

		/// <summary>
		/// See <see cref="XmlWriter.WriteQualifiedName"/>.
		/// </summary>
		public override void WriteQualifiedName(string localName, string ns)
		{
			base.WriteQualifiedName(MakeFirstLower(localName), ns);
		}

		/// <summary>
		/// See <see cref="XmlWriter.WriteStartAttribute(string, string, string)"/>.
		/// </summary>
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			base.WriteStartAttribute(prefix, MakeFirstLower(localName), ns);
		}


		/// <summary>
		/// See <see cref="XmlWriter.WriteStartElement(string, string, string)"/>.
		/// </summary>
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			base.WriteStartElement(prefix, MakeFirstLower(localName), ns);
		}

		#endregion Methods
	}
}
