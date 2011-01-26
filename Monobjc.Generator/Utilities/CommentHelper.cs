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

namespace Monobjc.Tools.Generator.Utilities
{
    /// <summary>
    ///   Helper class to deal with C# XML comments.
    /// </summary>
    internal static class CommentHelper
    {
        /// <summary>
        ///   Determines whether the specified comment is a summary.
        /// </summary>
        /// <param name = "comment">The comment.</param>
        /// <returns>
        ///   <c>true</c> if the specified comment is a summary; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSummary(String comment)
        {
            return comment.Contains("<summary>") || comment.Contains("</summary>");
        }

        /// <summary>
        ///   Determines whether the specified comment is an availability.
        /// </summary>
        /// <param name = "comment">The comment.</param>
        /// <returns>
        ///   <c>true</c> if the specified comment is an availability; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAvailability(String comment)
        {
            return comment.Contains("Available in ");
        }

        /// <summary>
        ///   Determines whether the specified comment is a declaration.
        /// </summary>
        /// <param name = "comment">The comment.</param>
        /// <returns>
        ///   <c>true</c> if the specified comment is a declaration; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDeclaration(String comment)
        {
            return comment.Contains("Declared in ");
        }

        /// <summary>
        ///   Determines whether the specified comment is a signature.
        /// </summary>
        /// <param name = "comment">The comment.</param>
        /// <returns>
        ///   <c>true</c> if the specified comment is a signature; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSignature(String comment)
        {
            return comment.Contains("Original signature is ");
        }

        /// <summary>
        ///   Determines whether the specified comment is a paragraph.
        /// </summary>
        /// <param name = "comment">The comment.</param>
        /// <returns>
        ///   <c>true</c> if the specified comment is a paragraph; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsParagraph(String comment)
        {
            return comment.StartsWith("<para>") || comment.StartsWith("&lt;para&gt;");
        }

        /// <summary>
        ///   Determines whether the specified comment is a parameter.
        /// </summary>
        /// <param name = "comment">The comment.</param>
        /// <returns>
        ///   <c>true</c> if the specified comment is a parameter; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsParameter(String comment)
        {
            return comment.StartsWith("<param") || comment.StartsWith("&lt;param");
        }

        /// <summary>
        ///   Determines whether the specified comment is a return.
        /// </summary>
        /// <param name = "comment">The comment.</param>
        /// <returns>
        ///   <c>true</c> if the specified comment is a return; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsReturn(String comment)
        {
            return comment.StartsWith("<return") || comment.StartsWith("&lt;return");
        }

        /// <summary>
        ///   Determines whether the specified comment is remarks.
        /// </summary>
        /// <param name = "comment">The comment.</param>
        /// <returns>
        ///   <c>true</c> if the specified comment is remarks; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRemarks(String comment)
        {
            return comment.StartsWith("<remarks") || comment.StartsWith("&lt;remarks");
        }

        /// <summary>
        ///   Extracts the availability version.
        /// </summary>
        /// <param name = "comment">The comment.</param>
        /// <returns>The availability version.</returns>
        public static String ExtractAvailability(String comment)
        {
            if (String.IsNullOrEmpty(comment))
            {
                return "VERSION";
            }

            int index1 = "Available in ".Length;
            String result = comment.Substring(index1);
            int index2 = result.IndexOf(" and later");
            if (index2 > 0)
            {
                result = result.Substring(0, index2);
            }

            if (result.EndsWith("."))
            {
                result = result.Trim('.');
            }

            return result.Trim();
        }

        /// <summary>
        ///   Gets the availability formatted string.
        /// </summary>
        /// <param name = "version">The version.</param>
        /// <returns>The availability formatted string.</returns>
        public static String GetAvailability(String version)
        {
            return String.Format("Available in {0} and later.", version);
        }
    }
}