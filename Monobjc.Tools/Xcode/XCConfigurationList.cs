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
using System.Globalization;
using System.IO;
using System.Linq;

namespace Monobjc.Tools.Xcode
{
	public class XCConfigurationList : PBXElement
	{
		private readonly IList<XCBuildConfiguration> buildConfigurations;

		/// <summary>
		///   Initializes a new instance of the <see cref = "XCConfigurationList" /> class.
		/// </summary>
		public XCConfigurationList ()
		{
			this.buildConfigurations = new List<XCBuildConfiguration> ();
			this.DefaultConfigurationIsVisible = 0;
		}

		/// <summary>
		///   Gets or sets the build configurations.
		/// </summary>
		/// <value>The build configurations.</value>
		public IEnumerable<XCBuildConfiguration> BuildConfigurations {
			get { return this.buildConfigurations; }
		}

		/// <summary>
		///   Adds the build configuration.
		/// </summary>
		/// <param name = "configuration">The configuration.</param>
		public void AddBuildConfiguration (XCBuildConfiguration configuration)
		{
			this.buildConfigurations.Add (configuration);
		}

		/// <summary>
		///   Removes the build configuration.
		/// </summary>
		/// <param name = "configuration">The configuration.</param>
		public void RemoveBuildConfiguration (XCBuildConfiguration configuration)
		{
			this.buildConfigurations.Remove (configuration);
		}

		public XCBuildConfiguration FindConfiguration (String name)
		{
			return this.buildConfigurations.FirstOrDefault (c => String.Equals (c.Name, name));
		}

		/// <summary>
		///   Gets or sets the default configuration is visible.
		/// </summary>
		/// <value>The default configuration is visible.</value>
		public int DefaultConfigurationIsVisible { get; set; }

		/// <summary>
		///   Gets or sets the default name of the configuration.
		/// </summary>
		/// <value>The default name of the configuration.</value>
		public string DefaultConfigurationName { get; set; }

		/// <summary>
		///   Gets or sets the target.
		/// </summary>
		/// <value>The target.</value>
		internal PBXElement Target { get; set; }

		/// <summary>
		///   Gets the elemnt's nature.
		/// </summary>
		/// <value>The nature.</value>
		public override PBXElementType Nature {
			get { return PBXElementType.XCConfigurationList; }
		}

		/// <summary>
		///   Gets the description.
		/// </summary>
		/// <value>The description.</value>
		public override string Description {
			get { return String.Format (CultureInfo.CurrentCulture, "Build configuration list for {0} \"{1}\"", this.Target.Isa, this.Target.Description); }
		}

		/// <summary>
		///   Accepts the specified visitor.
		/// </summary>
		/// <param name = "visitor">The visitor.</param>
		public override void Accept (IPBXVisitor visitor)
		{
			visitor.Visit (this);

			if (this.BuildConfigurations != null) {
				foreach (XCBuildConfiguration configuration in this.BuildConfigurations) {
					configuration.Accept (visitor);
				}
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
			writer.WritePBXProperty (3, map, "buildConfigurations", this.BuildConfigurations);
			writer.WritePBXProperty (3, map, "defaultConfigurationIsVisible", this.DefaultConfigurationIsVisible);
			writer.WritePBXProperty (3, map, "defaultConfigurationName", this.DefaultConfigurationName);
			writer.WritePBXElementEpilogue (2);
		}
	}
}