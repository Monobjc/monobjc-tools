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
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Monobjc.Tools.Xcode
{
    public static class TextWriterExtensions
    {
        /// <summary>
        ///   Writes the indent.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "indentLevel">The indent level.</param>
        public static void WriteIndent(this TextWriter writer, int indentLevel)
        {
            while (indentLevel-- > 0)
            {
                writer.Write("    ");
            }
        }

        /// <summary>
        ///   Writes the element prologue.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "indentLevel">The indent level.</param>
        /// <param name = "map">The map.</param>
        /// <param name = "element">The element.</param>
        public static void WritePBXElementPrologue(this TextWriter writer, int indentLevel, IDictionary<IPBXElement, string> map, IPBXElement element)
        {
            writer.WriteIndent(indentLevel);
            writer.WriteLine("{0} /* {1} */ = {{", map[element], element.Description);
            writer.WriteIndent(indentLevel + 1);
            writer.WriteLine("{0} = {1};", "isa", element.Isa);
        }

        /// <summary>
        ///   Writes the element epilogue.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "indentLevel">The indent level.</param>
        public static void WritePBXElementEpilogue(this TextWriter writer, int indentLevel)
        {
            writer.WriteIndent(indentLevel);
            writer.WriteLine("};");
        }

        /// <summary>
        ///   Writes the PBX property.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "indentLevel">The indent level.</param>
        /// <param name = "map">The map.</param>
        /// <param name = "name">The name.</param>
        /// <param name = "value">The value.</param>
        public static void WritePBXProperty(this TextWriter writer, int indentLevel, IDictionary<IPBXElement, String> map, String name, Object value)
        {
            if (value == null)
            {
                return;
            }

            writer.WriteIndent(indentLevel);
            writer.Write("{0} = ", name);
            writer.WritePBXValue(indentLevel, map, value);
            writer.WriteLine(";");
        }

        /// <summary>
        ///   Writes the PBX value.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "indentLevel">The indent level.</param>
        /// <param name = "map">The map.</param>
        /// <param name = "value">The value.</param>
        public static void WritePBXValue(this TextWriter writer, int indentLevel, IDictionary<IPBXElement, String> map, Object value)
        {
            Type type = value.GetType();
            if (type == typeof (int))
            {
                writer.Write("{0}", value);
            }
            else if (type == typeof (String))
            {
                writer.Write("\"{0}\"", value);
            }
            else if (typeof (IPBXElement).IsAssignableFrom(type))
            {
                writer.Write("{0} /* {1} */", map[(IPBXElement) value], ((IPBXElement) value).Description);
            }
            else if (typeof (IList).IsAssignableFrom(type))
            {
                writer.WritePBXList(indentLevel, map, (IList) value);
            }
            else if (typeof (IDictionary).IsAssignableFrom(type))
            {
                writer.WritePBXDictionary(indentLevel, map, (IDictionary) value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///   Writes the list.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "indentLevel">The indent level.</param>
        /// <param name = "map">The map.</param>
        /// <param name = "values">The values.</param>
        public static void WritePBXList(this TextWriter writer, int indentLevel, IDictionary<IPBXElement, String> map, IList values)
        {
            writer.WriteLine("(");

            // For each value, the output is different
            foreach (Object value in values)
            {
                writer.WriteIndent(indentLevel + 1);
                writer.WritePBXValue(indentLevel + 1, map, value);
                writer.WriteLine(",");
            }

            writer.WriteIndent(indentLevel);
            writer.Write(")");
        }

        /// <summary>
        ///   Writes the dictionary.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "indentLevel">The indent level.</param>
        /// <param name = "map">The map.</param>
        /// <param name = "dictionary">The dictionary.</param>
        public static void WritePBXDictionary(this TextWriter writer, int indentLevel, IDictionary<IPBXElement, String> map, IDictionary dictionary)
        {
            writer.WriteLine("{");

            // For each key/value pair, the output is different
            foreach (String key in dictionary.Keys)
            {
                Object value = dictionary[key];
                writer.WritePBXProperty(indentLevel + 1, map, key, value);
            }

            writer.WriteIndent(indentLevel);
            writer.Write("}");
        }
    }
}