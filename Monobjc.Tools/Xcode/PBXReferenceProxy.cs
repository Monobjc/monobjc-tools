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
using System.IO;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Xcode
{
	public class PBXReferenceProxy : PBXFileElement
	{
		private PBXContainerItemProxy containerItemProxy;
		
		/// <summary>
		///   Initializes a new instance of the <see cref = "PBXReferenceProxy" /> class.
		/// </summary>
		public PBXReferenceProxy ()
		{
		}
		
		/// <summary>
		/// Gets or sets the type of the file.
		/// </summary>
		/// <value>
		/// The type of the file.
		/// </value>
		public PBXFileType FileType { get; set; }
		
		/// <summary>
		/// Gets or sets the path.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		public String Path { get; set; }
		
		/// <summary>
		/// Gets or sets the remote reference.
		/// </summary>
		/// <value>
		/// The remote reference.
		/// </value>
		public PBXContainerItemProxy RemoteRef {
			get {
				return this.containerItemProxy;
			}set {
				this.containerItemProxy = value;
				if (this.containerItemProxy != null) {
					this.Name = this.containerItemProxy.RemoteInfo;
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the source tree.
		/// </summary>
		/// <value>
		/// The source tree.
		/// </value>
		public PBXSourceTree SourceTree { get; set; }
		
		/// <summary>
		///   Gets the nature.
		/// </summary>
		/// <value>The nature.</value>
		public override PBXElementType Nature {
			get { return PBXElementType.PBXReferenceProxy; }
		}
		
		/// <summary>
		///   Accepts the specified visitor.
		/// </summary>
		/// <param name = "visitor">The visitor.</param>
		public override void Accept (IPBXVisitor visitor)
		{
			visitor.Visit (this);

			if (this.RemoteRef != null) {
				this.RemoteRef.Accept (visitor);
			}
		}

		/// <summary>
		///   Writes this element to the writer.
		/// </summary>
		/// <param name = "writer">The writer.</param>
		/// <param name = "map">The map.</param>
		public override void WriteTo (ProjectWriter writer, IDictionary<IPBXElement, string> map)
		{
			writer.WritePBXElementPrologue (2, map, this);
			writer.WritePBXProperty (3, map, "fileType", this.FileType.ToDescription ());
			writer.WritePBXProperty (3, map, "path", this.Path);
			writer.WritePBXProperty (3, map, "remoteRef", this.RemoteRef);
			writer.WritePBXProperty (3, map, "sourceTree", this.SourceTree.ToDescription ());
			writer.WritePBXElementEpilogue (2);
		}
	}
}