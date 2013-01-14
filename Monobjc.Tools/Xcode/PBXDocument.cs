//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2013 - Laurent Etiemble
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
using System.Text;
using Monobjc.Tools.Xcode.Visitors;

namespace Monobjc.Tools.Xcode
{
    public class PBXDocument
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PBXDocument" /> class.
        /// </summary>
        public PBXDocument()
        {
            this.ArchivedVersion = 1;
            this.Classes = new List<String>();
            this.RootObject = new PBXProject();
            this.ObjectVersion = XcodeCompatibilityVersion.Xcode_3_2;
        }

        /// <summary>
        ///   Gets or sets the archived version.
        /// </summary>
        /// <value>The archived version.</value>
        public int ArchivedVersion { get; set; }

        /// <summary>
        ///   Gets or sets the classes.
        /// </summary>
        /// <value>The classes.</value>
        public IList<string> Classes { get; set; }

        /// <summary>
        ///   Gets or sets the object version.
        /// </summary>
        /// <value>The object version.</value>
        public XcodeCompatibilityVersion ObjectVersion
        {
            get { return this.Project.CompatibilityVersion; }
            set { this.Project.CompatibilityVersion = value; }
        }

        /// <summary>
        ///   Gets or sets the root object.
        /// </summary>
        /// <value>The root object.</value>
        public PBXProject RootObject { get; private set; }

        /// <summary>
        ///   Gets the project.
        /// </summary>
        /// <value>The project.</value>
        public PBXProject Project
        {
            get { return this.RootObject; }
        }
		
		/// <summary>
		/// Gets the mapping.
		/// </summary>
		/// <value>
		/// The mapping.
		/// </value>
		public IDictionary<IPBXElement, String> Mapping { get; private set; }
		
        /// <summary>
        ///   Loads the specified content.
        /// </summary>
        /// <param name = "content">The content.</param>
        /// <returns></returns>
        public static PBXDocument Load(String content)
        {
            using (StringReader reader = new StringReader(content))
            {
                return Load(reader);
            }
        }

        /// <summary>
        ///   Loads the specified reader.
        /// </summary>
        /// <param name = "reader">The reader.</param>
        /// <returns></returns>
        public static PBXDocument Load(TextReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Loads from file.
        /// </summary>
        /// <param name = "path">The path.</param>
        /// <returns></returns>
        public static PBXDocument LoadFromFile(String path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return Load(reader);
            }
        }

        /// <summary>
        ///   Writes the specified writer.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        public void Write(ProjectWriter writer)
        {
            // 1. Collect all the objects
            CollectVisitor collectVisitor = new CollectVisitor();
            this.RootObject.Accept(collectVisitor);
            IDictionary<IPBXElement, String> map = collectVisitor.Map;
			this.Mapping = map;

            // 2. Ouput the prologue
            writer.WriteLine("// !$*UTF8*$!");
            writer.WriteLine("{");
            writer.WritePBXProperty(1, map, "archiveVersion", this.ArchivedVersion);
            writer.WritePBXProperty(1, map, "classes", new Dictionary<String, String>().ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            writer.WritePBXProperty(1, map, "objectVersion", (int) this.ObjectVersion);

            // 3. Output the objects by nature
            writer.WriteIndent(1);
            writer.WriteLine("{0} = {{", "objects");
            writer.WriteLine();

            // Iterate and output being/end section before dumping objects
            IEnumerable<KeyValuePair<IPBXElement, String>> list = from entry in map orderby entry.Key.Isa ascending select entry;
            PBXElementType currentType = PBXElementType.None;
            foreach (KeyValuePair<IPBXElement, string> pair in list)
            {
                if (currentType != pair.Key.Nature)
                {
                    if (currentType != PBXElementType.None)
                    {
                        writer.WriteLine("/* End {0} section */", currentType);
                        writer.WriteLine();
                    }
                    currentType = pair.Key.Nature;
                    writer.WriteLine("/* Begin {0} section */", currentType);
                }

                pair.Key.WriteTo(writer, map);
            }

            if (currentType != PBXElementType.None)
            {
                writer.WriteLine("/* End {0} section */", currentType);
                writer.WriteLine();
            }

            writer.WriteIndent(1);
            writer.WriteLine("};");

            // 4. Output the epilogue
            writer.WritePBXProperty(1, map, "rootObject", this.RootObject);
            writer.WriteLine("}");
        }

        /// <summary>
        ///   Writes to file.
        /// </summary>
        /// <param name = "path">The path.</param>
        public void WriteToFile(String path)
        {
            // Create a BOM-less encoding
            Encoding encoding = new UTF8Encoding(false);
            using (StreamWriter writer = new StreamWriter(path, false, encoding))
            {
				ProjectWriter projectWriter = new ProjectNSWriter(writer);
                this.Write(projectWriter);
            }
        }
    }
}