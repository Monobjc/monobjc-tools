//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2014 - Laurent Etiemble
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
using System.Linq;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Parsers.CodeDom.Utilities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.CodeDom
{
	public class CodeDomClassParser : CodeDomTypeParser
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "CodeDomClassParser" /> class.
		/// </summary>
		/// <param name = "settings">The settings.</param>
		/// <param name = "typeManager">The type manager.</param>
		public CodeDomClassParser (NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger)
		{
		}

		protected virtual ClassType ClassType {
			get { return ClassType.Class; }
		}

		/// <summary>
		///   Parses the specified entity.
		/// </summary>
		/// <param name = "entity">The entity.</param>
		/// <param name = "reader">The reader.</param>
		public override void Parse (BaseEntity entity, TextReader reader)
		{
			ClassEntity classEntity = entity as ClassEntity;
			if (classEntity == null) {
				throw new ArgumentException ("ClassEntity expected", "entity");
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
					if (declaration.Type != this.ClassType) {
						continue;
					}

					// Extract super-class
					if (declaration.BaseTypes.Count > 0) {
						classEntity.BaseType = declaration.BaseTypes [0].Type;
					}

					// Extract comments
					IEnumerable<Comment> comments = this.GetDocumentationCommentsBefore (declaration);
					AppendComment (entity, comments);

					// Append each values
					foreach (INode child3 in declaration.Children) {
						MethodDeclaration methodDeclaration = child3 as MethodDeclaration;
						if (methodDeclaration != null) {
							MethodEntity methodEntity = this.ExtractMethod (methodDeclaration);
							classEntity.Methods.Add (methodEntity);
						}

						PropertyDeclaration propertyDeclaration = child3 as PropertyDeclaration;
						if (propertyDeclaration != null) {
							PropertyEntity propertyEntity = this.ExtractProperty (propertyDeclaration);
							classEntity.Properties.Add (propertyEntity);
						}

						FieldDeclaration fieldDeclaration = child3 as FieldDeclaration;
						if (fieldDeclaration != null) {
							if ((fieldDeclaration.Modifier & Modifiers.Static) != Modifiers.Static) {
								continue;
							}
							if (fieldDeclaration.TypeReference.Type == "Class") {
								continue;
							}
							ConstantEntity constantEntity = this.ExtractConstant (fieldDeclaration);
							classEntity.Constants.Add (constantEntity);
						}
					}
				}
			}

			// Ensure that availability is set on entity.
			entity.AdjustAvailability ();
		}

		private MethodEntity ExtractMethod (MethodDeclaration methodDeclaration)
		{
			MethodEntity methodEntity = new MethodEntity ();
			methodEntity.Name = methodDeclaration.Name;

			// Get the method's comment
			IEnumerable<Comment> comments = this.GetDocumentationCommentsBefore (methodDeclaration);

			// Extract signature from comments
			Comment signatureComment = comments.FirstOrDefault (c => CommentHelper.IsSignature (c.CommentText.Trim ()));
			if (signatureComment != null) {
				methodEntity.Signature = signatureComment.Trim ("Original signature is", "'", ";", "private");
			}
			AppendComment (methodEntity, comments);

			methodEntity.Static = (methodDeclaration.Modifier & Modifiers.Static) == Modifiers.Static;

			// Extract selector
			MethodSelectorExtractor extractor = new MethodSelectorExtractor (methodEntity.Signature);
			methodEntity.Selector = extractor.Extract ();

			// Parse the signature for return type
			MethodSignatureEnumerator signatureEnumerator = new MethodSignatureEnumerator (methodEntity.Signature);
			if (signatureEnumerator.MoveNext ()) {
				bool isOut, isByRef, isBlock;
				methodEntity.ReturnType = this.TypeManager.ConvertType (signatureEnumerator.Current.TrimAll (), out isOut, out isByRef, out isBlock, this.Logger);
			} else {
				methodEntity.ReturnType = "Id";
			}

			// Parse signature for parameter names and types
			MethodParametersEnumerator parameterTypesEnumerator = new MethodParametersEnumerator (methodEntity.Signature, false);
			MethodParametersEnumerator parameterNamesEnumerator = new MethodParametersEnumerator (methodEntity.Signature, true);
			while (parameterTypesEnumerator.MoveNext() && parameterNamesEnumerator.MoveNext()) {
				MethodParameterEntity parameterEntity = new MethodParameterEntity ();
				bool isOut, isByRef, isBlock;
				parameterEntity.Type = this.TypeManager.ConvertType (parameterTypesEnumerator.Current, out isOut, out isByRef, out isBlock, this.Logger);
				parameterEntity.IsOut = isOut;
				parameterEntity.IsByRef = isByRef;
				parameterEntity.IsBlock = isBlock;
				parameterEntity.Name = parameterNamesEnumerator.Current.Trim ();
				methodEntity.Parameters.Add (parameterEntity);
			}

			// Extract the corresponding comments
			foreach (MethodParameterEntity methodParameterEntity in methodEntity.Parameters) {
				String s = comments.GetParameterDescription (methodParameterEntity.Name);
				methodParameterEntity.Summary.Add (s);
			}
			return methodEntity;
		}

		private PropertyEntity ExtractProperty (PropertyDeclaration propertyDeclaration)
		{
			PropertyEntity propertyEntity = new PropertyEntity ();
			propertyEntity.Name = propertyDeclaration.Name;
			propertyEntity.Static = (propertyDeclaration.Modifier & Modifiers.Static) == Modifiers.Static;

			// Get the method's comment
			IEnumerable<Comment> comments = this.GetDocumentationCommentsBefore (propertyDeclaration);
			AppendComment (propertyEntity, comments);

			// Extract getter
			MethodEntity getterEntity = new MethodEntity ();
			propertyEntity.Getter = getterEntity;

			// Extract signature from comments
			Comment signatureComment = comments.FirstOrDefault (c => CommentHelper.IsSignature (c.CommentText.Trim ()));
			if (signatureComment != null) {
				getterEntity.Signature = signatureComment.Trim ("Original signature is", "'", ";", "private");
			}

			// Extract selector
			MethodSelectorExtractor extractor = new MethodSelectorExtractor (getterEntity.Signature);
			getterEntity.Selector = extractor.Extract ();

			// Parse the signature for return type
			MethodSignatureEnumerator signatureEnumerator = new MethodSignatureEnumerator (getterEntity.Signature);
			if (signatureEnumerator.MoveNext ()) {
				bool isOut, isByRef, isBlock;
				propertyEntity.Type = this.TypeManager.ConvertType (signatureEnumerator.Current.TrimAll (), out isOut, out isByRef, out isBlock, this.Logger);
			} else {
				propertyEntity.Type = "Id";
			}

			if (propertyDeclaration.HasSetRegion) {
				MethodEntity setterEntity = new MethodEntity ();
				setterEntity.Selector = "MISSING";

				MethodParameterEntity methodParameterEntity = new MethodParameterEntity ();
				methodParameterEntity.Name = "value";
				methodParameterEntity.Type = propertyEntity.Type;
				setterEntity.Parameters.Add (methodParameterEntity);
				setterEntity.ReturnType = "void";

				propertyEntity.Setter = setterEntity;
			}

			return propertyEntity;
		}

		private ConstantEntity ExtractConstant (FieldDeclaration fieldDeclaration)
		{
			ConstantEntity constantEntity = new ConstantEntity ();

			constantEntity.Name = fieldDeclaration.Fields [0].Name;
			constantEntity.Type = fieldDeclaration.TypeReference.Type;
			constantEntity.Value = "MISSING";

			// Get the method's comment
			IEnumerable<Comment> comments = this.GetDocumentationCommentsBefore (fieldDeclaration);
			AppendComment (constantEntity, comments);

			return constantEntity;
		}
	}
}