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
using System.Linq;
using Monobjc.NAnt.Properties;
using Monobjc.Tools.Utilities;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;

namespace Monobjc.NAnt.Tasks
{
    /// <summary>
    ///   This task can sign native binaries.
    /// </summary>
    public abstract class SigningTask : Task
    {
        /// <summary>
        ///   Gets or sets the bundle directory.
        /// </summary>
        /// <value>The bundle directory.</value>
        [TaskAttribute("bundle", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public DirectoryInfo Bundle { get; set; }

        /// <summary>
        ///   Gets or sets the signing identity.
        /// </summary>
        /// <value>The identity.</value>
        [BuildElement("identity", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public Argument Identity { get; set; }

        /// <summary>
        ///   Executes the task.
        /// </summary>
        protected override void ExecuteTask()
        {
            String identity = this.Identity.Value;

            // Fetch signing certificates
            IList<string> identities = KeyChainAccess.SigningIdentities;
            if (identities.Count == 0)
            {
                throw new BuildException(Resources.NoValidSigningIdentity);
            }

            // Select the required identity if hint is given
            if (!String.IsNullOrEmpty(identity))
            {
                identity = identities.FirstOrDefault(i => i.Contains(identity));
            }

            // Take the first one if the the default is required
            identity = identity ?? identities.FirstOrDefault();

            this.PerformSigning(identity);
        }

        /// <summary>
        ///   Performs the signing.
        /// </summary>
        /// <param name = "identity">The identity.</param>
        protected abstract void PerformSigning(String identity);
    }
}