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
        public static void writeElementPrologue(this TextWriter writer, IDictionary<IPBXElement, string> map, IPBXElement element)
        {
            writer.WriteLine("{0} /* {1} */ = {{", map[element], element.Description);
        }

        public static void writeElementEpilogue(this TextWriter writer)
        {
            writer.WriteLine("};");
        }

        public static void WriteReferences<T>(this TextWriter writer, IDictionary<IPBXElement, string> map, String name, IEnumerable<T> elements) where T : IPBXElement
        {
            writer.WriteLine("    {0} = (", name);
            foreach (IPBXElement element in elements)
            {
                writer.WriteLine("        {0}, /* {1} */", map[element], element.Description);
            }
            writer.WriteLine("    );");
        }

        public static void WriteReference(this TextWriter writer, IDictionary<IPBXElement, string> map, String name, IPBXElement element)
        {
            writer.WriteAttribute(name, map[element], element.Description);
        }

        public static void WriteAttribute(this TextWriter writer, String name, int value)
        {
            writer.WriteAttribute(name, value.ToString(), null);
        }

        public static void WriteAttribute(this TextWriter writer, String name, int value, String comment)
        {
            writer.WriteAttribute(name, value.ToString(), comment);
        }

        public static void WriteAttribute(this TextWriter writer, String name, String value)
        {
            writer.WriteAttribute(name, value, null);
        }

        public static void WriteAttribute(this TextWriter writer, String name, String value, String comment)
        {
            if (value == null)
            {
                return;
            }
            if (String.IsNullOrEmpty(value))
            {
                value = "\"\"";
            }
            if (value.Contains(" "))
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