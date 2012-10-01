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
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model
{
	/// <summary>
	///   Represents the model for a protocol.
	/// </summary>
	[Serializable]
	[XmlRoot("Protocol")]
	public partial class ProtocolEntity : ClassEntity
	{

		/// <summary>
		///   Gets or sets the class owner.
		/// </summary>
		/// <value>The class owner.</value>
		[XmlElement ("DelegateFor")]
		public String DelegateFor {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the delegate property.
		/// </summary>
		/// <value>The delegate property.</value>
		[XmlElement ("DelegateProperty")]
		public String DelegateProperty {
			get;
			set;
		}

		/// <summary>
		///   Gets or sets the delegate owner.
		/// </summary>
		/// <value>The delegate owner.</value>
		[XmlIgnore]
		public ClassEntity DelegatorEntity {
			get;
			set;
		}

		/// <summary>
		///   Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		///   A hash code for the current <see cref = "T:System.Object" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode ()
		{
			unchecked {
				int hash = base.GetHashCode();
				hash = hash * 23 + (this.DelegateFor != null ? this.DelegateFor.GetHashCode () : 0);
				hash = hash * 23 + (this.DelegateProperty != null ? this.DelegateProperty.GetHashCode () : 0);
				return hash;
			}
		}
	}
}