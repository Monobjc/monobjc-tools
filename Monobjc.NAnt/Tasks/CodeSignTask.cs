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
using System.Linq;
using Monobjc.Tools.External;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;
using System.Collections.Generic;

namespace Monobjc.NAnt.Tasks
{
	/// <summary>
	///   This task signs application bundle.
	/// </summary>
	[TaskName("codesign")]
	public class CodeSignTask : SigningTask
	{
		/// <summary>
		///   Gets or sets the entitlements.
		/// </summary>
		/// <value>The entitlements.</value>
		[TaskAttribute("entitlements", Required = false)]
		public FileInfo Entitlements { get; set; }

		/// <summary>
		/// Gets or sets the targets.
		/// </summary>
		/// <value>The targets.</value>
		[BuildElement ("targets", Required = false)]
		public FileSet Targets { get; set; }

		/// <summary>
		/// Performs the signing.
		/// </summary>
		/// <param name="identity">The identity.</param>
		protected override void PerformSigning (String identity)
		{
			List<String> paths = new List<String> ();
			if (this.Bundle != null) {
				paths.Add (this.Bundle.ToString ());
			} else if (this.Targets != null) {
				paths.AddRange (this.Targets.FileNames.OfType<String> ());
			} else {
				// TODO: I18N
				this.Log (Level.Error, "You must provide at least one element to sign.");
				return;
			}

			String entitlements = null;
			if (this.Entitlements != null && File.Exists (this.Entitlements.ToString ())) {
				entitlements = this.Entitlements.ToString ();
			}

			foreach (String path in paths) {
				using (StringWriter outputWriter = new StringWriter()) {
					using (StringWriter errorWriter = new StringWriter()) {
						CodeSign.PerformSigning (path, identity, entitlements, outputWriter, errorWriter);
						String outputLog = outputWriter.ToString ();
						String errorLog = errorWriter.ToString ();
						this.Log (Level.Info, outputLog);
						this.Log (Level.Info, errorLog);
					}
				}
			}
		}
	}
}