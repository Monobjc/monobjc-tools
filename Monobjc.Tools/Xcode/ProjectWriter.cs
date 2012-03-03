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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monobjc.Tools.Xcode
{
	/// <summary>
	/// Project writer.
	/// </summary>
	public abstract class ProjectWriter : TextWriter
	{
		private readonly TextWriter writer;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Monobjc.Tools.Xcode.ProjectWriter"/> class.
		/// </summary>
		/// <param name='writer'>
		/// Writer.
		/// </param>
		protected ProjectWriter (TextWriter writer)
		{
			this.writer = writer;
		}
		
		/// <summary>
		/// Gets the encoding.
		/// </summary>
		/// <value>
		/// The encoding.
		/// </value>
		public override Encoding Encoding {
			get {
				return this.writer.Encoding;
			}
		}
		
		/// <summary>
		/// Gets the format provider.
		/// </summary>
		/// <value>
		/// The format provider.
		/// </value>
		public override IFormatProvider FormatProvider {
			get {
				return this.writer.FormatProvider;
			}
		}
		
		/// <summary>
		/// Close this instance.
		/// </summary>
		public override void Close ()
		{
			this.writer.Close ();
		}
		
		/// <summary>
		/// Write the specified value.
		/// </summary>
		/// <param name='value'>
		/// Value.
		/// </param>
		public override void Write (char value)
		{
			this.writer.Write (value);
		}
		
		/// <summary>
		/// Writes the indent.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		public abstract void WriteIndent (int indentLevel);
		
		/// <summary>
		/// Writes the PBX element prologue.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "element">The element.</param>
		public abstract void WritePBXElementPrologue (int indentLevel, IDictionary<IPBXElement, string> map, IPBXElement element);
		
		/// <summary>
		/// Writes the PBX element prologue.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "element">The element.</param>
		/// <param name = "singleLine">Output result is on a single line.</param>
		public abstract void WritePBXElementPrologue (int indentLevel, IDictionary<IPBXElement, string> map, IPBXElement element, bool singleLine);
		
		/// <summary>
		/// Writes the PBX element epilogue.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		public abstract void WritePBXElementEpilogue (int indentLevel);
		
		/// <summary>
		/// Writes the PBX element epilogue.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "singleLine">Output result is on a single line.</param>
		public abstract void WritePBXElementEpilogue (int indentLevel, bool singleLine);
		
		/// <summary>
		/// Writes the PBX property.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "name">The name.</param>
		/// <param name = "value">The value.</param>
		public abstract void WritePBXProperty (int indentLevel, IDictionary<IPBXElement, String> map, String name, Object value);
		
		/// <summary>
		/// Writes the PBX property.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "name">The name.</param>
		/// <param name = "value">The value.</param>
		/// <param name = "singleLine">Output result is on a single line.</param>
		public abstract void WritePBXProperty (int indentLevel, IDictionary<IPBXElement, String> map, String name, Object value, bool singleLine);
		
		/// <summary>
		///   Writes the PBX value.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "value">The value.</param>
		public abstract void WritePBXValue (int indentLevel, IDictionary<IPBXElement, String> map, Object value);
		
		/// <summary>
		///   Writes the list.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "values">The values.</param>
		public abstract void WritePBXList (int indentLevel, IDictionary<IPBXElement, String> map, IList values);
		
		/// <summary>
		///   Writes the dictionary.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "indentLevel">The indent level.</param>
		/// <param name = "map">The map.</param>
		/// <param name = "dictionary">The dictionary.</param>
		public abstract void WritePBXDictionary (int indentLevel, IDictionary<IPBXElement, String> map, IDictionary dictionary);
	}
}
