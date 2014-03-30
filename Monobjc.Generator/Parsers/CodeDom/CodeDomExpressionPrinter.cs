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
using System.Text;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using Attribute = ICSharpCode.NRefactory.Ast.Attribute;

namespace Monobjc.Tools.Generator.Parsers.CodeDom
{
	/// <summary>
	///   Expression printer for <see cref = "Expression" /> nodes.
	/// </summary>
	internal class CodeDomExpressionPrinter : IAstVisitor
	{
		private readonly StringBuilder builder = new StringBuilder ();

		/// <summary>
		///   Returns a <see cref = "System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		///   A <see cref = "System.String" /> that represents this instance.
		/// </returns>
		public override string ToString ()
		{
			return this.builder.ToString ();
		}

		public object VisitAddHandlerStatement (AddHandlerStatement addHandlerStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitAddressOfExpression (AddressOfExpression addressOfExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitAnonymousMethodExpression (AnonymousMethodExpression anonymousMethodExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitArrayCreateExpression (ArrayCreateExpression arrayCreateExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitAssignmentExpression (AssignmentExpression assignmentExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitAttribute (Attribute attribute, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitAttributeSection (AttributeSection attributeSection, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitBaseReferenceExpression (BaseReferenceExpression baseReferenceExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitBinaryOperatorExpression (BinaryOperatorExpression binaryOperatorExpression, object data)
		{
			binaryOperatorExpression.Left.AcceptVisitor (this, data);
			String op;
			switch (binaryOperatorExpression.Op) {
			case BinaryOperatorType.Add:
				op = "+";
				break;
			case BinaryOperatorType.BitwiseAnd:
				op = "&";
				break;
			case BinaryOperatorType.BitwiseOr:
				op = "|";
				break;
			case BinaryOperatorType.Divide:
				op = "/";
				break;
			case BinaryOperatorType.DivideInteger:
				op = "/";
				break;
			case BinaryOperatorType.Equality:
				op = "==";
				break;
			case BinaryOperatorType.ExclusiveOr:
				op = "^";
				break;
			case BinaryOperatorType.GreaterThan:
				op = ">";
				break;
			case BinaryOperatorType.GreaterThanOrEqual:
				op = ">=";
				break;
			case BinaryOperatorType.LessThan:
				op = "<";
				break;
			case BinaryOperatorType.LessThanOrEqual:
				op = "=<";
				break;
			case BinaryOperatorType.LogicalAnd:
				op = "&&";
				break;
			case BinaryOperatorType.LogicalOr:
				op = "||";
				break;
			case BinaryOperatorType.Modulus:
				op = "%";
				break;
			case BinaryOperatorType.Multiply:
				op = "*";
				break;
			case BinaryOperatorType.Power:
				op = "^";
				break;
			case BinaryOperatorType.ShiftLeft:
				op = "<<";
				break;
			case BinaryOperatorType.ShiftRight:
				op = ">>";
				break;
			case BinaryOperatorType.Subtract:
				op = "-";
				break;
			default:
				op = String.Empty;
				break;
			}
			this.builder.AppendFormat (" {0} ", op);
			binaryOperatorExpression.Right.AcceptVisitor (this, data);
			return data;
		}

		public object VisitBlockStatement (BlockStatement blockStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitBreakStatement (BreakStatement breakStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitCaseLabel (CaseLabel caseLabel, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitCastExpression (CastExpression castExpression, object data)
		{
			this.builder.Append ("(");
			castExpression.CastTo.AcceptVisitor (this, data);
			this.builder.Append (") ");
			castExpression.Expression.AcceptVisitor (this, data);
			return data;
		}

		public object VisitCatchClause (CatchClause catchClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitCheckedExpression (CheckedExpression checkedExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitCheckedStatement (CheckedStatement checkedStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitClassReferenceExpression (ClassReferenceExpression classReferenceExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitCollectionInitializerExpression (CollectionInitializerExpression collectionInitializerExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitCompilationUnit (CompilationUnit compilationUnit, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitConditionalExpression (ConditionalExpression conditionalExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitConstructorDeclaration (ConstructorDeclaration constructorDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitConstructorInitializer (ConstructorInitializer constructorInitializer, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitContinueStatement (ContinueStatement continueStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitDeclareDeclaration (DeclareDeclaration declareDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitDefaultValueExpression (DefaultValueExpression defaultValueExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitDelegateDeclaration (DelegateDeclaration delegateDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitDestructorDeclaration (DestructorDeclaration destructorDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitDirectionExpression (DirectionExpression directionExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitDoLoopStatement (DoLoopStatement doLoopStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitElseIfSection (ElseIfSection elseIfSection, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitEmptyStatement (EmptyStatement emptyStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitEndStatement (EndStatement endStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitEraseStatement (EraseStatement eraseStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitErrorStatement (ErrorStatement errorStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitEventAddRegion (EventAddRegion eventAddRegion, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitEventDeclaration (EventDeclaration eventDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitEventRaiseRegion (EventRaiseRegion eventRaiseRegion, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitEventRemoveRegion (EventRemoveRegion eventRemoveRegion, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitExitStatement (ExitStatement exitStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitExpressionRangeVariable (ExpressionRangeVariable expressionRangeVariable, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitExpressionStatement (ExpressionStatement expressionStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitExternAliasDirective (ExternAliasDirective externAliasDirective, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitFieldDeclaration (FieldDeclaration fieldDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitFixedStatement (FixedStatement fixedStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitForNextStatement (ForNextStatement forNextStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitForStatement (ForStatement forStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitForeachStatement (ForeachStatement foreachStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitGotoCaseStatement (GotoCaseStatement gotoCaseStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitGotoStatement (GotoStatement gotoStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitIdentifierExpression (IdentifierExpression identifierExpression, object data)
		{
			this.builder.AppendFormat ("{0}", identifierExpression.Identifier);
			return data;
		}

		public object VisitIfElseStatement (IfElseStatement ifElseStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitIndexerDeclaration (IndexerDeclaration indexerDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitIndexerExpression (IndexerExpression indexerExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitInnerClassTypeReference (InnerClassTypeReference innerClassTypeReference, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitInterfaceImplementation (InterfaceImplementation interfaceImplementation, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitInvocationExpression (InvocationExpression invocationExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitLabelStatement (LabelStatement labelStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitLambdaExpression (LambdaExpression lambdaExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitLocalVariableDeclaration (LocalVariableDeclaration localVariableDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitLockStatement (LockStatement lockStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitMemberReferenceExpression (MemberReferenceExpression memberReferenceExpression, object data)
		{
			memberReferenceExpression.TargetObject.AcceptVisitor (this, data);
			this.builder.AppendFormat (".{0}", memberReferenceExpression.MemberName);
			return data;
		}

		public object VisitMethodDeclaration (MethodDeclaration methodDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitNamedArgumentExpression (NamedArgumentExpression namedArgumentExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitNamespaceDeclaration (NamespaceDeclaration namespaceDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitObjectCreateExpression (ObjectCreateExpression objectCreateExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitOnErrorStatement (OnErrorStatement onErrorStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitOperatorDeclaration (OperatorDeclaration operatorDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitOptionDeclaration (OptionDeclaration optionDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitParameterDeclarationExpression (ParameterDeclarationExpression parameterDeclarationExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitParenthesizedExpression (ParenthesizedExpression parenthesizedExpression, object data)
		{
			this.builder.Append ("(");
			parenthesizedExpression.Expression.AcceptVisitor (this, data);
			this.builder.Append (")");
			return data;
		}

		public object VisitPointerReferenceExpression (PointerReferenceExpression pointerReferenceExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitPrimitiveExpression (PrimitiveExpression primitiveExpression, object data)
		{
			this.builder.AppendFormat ("{0}", primitiveExpression.StringValue);
			return data;
		}

		public object VisitPropertyDeclaration (PropertyDeclaration propertyDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitPropertyGetRegion (PropertyGetRegion propertyGetRegion, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitPropertySetRegion (PropertySetRegion propertySetRegion, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpression (QueryExpression queryExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionAggregateClause (QueryExpressionAggregateClause queryExpressionAggregateClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionDistinctClause (QueryExpressionDistinctClause queryExpressionDistinctClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionFromClause (QueryExpressionFromClause queryExpressionFromClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionGroupClause (QueryExpressionGroupClause queryExpressionGroupClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionGroupJoinVBClause (QueryExpressionGroupJoinVBClause queryExpressionGroupJoinVBClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionGroupVBClause (QueryExpressionGroupVBClause queryExpressionGroupVBClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionJoinClause (QueryExpressionJoinClause queryExpressionJoinClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionJoinConditionVB (QueryExpressionJoinConditionVB queryExpressionJoinConditionVB, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionJoinVBClause (QueryExpressionJoinVBClause queryExpressionJoinVBClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionLetClause (QueryExpressionLetClause queryExpressionLetClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionLetVBClause (QueryExpressionLetVBClause queryExpressionLetVBClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionOrderClause (QueryExpressionOrderClause queryExpressionOrderClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionOrdering (QueryExpressionOrdering queryExpressionOrdering, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionPartitionVBClause (QueryExpressionPartitionVBClause queryExpressionPartitionVBClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionSelectClause (QueryExpressionSelectClause queryExpressionSelectClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionSelectVBClause (QueryExpressionSelectVBClause queryExpressionSelectVBClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitQueryExpressionWhereClause (QueryExpressionWhereClause queryExpressionWhereClause, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitRaiseEventStatement (RaiseEventStatement raiseEventStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitReDimStatement (ReDimStatement reDimStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitRemoveHandlerStatement (RemoveHandlerStatement removeHandlerStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitResumeStatement (ResumeStatement resumeStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitReturnStatement (ReturnStatement returnStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitSizeOfExpression (SizeOfExpression sizeOfExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitStackAllocExpression (StackAllocExpression stackAllocExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitStopStatement (StopStatement stopStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitSwitchSection (SwitchSection switchSection, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitSwitchStatement (SwitchStatement switchStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitTemplateDefinition (TemplateDefinition templateDefinition, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitThisReferenceExpression (ThisReferenceExpression thisReferenceExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitThrowStatement (ThrowStatement throwStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitTryCatchStatement (TryCatchStatement tryCatchStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitTypeDeclaration (TypeDeclaration typeDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitTypeOfExpression (TypeOfExpression typeOfExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitTypeOfIsExpression (TypeOfIsExpression typeOfIsExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitTypeReference (TypeReference typeReference, object data)
		{
			this.builder.Append (typeReference.Type);
			return data;
		}

		public object VisitTypeReferenceExpression (TypeReferenceExpression typeReferenceExpression, object data)
		{
			typeReferenceExpression.TypeReference.AcceptVisitor (this, data);
			return data;
		}

		public object VisitUnaryOperatorExpression (UnaryOperatorExpression unaryOperatorExpression, object data)
		{
			String op;
			bool before = true;
			switch (unaryOperatorExpression.Op) {
			case UnaryOperatorType.AddressOf:
				op = "*";
				break;
			case UnaryOperatorType.BitNot:
				op = "~";
				break;
			case UnaryOperatorType.Decrement:
				op = "--";
				break;
			case UnaryOperatorType.Increment:
				op = "++";
				break;
			case UnaryOperatorType.Minus:
				op = "-";
				break;
			case UnaryOperatorType.Not:
				op = "!";
				break;
			case UnaryOperatorType.Plus:
				op = "+";
				break;
			case UnaryOperatorType.PostDecrement:
				op = "--";
				before = false;
				break;
			case UnaryOperatorType.PostIncrement:
				op = "++";
				before = false;
				break;
			case UnaryOperatorType.Dereference:
			case UnaryOperatorType.None:
			default:
				op = String.Empty;
				break;
			}
			if (before) {
				this.builder.AppendFormat ("{0}", op);
				unaryOperatorExpression.Expression.AcceptVisitor (this, data);
			} else {
				unaryOperatorExpression.Expression.AcceptVisitor (this, data);
				this.builder.AppendFormat ("{0}", op);
			}
			return data;
		}

		public object VisitUncheckedExpression (UncheckedExpression uncheckedExpression, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitUncheckedStatement (UncheckedStatement uncheckedStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitUnsafeStatement (UnsafeStatement unsafeStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitUsing (Using @using, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitUsingDeclaration (UsingDeclaration usingDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitUsingStatement (UsingStatement usingStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitVariableDeclaration (VariableDeclaration variableDeclaration, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitWithStatement (WithStatement withStatement, object data)
		{
			throw new NotImplementedException ();
		}

		public object VisitYieldStatement (YieldStatement yieldStatement, object data)
		{
			throw new NotImplementedException ();
		}
	}
}