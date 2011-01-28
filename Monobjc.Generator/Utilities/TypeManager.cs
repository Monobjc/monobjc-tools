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
using System.Xml.Serialization;
using Monobjc.Tools.Generator.Model.Configuration;

namespace Monobjc.Tools.Generator.Utilities
{
    public class TypeManager
    {
        private readonly List<String> Classes = new List<String>();

        private readonly Dictionary<String, String> Mappings = new Dictionary<String, String>();

        /// <summary>
        /// Converts the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string ConvertName(string name)
        {
            switch (name)
            {
                case "class":
                    return "@class";
                case "decimal":
                    return "@decimal";
                case "event":
                    return "@event";
                case "delegate":
                    return "@delegate";
                case "interface":
                    return "@interface";
                case "object":
                    return "@object";
                case "string":
                    return "@string";
                default:
                    return name;
            }
        }

        /// <summary>
        /// Converts the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public String ConvertType(String type)
        {
            bool isOut;
            bool isByRef;
            bool isBlock;
            return this.ConvertType(type, out isOut, out isByRef, out isBlock);
        }

        /// <summary>
        /// Converts the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="isOut">if set to <c>true</c> [is out].</param>
        /// <param name="isByRef">if set to <c>true</c> [is by ref].</param>
        /// <param name="isBlock">if set to <c>true</c> [is block].</param>
        /// <returns></returns>
        public String ConvertType(String type, out bool isOut, out bool isByRef, out bool isBlock)
        {
            isOut = false;
            isByRef = false;
            isBlock = false;
            type = type.Trim();

            // Types to map
            if (this.Mappings.ContainsKey(type))
            {
                String value = this.Mappings[type];
                if (value.StartsWith("out:"))
                {
                    isOut = true;
                    return value.Substring(4);
                }
                if (value.StartsWith("ref:"))
                {
                    isByRef = true;
                    return value.Substring(4);
                }
                if (value.StartsWith("block:"))
                {
                    isBlock = true;
                    return value.Substring(6);
                }
                return value;
            }

            // Protocols
            int protocolStart = 0;
            int protocolEnd = 0;
            if (type.StartsWith("id<"))
            {
                protocolStart = 3;
            }
            else if (type.StartsWith("id <"))
            {
                protocolStart = 4;
            }
            else if (type.StartsWith("id&lt;"))
            {
                protocolStart = 6;
            }
            else if (type.StartsWith("id &lt;"))
            {
                protocolStart = 7;
            }

            if (type.EndsWith(">"))
            {
                protocolEnd = 1;
            }
            else if (type.EndsWith("&gt;"))
            {
                protocolEnd = 4;
            }

            if (protocolStart > 0 && protocolEnd > 0)
            {
                return "I" + type.Substring(protocolStart, type.Length - protocolStart - protocolEnd).Trim();
            }

            // Classes
            String klass = type.Trim(' ', '*', ' ');
            klass = this.Classes.Find(klass.Equals);
            if (klass != null)
            {
                if (type.EndsWith("**"))
                {
                    isOut = true;
                    return klass;
                }
                if (type.EndsWith("*"))
                {
                    return klass;
                }
            }

            return type;
        }

        /// <summary>
        /// Sets the mappings.
        /// </summary>
        /// <param name="file">The file.</param>
        public void SetMappings(String file)
        {
            this.Mappings.Clear();

            using (StreamReader reader = new StreamReader(file))
            {
                XmlSerializer serializer = new XmlSerializer(typeof (Mappings));
                Mappings mappings = (Mappings) serializer.Deserialize(reader);

                foreach (MappingsMapping m in mappings.Items)
                {
                    String type = m.type;
                    String mapping = m.Value;
                    this.Mappings.Add(type, mapping);
                }
            }
        }

        /// <summary>
        /// Sets the classes.
        /// </summary>
        /// <param name="classes">The classes.</param>
        public void SetClasses(IEnumerable<string> classes)
        {
            this.Classes.Clear();

            this.Classes.AddRange(classes);
        }

        /// <summary>
        /// Determines whether the specified instance has class.
        /// </summary>
        /// <param name="class">The @class.</param>
        /// <returns>
        /// 	<c>true</c> if the specified instance has class; otherwise, <c>false</c>.
        /// </returns>
        public bool HasClass(string @class)
        {
            return this.Classes.Contains(@class);
        }
    }
}