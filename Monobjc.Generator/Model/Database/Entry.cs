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
using System.Xml.Serialization;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Model.Database
{
    /// <summary>
    ///   An entry in the database.
    /// </summary>
    [Serializable]
    [XmlRoot("Entry")]
    public class Entry : IEquatable<Entry>, IComparable<Entry>
    {
        private const String HTML_FOLDER = "Html";
        private const String XHTML_FOLDER = "Xhtml";
        private const String XML_FOLDER = "Xml";
        private const String CSHARP_FOLDER = "CSharp";
        private const String GENERATED_FOLDER = "Generated";
        private const String MARKER = "${DOCSET}";

        /// <summary>
        ///   Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        [XmlAttribute("ns")]
        public String Namespace { get; set; }

        /// <summary>
        ///   Gets or sets the nature.
        /// </summary>
        /// <value>The nature.</value>
        [XmlAttribute("nature")]
        public String Nature { get; set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute("name")]
        public String Name { get; set; }

        /// <summary>
        ///   Gets or sets the page style.
        /// </summary>
        /// <value>The page style.</value>
        [XmlAttribute("style")]
        public PageStyle PageStyle { get; set; }

        /// <summary>
        ///   Gets or sets the remote file.
        /// </summary>
        /// <value>The remote file.</value>
        [XmlAttribute("file")]
        public String RemoteFile { get; set; }

        /// <summary>
        ///   Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [XmlElement("url")]
        public string RemoteUrl { get; set; }

        /// <summary>
        ///   Gets or sets the doc set.
        /// </summary>
        /// <value>The doc set.</value>
        [XmlIgnore]
        public String DocSet { get; set; }

        /// <summary>
        ///   Gets the remote URL.
        /// </summary>
        /// <returns></returns>
        public String GetRemoteUrl()
        {
            if (this.RemoteUrl != null && this.RemoteUrl.Contains(MARKER))
            {
                return this.RemoteUrl.Replace(MARKER, this.DocSet);
            }
            return this.RemoteUrl;
        }

        /// <summary>
        ///   Gets the folder of the specified type for this entry.
        /// </summary>
        public String this[EntryFolderType type]
        {
            get
            {
                String folder = null;
                switch (type)
                {
                    case EntryFolderType.Html:
                        folder = HTML_FOLDER;
                        break;
                    case EntryFolderType.Xhtml:
                        folder = XHTML_FOLDER;
                        break;
                    case EntryFolderType.Xml:
                        folder = XML_FOLDER;
                        break;
                    case EntryFolderType.CSharp:
                        folder = CSHARP_FOLDER;
                        break;
                    case EntryFolderType.Generated:
                        folder = GENERATED_FOLDER;
                        break;
                    default:
                        return null;
                }
                String path = PathExtensions.Combine(folder, this.Namespace, this.Nature, this.Name);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                return path;
            }
        }

        /// <summary>
        ///   Gets an entry based on the given descriptor.
        /// </summary>
        /// <param name = "entryDescriptor">The entry descriptor.</param>
        /// <returns></returns>
        public static Entry GetPrototype(String entryDescriptor)
        {
            String[] parts = entryDescriptor.Split('/');
            return new Entry {Namespace = parts[0], Nature = parts[1], Name = parts[2]};
        }

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name = "other">An object to compare with this object.
        /// </param>
        public bool Equals(Entry other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.Namespace, this.Namespace) && Equals(other.Nature, this.Nature) && Equals(other.Name, this.Name);
        }

        /// <summary>
        ///   Determines whether the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name = "obj">The <see cref = "T:System.Object" /> to compare with the current <see cref = "T:System.Object" />. 
        /// </param>
        /// <exception cref = "T:System.NullReferenceException">The <paramref name = "obj" /> parameter is null.
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (Entry))
            {
                return false;
            }
            return this.Equals((Entry) obj);
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref = "T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = (this.Namespace != null ? this.Namespace.GetHashCode() : 0);
                result = (result*397) ^ (this.Nature != null ? this.Nature.GetHashCode() : 0);
                result = (result*397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        ///   Implements the operator ==.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Entry left, Entry right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///   Implements the operator !=.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Entry left, Entry right)
        {
            return !Equals(left, right);
        }

        public int CompareTo(Entry other)
        {
            int result;
            result = this.Namespace.CompareTo(other.Namespace);
            if (result != 0)
            {
                return result;
            }
            result = this.Nature.CompareTo(other.Nature);
            if (result != 0)
            {
                return result;
            }
            result = this.Name.CompareTo(other.Name);
            if (result != 0)
            {
                return result;
            }
            return 0;
        }
    }
}