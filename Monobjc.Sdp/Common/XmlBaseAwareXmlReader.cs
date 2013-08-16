#region using

using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

#endregion

namespace Mvp.Xml.Common
{    
    /// <summary>
    /// Custom <see cref="XmlReader"/> supporting <a href="http://www.w3.org/TR/xmlbase/">XML Base</a>.
    /// </summary>
    /// <remarks>
    /// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
    /// </remarks>
    public class XmlBaseAwareXmlReader : XmlWrappingReader
    {
        #region private

        private XmlBaseState _state = new XmlBaseState();
        private Stack<XmlBaseState> _states = null;

        private static XmlReaderSettings CreateReaderSettings() {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            return settings;
        }

        private static XmlReaderSettings CreateReaderSettings(XmlResolver resolver)
        {
            XmlReaderSettings settings = CreateReaderSettings();            
            settings.XmlResolver = resolver;            
            return settings;
        } 
       
        private static XmlReaderSettings CreateReaderSettings(XmlNameTable nt)
        {
            XmlReaderSettings settings = CreateReaderSettings();            
            settings.NameTable = nt;
            return settings;
        } 

        #endregion

        #region constructors

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given URI.
        /// </summary>        
        public XmlBaseAwareXmlReader(string uri)
            : base(XmlReader.Create(uri, CreateReaderSettings()))
        {
            _state.BaseUri = new Uri(base.BaseURI);
            
        }

		/// <summary>
		/// Creates XmlBaseAwareXmlReader instance for given URI using the given resolver.
		/// </summary>        
		public XmlBaseAwareXmlReader(string uri, XmlResolver resolver)
            : base(XmlReader.Create(uri, CreateReaderSettings(resolver)))
        {            
            _state.BaseUri = new Uri(base.BaseURI);         
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given URI and 
        /// name table.
        /// </summary>        
        public XmlBaseAwareXmlReader(string uri, XmlNameTable nt)
            : base(XmlReader.Create(uri, CreateReaderSettings(nt)))
        {
            _state.BaseUri = new Uri(base.BaseURI);
            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given TextReader.
        /// </summary>        
        public XmlBaseAwareXmlReader(TextReader reader)
            : base(XmlReader.Create(reader, CreateReaderSettings())) 
        {
            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given uri and 
        /// TextReader.
        /// </summary>        
        public XmlBaseAwareXmlReader(string uri, TextReader reader)
            : base(XmlReader.Create(reader, CreateReaderSettings(), uri))
        {
            _state.BaseUri = new Uri(base.BaseURI);
            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given TextReader 
        /// and name table.
        /// </summary>        
        public XmlBaseAwareXmlReader(TextReader reader, XmlNameTable nt)
            : base(XmlReader.Create(reader, CreateReaderSettings(nt))) 
        {
            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given uri, name table
        /// and TextReader.
        /// </summary>        
        public XmlBaseAwareXmlReader(string uri, TextReader reader, XmlNameTable nt)
            : base(XmlReader.Create(reader, CreateReaderSettings(nt), uri))
        {
            _state.BaseUri = new Uri(base.BaseURI);
            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given stream.
        /// </summary>        
        public XmlBaseAwareXmlReader(Stream stream)
            : base(XmlReader.Create(stream, CreateReaderSettings()))
        {
            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given uri and stream.
        /// </summary>        
        public XmlBaseAwareXmlReader(string uri, Stream stream)
            : base(XmlReader.Create(stream, CreateReaderSettings(), uri))
        {
            _state.BaseUri = new Uri(base.BaseURI);
            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given uri and stream.
        /// </summary>        
        public XmlBaseAwareXmlReader(string uri, Stream stream, XmlResolver resolver)
            : base(XmlReader.Create(stream, CreateReaderSettings(resolver), uri))
        {
            _state.BaseUri = new Uri(base.BaseURI);            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given stream 
        /// and name table.
        /// </summary>        
        public XmlBaseAwareXmlReader(Stream stream, XmlNameTable nt)
            : base(XmlReader.Create(stream, CreateReaderSettings(nt))) 
        {
            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given stream,
        /// uri and name table.
        /// </summary>        
        public XmlBaseAwareXmlReader(string uri, Stream stream, XmlNameTable nt)
            : base(XmlReader.Create(stream, CreateReaderSettings(nt), uri))
        {
            _state.BaseUri = new Uri(base.BaseURI);            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given uri and <see cref="XmlReaderSettings"/>.        
        /// </summary>        
        public XmlBaseAwareXmlReader(string uri, XmlReaderSettings settings)
            : base(XmlReader.Create(uri, settings))
        {            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given <see cref="TextReader"/> and <see cref="XmlReaderSettings"/>.        
        /// </summary>        
        public XmlBaseAwareXmlReader(TextReader reader, XmlReaderSettings settings)
            : base(XmlReader.Create(reader, settings))
        {            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given <see cref="Stream"/> and <see cref="XmlReaderSettings"/>.        
        /// </summary>        
        public XmlBaseAwareXmlReader(Stream stream, XmlReaderSettings settings)
            : base(XmlReader.Create(stream, settings))
        {            
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given <see cref="XmlReader"/> and <see cref="XmlReaderSettings"/>.        
        /// </summary>        
        public XmlBaseAwareXmlReader(XmlReader reader, XmlReaderSettings settings)
            : base(XmlReader.Create(reader, settings))
        {
        }        

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given 
        /// <see cref="TextReader"/>, <see cref="XmlReaderSettings"/>
        /// and base uri.
        /// </summary>        
        public XmlBaseAwareXmlReader(TextReader reader, XmlReaderSettings settings, string baseUri)
            : base(XmlReader.Create(reader, settings, baseUri))
        {
        }

        /// <summary>
        /// Creates XmlBaseAwareXmlReader instance for given 
        /// <see cref="Stream"/>, <see cref="XmlReaderSettings"/>
        /// and base uri.
        /// </summary>        
        public XmlBaseAwareXmlReader(Stream stream, XmlReaderSettings settings, string baseUri)
            : base(XmlReader.Create(stream, settings, baseUri))
        {
        }        

        #endregion

        #region XmlReader overrides

        /// <summary>
        /// See <see cref="XmlTextReader.BaseURI"/>.
        /// </summary>
        public override string BaseURI
        {
            get
            {
                return _state.BaseUri == null ? "" : _state.BaseUri.AbsoluteUri;
            }
        }

        /// <summary>
        /// See <see cref="XmlTextReader.Read"/>.
        /// </summary>
        public override bool Read()
        {
            bool baseRead = base.Read();
            if (baseRead)
            {
                if (base.NodeType == XmlNodeType.Element &&
                    base.HasAttributes)
                {
                    string baseAttr = GetAttribute("xml:base");
                    if (baseAttr == null)
                        return baseRead;
                    Uri newBaseUri = null;
                    if (_state.BaseUri == null)
                        newBaseUri = new Uri(baseAttr);
                    else
                        newBaseUri = new Uri(_state.BaseUri, baseAttr);
                    if (_states == null)
                        _states = new Stack<XmlBaseState>();
                    //Push current state and allocate new one
                    _states.Push(_state);
                    _state = new XmlBaseState(newBaseUri, base.Depth);
                }
                else if (base.NodeType == XmlNodeType.EndElement)
                {
                    if (base.Depth == _state.Depth && _states != null && _states.Count > 0)
                    {
                        //Pop previous state
                        _state = _states.Pop();
                    }
                }
            }
            return baseRead;
        }

        #endregion
    }

    internal class XmlBaseState
    {
        public XmlBaseState() { }

        public XmlBaseState(Uri baseUri, int depth)
        {
            this.BaseUri = baseUri;
            this.Depth = depth;
        }

        public Uri BaseUri;
        public int Depth;
    }
}
