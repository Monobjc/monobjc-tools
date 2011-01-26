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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Monobjc.Tools.PropertyList
{
    /// <summary>
    ///   Represents a PList document.
    /// </summary>
    public class PListDocument
    {
        internal const String PUBLIC_ID = "-//Apple Computer//DTD PLIST 1.0//EN";
        internal const String SYSTEM_ID = "http://www.apple.com/DTDs/PropertyList-1.0.dtd";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListDocument" /> class.
        /// </summary>
        public PListDocument() : this(false) {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PListDocument" /> class.
        /// </summary>
        /// <param name = "empty">if set to <c>true</c>, don't add a PList node.</param>
        private PListDocument(bool empty)
        {
            if (!empty)
            {
                this.Root = new PList();
            }
        }

        /// <summary>
        ///   Gets the root of this document as a <see cref = "PList" /> instance.
        /// </summary>
        /// <value>The root.</value>
        public PList Root { get; private set; }

        /// <summary>
        ///   Loads a <see cref = "PListDocument" /> from a Xml content.
        /// </summary>
        /// <param name = "content">The content.</param>
        /// <returns>An instance of <see cref = "PListDocument" /></returns>
        public static PListDocument LoadFromXml(String content)
        {
            XmlReaderSettings settings = new XmlReaderSettings {CloseInput = true, ProhibitDtd = false, XmlResolver = new PListXmlResolver()};
            using (XmlReader reader = XmlReader.Create(new StringReader(content), settings))
            {
                return LoadFromXml(reader);
            }
        }

        /// <summary>
        ///   Loads a <see cref = "PListDocument" /> from a Xml content.
        /// </summary>
        /// <param name = "reader">The reader.</param>
        /// <returns>An instance of <see cref = "PListDocument" /></returns>
        public static PListDocument LoadFromXml(XmlReader reader)
        {
            PListDocument document = new PListDocument(true);

            // The XML parsing is based on a top-down stack exploration
            StringBuilder tempContent = new StringBuilder();
            Stack<PListItemBase> stack = new Stack<PListItemBase>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            String name = reader.Name;
                            bool empty = reader.IsEmptyElement;

                            // Collect all the attributes
                            Dictionary<String, String> attributes = new Dictionary<String, String>();
                            if (reader.HasAttributes)
                            {
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);
                                    attributes.Add(reader.Name, reader.Value);
                                }
                            }

                            // Create an object corresponding to the node
                            PListItemBase item = Create(name);
                            if (String.Equals("plist", name))
                            {
                                // The "plist" node has a special treatment
                                document.Root = item as PList;
                            }
                            else
                            {
                                // Append the node to the current parent node
                                PListItemBase current = stack.Peek();
                                current.AppendChild(item);
                            }

                            // Push the new node in the context stack
                            stack.Push(item);

                            // Prepare the content collector for node with text
                            tempContent = new StringBuilder();

                            // An empty node is pop right-away from the context stack
                            if (empty)
                            {
                                stack.Pop();
                                item.SetValue(tempContent.ToString());
                            }
                        }
                        break;

                    case XmlNodeType.EndElement:
                        {
                            // When the node ends, it is pop from the context stack and its content is set
                            PListItemBase item = stack.Pop();
                            item.SetValue(tempContent.ToString());
                        }
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                        {
                            // Collect text content between start and end of the node
                            String value = reader.Value;
                            tempContent.Append(value);
                        }
                        break;
                }
            }
            return document;
        }

        /// <summary>
        ///   Loads a <see cref = "PListDocument" /> from a file.
        /// </summary>
        /// <param name = "path">The path.</param>
        /// <returns>An instance of <see cref = "PListDocument" /></returns>
        public static PListDocument LoadFromFile(String path)
        {
            XmlReaderSettings settings = new XmlReaderSettings {CloseInput = true, ProhibitDtd = false, XmlResolver = new PListXmlResolver()};
            using (XmlReader reader = XmlReader.Create(path, settings))
            {
                return LoadFromXml(reader);
            }
        }

        /// <summary>
        ///   Writes this instance to Xml stream.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartDocument();
            this.Root.WriteXml(writer);
            writer.WriteEndDocument();
        }

        /// <summary>
        ///   Writes this instance to a file.
        /// </summary>
        /// <param name = "path">The path.</param>
        public void WriteToFile(String path)
        {
            XmlWriterSettings settings = new XmlWriterSettings {CloseOutput = true, Indent = true};
            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                this.WriteXml(writer);
            }
        }

        /// <summary>
        ///   Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///   A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            using (XmlWriter xmlWriter = XmlWriter.Create(builder))
            {
                this.WriteXml(xmlWriter);
            }
            return builder.ToString();
        }

        private static PListItemBase Create(String name)
        {
            switch (name)
            {
                case "plist":
                    return new PList();

                case "array":
                    return new PListArray();
                case "data":
                    return new PListData();
                case "date":
                    return new PListDate();
                case "dict":
                    return new PListDict();
                case "real":
                    return new PListReal();
                case "integer":
                    return new PListInteger();
                case "string":
                    return new PListString();
                case "true":
                    return new PListBoolean(true);
                case "false":
                    return new PListBoolean(false);

                    // Internal helper class for dictionary
                case "key":
                    return new PListKey();

                default:
                    return null;
            }
        }
    }
}