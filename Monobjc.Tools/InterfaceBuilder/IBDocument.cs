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

namespace Monobjc.Tools.InterfaceBuilder
{
    /// <summary>
    ///   Represents an Interface Builder document.
    /// </summary>
    public class IBDocument
    {
        /// <summary>
        ///   Gets the root of this document as a <see cref = "IBArchive" /> instance.
        /// </summary>
        /// <value>The root.</value>
        public IBArchive Root { get; private set; }

        /// <summary>
        ///   Loads a <see cref = "IBDocument" /> from a Xml content.
        /// </summary>
        /// <param name = "content">The content.</param>
        /// <returns>An instance of <see cref = "IBDocument" /></returns>
        public static IBDocument LoadFromXml(String content)
        {
            XmlReaderSettings settings = new XmlReaderSettings {CloseInput = true, ProhibitDtd = false, XmlResolver = new IBXmlResolver()};
            using (XmlReader reader = XmlReader.Create(new StringReader(content), settings))
            {
                return LoadFromXml(reader);
            }
        }

        /// <summary>
        ///   Loads a <see cref = "IBDocument" /> from a Xml content.
        /// </summary>
        /// <param name = "reader">The reader.</param>
        /// <returns>An instance of <see cref = "IBDocument" /></returns>
        public static IBDocument LoadFromXml(XmlReader reader)
        {
            IBDocument document = new IBDocument();
            IIBReferenceResolver resolver = new IBReferenceResolver();

            // The XML parsing is based on a top-down stack exploration
            StringBuilder tempContent = new StringBuilder();
            Stack<IIBItem> stack = new Stack<IIBItem>();

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
                            IIBItem item = Create(name, attributes);
                            if (String.Equals("archive", name))
                            {
                                // The "archive" node has a special treatment
                                document.Root = item as IBArchive;
                            }
                            else
                            {
                                // Append the node to the current parent node
                                IIBItem current = stack.Peek();
                                current.AppendChild(item);
                            }

                            // If the node has an id, then add it to the resolver for latter processing
                            resolver.AddReference(item);

                            // Push the new node in the context stack
                            stack.Push(item);

                            // Prepare the content collector for node with text
                            tempContent = new StringBuilder();

                            // An empty node is pop right-away from the context stack
                            if (empty)
                            {
                                stack.Pop();
                                item.SetValue(tempContent.ToString());
                                item.Finish(resolver);
                            }
                        }
                        break;

                    case XmlNodeType.EndElement:
                        {
                            // When the node ends, it is pop from the context stack and its content is set
                            // After that, the node can perform additionnal processing.
                            IIBItem item = stack.Pop();
                            item.SetValue(tempContent.ToString());
                            item.Finish(resolver);
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
        ///   Loads a <see cref = "IBDocument" /> from a file.
        /// </summary>
        /// <param name = "path">The path.</param>
        /// <returns>An instance of <see cref = "IBDocument" /></returns>
        public static IBDocument LoadFromFile(String path)
        {
            XmlReaderSettings settings = new XmlReaderSettings {CloseInput = true, ProhibitDtd = false, XmlResolver = new IBXmlResolver()};
            using (XmlReader reader = XmlReader.Create(path, settings))
            {
                return LoadFromXml(reader);
            }
        }

        private static IIBItem Create(String name, IDictionary<string, string> attributes)
        {
            switch (name)
            {
                case "archive":
                    return new IBArchive(attributes);

                case "bool":
                case "boolean":
                    return new IBBoolean(attributes);

                case "bytes":
                    return new IBBytes(attributes);

                case "dictionary":
                    return new IBDictionary(attributes);

                case "double":
                    return new IBDouble(attributes);

                case "float":
                    return new IBFloat(attributes);

                case "nil":
                    return new IBNil(attributes);

                case "int":
                case "integer":
                    return new IBInteger(attributes);

                case "object":
                    return GetInstance(attributes);

                case "reference":
                    return new IBReference(attributes);

                case "string":
                    return new IBString(attributes);

                case "characters":
                case "data":
                    return new IBObject(attributes);

                default:
                    throw new NotSupportedException("Element not supported: " + name);
            }
        }

        public static IBObject GetInstance(IDictionary<String, String> attributes)
        {
            IBObject obj;
            switch (attributes["class"])
            {
                case "NSArray":
                case "NSMutableArray":
                    obj = new IBArray(attributes);
                    break;
                case "NSDictionary":
                case "NSMutableDictionary":
                    obj = new IBDictionary(attributes);
                    break;
                case "IBClassDescriber":
                    obj = new IBClassDescriber(attributes);
                    break;
                case "IBPartialClassDescription":
                    obj = new IBPartialClassDescription(attributes);
                    break;
                default:
                    obj = new IBObject(attributes);
                    break;
            }
            return obj;
        }
    }
}