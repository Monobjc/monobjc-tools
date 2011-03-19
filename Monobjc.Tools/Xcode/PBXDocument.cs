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
            this.ObjectVersion = XcodeCompatibilityVersion.Xcode_3_2;
            this.RootObject = new PBXProject();
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
        public XcodeCompatibilityVersion ObjectVersion { get; set; }

        /// <summary>
        ///   Gets or sets the root object.
        /// </summary>
        /// <value>The root object.</value>
        public PBXProject RootObject { get; set; }

        public static PBXDocument Load(String content)
        {
            using (StringReader reader = new StringReader(content))
            {
                return Load(reader);
            }
        }

        public static PBXDocument Load(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public static PBXDocument LoadFromFile(String path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return Load(reader);
            }
        }

        public void Write(TextWriter writer)
        {
            // 1. Collect all the objects
            CollectVisitor collectVisitor = new CollectVisitor();
            this.RootObject.Accept(collectVisitor);
            IDictionary<IPBXElement, String> map = collectVisitor.Map;

            // 2. Ouput the prologue
            writer.WriteLine("// !$*UTF8*$!");
            writer.WriteLine("{");
            writer.WriteAttribute("archiveVersion", this.ArchivedVersion);
            writer.WriteLine("    {0} = {{", "classes");
            writer.WriteLine("    };");
            writer.WriteAttribute("objectVersion", (int) this.ObjectVersion);

            // 3. Output the objects by nature
            writer.WriteLine("    {0} = {{", "objects");
            IEnumerable<KeyValuePair<IPBXElement, String>> list = from entry in map orderby entry.Key.Nature ascending select entry;
            foreach (KeyValuePair<IPBXElement, string> pair in list)
            {
                pair.Key.WriteTo(writer, map);
            }
            writer.WriteLine("    };");

            // 4. Output the epilogue
            writer.WriteReference(map, "rootObject", this.RootObject);
            writer.WriteLine("}");
        }

        public void WriteToFile(String path)
        {
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                this.Write(writer);
            }
        }
    }
}