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
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Monobjc.MSBuild.Properties;
using Monobjc.Tools.Utilities;

namespace Monobjc.MSBuild.Tasks
{
    /// <summary>
    ///   This task can sign native binaries.
    /// </summary>
    public abstract class Signing : Task
    {
        /// <summary>
        ///   Gets or sets the bundle directory.
        /// </summary>
        /// <value>The bundle directory.</value>
        [Required]
        public ITaskItem Bundle { get; set; }

        /// <summary>
        ///   Gets or sets the signing identity.
        /// </summary>
        /// <value>The identity.</value>
        [Required]
        public String Identity { get; set; }

        /// <summary>
        ///   Executes the task.
        /// </summary>
		public override bool Execute ()
		{
            String identity = this.Identity;

            // Fetch signing certificates
            IList<string> identities = KeyChainAccess.SigningIdentities;
            if (identities.Count == 0)
            {
				this.Log.LogError (Resources.NoValidSigningIdentity);
				return false;
            }

            // Select the required identity if hint is given
            if (!String.IsNullOrEmpty(identity))
            {
                identity = identities.FirstOrDefault(i => i.Contains(identity));
            }

            // Take the first one if the the default is required
            identity = identity ?? identities.FirstOrDefault();

            return this.PerformSigning(identity);
        }

        /// <summary>
        ///   Performs the signing.
        /// </summary>
        /// <param name = "identity">The identity.</param>
        protected abstract bool PerformSigning(String identity);
    }
}