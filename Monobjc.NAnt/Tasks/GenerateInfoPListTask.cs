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
using System.Reflection;
using Monobjc.NAnt.Properties;
using Monobjc.Tools.Generators;
using Monobjc.Tools.Utilities;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Monobjc.NAnt.Tasks
{
    [TaskName("gen-plist")]
    public class GenerateInfoPListTask : Task
    {
        private readonly InfoPListGenerator generator;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "GenerateInfoPListTask" /> class.
        /// </summary>
        public GenerateInfoPListTask()
        {
            this.generator = new InfoPListGenerator();
        }

        /// <summary>
        ///   Gets or sets the main assembly.
        /// </summary>
        /// <value>The main assembly.</value>
        [TaskAttribute("assembly")]
        [StringValidator(AllowEmpty = false)]
        public FileInfo MainAssembly { get; set; }

        /// <summary>
        ///   Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        [TaskAttribute("template")]
        [StringValidator(AllowEmpty = false)]
        public FileInfo Template { get; set; }

        /// <summary>
        ///   Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        [TaskAttribute("name")]
        [StringValidator(AllowEmpty = false)]
        public string ApplicationName
        {
            get { return this.generator.ApplicationName; }
            set { this.generator.ApplicationName = value; }
        }

        /// <summary>
        ///   Gets or sets the development region.
        /// </summary>
        /// <value>The development region.</value>
        [TaskAttribute("region")]
        public string DevelopmentRegion
        {
            get { return this.generator.DevelopmentRegion; }
            set { this.generator.DevelopmentRegion = value; }
        }

        /// <summary>
        ///   Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        [TaskAttribute("icon")]
        [StringValidator(AllowEmpty = false)]
        public String Icon
        {
            get { return this.generator.Icon; }
            set { this.generator.Icon = value; }
        }

        /// <summary>
        ///   Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [TaskAttribute("identifier")]
        [StringValidator(AllowEmpty = false)]
        public String Identifier
        {
            get { return this.generator.Identifier; }
            set { this.generator.Identifier = value; }
        }

        /// <summary>
        ///   Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        [TaskAttribute("version")]
        [StringValidator(AllowEmpty = false)]
        public String Version
        {
            get { return this.generator.Version; }
            set { this.generator.Version = value; }
        }

        /// <summary>
        ///   Gets or sets the mininum required OS version.
        /// </summary>
        /// <value>The mininum required OS version.</value>
        [TaskAttribute("target-os")]
        [StringValidator(AllowEmpty = false)]
        public MacOSVersion TargetOSVersion
        {
            get { return this.generator.TargetOSVersion; }
            set { this.generator.TargetOSVersion = value; }
        }

        /// <summary>
        ///   Gets or sets the main nib file.
        /// </summary>
        /// <value>The main nib file.</value>
        [TaskAttribute("main-nib")]
        [StringValidator(AllowEmpty = false)]
        public String MainNibFile
        {
            get { return this.generator.MainNibFile; }
            set { this.generator.MainNibFile = value; }
        }

        /// <summary>
        ///   Gets or sets the principal class.
        /// </summary>
        /// <value>The principal class.</value>
        [TaskAttribute("principal")]
        [StringValidator(AllowEmpty = false)]
        public String PrincipalClass
        {
            get { return this.generator.PrincipalClass; }
            set { this.generator.PrincipalClass = value; }
        }

        /// <summary>
        ///   Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
        [TaskAttribute("todir", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public DirectoryInfo ToDirectory { get; set; }

        /// <summary>
        ///   Executes the task.
        /// </summary>
        protected override void ExecuteTask()
        {
            this.Log(Level.Info, "Generating Info.plist...");

            // Check that the mandatory information is set);
            if (this.MainAssembly == null && this.ApplicationName == null)
            {
                throw new BuildException(Resources.CannotComputeApplicationName);
            }

            // Use the given template if any
            if (this.Template != null)
            {
                using (StreamReader reader = this.Template.OpenText())
                {
                    this.generator.Content = reader.ReadToEnd();
                }
            }

            // Extract information from the assembly if set
            if (this.MainAssembly != null)
            {
                Assembly assembly = Assembly.ReflectionOnlyLoadFrom(this.MainAssembly.ToString());
                AssemblyName assemblyName = assembly.GetName();
                this.generator.ApplicationName = assemblyName.Name;
                this.generator.Identifier = assembly.EntryPoint.DeclaringType.Namespace;
                this.generator.Version = assemblyName.Version.ToString();
            }

            // Write the file
            String path = Path.Combine(this.ToDirectory.ToString(), "Info.plist");
            this.generator.WriteTo(path);
        }
    }
}