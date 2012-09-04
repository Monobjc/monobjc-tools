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
using System.IO;
using Monobjc.Tools.External;
using Microsoft.Build.Framework;

namespace Monobjc.MSBuild.Tasks
{
	/// <summary>
	///   This task signs application bundle.
	/// </summary>
	public class CodeSigning : Signing
	{
		/// <summary>
		///   Gets or sets the entitlements.
		/// </summary>
		/// <value>The entitlements.</value>
		public ITaskItem Entitlements { get; set; }
		
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>The target.</value>
		public ITaskItem Target { get; set; }

		/// <summary>
		/// Gets or sets the targets.
		/// </summary>
		/// <value>The targets.</value>
		public ITaskItem[] Targets { get; set; }

		/// <summary>
		///   Performs the signing.
		/// </summary>
		/// <param name = "identity">The identity.</param>
		protected override bool PerformSigning (String identity)
		{
			ITaskItem[] items;
			if (this.Bundle != null) {
				items = new ITaskItem[]{this.Bundle};
			} else if (this.Target != null) {
				items = new ITaskItem[]{this.Target};
			} else if (this.Targets != null) {
				items = this.Targets;
			} else {
				// TODO: I18N
				this.Log.LogError ("You must provide at least one element to sign.");
				return false;
			}

			String entitlements = null;
			if (this.Entitlements != null && File.Exists (this.Entitlements.ItemSpec)) {
				entitlements = this.Entitlements.ItemSpec;
			}

			foreach (ITaskItem item in items) {
				using (StringWriter outputWriter = new StringWriter()) {
					using (StringWriter errorWriter = new StringWriter()) {
						CodeSign.SignApplication (item.ItemSpec, identity, entitlements, outputWriter, errorWriter);
						String outputLog = outputWriter.ToString ();
						String errorLog = errorWriter.ToString ();
						this.Log.LogMessage (outputLog);
						this.Log.LogMessage (errorLog);
					}
				}
			}
			return true;
		}
	}
}