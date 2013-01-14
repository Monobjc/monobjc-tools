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
using System.IO;
using Microsoft.Build.Framework;
using Monobjc.Tools.External;

namespace Monobjc.MSBuild.Tasks
{
	/// <summary>
	///   This task generate and signs the application installer.
	/// </summary>
	public class ProductBuilding : Signing
	{
		/// <summary>
		///   Gets or sets the product definition.
		/// </summary>
		/// <value>The product definition.</value>
		public ITaskItem ProductDefinition { get; set; }

		/// <summary>
		///   Performs the signing.
		/// </summary>
		/// <param name = "identity">The identity.</param>
		protected override bool PerformSigning (String identity)
		{
			String productDefinition = null;
			if (this.ProductDefinition != null && File.Exists (this.ProductDefinition.ItemSpec)) {
				productDefinition = this.ProductDefinition.ItemSpec;
			}

			using (StringWriter outputWriter = new StringWriter()) {
				using (StringWriter errorWriter = new StringWriter()) {
					ProductBuild.ArchiveApplication (this.Bundle.ItemSpec, identity, productDefinition, outputWriter, errorWriter);
					String outputLog = outputWriter.ToString ();
					String errorLog = errorWriter.ToString ();
					this.Log.LogMessage (outputLog);
					this.Log.LogMessage (errorLog);
				}
			}

			return true;
		}
	}
}