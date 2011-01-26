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
using System.Collections.Specialized;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Parsers.CodeDom.Utilities;
using Monobjc.Tools.Generator.Utilities;
using Attribute = ICSharpCode.NRefactory.Ast.Attribute;

namespace Monobjc.Tools.Generator.Parsers.CodeDom
{
    /// <summary>
    ///   Base class for code DOM parsing.
    /// </summary>
    public abstract class CodeDomBaseParser : BaseParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "CodeDomBaseParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        protected CodeDomBaseParser(NameValueCollection settings, TypeManager typeManager) : base(settings, typeManager) {}

        /// <summary>
        ///   Gets or sets the code DOM special parser.
        /// </summary>
        /// <value>The code DOM special parser.</value>
        protected CodeDomSpecialParser CodeDomSpecialParser { get; set; }

        /// <summary>
        ///   Appends the comment.
        /// </summary>
        /// <param name = "node">The node.</param>
        protected IEnumerable<Comment> GetDocumentationCommentsBefore(INode node)
        {
            return this.CodeDomSpecialParser.GetDocumentationCommentsBefore(node);
        }

        /// <summary>
        ///   Appends the comment.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "node">The node.</param>
        /// <param name = "comments">The comments.</param>
        protected static void AppendComment(BaseEntity entity, IEnumerable<Comment> comments)
        {
            foreach (Comment comment in comments)
            {
                String c = comment.CommentText.Trim();
                if (CommentHelper.IsSummary(c))
                {
                    continue;
                }
                if (CommentHelper.IsAvailability(c))
                {
                    String str = c;
                    foreach (String s in new[] {"<para>", "</para>", "&lt;para&gt;", "&lt;/para&gt;"})
                    {
                        str = str.Replace(s, String.Empty);
                    }
                    entity.MinAvailability = CommentHelper.ExtractAvailability(str.Trim());
                }
                else if (CommentHelper.IsParameter(c) || CommentHelper.IsReturn(c) || CommentHelper.IsSignature(c))
                {
                    // Do nothing
                }
                else if (CommentHelper.IsParagraph(c))
                {
                    String str = c;
                    foreach (String s in new[] {"<para>", "</para>", "&lt;para&gt;", "&lt;/para&gt;"})
                    {
                        str = str.Replace(s, String.Empty);
                    }
                    entity.Summary.Add(str.Trim());
                }
                else if (CommentHelper.IsRemarks(c))
                {
                    String str = c;
                    foreach (String s in new[] {"<remarks>", "</remarks>", "&lt;remarks&gt;", "&lt;/remarks&gt;"})
                    {
                        str = str.Replace(s, String.Empty);
                    }
                    entity.Summary.Add(str.Trim());
                }
                else
                {
                    entity.Summary.Add(c);
                }
            }
        }

        /// <summary>
        ///   Finds the attribute on the given node.
        /// </summary>
        /// <param name = "declaration">The declaration.</param>
        /// <param name = "attributeName">Name of the attribute.</param>
        /// <returns></returns>
        protected static Attribute FindAttribute(AttributedNode declaration, String attributeName)
        {
            foreach (AttributeSection attributeSection in declaration.Attributes)
            {
                foreach (Attribute attribute in attributeSection.Attributes)
                {
                    if (String.Equals(attribute.Name, attributeName))
                    {
                        return attribute;
                    }
                }
            }
            return null;
        }
    }
}