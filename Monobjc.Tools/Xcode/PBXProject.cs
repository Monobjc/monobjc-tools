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
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Xcode
{
    /// <summary>
    ///   The element for the project.
    /// </summary>
    public class PBXProject : PBXElement
    {
        private XCConfigurationList buildConfigurationList;
        private readonly IList<String> knownRegions;
        private readonly IList<PBXTarget> targets;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXProject" /> class.
        /// </summary>
        public PBXProject()
        {
            this.BuildConfigurationList = new XCConfigurationList();
            this.CompatibilityVersion = XcodeCompatibilityVersion.Xcode_3_2;
            this.DevelopmentRegion = "English";
            this.HasScannedForEncodings = 1;
            this.knownRegions = new List<string> {"English", "Japanese", "French", "German"};
            this.MainGroup = new PBXGroup();
            this.ProjectDirPath = String.Empty;
            this.ProjectRoot = String.Empty;
            this.targets = new List<PBXTarget>();
        }

        /// <summary>
        ///   Gets or sets the build configuration list.
        /// </summary>
        /// <value>The build configuration list.</value>
        public XCConfigurationList BuildConfigurationList
        {
            get { return this.buildConfigurationList; }
            set
            {
                this.buildConfigurationList = value;
                this.buildConfigurationList.Target = this;
            }
        }

        /// <summary>
        ///   Gets or sets the compatibility version.
        /// </summary>
        /// <value>The compatibility version.</value>
        public XcodeCompatibilityVersion CompatibilityVersion { get; set; }

        /// <summary>
        ///   Gets or sets the development region.
        /// </summary>
        /// <value>The development region.</value>
        public String DevelopmentRegion { get; set; }

        /// <summary>
        ///   Gets or sets the has scanned for encodings.
        /// </summary>
        /// <value>The has scanned for encodings.</value>
        public int HasScannedForEncodings { get; set; }

        /// <summary>
        ///   Gets or sets the known regions.
        /// </summary>
        /// <value>The known regions.</value>
        public IEnumerable<String> KnownRegions
        {
            get { return this.knownRegions; }
        }

        /// <summary>
        ///   Adds the region.
        /// </summary>
        /// <param name = "region">The region.</param>
        public void AddRegion(String region)
        {
            this.knownRegions.Add(region);
        }

        /// <summary>
        ///   Removes the region.
        /// </summary>
        /// <param name = "region">The region.</param>
        public void RemoveRegion(String region)
        {
            this.knownRegions.Remove(region);
        }

        /// <summary>
        ///   Gets or sets the main group.
        /// </summary>
        /// <value>The main group.</value>
        public PBXGroup MainGroup { get; set; }

        /// <summary>
        /// Gets or sets the product ref group.
        /// </summary>
        /// <value>The product ref group.</value>
        public PBXGroup ProductRefGroup { get; set; }

        /// <summary>
        ///   Gets or sets the project dir path.
        /// </summary>
        /// <value>The project dir path.</value>
        public String ProjectDirPath { get; set; }

        /// <summary>
        ///   Gets or sets the project root.
        /// </summary>
        /// <value>The project root.</value>
        public String ProjectRoot { get; set; }

        /// <summary>
        ///   Gets or sets the targets.
        /// </summary>
        /// <value>The targets.</value>
        public IEnumerable<PBXTarget> Targets
        {
            get { return this.targets; }
        }

        /// <summary>
        ///   Adds the target.
        /// </summary>
        /// <param name = "target">The target.</param>
        public void AddTarget(PBXTarget target)
        {
            this.targets.Add(target);
        }

        /// <summary>
        ///   Removes the target.
        /// </summary>
        /// <param name = "target">The target.</param>
        public void RemoveTarget(PBXTarget target)
        {
            this.targets.Remove(target);
        }

        /// <summary>
        ///   Gets the nature.
        /// </summary>
        /// <value>The nature.</value>
        public override PBXElementType Nature
        {
            get { return PBXElementType.PBXProject; }
        }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get { return this.MainGroup.Name ?? this.Isa; }
        }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name = "visitor">The visitor.</param>
        public override void Accept(IPBXVisitor visitor)
        {
            visitor.Visit(this);

            if (this.BuildConfigurationList != null)
            {
                this.BuildConfigurationList.Accept(visitor);
            }
            if (this.MainGroup != null)
            {
                this.MainGroup.Accept(visitor);
            }
            if (this.Targets != null)
            {
                foreach (PBXTarget target in this.Targets)
                {
                    target.Accept(visitor);
                }
            }
        }

        /// <summary>
        ///   Writes this element to the writer.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "map">The map.</param>
        public override void WriteTo(TextWriter writer, IDictionary<IPBXElement, string> map)
        {
            writer.writeElementPrologue(map, this);
            if (this.BuildConfigurationList != null)
            {
                writer.WriteReference(map, "buildConfigurationList", this.BuildConfigurationList);
            }
            writer.WriteAttribute("compatibilityVersion", this.CompatibilityVersion.ToDescription());
            writer.WriteAttribute("developmentRegion", this.DevelopmentRegion);
            writer.WriteAttribute("hasScannedForEncodings", this.HasScannedForEncodings);
            writer.WriteLine("    {0} = (", "knownRegions");
            foreach (string region in this.KnownRegions)
            {
                writer.WriteLine("        {0},", region);
            }
            writer.WriteLine("    );");
            if (this.MainGroup != null)
            {
                writer.WriteReference(map, "mainGroup", this.MainGroup);
            }
            if (this.ProductRefGroup != null)
            {
                writer.WriteReference(map, "productRefGroup", this.ProductRefGroup);
            }
            writer.WriteAttribute("projectDirPath", this.ProjectDirPath);
            writer.WriteAttribute("projectRoot", this.ProjectRoot);
            writer.WriteReferences(map, "targets", this.Targets);
            writer.writeElementEpilogue();
        }
    }
}