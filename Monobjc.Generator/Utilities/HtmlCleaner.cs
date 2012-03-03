//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2012 - Laurent Etiemble
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
using System.Text;
using System.Xml;
using Monobjc.Tools.Generator.Parsers.Sgml;

namespace Monobjc.Tools.Generator.Utilities
{
    /// <summary>
    ///   Helper class for HTML cleaning.
    /// </summary>
    public static class HtmlCleaner
    {
        /// <summary>
        ///   Convert a HTML document into a XHTML document.
        /// </summary>
        /// <param name = "htmlFile">The HTML file.</param>
        /// <param name = "xhtmlFile">The XHTML file.</param>
        public static void HtmlToXhtml(String htmlFile, String xhtmlFile)
        {
            using (SgmlReader reader = new SgmlReader())
            {
                reader.DocType = "HTML";
                reader.WhitespaceHandling = WhitespaceHandling.None;

                using (StreamReader r = new StreamReader(htmlFile))
                {
                    reader.InputStream = r;

                    using (XmlTextWriter writer = new XmlTextWriter(xhtmlFile, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;

                        reader.Read();
                        while (!reader.EOF)
                        {
                            writer.WriteNode(reader, true);
                        }
                    }
                }
            }
        }
    }
}