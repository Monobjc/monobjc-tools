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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Generators
{
	/// <summary>
	///   Code generator for class's additions.
	/// </summary>
	public class ClassDelegateGenerator : ClassGenerator
	{
		/// <summary>
		///   Generates the specified entity.
		/// </summary>
		/// <param name = "entity">The entity.</param>
		public override void Generate (BaseEntity entity)
		{
			ProtocolEntity protocolEntity = (ProtocolEntity)entity;
			ClassEntity delegatorEntity = protocolEntity.DelegatorEntity;
			//String property = protocolEntity.DelegateProperty;
			PropertyEntity propertyEntity = delegatorEntity.GetProperties (true, false).FirstOrDefault (p => p.Name == protocolEntity.DelegateProperty);

			// Gather methods for delegate
			IEnumerable<MethodEntity> methods = protocolEntity.Methods.Where (e => e.Generate);
			methods = methods.Concat (delegatorEntity.DelegateMethods.Where (e => e.Generate));

			// Append License)
			this.Writer.WriteLineFormat (0, License);

			// Append usings
			this.AppendStandardNamespaces ();

			// Append namespace starter
			this.Writer.WriteLineFormat (0, "namespace Monobjc.{0}", protocolEntity.Namespace);
			this.Writer.WriteLineFormat (0, "{{");

			// Append static condition if needed
			this.AppendStartCondition (delegatorEntity);

			// Append class starter
			this.Writer.WriteLineFormat (1, "public partial class {0}", delegatorEntity.Name, protocolEntity.Namespace);
			this.Writer.WriteLineFormat (1, "{{");

			// Emit delegate handlers
			foreach (MethodEntity methodEntity in methods) {
				// Append static condition if needed
				this.AppendStartCondition (methodEntity);

				this.AppendDocumentation (2, methodEntity);

				StringBuilder signature = new StringBuilder ();
				signature.AppendFormat ("public delegate {0} {1}EventHandler(", methodEntity.ReturnType, methodEntity.Name);
				List<String> parameters = new List<String> ();
				foreach (MethodParameterEntity methodParameterEntity in methodEntity.Parameters.Where(e => e.Generate)) {
					parameters.Add (GetTypeSignature (methodParameterEntity));
				}
				signature.Append (String.Join (", ", parameters.ToArray ()));
				signature.Append (");");

				this.Writer.WriteLineFormat (2, signature.ToString ());

				// Append static condition if needed
				this.AppendEndCondition (methodEntity);

				this.Writer.WriteLine ();
			}

			//if (property != null)
			if (propertyEntity != null) {
				// Append static condition if needed
				this.AppendStartCondition (propertyEntity);

				// Emit the delegate assignment
				this.Writer.WriteLineFormat (2, "/// <summary>");
				//this.Writer.WriteLineFormat(2, "/// Set the {0} property of a <see cref=\"{1}\"/> instance.", property, delegatorEntity.Name);
				this.Writer.WriteLineFormat (2, "/// Set the {0} property of a <see cref=\"{1}\"/> instance.", propertyEntity.Name, delegatorEntity.Name);
				this.Writer.WriteLineFormat (2, "/// </summary>");
				this.Writer.WriteLineFormat (2, "/// <param name=\"assignment\">The assignment of delegation methods.</param>");
				//this.Writer.WriteLineFormat(2, "public void Set{0}(Action<{1}EventDispatcher> assignment)", property, protocolEntity.Name);
				this.Writer.WriteLineFormat (2, "public void Set{0}(Action<{1}EventDispatcher> assignment)", propertyEntity.Name, protocolEntity.Name);
				this.Writer.WriteLineFormat (2, "{{");
				//this.Writer.WriteLineFormat(3, "{0}EventDispatcher @delegate = this.{1}.SafeCastAs<{0}EventDispatcher>();", protocolEntity.Name, property);
				this.Writer.WriteLineFormat (3, "{0}EventDispatcher @delegate = this.{1}.SafeCastAs<{0}EventDispatcher>();", protocolEntity.Name, propertyEntity.Name);
				this.Writer.WriteLineFormat (3, "if (@delegate != null)");
				this.Writer.WriteLineFormat (3, "{{");
				this.Writer.WriteLineFormat (4, "@delegate.Release();");
				//this.Writer.WriteLineFormat(4, "this.{0} = null;", property);
				this.Writer.WriteLineFormat (4, "this.{0} = null;", propertyEntity.Name);
				this.Writer.WriteLineFormat (3, "}}");
				this.Writer.WriteLineFormat (3, "if (assignment != null)");
				this.Writer.WriteLineFormat (3, "{{");
				this.Writer.WriteLineFormat (4, "@delegate = new {0}EventDispatcher();", protocolEntity.Name);
				this.Writer.WriteLineFormat (4, "assignment(@delegate);");
				//this.Writer.WriteLineFormat(4, "this.{0} = @delegate;", property);
				this.Writer.WriteLineFormat (4, "this.{0} = @delegate;", propertyEntity.Name);
				this.Writer.WriteLineFormat (3, "}}");
				this.Writer.WriteLineFormat (2, "}}");
				this.Writer.WriteLine ();

				// Append static condition if needed
				this.AppendEndCondition (propertyEntity);
			}

			// Emit the inner class
			this.Writer.WriteLineFormat (2, "[ObjectiveCClass]");

#if GENERATED_ATTRIBUTE
            this.Writer.WriteLineFormat(2, "[GeneratedCodeAttribute(\"{0}\", \"{1}\")]", GenerationHelper.ToolName, GenerationHelper.ToolVersion);
#endif

			// TODO: Set correct super-class
			this.Writer.WriteLineFormat (2, "public class {0}EventDispatcher : NSObject", protocolEntity.Name, protocolEntity.Namespace);
			this.Writer.WriteLineFormat (2, "{{");

			// Add constant for class access
			this.Writer.WriteLineFormat (3, "/// <summary>");
			this.Writer.WriteLineFormat (3, "/// Static field for a quick access to the {0}EventDispatcher class.", protocolEntity.Name);
			this.Writer.WriteLineFormat (3, "/// </summary>");
			this.Writer.WriteLineFormat (3, "public static readonly Class {0}EventDispatcherClass = Class.Get(typeof ({0}EventDispatcher));", protocolEntity.Name);
			this.Writer.WriteLine ();

			// Add default constructor
			this.Writer.WriteLineFormat (3, "/// <summary>");
			this.Writer.WriteLineFormat (3, "/// Initializes a new instance of the <see cref=\"{0}EventDispatcher\"/> class.", protocolEntity.Name);
			this.Writer.WriteLineFormat (3, "/// </summary>");
			this.Writer.WriteLineFormat (3, "public {0}EventDispatcher() {{}}", protocolEntity.Name);
			this.Writer.WriteLine ();

			// Add constructor with pointer parameter
			this.Writer.WriteLineFormat (3, "/// <summary>");
			this.Writer.WriteLineFormat (3, "/// Initializes a new instance of the <see cref=\"{0}EventDispatcher\"/> class.", protocolEntity.Name);
			this.Writer.WriteLineFormat (3, "/// </summary>");
			this.Writer.WriteLineFormat (3, "/// <param name=\"value\">The native pointer.</param>");
			this.Writer.WriteLineFormat (3, "public {0}EventDispatcher(IntPtr value)", protocolEntity.Name);
			this.Writer.WriteLineFormat (4, ": base(value) {{}}");
			this.Writer.WriteLine ();

			// Emit the respondsTo method
			this.Writer.WriteLineFormat (3, "[ObjectiveCMessage(\"respondsToSelector:\")]");
			this.Writer.WriteLineFormat (3, "public override bool RespondsToSelector(IntPtr aSelector)");
			this.Writer.WriteLineFormat (3, "{{");
			this.Writer.WriteLineFormat (4, "String message = ObjectiveCRuntime.Selector(aSelector);");
			this.Writer.WriteLineFormat (4, "switch (message)");
			this.Writer.WriteLineFormat (4, "{{");
			foreach (MethodEntity methodEntity in methods) {
				// Append static condition if needed
				this.AppendStartCondition (methodEntity);

				this.Writer.WriteLineFormat (5, "case \"{0}\":", methodEntity.Selector);
				this.Writer.WriteLineFormat (6, "return (this.{0} != null);", methodEntity.Name);

				// Append static condition if needed);
				this.AppendEndCondition (methodEntity);
			}
			this.Writer.WriteLineFormat (5, "default:");
			this.Writer.WriteLineFormat (6, "return this.SendMessageSuper<bool>({0}EventDispatcherClass, \"respondsToSelector:\", aSelector);", protocolEntity.Name);
			this.Writer.WriteLineFormat (4, "}}");
			this.Writer.WriteLineFormat (3, "}}");
			this.Writer.WriteLine ();

			// Emit the dealloc method
			this.Writer.WriteLineFormat (3, "[ObjectiveCMessage(\"dealloc\")]");
			this.Writer.WriteLineFormat (3, "public override void Dealloc()");
			this.Writer.WriteLineFormat (3, "{{");
			foreach (MethodEntity methodEntity in methods) {
				// Append static condition if needed
				this.AppendStartCondition (methodEntity);

				this.Writer.WriteLineFormat (4, "if (this.{0} != null)", methodEntity.Name);
				this.Writer.WriteLineFormat (4, "{{");
				this.Writer.WriteLineFormat (5, "foreach({0}EventHandler handler in this.{0}.GetInvocationList())", methodEntity.Name);
				this.Writer.WriteLineFormat (5, "{{");
				this.Writer.WriteLineFormat (6, "this.{0} -= handler;", methodEntity.Name);
				this.Writer.WriteLineFormat (5, "}}");
				this.Writer.WriteLineFormat (4, "}}");

				// Append static condition if needed);
				this.AppendEndCondition (methodEntity);
			}
			this.Writer.WriteLineFormat (4, "this.SendMessageSuper({0}EventDispatcherClass, \"dealloc\");", protocolEntity.Name);
			this.Writer.WriteLineFormat (3, "}}");
			this.Writer.WriteLine ();

			// Emit the event handlers
			foreach (MethodEntity methodEntity in methods) {
				// Append static condition if needed
				this.AppendStartCondition (methodEntity);

				this.AppendDocumentation (3, methodEntity);
				this.Writer.WriteLineFormat (3, "public event {0}EventHandler {0};", methodEntity.Name);

				// Append static condition if needed);
				this.AppendEndCondition (methodEntity);

				this.Writer.WriteLine ();
			}

			// Emit the handler methods
			foreach (MethodEntity methodEntity in methods) {
				bool hasReturnType = !String.Equals (methodEntity.ReturnType, "void", StringComparison.OrdinalIgnoreCase);

				// Append static condition if needed
				this.AppendStartCondition (methodEntity);

				this.AppendDocumentation (3, methodEntity);

				StringBuilder signature = new StringBuilder ();
				this.Writer.WriteLineFormat (3, "[ObjectiveCMessage(\"{0}\")]", methodEntity.Selector);
				signature.AppendFormat ("public {0} {1}Message(", methodEntity.ReturnType, methodEntity.Name);
				List<String> parameters = new List<String> ();
				foreach (MethodParameterEntity methodParameterEntity in methodEntity.Parameters.Where(p => p.Generate)) {
					parameters.Add (GetTypeSignature (methodParameterEntity));
				}
				signature.Append (String.Join (", ", parameters.ToArray ()));
				signature.Append (")");
				this.Writer.WriteLineFormat (3, signature.ToString ());

				this.Writer.WriteLineFormat (3, "{{");

				this.Writer.WriteLineFormat (4, "if (this.{0} == null)", methodEntity.Name);
				this.Writer.WriteLineFormat (4, "{{");
				this.Writer.WriteLineFormat (5, "throw new NotSupportedException(\"The delegate does not respond to '{0}' message.\");", methodEntity.Selector);
				this.Writer.WriteLineFormat (4, "}}");

				StringBuilder invocation = new StringBuilder ();
				if (hasReturnType) {
					invocation.Append ("return ");
				}
				invocation.AppendFormat ("this.{0}(", methodEntity.Name);
				parameters = new List<string> ();
				foreach (MethodParameterEntity p in methodEntity.Parameters.Where(p => p.Generate)) {
					parameters.Add (GetInvocationParameter (p));
				}
				invocation.Append (String.Join (", ", parameters.ToArray ()));
				invocation.Append (");");
				this.Writer.WriteLineFormat (4, invocation.ToString ());

				this.Writer.WriteLineFormat (3, "}}");

				// Append static condition if needed);
				this.AppendEndCondition (methodEntity);

				this.Writer.WriteLine ();
			}

			// Append inner-class ender
			this.Writer.WriteLineFormat (2, "}}");

			// Append class ender
			this.Writer.WriteLineFormat (1, "}}");

			// Append static condition if needed
			this.AppendEndCondition (delegatorEntity);

			// Append namespace ender
			this.Writer.WriteLineFormat (0, "}}");
		}

		/// <summary>
		///   Appends the documentation.
		/// </summary>
		private void AppendDocumentation (int indent, MethodEntity methodEntity)
		{
			// Append method comments
			this.Writer.WriteLineFormat (indent, "/// <summary>");
			foreach (String line in methodEntity.Summary) {
				this.Writer.WriteLineFormat (indent, "/// <para>{0}</para>", line.EscapeAll ());
			}
			this.Writer.WriteLineFormat (indent, "/// <para>Original signature is '{0}'</para>", methodEntity.Signature);
			this.AppendAvailability (indent, methodEntity);
			this.Writer.WriteLineFormat (indent, "/// </summary>");

			// Append parameters' comments
			foreach (MethodParameterEntity methodParameterEntity in methodEntity.Parameters.Where(p => p.Generate)) {
				this.Writer.WriteLineFormat (indent, "/// <param name=\"{0}\">{1}</param>", methodParameterEntity.Name.Trim ('@'), methodParameterEntity.Summary.Count > 0 ? methodParameterEntity.Summary [0].EscapeAll () : "MISSING");
			}

			// Append returns' comments
			if (!String.Equals (methodEntity.ReturnType, "void")) {
				this.Writer.WriteLineFormat (indent, "/// <returns>{0}</returns>", methodEntity.ReturnsDocumentation.EscapeAll ());
			}
		}
	}
}