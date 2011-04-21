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
using System.Linq;
using Monobjc.Tools.Sdp.Model;

namespace Monobjc.Tools.Sdp.Generation
{
    public class GenerationContext
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "GenerationContext" /> class.
        /// </summary>
        /// <param name = "prefix">The prefix.</param>
        /// <param name = "classes">The classes.</param>
        /// <param name = "classExtensions">The class extensions.</param>
        /// <param name = "commands">The commands.</param>
        /// <param name = "enumerations">The enumerations.</param>
        public GenerationContext(String prefix, IEnumerable<@class> classes, IEnumerable<classextension> classExtensions, IEnumerable<command> commands, IEnumerable<enumeration> enumerations)
        {
            this.Prefix = prefix;
            this.Classes = classes;
            this.ClassExtensions = classExtensions;
            this.Commands = commands;
            this.Enumerations = enumerations;
        }

        /// <summary>
        ///   Gets or sets the prefix.
        /// </summary>
        /// <value>The prefix.</value>
        public String Prefix { get; private set; }

        /// <summary>
        ///   Gets or sets the classes.
        /// </summary>
        /// <value>The classes.</value>
        public IEnumerable<@class> Classes { get; private set; }

        /// <summary>
        ///   Gets or sets the classes' extension.
        /// </summary>
        /// <value>The classes.</value>
        public IEnumerable<classextension> ClassExtensions { get; private set; }

        /// <summary>
        ///   Gets or sets the commands.
        /// </summary>
        /// <value>The commands.</value>
        public IEnumerable<command> Commands { get; private set; }

        /// <summary>
        ///   Gets or sets the enumerations.
        /// </summary>
        /// <value>The enumerations.</value>
        public IEnumerable<enumeration> Enumerations { get; set; }

        /// <summary>
        ///   Gets the class extension for the given class.
        /// </summary>
        private IEnumerable<classextension> GetClassExtensionFor(@class cls)
        {
            return this.ClassExtensions.Where(classExtension => String.Equals(classExtension.extends, cls.name));
        }

        /// <summary>
        ///   Gets the elements for the given class.
        /// </summary>
        public IEnumerable<element> GetElementsFor(@class cls)
        {
            List<element> items = new List<element>();

            // Add class items if any
            if (cls.element != null)
            {
                items.AddRange(cls.element);
            }

            // Add extension items if any
            IEnumerable<element> extensionItems = this.GetClassExtensionFor(cls).Where(c => c.element != null).SelectMany(c => c.element);
            if (extensionItems != null)
            {
                items.AddRange(extensionItems);
            }

            // Retrieve the base class if any
            @class baseClass = null;
            String type = cls.inherits;
            if (type != null)
            {
                baseClass = (from c in this.Classes
                             where (c.id != null && String.Equals(c.id, type))
                                   || (c.id == null && String.Equals(c.name, type))
                             select c).FirstOrDefault();
            }

            // Filter out elements to avoid override
            if (baseClass != null && baseClass.element != null)
            {
                IEnumerable<element> baseElements = this.GetElementsFor(baseClass);
                foreach (element item in items)
                {
                    if (item.hiddenSpecified && item.hidden == yorn.yes)
                    {
                        continue;
                    }
                    element baseItem = baseElements.FirstOrDefault(e => String.Equals(e.type, item.type));
                    if (baseItem != null)
                    {
                        continue;
                    }
                    yield return item;
                }
            }
            else
            {
                foreach (element item in items)
                {
                    if (item.hiddenSpecified && item.hidden == yorn.yes)
                    {
                        continue;
                    }
                    yield return item;
                }
            }
        }

        /// <summary>
        ///   Gets the properties for the given class.
        /// </summary>
        public IEnumerable<property> GetPropertiesFor(@class cls)
        {
            List<property> items = new List<property>();

            // Add class items if any
            if (cls.property != null)
            {
                items.AddRange(cls.property);
            }

            // Add extension items if any
            IEnumerable<property> extensionItems = this.GetClassExtensionFor(cls).Where(c => c.property != null).SelectMany(c => c.property);
            if (extensionItems != null)
            {
                items.AddRange(extensionItems);
            }

            foreach (property item in items)
            {
                if ("class;description;version".Contains(item.name))
                {
                    continue;
                }
                if (item.hiddenSpecified && item.hidden == yorn.yes)
                {
                    continue;
                }
                yield return item;
            }
        }

        /// <summary>
        ///   Gets the commands for the given class.
        /// </summary>
        public IEnumerable<command> GetCommandsFor(@class cls)
        {
            bool isApplication = (cls.name == "application");
            bool isObject = String.IsNullOrEmpty(cls.inherits) && !isApplication;

            foreach (command item in this.Commands)
            {
                if (item.hiddenSpecified && item.hidden == yorn.yes)
                {
                    continue;
                }
                if (item.parameter != null && item.parameter.Any(p => (p.name == "each" || p.name == "new")))
                {
                    continue;
                }
                if (item.parameter != null && item.parameter.Any(p => p.type1 == "any"))
                {
                    continue;
                }
                if (item.result != null && item.result.type1 == "any")
                {
                    continue;
                }

                if (isApplication)
                {
                    if (item.directparameter != null)
                    {
                        String type = item.directparameter.type1 ?? item.directparameter.type[0].type1;
                        if (type == "specifier")
                        {
                            if (!item.directparameter.optionalSpecified || item.directparameter.optional != yorn.yes)
                            {
                                continue;
                            }
                        }
                        if (this.Classes.Any(c => String.Equals(c.name, type)))
                        {
                            continue;
                        }
                    }
                    yield return item;
                }
                else
                {
                    if (item.directparameter == null)
                    {
                        continue;
                    }

                    String type = item.directparameter.type1 ?? item.directparameter.type[0].type1;
                    if (type == "specifier" && isObject)
                    {
                        yield return item;
                    }
                    if (type == cls.name)
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        ///   Converts the name of the parameter.
        /// </summary>
        public static String ConvertParameterName(String name)
        {
            switch (name)
            {
                case "as":
                case "class":
                case "delegate":
                case "enum":
                case "event":
                case "for":
                case "foreach":
                case "in":
                case "interface":
                case "using":
                    return "@" + name;
                default:
                    return name;
            }
        }

        /// <summary>
        ///   Converts the type.
        /// </summary>
        public String ConvertType(String type, bool anyType)
        {
            String result = type;
            bool replaced = false;

            if (anyType)
            {
                // Search for a common type
                replaced = true;
                switch (type)
                {
                    case "boolean":
                        result = "bool";
                        break;
                    case "integer":
                    case "unsigned integer":
                        result = "NSInteger";
                        break;
                    case "double integer":
                        result = "long";
                        break;
                    case "real":
                        result = "double";
                        break;
                    case "property":
                        result = "IntPtr";
                        break;
                    case "list":
                        result = "NSArray";
                        break;
                    case "color":
                    case "RGB color":
                        result = "NSColor";
                        break;
                    case "data":
                    case "tdta":
                    case "picture":
                        result = "NSData";
                        break;
                    case "date":
                        result = "NSDate";
                        break;
                    case "record":
                        result = "NSDictionary";
                        break;
                    case "type":
                        result = "NSNumber";
                        break;
                    case "point":
                        result = "NSPoint";
                        break;
                    case "text":
                    case "version":
                        result = "NSString";
                        break;
                    case "rectangle":
                        result = "NSRect";
                        break;
                    case "alias":
                        result = "NSURL";
                        break;
                    case "specifier":
                    case "location specifier":
                        result = "SBObject";
                        break;
                    default:
                        result = "Id";
                        replaced = false;
                        break;
                }
            }

            // If not replaced, then search for a known class or enumeration
            if (!replaced)
            {
                @class typeCls = (from cls in this.Classes
                                  where (cls.id != null && String.Equals(cls.id, type))
                                        || (cls.id == null && String.Equals(cls.name, type))
                                  select cls).FirstOrDefault();
                enumeration typeEnumeration = (from enm in this.Enumerations
                                               where (enm.id != null && String.Equals(enm.id, type))
                                                     || (enm.id == null && String.Equals(enm.name, type))
                                               select enm).FirstOrDefault();

                if (typeCls != null)
                {
                    result = NamingHelper.GenerateDotNetName(this.Prefix, typeCls.name);
                }
                else if (typeEnumeration != null)
                {
                    result = NamingHelper.GenerateDotNetName(this.Prefix, typeEnumeration.name);
                }
            }

            return result;
        }
    }
}