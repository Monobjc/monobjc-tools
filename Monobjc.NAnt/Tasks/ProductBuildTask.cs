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
using System.IO;
using Monobjc.Tools.External;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Monobjc.NAnt.Tasks
{
	/// <summary>
	///   This task generate and signs the application installer.
	/// </summary>
	[TaskName("product-build")]
	public class ProductBuildTask : SigningTask
	{
		/// <summary>
		///   Gets or sets the product definition.
		/// </summary>
		/// <value>The product definition.</value>
		[TaskAttribute("definition", Required = false)]
		[StringValidator(AllowEmpty = false)]
		public FileInfo ProductDefinition { get; set; }

		/// <summary>
		///   Performs the signing.
		/// </summary>
		/// <param name = "identity">The identity.</param>
		protected override void PerformSigning (String identity)
		{
			String productDefinition = null;
			if (this.ProductDefinition != null && File.Exists (this.ProductDefinition.ToString ())) {
				productDefinition = this.ProductDefinition.ToString ();
			}

			using (StringWriter outputWriter = new StringWriter()) {
				using (StringWriter errorWriter = new StringWriter()) {
					ProductBuild.ArchiveApplication (this.Bundle.ToString (), identity, productDefinition, outputWriter, errorWriter);
					String outputLog = outputWriter.ToString ();
					String errorLog = errorWriter.ToString ();
					this.Log (Level.Info, outputLog);
					this.Log (Level.Info, errorLog);
				}
			}
		}
	}
}