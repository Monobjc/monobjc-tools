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
using System.Collections.Specialized;
using System.IO;
using System.Xml.Serialization;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Tasks
{
    /// <summary>
    ///   Context for task execution.
    /// </summary>
    public class TaskContext
    {
        /// <summary>
        ///   Gets or sets a value indicating whether the context will force the execution.
        /// </summary>
        /// <value><c>true</c> if the execution is forced; otherwise, <c>false</c>.</value>
        public bool Force { get; set; }

        /// <summary>
        ///   Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public NameValueCollection Settings { get; set; }

        /// <summary>
        ///   Gets or sets the entries.
        /// </summary>
        /// <value>The entries.</value>
        public Entries Entries { get; private set; }

        /// <summary>
        ///   Gets or sets the type manager.
        /// </summary>
        /// <value>The type manager.</value>
        public TypeManager TypeManager { get; set; }

        /// <summary>
        ///   Loads the database.
        /// </summary>
        public void Load()
        {
            if (File.Exists(Storage))
            {
                Console.WriteLine("Loading database...");
                using (StreamReader reader = new StreamReader(Storage))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof (Entries));
                    this.Entries = (Entries) serializer.Deserialize(reader);
                }
            }
            else
            {
                this.Entries = new Entries();
            }
        }

        /// <summary>
        ///   Saves the database.
        /// </summary>
        public void Save()
        {
            lock (this)
            {
                this.Entries.Sort();

                Console.WriteLine("Saving database...");
                using (StreamWriter writer = new StreamWriter(Storage))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof (Entries));
                    serializer.Serialize(writer, this.Entries);
                }
            }
        }

        private String Storage
        {
            get { return this.Settings["Storage"]; }
        }
    }
}