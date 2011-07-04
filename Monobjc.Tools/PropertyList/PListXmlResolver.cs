//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2011 - Laurent Etiemble
//
// Monobjc is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// Monobjc is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Monobjc.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.IO;
using System.Net;
using System.Xml;
using Monobjc.Tools.Properties;

namespace Monobjc.Tools.PropertyList
{
    /// <summary>
    ///   Custom Xml resolver to handle PList file.
    /// </summary>
    internal class PListXmlResolver : XmlResolver
    {
        /// <summary>
        ///   When overridden in a derived class, sets the credentials used to authenticate Web requests.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///   An <see cref = "T:System.Net.ICredentials" /> object. If this property is not set, the value defaults to null; that is, the XmlResolver has no user credentials.
        /// </returns>
        public override ICredentials Credentials
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        ///   When overridden in a derived class, maps a URI to an object containing the actual resource.
        /// </summary>
        /// <param name = "absoluteUri">The URI returned from <see cref = "M:System.Xml.XmlResolver.ResolveUri(System.Uri,System.String)" />.</param>
        /// <param name = "role">The current version does not use this parameter when resolving URIs. This is provided for future extensibility purposes. For example, this can be mapped to the xlink:role and used as an implementation specific argument in other scenarios.</param>
        /// <param name = "ofObjectToReturn">The type of object to return. The current version only returns System.IO.Stream objects.</param>
        /// <returns>
        ///   A System.IO.Stream object or null if a type other than stream is specified.
        /// </returns>
        /// <exception cref = "T:System.Xml.XmlException">
        ///   <paramref name = "ofObjectToReturn" /> is not a Stream type.
        /// </exception>
        /// <exception cref = "T:System.UriFormatException">
        ///   The specified URI is not an absolute URI.
        /// </exception>
        /// <exception cref = "T:System.ArgumentNullException">
        ///   <paramref name = "absoluteUri" /> is null.
        /// </exception>
        /// <exception cref = "T:System.Exception">
        ///   There is a runtime error (for example, an interrupted server connection).
        /// </exception>
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri.Scheme == Uri.UriSchemeHttp)
            {
                String url = absoluteUri.AbsoluteUri;
                if (url.Equals(PListDocument.SYSTEM_ID))
                {
                    return new MemoryStream(Resources.PropertyList_Dtd);
                }
            }

            if (absoluteUri.Scheme == Uri.UriSchemeFile)
            {
                String url = absoluteUri.AbsoluteUri;
                url = url.Replace("%20", " ");
                if (url.EndsWith(PListDocument.PUBLIC_ID.Replace("//", "/")))
                {
                    return new MemoryStream(Resources.PropertyList_Dtd);
                }
                return new FileStream(absoluteUri.AbsolutePath, FileMode.Open, FileAccess.Read);
            }

            return null;
        }
    }
}