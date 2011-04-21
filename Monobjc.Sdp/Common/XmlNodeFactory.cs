#region using

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

using Mvp.Xml.Common.XPath;

#endregion

namespace Mvp.Xml.Common
{
	/// <summary>
	/// Creates <see cref="XmlNode"/> wrapper instances 
	/// for different XML APIs, for use in XML serialization.
	/// </summary>
	/// <remarks>
	/// <see cref="XmlNode"/> instances returned by this factory only 
	/// support the <see cref="XmlNode.WriteTo"/> and <see cref="XmlNode.WriteContentTo"/> 
	/// methods, as they are intended for use only for serialization, and to avoid 
	/// <see cref="XmlDocument"/> loading for fast performance. All other members 
	/// will throw an <see cref="NotSupportedException"/>.
	/// <para>Author: Daniel Cazzulino, <a href="http://clariusconsulting.net/kzu">blog</a></para>
	/// See: http://weblogs.asp.net/cazzu/archive/2004/05/31/144922.aspx and 
	/// http://weblogs.asp.net/cazzu/posts/XmlMessagePerformance.aspx.
	/// </remarks>
	public class XmlNodeFactory
	{
		private XmlNodeFactory() { }

		#region Create overloads

		/// <summary>
		/// Creates an <see cref="XmlNode"/> wrapper for any object, 
		/// to be serialized through the <see cref="XmlSerializer"/>.
		/// </summary>
		/// <param name="value">The object to wrap.</param>
		/// <returns>A node that can only be used for XML serialization.</returns>
		public static XmlNode Create(object value)
		{
			return new ObjectNode(value);
		}

		/// <summary>
		/// Creates an <see cref="XmlNode"/> serializable 
		/// wrapper for an <see cref="XPathNavigator"/>.
		/// </summary>
		/// <param name="navigator">The navigator to wrap.</param>
		/// <returns>A node that can only be used for XML serialization.</returns>
		public static XmlNode Create(XPathNavigator navigator)
		{
			return new XPathNavigatorNode(navigator);
		}

		/// <summary>
		/// Creates an <see cref="XmlNode"/> serializable 
		/// wrapper for an <see cref="XmlReader"/>.
		/// </summary>
		/// <param name="reader">The reader to wrap.</param>
		/// <returns>A node that can only be used for XML serialization.</returns>
		/// <remarks>
		/// After serialization, the reader is automatically closed.
		/// </remarks>
		public static XmlNode Create(XmlReader reader)
		{
			return Create(reader, false);
		}

		/// <summary>
		/// Creates an <see cref="XmlDocument"/> serializable 
		/// wrapper for an <see cref="XPathNavigator"/>.
		/// </summary>
		/// <param name="reader">The reader to wrap.</param>
		/// <param name="defaultAttrs">Whether default attributes should be serialized.</param>
		/// <returns>A document that can only be used for XML serialization.</returns>
		/// <remarks>
		/// After serialization, the reader is automatically closed.
		/// </remarks>
		public static XmlNode Create(XmlReader reader, bool defaultAttrs)
		{
			return new XmlReaderNode(reader, defaultAttrs);
		}

		#endregion Create overloads

		#region SerializableNode

		private abstract class SerializableNode : XmlElement
		{
			public SerializableNode() : base("", "dummy", "", new XmlDocument()) { }

			public override XmlNode AppendChild(XmlNode newChild)
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override XmlAttributeCollection Attributes
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override string BaseURI
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override XmlNodeList ChildNodes
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override XmlNode Clone()
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override XmlNode CloneNode(bool deep)
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override XmlNode FirstChild
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override string GetNamespaceOfPrefix(string prefix)
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override string GetPrefixOfNamespace(string namespaceURI)
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override bool HasChildNodes
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override string InnerText
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
				set { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override string InnerXml
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
				set { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override XmlNode InsertAfter(XmlNode newChild, XmlNode refChild)
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override XmlNode InsertBefore(XmlNode newChild, XmlNode refChild)
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override bool IsReadOnly
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override XmlNode LastChild
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override string LocalName
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override string Name
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override string NamespaceURI
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override XmlNode NextSibling
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override XmlNodeType NodeType
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override void Normalize()
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override string OuterXml
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override XmlDocument OwnerDocument
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override XmlNode ParentNode
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override string Prefix
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
				set { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override XmlNode PrependChild(XmlNode newChild)
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override XmlNode PreviousSibling
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override void RemoveAll()
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override XmlNode RemoveChild(XmlNode oldChild)
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild)
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override bool Supports(string feature, string version)
			{
				throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM);
			}

			public override XmlElement this[string localname, string ns]
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override XmlElement this[string name]
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override string Value
			{
				get { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
				set { throw new NotSupportedException(Monobjc.Tools.Sdp.Properties.Resources.XmlDocumentFactory_NotImplementedDOM); }
			}

			public override void WriteContentTo(XmlWriter w)
			{
				WriteTo(w);
			}

			public abstract override void WriteTo(XmlWriter w);
		}

		#endregion SerializableNode

		#region XPathNavigatorNode

		private class XPathNavigatorNode : SerializableNode
		{
			private XPathNavigator _navigator;

			public XPathNavigatorNode() { }

			public XPathNavigatorNode(XPathNavigator navigator)
			{
				_navigator = navigator;
			}

			public override void WriteTo(XmlWriter w)
			{
				w.WriteNode(_navigator.ReadSubtree(), false);
			}
		}

		#endregion XPathNavigatorNode

		#region XmlReaderNode

		private class XmlReaderNode : SerializableNode
		{
			private XmlReader _reader;
			private bool _default;

			public XmlReaderNode() { }

			public XmlReaderNode(XmlReader reader, bool defaultAttrs)
			{
				_reader = reader;
				_reader.MoveToContent();
				_default = defaultAttrs;
			}

			public override void WriteTo(XmlWriter w)
			{
				w.WriteNode(_reader, _default);
				_reader.Close();
			}
		}

		#endregion XmlReaderNode

		#region ObjectNode

		private class ObjectNode : SerializableNode
		{
			private object serializableObject;

			public ObjectNode() { }

			public ObjectNode(object serializableObject)
			{
				this.serializableObject = serializableObject;
			}

			public override void WriteTo(XmlWriter w)
			{
				XmlSerializer ser = new XmlSerializer(serializableObject.GetType());
				ser.Serialize(w, serializableObject);
			}
		}

		#endregion XmlReaderNode
	}
}
