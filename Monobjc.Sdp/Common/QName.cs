using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Common {
    /// <summary>
    /// XML Qualified Name (<a href="http://www.w3.org/TR/REC-xml-names/#dt-qualname">http://www.w3.org/TR/REC-xml-names/#dt-qualname</a>).
    /// Holds local name, prefix and namespace URI.
    /// </summary>
    /// <remarks><para>Is immutable.</para>
    /// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
    /// </remarks>    
    public class QName {
        private string local;
        private string nsUri;
        private string prefix;

        /// <summary>
        /// Creates new <see cref="QName"/> instance using specified local name, namespace URI and prefix.
        /// </summary>
        /// <param name="local">Local name</param>
        /// <param name="nsUri">Namespace URI</param>
        /// <param name="prefix">Prefix</param>
        public QName(string local, string nsUri, string prefix) {
            this.local = local;
            this.nsUri = nsUri;
            this.prefix = prefix;
        }

        /// <summary>
        /// Gets local name.
        /// </summary>
        public string Local { get { return this.local; } }
        /// <summary>
        /// Gets namespace URI.
        /// </summary>
        public string NsUri { get { return this.nsUri; } }
        /// <summary>
        /// Gets prefix.
        /// </summary>
        public string Prefix { get { return this.prefix; } }

        /// <summary>
        /// Overrides <see cref="object.ToString()"/> to create XML QName serialization in prefix:localname form.
        /// </summary>
        /// <returns>XML QName serialization in prefix:localname form.</returns>
        public override string ToString() {
            return (Prefix != null && Prefix.Length != 0) ? (Prefix + ':' + Local) : Local;
        }
    }
}
