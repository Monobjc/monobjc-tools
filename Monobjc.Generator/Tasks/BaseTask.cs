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
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Monobjc.Tools.Generator.Generators;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Tasks
{
    /// <summary>
    ///   Base implementation of a unit of work.
    /// </summary>
    public abstract class BaseTask : ITask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "BaseTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        protected BaseTask(String name)
        {
            this.Name = name;
        }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public String Name { get; private set; }

        /// <summary>
        ///   Gets or sets the task context.
        /// </summary>
        /// <value>The task context.</value>
        public TaskContext Context { get; set; }

        /// <summary>
        ///   Gets the settings.
        /// </summary>
        /// <value>The settings.</value>
        protected NameValueCollection Settings
        {
            get { return this.Context.Settings; }
        }

        /// <summary>
        ///   Gets the entries.
        /// </summary>
        /// <value>The entries.</value>
        protected Entries Entries
        {
            get { return this.Context.Entries; }
        }

        /// <summary>
        ///   Gets the type manager.
        /// </summary>
        /// <value>The type manager.</value>
        protected TypeManager TypeManager
        {
            get { return this.Context.TypeManager; }
        }

        /// <summary>
        ///   Gets the entries with urls.
        /// </summary>
        /// <value>The entries with urls.</value>
        protected IEnumerable<Entry> EntriesWithUrls
        {
            get { return this.Entries.Where(entry => !String.IsNullOrEmpty(entry.RemoteUrl)); }
        }

        /// <summary>
        ///   Gets the entries with empty urls.
        /// </summary>
        /// <value>The entries with empty urls.</value>
        protected IEnumerable<Entry> EntriesWithEmptyUrls
        {
            get { return this.Entries.Where(entry => String.IsNullOrEmpty(entry.RemoteUrl)); }
        }

        /// <summary>
        ///   Loads the license.
        /// </summary>
        public void LoadLicense()
        {
            GenerationHelper.LoadLicense(this.Settings);
        }

        /// <summary>
        ///   Display a nice banner.
        /// </summary>
        public void DisplayBanner()
        {
            Console.WriteLine("########################################");
            Console.WriteLine("Task: {0}", this.Name);
            Console.WriteLine("########################################");
        }

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public abstract void Execute();

        protected bool NeedProcessing(String sourceFile, String destFile)
        {
            if (!File.Exists(sourceFile))
            {
                return false;
            }
            if (!this.Context.Force && destFile.IsYoungerThan(sourceFile))
            {
                return false;
            }
            return true;
        }

        protected Entry Find(String framework, String nature, String name)
        {
            if (String.IsNullOrEmpty(framework))
            {
                return this.Entries.SingleOrDefault(t => t.Nature.Equals(nature) &&
                                                         t.Name.Equals(name));
            }
            return this.Entries.SingleOrDefault(t => t.Namespace.Equals(framework) &&
                                                     t.Nature.Equals(nature) &&
                                                     t.Name.Equals(name));
        }
    }
}