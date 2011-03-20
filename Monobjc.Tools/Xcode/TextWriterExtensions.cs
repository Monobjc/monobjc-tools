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

namespace Monobjc.Tools.Xcode
{
    public static class TextWriterExtensions
    {
        /// <summary>
        ///   Writes the element prologue.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "map">The map.</param>
        /// <param name = "element">The element.</param>
        public static void writeElementPrologue(this TextWriter writer, IDictionary<IPBXElement, string> map, IPBXElement element)
        {
            writer.WriteLine("{0} /* {1} */ = {{", map[element], element.Description);
            writer.WriteAttribute("isa", element.Isa);
        }

        /// <summary>
        ///   Writes the element epilogue.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        public static void writeElementEpilogue(this TextWriter writer)
        {
            writer.WriteLine("};");
        }

        /// <summary>
        ///   Writes the list.
        /// </summary>
        public static void WriteList(this TextWriter writer, String name, IEnumerable<String> values)
        {
            writer.WriteLine("    {0} = (", name);
            foreach (String value in values)
            {
                writer.WriteLine("        \"{0}\",", value);
            }
            writer.WriteLine("    );");
        }

        /// <summary>
        ///   Writes the list.
        /// </summary>
        public static void WriteMap(this TextWriter writer, String name, IDictionary<String, Object> map)
        {
            writer.WriteLine("    {0} = {{", name);
            foreach (KeyValuePair<String, Object> pair in map)
            {
                if (pair.Value.GetType() == typeof (int))
                {
                    writer.WriteLine("        {0} = {1};", pair.Key, (int) pair.Value);
                }
                String s = pair.Value as String;
                if (s != null)
                {
                    writer.WriteLine("        {0} = \"{1}\";", pair.Key, s);
                }
                List<String> values = pair.Value as List<String>;
                if (values != null)
                {
                    writer.WriteLine("        {0} = (", name);
                    foreach (String value in values)
                    {
                        writer.WriteLine("            \"{0}\",", value);
                    }
                    writer.WriteLine("        );");
                }
            }
            writer.WriteLine("    };");
        }

        /// <summary>
        ///   Writes the references.
        /// </summary>
        public static void WriteReferences<T>(this TextWriter writer, IDictionary<IPBXElement, string> map, String name, IEnumerable<T> elements) where T : IPBXElement
        {
            writer.WriteLine("    {0} = (", name);
            foreach (IPBXElement element in elements)
            {
                writer.WriteLine("        {0}, /* {1} */", map[element], element.Description);
            }
            writer.WriteLine("    );");
        }

        /// <summary>
        ///   Writes the reference.
        /// </summary>
        public static void WriteReference(this TextWriter writer, IDictionary<IPBXElement, string> map, String name, IPBXElement element)
        {
            writer.WriteAttribute(name, map[element], element.Description, false);
        }

        /// <summary>
        ///   Writes the attribute.
        /// </summary>
        public static void WriteAttribute(this TextWriter writer, String name, int value)
        {
            writer.WriteAttribute(name, value.ToString(), null, false);
        }

        /// <summary>
        ///   Writes the attribute.
        /// </summary>
        public static void WriteAttribute(this TextWriter writer, String name, int value, String comment)
        {
            writer.WriteAttribute(name, value.ToString(), comment, false);
        }

        /// <summary>
        ///   Writes the attribute.
        /// </summary>
        public static void WriteAttribute(this TextWriter writer, String name, String value)
        {
            writer.WriteAttribute(name, value, null, true);
        }

        /// <summary>
        ///   Writes the attribute.
        /// </summary>
        public static void WriteAttribute(this TextWriter writer, String name, String value, String comment, bool quotes)
        {
            if (value == null)
            {
                return;
            }
            if (quotes)
            {
                value = "\"" + value + "\"";
            }
            if (String.IsNullOrEmpty(comment))
            {
                writer.WriteLine("    {0} = {1};", name, value);
            }
            else
            {
                writer.WriteLine("    {0} = {1}; /* {2} */", name, value, comment);
            }
        }
    }
}