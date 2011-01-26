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
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Monobjc.Tools.Generator.Utilities
{
    /// <summary>
    ///   Extensions methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///   Trims all the leading and tailing spaces, as well as the the CR-LF characters.
        /// </summary>
        /// <param name = "str">The string.</param>
        /// <returns>A trimmed string.</returns>
        public static String TrimAll(this String str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return String.Empty;
            }
            str = str.Replace('\r', ' ').Replace('\n', ' ');
            str = str.Replace("  ", " ");
            return str.Trim();
        }

        /// <summary>
        ///   Escape special characters in the given string.
        /// </summary>
        /// <param name = "str">The string to escape.</param>
        /// <returns>The escaped string.</returns>
        public static String EscapeAll(this String str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return String.Empty;
            }
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            return str;
        }

        /// <summary>
        ///   Uppers the case of the first letter.
        /// </summary>
        /// <param name = "str">The string.</param>
        /// <returns>The result string.</returns>
        public static String UpperCaseFirstLetter(this String str)
        {
            if (str.Length < 2)
            {
                return str;
            }
            str = str.Trim();
            str = str.Substring(0, 1).ToUpperInvariant() + str.Substring(1);
            return str;
        }

        /// <summary>
        ///   Trims all the leading and tailing spaces, as well as the the CR-LF characters.
        /// </summary>
        /// <param name = "element">The element.</param>
        /// <returns>A trimmed string value.</returns>
        public static String TrimAll(this XElement element)
        {
            if (element == null)
            {
                return String.Empty;
            }
            String str = element.Value;
            str = str.Replace('\r', ' ').Replace('\n', ' ').Replace("  ", " ");
            return str.Trim();
        }

        /// <summary>
        ///   Trims all the leading and tailing spaces, as well as the the CR-LF characters.
        /// </summary>
        /// <param name = "navigator">The navigator.</param>
        /// <returns>A trimmed string value.</returns>
        public static String TrimAll(this XPathNavigator navigator)
        {
            if (navigator == null)
            {
                return String.Empty;
            }
            String str = navigator.ToString();
            str = str.Replace('\r', ' ').Replace('\n', ' ');
            str = str.Replace("  ", " ");
            return str.Trim();
        }

        public static bool IsOlderThan(this String fileName1, String fileName2)
        {
            DateTime dt1 = File.GetLastWriteTime(fileName1);
            DateTime dt2 = File.GetLastWriteTime(fileName2);
            return (dt1.CompareTo(dt2) < 0);
        }

        public static bool IsYoungerThan(this String fileName1, String fileName2)
        {
            DateTime dt1 = File.GetLastWriteTime(fileName1);
            DateTime dt2 = File.GetLastWriteTime(fileName2);
            return (dt1.CompareTo(dt2) >= 0);
        }

        /// <summary>
        ///   Appends the line using the format and the parameters.
        /// </summary>
        /// <param name = "builder">The builder.</param>
        /// <param name = "level">The level.</param>
        /// <param name = "format">The format.</param>
        /// <param name = "parameters">The parameters.</param>
        public static void AppendLineFormat(this StringBuilder builder, int level, String format, params object[] parameters)
        {
            builder.Append(new String(' ', level*4));
            builder.AppendFormat(format, parameters);
            builder.AppendLine();
        }

        /// <summary>
        ///   Writes the line using the format and the paramters.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "level">The level.</param>
        /// <param name = "format">The format.</param>
        /// <param name = "parameters">The parameters.</param>
        public static void WriteLineFormat(this StreamWriter writer, int level, String format, params object[] parameters)
        {
            writer.Write(new String(' ', level*4));
            writer.Write(String.Format(format, parameters));
            writer.WriteLine();
        }
    }
}