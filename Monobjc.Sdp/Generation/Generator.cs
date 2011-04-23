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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using Monobjc.Tools.Sdp.Model;
using Mvp.Xml.XInclude;

namespace Monobjc.Tools.Sdp.Generation
{
    /// <summary>
    ///   Generator class.
    /// </summary>
    public class Generator
    {
        private readonly CodeDomProvider provider;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Generator" /> class.
        /// </summary>
        /// <param name = "provider">The provider.</param>
        private Generator(CodeDomProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        ///   Creates the generator.
        /// </summary>
        /// <param name = "language">The language.</param>
        /// <returns>A generator.</returns>
        public static Generator CreateGenerator(String language)
        {
            switch (language)
            {
                case "CSharp":
                    return new Generator(new CSharpCodeProvider());
                case "VBSharp":
                    return new Generator(new VBCodeProvider());
                default:
                    throw new NotSupportedException("Unsupported language: " + language);
            }
        }

        /// <summary>
        ///   Generates a wrapper.
        /// </summary>
        /// <param name = "prefix">The prefix.</param>
        /// <param name = "inputFile">The input file.</param>
        public void Generate(String prefix, String inputFile)
        {
            String workFile = Path.GetTempFileName();
            String outputFile = prefix + "." + this.Extension;
            if (!File.Exists(inputFile))
            {
                throw new ArgumentException("Input file not found: " + inputFile);
            }

            // Use a XInclude compliant parser to load all the inclusions
            Console.WriteLine("Parsing '" + inputFile + "' ...");
            using (XIncludingReader reader = new XIncludingReader(inputFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                doc.Save(workFile);
            }

            // Process the input file
            Console.WriteLine("Processing '" + inputFile + "' ...");
            using (TextReader reader = new StreamReader(workFile))
            {
                using (TextWriter writer = new StreamWriter(outputFile))
                {
                    this.Generate(prefix, reader, writer);
                }
            }
        }

        /// <summary>
        ///   Generates a wrapper.
        /// </summary>
        /// <param name = "prefix">The prefix.</param>
        /// <param name = "reader">The reader.</param>
        /// <param name = "writer">The writer.</param>
        public void Generate(String prefix, TextReader reader, TextWriter writer)
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(dictionary));
            XmlSerializer serializer = new dictionarySerializer();
            dictionary dictionary = serializer.Deserialize(reader) as dictionary;

            if (dictionary == null)
            {
                return;
            }

            IEnumerable<@class> classes = dictionary.suite.Where(s => s.Items != null).SelectMany(s => s.Items).Where(i => i is @class).Cast<@class>();
            IEnumerable<classextension> classExtensions = dictionary.suite.Where(s => s.Items != null).SelectMany(s => s.Items).Where(i => i is classextension).Cast<classextension>();
            IEnumerable<command> commands = dictionary.suite.Where(s => s.Items != null).SelectMany(s => s.Items).Where(i => i is command).Cast<command>();
            IEnumerable<enumeration> enumerations = dictionary.suite.Where(s => s.Items != null).SelectMany(s => s.Items).Where(i => i is enumeration).Cast<enumeration>();
            GenerationContext context = new GenerationContext(prefix, classes, classExtensions, commands, enumerations);

            // Create the compilation unit
            CodeCompileUnit compileUnit = this.Generate(context);

            // Write the result
            using (IndentedTextWriter tw = new IndentedTextWriter(writer))
            {
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                options.BlankLinesBetweenMembers = true;
                options.BracingStyle = "C";
                options.IndentString = " ";
                options.VerbatimOrder = true;
                this.provider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());
            }
        }

        private String Extension
        {
            get { return this.provider.FileExtension; }
        }

        private CodeCompileUnit Generate(GenerationContext context)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            // Add namespace
            CodeNamespace codeNamespace = new CodeNamespace("Monobjc.ScriptingBridge." + context.Prefix);
            compileUnit.Namespaces.Add(codeNamespace);

            // Add usings
            codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Monobjc"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Monobjc.Foundation"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Monobjc.AppKit"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Monobjc.ScriptingBridge"));

            // Add enumerations
            codeNamespace.Types.AddRange(GenerateEnumerations(context).ToArray());

            // Add application class
            @class appCls = context.Classes.FirstOrDefault(GenerationContext.IsApplicationClass);
            if (appCls == null)
            {
                throw new NotSupportedException("No application class found");
            }
            codeNamespace.Types.Add(this.GenerateClass(context, appCls));

            // Add other classes
            foreach (@class cls in context.Classes.Where(c => !GenerationContext.IsApplicationClass(c)))
            {
                codeNamespace.Types.Add(this.GenerateClass(context, cls));
            }

            return compileUnit;
        }

        private static IEnumerable<CodeTypeDeclaration> GenerateEnumerations(GenerationContext context)
        {
            foreach (enumeration enumeration in context.Enumerations)
            {
                yield return GenerateEnumeration(context, enumeration);
            }
        }

        private static CodeTypeDeclaration GenerateEnumeration(GenerationContext context, enumeration enumeration)
        {
            String enumerationName = NamingHelper.GenerateDotNetName(context.Prefix, enumeration.name);

            CodeTypeDeclaration typeDeclaration = new CodeTypeDeclaration();
            typeDeclaration.Name = enumerationName;
            typeDeclaration.IsEnum = true;
            typeDeclaration.BaseTypes.Add(typeof (uint));

            IEnumerable<enumerator> values = enumeration.Items.Where(i => i is enumerator).Cast<enumerator>();
            foreach (enumerator value in values)
            {
                CodeMemberField memberField = new CodeMemberField();
                memberField.Name = NamingHelper.GenerateDotNetName(enumerationName, value.name);

                if (!String.IsNullOrEmpty(value.code))
                {
                    uint constant = NamingHelper.ToUInt32(value.code);
                    memberField.InitExpression = new CodePrimitiveExpression(constant);
                    memberField.Comments.Add(new CodeCommentStatement("'" + value.code + "' => '0x" + constant.ToString("X8") + "'"));
                }

                typeDeclaration.Members.Add(memberField);
            }

            return typeDeclaration;
        }

        private CodeTypeDeclaration GenerateClass(GenerationContext context, @class cls)
        {
            bool isApplication = GenerationContext.IsApplicationClass(cls);

            CodeTypeDeclaration typeDeclaration = new CodeTypeDeclaration();
            typeDeclaration.IsClass = true;

            // Set the name and the base class name
            String className = context.ConvertType(cls.name, false);
            String baseClassName;
            if (isApplication)
            {
                baseClassName = "SBApplication";
            }
            else
            {
                baseClassName = String.IsNullOrEmpty(cls.inherits) ? "SBObject" : context.ConvertType(cls.inherits, false);
            }
            typeDeclaration.Name = className;
            typeDeclaration.BaseTypes.Add(baseClassName);

            // Add default constructor
            CodeConstructor defaultConstructor1 = new CodeConstructor();
            defaultConstructor1.Attributes = MemberAttributes.Public;
            typeDeclaration.Members.Add(defaultConstructor1);

            // Add default constructor with pointer
            CodeConstructor defaultConstructor2 = new CodeConstructor();
            defaultConstructor2.Attributes = MemberAttributes.Public;
            defaultConstructor2.Parameters.Add(new CodeParameterDeclarationExpression(typeof (IntPtr), "pointer"));
            defaultConstructor2.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("pointer"));
            typeDeclaration.Members.Add(defaultConstructor2);

            // Generate elements
            typeDeclaration.Members.AddRange(GenerateElements(context, cls).ToArray());

            // Generate properties
            typeDeclaration.Members.AddRange(GenerateProperties(context, cls).ToArray());

            // Generate commands
            typeDeclaration.Members.AddRange(GenerateCommands(context, cls).ToArray());

            return typeDeclaration;
        }

        private static IEnumerable<CodeMemberProperty> GenerateElements(GenerationContext context, @class cls)
        {
            foreach (element item in context.GetElementsFor(cls).OrderBy(i => i.type))
            {
                yield return GenerateElement(context, cls, item);
            }
        }

        private static IEnumerable<CodeMemberProperty> GenerateProperties(GenerationContext context, @class cls)
        {
            foreach (property item in context.GetPropertiesFor(cls).OrderBy(i => i.name))
            {
                yield return GenerateProperty(context, cls, item);
            }
        }

        private static IEnumerable<CodeMemberMethod> GenerateCommands(GenerationContext context, @class cls)
        {
            foreach (command item in context.GetCommandsFor(cls).OrderBy(i => i.name))
            {
                yield return GenerateCommand(context, cls, item);
            }
        }

        private static CodeMemberProperty GenerateElement(GenerationContext context, @class cls, element element)
        {
            String value;
            @class typeCls = context.Classes.FirstOrDefault(c => String.Equals(c.name, element.type));
            if (typeCls != null)
            {
                // Make sure that we use the correct plural
                value = typeCls.plural ?? typeCls.name + "s";
            }
            else
            {
                // Use the default name
                value = element.type;
            }
            String elementName = NamingHelper.GenerateDotNetName(String.Empty, value);
            String selector = NamingHelper.GenerateObjCName(value);

            // Define various references
            CodeTypeReference typeReference = new CodeTypeReference("SBElementArray");
            CodeThisReferenceExpression thisReferenceExpression = new CodeThisReferenceExpression();
            CodeTypeReferenceExpression typeReferenceExpression = new CodeTypeReferenceExpression("ObjectiveCRuntime");

            // Define the property
            CodeMemberProperty memberProperty = new CodeMemberProperty();
            memberProperty.Attributes = MemberAttributes.Public;
            memberProperty.Name = elementName;
            memberProperty.Type = typeReference;

            // Generate the getter
            CodeMethodReferenceExpression methodReferenceExpression = new CodeMethodReferenceExpression(typeReferenceExpression, "SendMessage");
            methodReferenceExpression.TypeArguments.Add(typeReference);
            CodeMethodInvokeExpression invokeExpression = new CodeMethodInvokeExpression(methodReferenceExpression, thisReferenceExpression, new CodePrimitiveExpression(selector));

            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(invokeExpression);
            memberProperty.GetStatements.Add(returnStatement);

            return memberProperty;
        }

        private static CodeMemberProperty GenerateProperty(GenerationContext context, @class cls, property property)
        {
            String propertyType = context.ConvertType(property.type, true);
            String propertyName = NamingHelper.GenerateDotNetName(String.Empty, property.name);

            // Define various references
            CodeTypeReference typeReference = new CodeTypeReference(propertyType);
            CodeThisReferenceExpression thisReferenceExpression = new CodeThisReferenceExpression();
            CodeTypeReferenceExpression typeReferenceExpression = new CodeTypeReferenceExpression("ObjectiveCRuntime");

            // Define the property
            CodeMemberProperty memberProperty = new CodeMemberProperty();
            memberProperty.Attributes = MemberAttributes.Public;
            memberProperty.Name = propertyName;
            memberProperty.Type = typeReference;

            // Generate getter
            switch (property.access)
            {
                case propertyAccess.r:
                case propertyAccess.rw:
                    {
                        String selector = NamingHelper.GenerateObjCName(property.name);

                        CodeMethodReferenceExpression methodReferenceExpression = new CodeMethodReferenceExpression(typeReferenceExpression, "SendMessage");
                        methodReferenceExpression.TypeArguments.Add(typeReference);
                        CodeMethodInvokeExpression invokeExpression = new CodeMethodInvokeExpression(methodReferenceExpression, thisReferenceExpression, new CodePrimitiveExpression(selector));

                        CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(invokeExpression);
                        memberProperty.GetStatements.Add(returnStatement);
                        break;
                    }
                default:
                    break;
            }

            // Generate setter
            switch (property.access)
            {
                case propertyAccess.rw:
                case propertyAccess.w:
                    {
                        String selector = "set" + propertyName + ":";

                        CodeMethodReferenceExpression methodReferenceExpression = new CodeMethodReferenceExpression(typeReferenceExpression, "SendMessage");
                        CodeMethodInvokeExpression invokeExpression = new CodeMethodInvokeExpression(methodReferenceExpression, thisReferenceExpression, new CodePrimitiveExpression(selector), new CodeVariableReferenceExpression("value"));

                        CodeExpressionStatement expressionStatement = new CodeExpressionStatement(invokeExpression);
                        memberProperty.SetStatements.Add(expressionStatement);
                        break;
                    }
                default:
                    break;
            }

            return memberProperty;
        }

        private static CodeMemberMethod GenerateCommand(GenerationContext context, @class cls, command command)
        {
            bool isApplication = GenerationContext.IsApplicationClass(cls);
            String returnType = context.ConvertType(GenerationContext.GetType(command.result) ?? "void", true);
            bool hasReturnType = returnType != "void";
            bool useDirectParameter = isApplication && command.directparameter != null && command.directparameter.type1 != "specifier";
            String methodName = NamingHelper.GenerateDotNetMethodsName(command);

            // Define various references
            CodeTypeReference typeReference = new CodeTypeReference(returnType);
            CodeThisReferenceExpression thisReferenceExpression = new CodeThisReferenceExpression();
            CodeTypeReferenceExpression typeReferenceExpression = new CodeTypeReferenceExpression("ObjectiveCRuntime");
            CodeMethodReferenceExpression methodReferenceExpression = new CodeMethodReferenceExpression(typeReferenceExpression, "SendMessage");
            if (hasReturnType)
            {
                methodReferenceExpression.TypeArguments.Add(typeReference);
            }

            // Define the method
            CodeMemberMethod memberMethod = new CodeMemberMethod();
            memberMethod.Attributes = MemberAttributes.Public;
            memberMethod.Name = methodName;
            if (hasReturnType)
            {
                memberMethod.ReturnType = typeReference;
            }

            // Gather all the expressions needed for the invocation.
            List<CodeExpression> expressions = new List<CodeExpression>();
            expressions.Add(thisReferenceExpression);
            expressions.Add(new CodePrimitiveExpression(NamingHelper.GenerateObjCSelector(command, isApplication)));

            // If the command is for the application and the direct parameter is not an object specifier, add it to the signature
            if (useDirectParameter)
            {
                CodeParameterDeclarationExpression parameterDeclarationExpression = new CodeParameterDeclarationExpression();
                parameterDeclarationExpression.Type = new CodeTypeReference(context.ConvertType(GenerationContext.GetType(command.directparameter), true));
                parameterDeclarationExpression.Name = "x";
                memberMethod.Parameters.Add(parameterDeclarationExpression);

                expressions.Add(new CodeVariableReferenceExpression(parameterDeclarationExpression.Name));
            }

            // Add all the parameters
            if (command.parameter != null)
            {
                foreach (parameter parameter in command.parameter)
                {
                    CodeParameterDeclarationExpression parameterDeclarationExpression = new CodeParameterDeclarationExpression();
                    parameterDeclarationExpression.Type = new CodeTypeReference(context.ConvertType(GenerationContext.GetType(parameter), true));
                    parameterDeclarationExpression.Name = GenerationContext.ConvertParameterName(NamingHelper.GenerateObjCName(parameter.name));
                    memberMethod.Parameters.Add(parameterDeclarationExpression);

                    expressions.Add(new CodeVariableReferenceExpression(parameterDeclarationExpression.Name));
                }
            }

            // Generate the runtime invocation
            CodeMethodInvokeExpression invokeExpression = new CodeMethodInvokeExpression(methodReferenceExpression, expressions.ToArray());
            CodeStatement expressionStatement;
            if (hasReturnType)
            {
                expressionStatement = new CodeMethodReturnStatement(invokeExpression);
            }
            else
            {
                expressionStatement = new CodeExpressionStatement(invokeExpression);
            }
            memberMethod.Statements.Add(expressionStatement);

            return memberMethod;
        }
    }
}