using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Reflection;
using System.Security.Permissions;
using System.Xml.XPath;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace Mvp.Xml.Common.XPath
{
	/// <summary>
	/// <see cref="XmlWriter"/> that can produce an 
	/// <see cref="XPathDocument"/> without the need 
	/// to parse XML.
	/// </summary>
	/// <remarks>
	/// Using this writer whenever you need to perform 
	/// fast chained transformations. Using this class 
	/// avoids reparsing of the intermediate output, 
	/// yielding lower memory and CPU usage.
	/// <para>
	/// Future versions of <see cref="XPathDocument"/> may 
	/// offer this functionality out of the box.
	/// </para>
	/// <para>
	/// <b>Important: </b> this class requires unrestricted 
	/// member access reflection permissions in order to run. 
	/// If the Mvp.Xml assembly is installed in the GAC, it will 
	/// run without problems. The Mvp.Xml allows partially trusted 
	/// callers, so only this assembly needs to be GAC'ed. 
	/// </para>
	/// <para>
	/// Alternatively, you can configure .NET security policy 
	/// to allow the appropriate permission (see <see cref="ReflectionPermission"/>, 
	/// <see cref="PermissionState.Unrestricted"/> and 
	/// <see cref="ReflectionPermissionFlag.MemberAccess"/>).
	/// </para>
	/// </remarks>
	public class XPathDocumentWriter : XmlWrappingWriter
	{
		static ConstructorInfo defaultConstructor;
		static MethodInfo loadWriterMethod;

		XPathDocument document;
		bool hasRoot;

		static XPathDocumentWriter()
		{
			ReflectionPermission perm = new ReflectionPermission(PermissionState.Unrestricted);
			perm.Flags = ReflectionPermissionFlag.MemberAccess;

			try
			{
				perm.Assert();

				Type t = typeof(XPathDocument);
				defaultConstructor = t.GetConstructor(
					BindingFlags.NonPublic | BindingFlags.Instance, null,
					Type.EmptyTypes,
					new ParameterModifier[0]);
				Debug.Assert(defaultConstructor != null, ".NET Framework implementation changed");

				loadWriterMethod = t.GetMethod("LoadFromWriter", BindingFlags.Instance | BindingFlags.NonPublic);
				Debug.Assert(loadWriterMethod != null, ".NET Framework implementation changed");
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		/// <summary>
		/// Initializes a new instance of <see cref="XPathDocumentWriter"/>.
		/// </summary>
		public XPathDocumentWriter() : this(String.Empty) { }

		/// <summary>
		/// Initializes a new instance of <see cref="XPathDocumentWriter"/> using 
		/// the given <paramref name="baseUri"/>.
		/// </summary>
		/// <param name="baseUri">Base URI to used to construct the <see cref="XPathDocument"/>.</param>
		public XPathDocumentWriter(string baseUri)
			: base(XmlWriter.Create(new StringWriter()))
		{
			Guard.ArgumentNotNull(baseUri, "baseUri");

			document = CreateDocument();
			base.BaseWriter = GetWriter(document, baseUri);
		}

		/// <summary>
		/// See <see cref="XmlWriter.WriteStartElement(string, string, string)"/>.
		/// </summary>
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			base.WriteStartElement(prefix, localName, ns);
			if (!hasRoot) hasRoot = true;
		}

		/// <summary>
		/// Closes the writer and retrieves the created <see cref="XPathDocument"/>.
		/// </summary>
		public new XPathDocument Close()
		{
			if (!hasRoot) throw new XmlException(Monobjc.Tools.Sdp.Properties.Resources.Xml_MissingRoot);

			base.Close();
			return document;
		}

		/// <summary>
		/// Reports <see cref="System.Xml.WriteState.Start"/> always, 
		/// as the underlying writer is always writing to 
		/// the <see cref="XPathDocument"/> directly.
		/// </summary>
		public override WriteState WriteState
		{
			get { return WriteState.Start; }
		}

		/// <summary>
		/// Never disposes the underlying writer as it's not 
		/// keeping in-memory state.
		/// </summary>
		/// <remarks>
		/// This override also avoids an exception that is thrown 
		/// otherwise by the base <see cref="System.Xml.XmlRawWriter"/> 
		/// being used internally.
		/// </remarks>
		protected override void Dispose(bool disposing)
		{
			Close();
			//base.Dispose(disposing);
		}

		private static XPathDocument CreateDocument()
		{
			return (XPathDocument)defaultConstructor.Invoke(new object[0]);
		}

		private static XmlWriter GetWriter(XPathDocument document, string baseUri)
		{
			return (XmlWriter)loadWriterMethod.Invoke(document, new object[] { 0, baseUri });
		}
	}
}
