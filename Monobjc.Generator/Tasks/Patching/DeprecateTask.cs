//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2012 - Laurent Etiemble
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
using System.Text.RegularExpressions;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Tasks.Patching
{
    public class DeprecateTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DeprecateTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public DeprecateTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            foreach (Entry entry in this.Entries)
            {
                if (!entry.Name.EndsWith(".Deprecated"))
                {
                    continue;
                }

                String xmlFile = entry[EntryFolderType.Xml];
                if (!File.Exists(xmlFile))
                {
                    continue;
                }

                bool modified = false;
                switch (entry.Nature)
                {
                    case TypedEntity.TYPE_NATURE:
                        {
                            TypedEntity typedEntity = BaseEntity.LoadFrom<TypedEntity>(xmlFile);
                            modified |= Deprecate(typedEntity.Functions);
                            if (modified)
                            {
                                typedEntity.SaveTo(xmlFile);
                            }
                            break;
                        }
                    case TypedEntity.CLASS_NATURE:
                        {
                            ClassEntity classEntity = BaseEntity.LoadFrom<ClassEntity>(xmlFile);
                            modified |= Deprecate(classEntity.Methods);
                            modified |= Deprecate(classEntity.Properties);
                            if (modified)
                            {
                                classEntity.SaveTo(xmlFile);
                            }
                            break;
                        }
                    case TypedEntity.PROTOCOL_NATURE:
                        {
                            ProtocolEntity protocolEntity = BaseEntity.LoadFrom<ProtocolEntity>(xmlFile);
                            modified |= Deprecate(protocolEntity.Methods);
                            modified |= Deprecate(protocolEntity.Properties);
                            if (modified)
                            {
                                protocolEntity.SaveTo(xmlFile);
                            }
                            break;
                        }
                }
            }
        }

        private static readonly Regex MIN_MAX = new Regex(@"\(Available in (Mac OS X v10\.\d) through (Mac OS X v10\.\d)");

        private static readonly Regex DEPRECATED = new Regex(@"\((Deprecated\sin\s)(Mac\sOS\sX\sv\s?10\.\d(?:\.\d\d)?)(?:\.| and later\.)(.*)\)");

        private bool Deprecate(IEnumerable<FunctionEntity> functions)
        {
            bool modified = false;
            foreach (FunctionEntity functionEntity in functions)
            {
                List<string> lines = functionEntity.Summary;
                String line = lines[0];
                modified |= this.Deprecate(functionEntity, line);
            }
            return modified;
        }

        private bool Deprecate(IEnumerable<MethodEntity> methods)
        {
            bool modified = false;
            foreach (MethodEntity methodEntity in methods)
            {
                List<string> lines = methodEntity.Summary;
                String line = lines[0];
                modified |= this.Deprecate(methodEntity, line);
            }
            return modified;
        }

        private bool Deprecate(IEnumerable<PropertyEntity> properties)
        {
            bool modified = false;
            foreach (PropertyEntity propertyEntity in properties)
            {
                List<string> lines = propertyEntity.Summary;
                String line = lines[0];
                modified |= this.Deprecate(propertyEntity, line);
            }
            return modified;
        }

        private bool Deprecate(BaseEntity baseEntity, String line)
        {
            bool modified = false;
            Match m = MIN_MAX.Match(line);
            if (m.Success)
            {
                String v1 = m.Groups[1].Value;
                String v2 = m.Groups[2].Value;
                if (!String.Equals(baseEntity.MinAvailability, v1))
                {
                    baseEntity.MinAvailability = v1;
                    modified |= true;
                }

                switch (v2)
                {
                    case "Mac OS X v10.0":
                        v2 = "Mac OS X v10.1";
                        break;
                    case "Mac OS X v10.1":
                        v2 = "Mac OS X v10.2";
                        break;
                    case "Mac OS X v10.2":
                        v2 = "Mac OS X v10.3";
                        break;
                    case "Mac OS X v10.3":
                        v2 = "Mac OS X v10.4";
                        break;
                    case "Mac OS X v10.4":
                        v2 = "Mac OS X v10.5";
                        break;
                    case "Mac OS X v10.5":
                        v2 = "Mac OS X v10.6";
                        break;
                    case "Mac OS X v10.6":
                        v2 = "Mac OS X v10.7";
                        break;
                    default:
                        break;
                }

                if (!String.Equals(baseEntity.MaxAvailability, v2))
                {
                    baseEntity.MaxAvailability = v2;
                    modified |= true;
                }
            }

            m = DEPRECATED.Match(line);
            if (m.Success)
            {
                String v2 = m.Groups[2].Value;
                String v3 = m.Groups[3].Value.Trim();
                if (!String.Equals(baseEntity.MaxAvailability, v2))
                {
                    baseEntity.MaxAvailability = v2;
                    modified |= true;
                }
                if (!String.Equals(baseEntity.Obsolete, v3))
                {
                    baseEntity.Obsolete = v3;
                    modified |= true;
                }
            }

            return modified;
        }
    }
}