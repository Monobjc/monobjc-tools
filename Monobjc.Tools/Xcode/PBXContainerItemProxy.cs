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

namespace Monobjc.Tools.Xcode
{
	public class PBXContainerItemProxy : PBXElement
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "PBXContainerItemProxy" /> class.
		/// </summary>
		public PBXContainerItemProxy ()
		{
			this.ProxyType = 1;
		}

		/// <summary>
		///   Gets or sets the container portal.
		/// </summary>
		/// <value>The container portal.</value>
		public PBXFileReference ContainerPortal { get; set; }

		/// <summary>
		///   Gets or sets the type of the proxy.
		/// </summary>
		/// <value>The type of the proxy.</value>
		public int ProxyType { get; set; }

		/// <summary>
		///   Gets or sets the remote global ID string.
		/// </summary>
		/// <value>The remote global ID string.</value>
		public String RemoteGlobalIDString { get; set; }

		/// <summary>
		///   Gets or sets the remote info.
		/// </summary>
		/// <value>The remote info.</value>
		public String RemoteInfo { get; set; }

		/// <summary>
		///   Gets the nature.
		/// </summary>
		/// <value>The nature.</value>
		public override PBXElementType Nature {
			get { return PBXElementType.PBXContainerItemProxy; }
		}

		/// <summary>
		///   Gets the description.
		/// </summary>
		/// <value>The description.</value>
		public override string Description {
			get { return "PBXContainerItemProxy"; }
		}

		/// <summary>
		///   Accepts the specified visitor.
		/// </summary>
		/// <param name = "visitor">The visitor.</param>
		public override void Accept (IPBXVisitor visitor)
		{
			visitor.Visit (this);

			if (this.ContainerPortal != null) {
				this.ContainerPortal.Accept (visitor);
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
			writer.WritePBXProperty (3, map, "containerPortal", this.ContainerPortal);
			writer.WritePBXProperty (3, map, "proxyType", this.ProxyType);
			writer.WritePBXProperty (3, map, "remoteGlobalIDString", this.RemoteGlobalIDString);
			writer.WritePBXProperty (3, map, "remoteInfo", this.RemoteInfo);
			writer.WritePBXElementEpilogue (2);
		}
	}
}