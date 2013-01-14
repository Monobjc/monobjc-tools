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
using System.Collections.Specialized;
using System.IO;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Parsers.CodeDom.Utilities;
using Monobjc.Tools.Generator.Utilities;
using Attribute = ICSharpCode.NRefactory.Ast.Attribute;

namespace Monobjc.Tools.Generator.Parsers.CodeDom
{
	/// <summary>
	///   Parse a file containing an enumeration and convert it to a model.
	/// </summary>
	public class CodeDomEnumParser : CodeDomTypeParser
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "CodeDomEnumParser" /> class.
		/// </summary>
		/// <param name = "settings">The settings.</param>
		/// <param name = "typeManager">The type manager.</param>
		public CodeDomEnumParser (NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger)
		{
		}

		/// <summary>
		///   Parses the specified entity.
		/// </summary>
		/// <param name = "entity">The entity.</param>
		/// <param name = "reader">The reader.</param>
		public override void Parse (BaseEntity entity, TextReader reader)
		{
			EnumerationEntity enumerationEntity = (EnumerationEntity)entity;
			if (enumerationEntity == null) {
				throw new ArgumentException ("EnumerationEntity expected", "entity");
			}

			IParser parser = ParserFactory.CreateParser (SupportedLanguage.CSharp, reader);
			parser.Parse ();

			// Extract the special nodes (comment, etc)
			List<ISpecial> specials = parser.Lexer.SpecialTracker.RetrieveSpecials ();
			this.CodeDomSpecialParser = new CodeDomSpecialParser (specials);

			// Parse the compilation unit
			CompilationUnit cu = parser.CompilationUnit;
			foreach (INode child1 in cu.Children) {
				NamespaceDeclaration namespaceDeclaration = child1 as NamespaceDeclaration;
				if (namespaceDeclaration == null) {
					continue;
				}
				foreach (INode child2 in child1.Children) {
					TypeDeclaration declaration = child2 as TypeDeclaration;
					if (declaration == null) {
						continue;
					}
					if (declaration.Type != ClassType.Enum) {
						continue;
					}

					// Extract basic informations
					enumerationEntity.BaseType = declaration.BaseTypes.Count > 0 ? declaration.BaseTypes [0].Type : "int";

					// Extract attributes
					Attribute attribute = FindAttribute (declaration, "Flags");
					enumerationEntity.Flags = (attribute != null);

					IEnumerable<Comment> comments = this.GetDocumentationCommentsBefore (declaration);
					AppendComment (entity, comments);

					// Append each values
					foreach (INode child3 in declaration.Children) {
						FieldDeclaration fieldDeclaration = child3 as FieldDeclaration;
						if (fieldDeclaration == null) {
							continue;
						}

						EnumerationValueEntity valueEntity = new EnumerationValueEntity ();
						valueEntity.Name = fieldDeclaration.Fields [0].Name;
						Expression expression = fieldDeclaration.Fields [0].Initializer;
						if (expression != null) {
							CodeDomExpressionPrinter printer = new CodeDomExpressionPrinter ();
							expression.AcceptVisitor (printer, null);
							valueEntity.Value = printer.ToString ();
						}

						comments = this.GetDocumentationCommentsBefore (fieldDeclaration);
						AppendComment (valueEntity, comments);

						enumerationEntity.Values.Add (valueEntity);
					}
				}
			}

			// Ensure that availability is set on entity.
			entity.AdjustAvailability ();
		}
	}
}