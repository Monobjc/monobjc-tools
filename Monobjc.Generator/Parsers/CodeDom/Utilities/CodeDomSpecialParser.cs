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
using System.Collections.Generic;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;

namespace Monobjc.Tools.Generator.Parsers.CodeDom.Utilities
{
	/// <summary>
	///   Parse for special nodes when building Code DOM.
	/// </summary>
	public class CodeDomSpecialParser
	{
		private readonly IList<ISpecial> specials;
		private int current;

		/// <summary>
		///   Initializes a new instance of the <see cref = "CodeDomSpecialParser" /> class.
		/// </summary>
		/// <param name = "specials">The specials.</param>
		public CodeDomSpecialParser (IList<ISpecial> specials)
		{
			this.specials = specials;
			this.current = 0;
		}

		/// <summary>
		///   Gets the documentation comments before the given node.
		/// </summary>
		/// <param name = "node">The node.</param>
		/// <returns>An enumeration of comments.</returns>
		public IEnumerable<Comment> GetDocumentationCommentsBefore (INode node)
		{
			IList<Comment> result = new List<Comment> ();

			// Append the comments for enumeration
			while (this.current < this.specials.Count) {
				Comment comment = this.specials [this.current++] as Comment;
				if (comment == null || comment.CommentType != CommentType.Documentation) {
					continue;
				}
				if (comment.EndPosition.CompareTo (node.StartLocation) > 0) {
					this.current--;
					break;
				}
				result.Add (comment);
			}

			return result;
		}
	}
}