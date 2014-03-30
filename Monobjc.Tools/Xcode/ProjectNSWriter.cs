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
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Monobjc.Tools.Xcode
{
	/// <summary>
	/// Project writer that uses the NextStep style.
	/// </summary>
	public class ProjectNSWriter : ProjectWriter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Monobjc.Tools.Xcode.ProjectNSWriter"/> class.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		public ProjectNSWriter (TextWriter writer) : base(writer)
		{
		}
		
		/// <summary>
		/// Writes the indent.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		public override void WriteIndent (int indentLevel)
		{
			while (indentLevel-- > 0) {
				this.Write ("    ");
			}
		}

		/// <summary>
		/// Writes the PBX element prologue.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "element">The element.</param>
		public override void WritePBXElementPrologue (int indentLevel, IDictionary<IPBXElement, string> map, IPBXElement element)
		{
			this.WritePBXElementPrologue (indentLevel, map, element, false);
		}
		
		/// <summary>
		/// Writes the PBX element prologue.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "element">The element.</param>
		/// <param name = "singleLine">Output result is on a single line.</param>
		public override void WritePBXElementPrologue (int indentLevel, IDictionary<IPBXElement, string> map, IPBXElement element, bool singleLine)
		{
			this.WriteIndent (indentLevel);
			if (singleLine) {
				this.Write ("{0} /* {1} */ = {{", map [element], element.Description);
				this.Write (" {0} = {1}; ", "isa", element.Isa);
			} else {
				this.WriteLine ("{0} /* {1} */ = {{", map [element], element.Description);
				this.WriteIndent (indentLevel + 1);
				this.WriteLine ("{0} = {1};", "isa", element.Isa);
			}
		}

		/// <summary>
		/// Writes the PBX element epilogue.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		public override void WritePBXElementEpilogue (int indentLevel)
		{
			this.WritePBXElementEpilogue (indentLevel, false);
		}
		
		/// <summary>
		/// Writes the PBX element epilogue.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "singleLine">Output result is on a single line.</param>
		public override void WritePBXElementEpilogue (int indentLevel, bool singleLine)
		{
			if (singleLine) {
				this.WriteLine (" };");
			} else {
				this.WriteIndent (indentLevel);
				this.WriteLine ("};");
			}
		}

		/// <summary>
		/// Writes the PBX property.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "name">The name.</param>
		/// <param name = "value">The value.</param>
		public override void WritePBXProperty (int indentLevel, IDictionary<IPBXElement, String> map, String name, Object value)
		{
			this.WritePBXProperty (indentLevel, map, name, value, false);
		}
		
		/// <summary>
		/// Writes the PBX property.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "name">The name.</param>
		/// <param name = "value">The value.</param>
		/// <param name = "singleLine">Output result is on a single line.</param>
		public override void WritePBXProperty (int indentLevel, IDictionary<IPBXElement, String> map, String name, Object value, bool singleLine)
		{
			if (value == null) {
				return;
			}

			if (singleLine) {
				this.Write ("{0} = ", name);
				this.WritePBXValue (indentLevel, map, value);
				this.Write (";");
			} else {
				this.WriteIndent (indentLevel);
				this.Write ("{0} = ", name);
				this.WritePBXValue (indentLevel, map, value);
				this.WriteLine (";");
			}
		}

		/// <summary>
		///   Writes the PBX value.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "value">The value.</param>
		public override void WritePBXValue (int indentLevel, IDictionary<IPBXElement, String> map, Object value)
		{
			Type type = value.GetType ();
			if (type == typeof(int)) {
				this.Write ("{0}", value);
			} else if (type == typeof(String)) {
				this.Write ("\"{0}\"", value);
			} else if (typeof(IPBXElement).IsAssignableFrom (type)) {
				this.Write ("{0} /* {1} */", map [(IPBXElement)value], ((IPBXElement)value).Description);
			} else if (typeof(IList).IsAssignableFrom (type)) {
				this.WritePBXList (indentLevel, map, (IList)value);
			} else if (typeof(IDictionary).IsAssignableFrom (type)) {
				this.WritePBXDictionary (indentLevel, map, (IDictionary)value);
			} else {
				throw new NotSupportedException ();
			}
		}

		/// <summary>
		///   Writes the list.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "values">The values.</param>
		public override void WritePBXList (int indentLevel, IDictionary<IPBXElement, String> map, IList values)
		{
			this.WriteLine ("(");

			// For each value, the output is different
			foreach (Object value in values) {
				this.WriteIndent (indentLevel + 1);
				this.WritePBXValue (indentLevel + 1, map, value);
				this.WriteLine (",");
			}

			this.WriteIndent (indentLevel);
			this.Write (")");
		}

		/// <summary>
		///   Writes the dictionary.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "dictionary">The dictionary.</param>
		public override void WritePBXDictionary (int indentLevel, IDictionary<IPBXElement, String> map, IDictionary dictionary)
		{
			this.WriteLine ("{");

			// For each key/value pair, the output is different
			foreach (String key in dictionary.Keys) {
				Object value = dictionary [key];
				this.WritePBXProperty (indentLevel + 1, map, key, value);
			}

			this.WriteIndent (indentLevel);
			this.Write ("}");
		}
	}
}