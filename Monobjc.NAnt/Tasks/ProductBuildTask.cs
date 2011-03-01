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
        protected override void PerformSigning(string identity)
        {
            string output = ProductBuild.ArchiveApplication(this.Bundle.ToString(), identity, (this.ProductDefinition != null && this.ProductDefinition.Exists) ? this.ProductDefinition.ToString() : null);
            this.Log(Level.Info, output);
        }
    }
}