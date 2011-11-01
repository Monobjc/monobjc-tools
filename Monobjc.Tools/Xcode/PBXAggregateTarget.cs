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
namespace Monobjc.Tools.Xcode
{
	public class PBXAggregateTarget : PBXTarget
	{
		/// <summary>
		///   Gets the nature.
		/// </summary>
		/// <value>The nature.</value>
		public override PBXElementType Nature {
			get { return PBXElementType.PBXAggregateTarget; }
		}

		/// <summary>
		///   Accepts the specified visitor.
		/// </summary>
		/// <param name = "visitor">The visitor.</param>
		public override void Accept (IPBXVisitor visitor)
		{
			visitor.Visit (this);

			if (this.BuildConfigurationList != null) {
				this.BuildConfigurationList.Accept (visitor);
			}
			if (this.BuildPhases != null) {
				foreach (PBXBuildPhase phase in this.BuildPhases) {
					phase.Accept (visitor);
				}
			}
			if (this.Dependencies != null) {
				foreach (PBXTargetDependency dependency in this.Dependencies) {
					dependency.Accept (visitor);
				}
			}
		}
	}
}