using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;

namespace Mvp.Xml.Common
{
    /// <summary>
    /// XHTML writer. Writes XML using the HTML compatibility guidelines defined in the XHTML 1.0 specification.
    /// </summary>
    /// <remarks>
    /// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
    /// </remarks>
    public class XhtmlWriter : XmlWrappingWriter
    {
        private const string XHTML_NAMESPACE = "http://www.w3.org/1999/xhtml";
        private Stack<QName> elementStack;

        /// <summary>
        /// Creates new instance of the <see cref="XhtmlWriter"/>.
        /// </summary>
        /// <param name="baseWriter">Base <see cref="XmlWriter"/> to write to.</param>
        public XhtmlWriter(XmlWriter baseWriter)
            : base(baseWriter)
        {
            this.elementStack = new Stack<QName>();
        }

        /// <summary>
        /// See <see cref="XmlWriter.WriteStartElement(string, string, string)"/>.
        /// </summary>        
        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            elementStack.Push(new QName(localName, ns, prefix));
            base.WriteStartElement(prefix, localName, ns);
        }

        /// <summary>
        /// See <see cref="XmlWriter.WriteEndElement"/>.
        /// </summary>
        public override void WriteEndElement()
        {
            WriteXHMLEndElement(false);
        }

        /// <summary>
        /// See <see cref="XmlWriter.WriteFullEndElement"/>.
        /// </summary>
        public override void WriteFullEndElement()
        {
            WriteXHMLEndElement(true);
        }

        private void WriteXHMLEndElement(bool fullEndTag)
        {
            bool writeFullEndTag = fullEndTag;
            QName elementName = elementStack.Pop();
            if (elementName.NsUri == XHTML_NAMESPACE)
            {
                switch (elementName.Local.ToLower(CultureInfo.InvariantCulture))
                {
                    case "area":
                    case "base":
                    case "basefont":
                    case "br":
                    case "col":
                    case "frame":
                    case "hr":
                    case "img":
                    case "input":
                    case "isindex":
                    case "link":
                    case "meta":
                    case "param":
                        writeFullEndTag = false;
                        break;
                    default:
                        writeFullEndTag = true;
                        break;
                }
            }
            if (writeFullEndTag)
            {
                base.WriteFullEndElement();
            }
            else
            {
                base.WriteEndElement();
            }
        }        
    }
}
