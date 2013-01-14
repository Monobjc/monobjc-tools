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
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml.Doxygen
{
	/// <summary>
	///   XHTML parser dedicated to functions.
	/// </summary>
	public class XhtmlDoxygenFunctionParser : XhtmlBaseParser
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "XhtmlFunctionParser" /> class.
		/// </summary>
		/// <param name = "settings">The settings.</param>
		/// <param name = "typeManager">The type manager.</param>
		public XhtmlDoxygenFunctionParser (NameValueCollection settings, TypeManager typeManager, TextWriter logger) : base(settings, typeManager, logger)
		{
		}

		/// <summary>
		///   Parses the specified entity.
		/// </summary>
		/// <param name = "entity">The entity.</param>
		/// <param name = "reader">The reader.</param>
		public override void Parse (BaseEntity entity, TextReader reader)
		{
			throw new NotImplementedException ();
		}

		public FunctionEntity Parse (TypedEntity typedEntity, string name, IEnumerable<XElement> elements)
		{
			FunctionEntity functionEntity = new FunctionEntity ();

			XElement declarationElement = (from el in elements
                                           where el.Name == "div" &&
				el.Attribute ("class") != null &&
				el.Attribute ("class").Value == "declaration_indent"
                                           select el).FirstOrDefault ();
			declarationElement = declarationElement ?? (from el in elements
                                                        where el.Name == "pre"
                                                        select el).FirstOrDefault ();

			XElement parameterElement = (from el in elements
                                         where el.Name == "div" &&
				el.Attribute ("class") != null &&
				el.Attribute ("class").Value == "param_indent"
                                         select el).FirstOrDefault ();

			XElement returnValueElement = (from el in elements
                                           where el.Name == "h5" && el.Value.Trim () == "Return Value"
                                           select el).FirstOrDefault ();

			//XElement discussionElement = (from el in elements
			//                              where el.Name == "h5" && el.Value.Trim() == "Discussion"
			//                              select el).FirstOrDefault();

			XElement availabilityElement = (from el in elements
                                            let term = el.Descendants ("dt").FirstOrDefault ()
                                            let definition = el.Descendants ("dd").FirstOrDefault ()
                                            where el.Name == "dl" &&
				term != null &&
				term.Value.Trim () == "Availability"
                                            select definition).FirstOrDefault ();

			functionEntity.Name = name;

			String signature = declarationElement.TrimAll ();
			if (signature.StartsWith ("#define")) {
				this.Logger.WriteLine ("SKIPPING define statement: " + name);
				return null;
			}
			functionEntity.Signature = signature;

			// Extract abstract
			IEnumerable<XElement> abstractElements = elements.SkipWhile (el => el.Name != "p").TakeWhile (el => el.Name == "p");
			foreach (XElement element in abstractElements) {
				String line = element.TrimAll ();
				if (!String.IsNullOrEmpty (line)) {
					functionEntity.Summary.Add (line);
				}
			}

			//// Extract discussion
			//if (discussionElement != null)
			//{
			//    IEnumerable<XElement> discussionElements = discussionElement.ElementsAfterSelf().TakeWhile(el => el.Name == "p");
			//    foreach (XElement element in discussionElements)
			//    {
			//        String line = element.TrimAll();
			//        if (!String.IsNullOrEmpty(line))
			//        {
			//            methodEntity.Summary.Add(line);
			//        }
			//    }
			//}

			// Parse signature
			signature = signature.Replace ("extern", String.Empty).Trim ();
			int pos = signature.IndexOf (name);
			if (pos == -1) {
				this.Logger.WriteLine ("MISMATCH between name and declaration: " + name);
				return null;
			}

			String returnType = signature.Substring (0, pos).Trim ();
			String parameters = signature.Substring (pos + name.Length).Trim ();
			parameters = parameters.Trim (';', '(', ')');
			if (parameters != "void") {
				foreach (string parameter in parameters.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries)) {
					String parameterType = "NOTYPE";
					String parameterName = "NONAME";

					Match r = PARAMETER_REGEX.Match (parameter);
					if (r.Success) {
						parameterType = r.Groups [2].Value.Trim ();
						parameterName = r.Groups [3].Value.Trim ();
					} else if (parameter.Trim () == "...") {
						parameterType = "params Object[]";
						parameterName = "values";
					} else {
						this.Logger.WriteLine ("FAILED to parse parameter: " + parameter);
						return null;
					}

					MethodParameterEntity parameterEntity = new MethodParameterEntity ();
					bool isOut, isByRef, isBlock;
					parameterEntity.Type = this.TypeManager.ConvertType (parameterType, out isOut, out isByRef, out isBlock, this.Logger);
					parameterEntity.IsOut = isOut;
					parameterEntity.IsByRef = isByRef;
					parameterEntity.IsBlock = isBlock;
					parameterEntity.Name = TypeManager.ConvertName(parameterName);
					functionEntity.Parameters.Add (parameterEntity);
				}
			}

			// Extract return type
			functionEntity.ReturnType = this.TypeManager.ConvertType (returnType, this.Logger);

			if (functionEntity.Parameters.Count > 0 && parameterElement != null) {
				XElement termList = parameterElement.Descendants ("dl").FirstOrDefault ();
				if (termList != null) {
					IEnumerable<XElement> dtList = from el in termList.Elements ("dt") select el;
					IEnumerable<XElement> ddList = from el in termList.Elements ("dd") select el;

					if (dtList.Count () == ddList.Count ()) {
						// Iterate over definitions
						for (int i = 0; i < dtList.Count(); i++) {
							String term = dtList.ElementAt (i).TrimAll ();
							//String summary = ddList.ElementAt(i).TrimAll();
							IEnumerable<String> summaries = ddList.ElementAt (i).Elements ("p").Select (p => p.Value.TrimAll ());

							// Find the parameter
							MethodParameterEntity parameterEntity = functionEntity.Parameters.Find (p => String.Equals (p.Name, term));
							if (parameterEntity != null) {
								//parameterEntity.Summary.Add(summary);
								foreach (string sum in summaries) {
									parameterEntity.Summary.Add (sum);
								}
							}
						}
					}
				}
			}

			// Fix the name only after looking for the documentation
			for (int i = 0; i < functionEntity.Parameters.Count; i++) {
				functionEntity.Parameters [i].Name = this.TypeManager.ConvertName (functionEntity.Parameters [i].Name);
			}

			// Get the summary for return type
			if (!String.Equals (functionEntity.ReturnType, "void", StringComparison.OrdinalIgnoreCase) && returnValueElement != null) {
				IEnumerable<XElement> returnTypeElements = returnValueElement.ElementsAfterSelf ().TakeWhile (el => el.Name == "p");
				functionEntity.ReturnsDocumentation = String.Empty;
				foreach (XElement element in returnTypeElements) {
					String line = element.TrimAll ();
					if (!String.IsNullOrEmpty (line)) {
						functionEntity.ReturnsDocumentation += line;
					}
				}
			}

			// Get the availability
			if (availabilityElement != null) {
				functionEntity.MinAvailability = CommentHelper.ExtractAvailability (availabilityElement.TrimAll ());
			}

			return functionEntity;
		}
	}
}