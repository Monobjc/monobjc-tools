//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2013 - Laurent Etiemble
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
using System.Linq;
using System.Text.RegularExpressions;
using ICSharpCode.NRefactory;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.CodeDom.Utilities
{
	public static class Comment_Extensions
	{
		private static readonly Regex regex = new Regex ("<param.+name.*=\"(.+)\">(.+)</param>");

		/// <summary>
		///   Remove common tags from the comment.
		/// </summary>
		/// <param name = "comment">The comment.</param>
		/// <param name = "strings">The additionnal strings to remove.</param>
		/// <returns>A clean comment</returns>
		public static String Trim (this Comment comment, params string[] strings)
		{
			String c = comment.CommentText;
			c = Trim (c, "para");
			c = Trim (c, "param");
			c = Trim (c, "return");
			c = Trim (c, "remarks");
			foreach (string s in strings) {
				c = c.Replace (s, String.Empty);
			}
			return c.Trim ();
		}

		public static String GetParameterDescription (this IEnumerable<Comment> comments, String name)
		{
			IEnumerable<Comment> parameterComments = comments.Where (c => CommentHelper.IsParameter (c.CommentText.Trim ()));
			foreach (Comment parameterComment in parameterComments) {
				Match m = regex.Match (parameterComment.CommentText.Trim ());
				if (m.Success) {
					return m.Groups [2].Value.Trim ();
				}
			}
			return "MISSING";
		}

		/// <summary>
		///   Remove a tag from the given string.
		/// </summary>
		private static string Trim (String comment, String tag)
		{
			foreach (String p in new[] {"<" + tag + ">", "</" + tag + ">", "&lt;" + tag + "&gt;", "&lt;/" + tag + "&gt;"}) {
				comment = comment.Replace (p, String.Empty);
			}
			return comment;
		}
	}
}