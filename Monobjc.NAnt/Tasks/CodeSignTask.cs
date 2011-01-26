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
using System.IO;
using System.Linq;
using Monobjc.NAnt.Properties;
using Monobjc.Tools.External;
using Monobjc.Tools.Utilities;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Monobjc.NAnt.Tasks
{
    /// <summary>
    ///   This task can sign application bundle and generate signed application installer.
    /// </summary>
    [TaskName("codesign")]
    public class CodeSignTask : Task
    {
        /// <summary>
        ///   Gets or sets the bundle directory.
        /// </summary>
        /// <value>The bundle directory.</value>
        [TaskAttribute("bundle", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public DirectoryInfo Bundle { get; set; }

        /// <summary>
        ///   Gets or sets the certiticate.
        /// </summary>
        /// <value>The certiticate.</value>
        [TaskAttribute("certificate", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string Certiticate { get; set; }

        /// <summary>
        ///   Gets or sets the product definition.
        /// </summary>
        /// <value>The product definition.</value>
        [TaskAttribute("definition")]
        [StringValidator(AllowEmpty = false)]
        public FileInfo ProductDefinition { get; set; }

        /// <summary>
        ///   Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        [TaskAttribute("action", Required = true)]
        public SignAction Action { get; set; }

        /// <summary>
        ///   Executes the task.
        /// </summary>
        protected override void ExecuteTask()
        {
            String identity = null;

            // Fetch signing certificates
            IList<string> identities = KeyChainAccess.SigningIdentities;
            if (identities.Count == 0)
            {
                throw new BuildException(Resources.NoValidSigningIdentity);
            }

            // Select the required identity if hint is given
            if (!String.IsNullOrEmpty(this.Certiticate))
            {
                identity = identities.FirstOrDefault(i => i.Contains(this.Certiticate));
            }

            // Take the first one if the the default is required
            identity = identity ?? identities.First();

            // Performs the action
            switch (this.Action)
            {
                case SignAction.Sign:
                    CodeSign.SignApplication(this.Bundle.ToString(), identity);
                    break;
                case SignAction.Archive:
                    if (this.ProductDefinition == null)
                    {
                        throw new BuildException(Resources.NoProductDefinition);
                    }
                    ProductBuild.ArchiveApplication(this.Bundle.ToString(), identity, this.ProductDefinition.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///   Lists all possibles actions for the <see cref = "CodeSignTask" />.
        /// </summary>
        public enum SignAction
        {
            /// <summary>
            ///   Sign an application bundle.
            /// </summary>
            Sign,
            /// <summary>
            ///   Archive and create an installer for the application bundle
            /// </summary>
            Archive,
        }
    }
}