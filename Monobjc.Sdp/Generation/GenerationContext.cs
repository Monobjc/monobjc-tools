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
            this.Classes = new List<@class>(classes);
            this.ClassExtensions = new List<classextension>(classExtensions);
            this.Commands = new List<command>(commands);
            this.Enumerations = new List<enumeration>(enumerations);
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
		
		public void Merge()
		{
			// Copy classes list
			IList<@class> allClasses = new List<@class>(this.Classes);
			
			// Get application classes
			IList<@class> applicationClasses = this.Classes.Where(c => IsApplicationClass(c)).ToList();
			if (applicationClasses.Count == 0) {
                throw new NotSupportedException("No application class found");
			}
			
			@class mainApplicationClass = applicationClasses[0];
			for(int i = 1; i < applicationClasses.Count; i++) {
				@class cls = applicationClasses[i];
				
				foreach(element element in cls.element) {
					if (mainApplicationClass.element.Any(e => e.type == element.type)) {
						continue;
					}
					mainApplicationClass.element.Add(element);
				}
				foreach(property property in cls.property) {
					if (mainApplicationClass.property.Any(p => p.name == property.name)) {
						continue;
					}
					mainApplicationClass.property.Add(property);
				}
				
				allClasses.Remove(cls);
			}
			
			this.Classes = allClasses;
		}
		
        /// <summary>
        ///   Gets the class extension for the given class.
        /// </summary>
        public IEnumerable<classextension> GetClassExtensionFor(@class cls)
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
                    if (item.hidden)
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
                    if (item.hidden)
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
                if (item.hidden)
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
            bool isApplication = IsApplicationClass(cls);
            bool isObject = String.IsNullOrEmpty(cls.inherits) && !isApplication;

            foreach (command item in this.Commands)
            {
                if (item.hidden)
                {
                    continue;
                }
                if (item.parameter != null && item.parameter.Any(p => (p.name == "each" || p.name == "new")))
                {
                    continue;
                }
                if (item.parameter != null && item.parameter.Any(p => p.type == "any"))
                {
                    continue;
                }
                if (item.result != null && item.result.type == "any")
                {
                    continue;
                }

                if (isApplication)
                {
                    if (item.directparameter != null)
                    {
                        String type = GetType(item.directparameter);
                        if (type == "specifier")
                        {
                            if (!item.directparameter.optional)
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

                    String type = GetType(item.directparameter);
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
        /// Determines whether the specified class is the application class.
        /// </summary>
        /// <param name="cls">The CLS.</param>
        /// <returns>
        /// 	<c>true</c> if the specified class is the application class; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsApplicationClass(@class cls)
        {
            return cls.name == "application";
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public static String GetType(directparameter parameter)
        {
            if (parameter.type != null)
            {
                return parameter.type;
            }
            if (parameter.Items != null)
            {
                type type = parameter.Items[0];
                if (type.list)
                {
                    return "list";
                }
                return type.type1;
            }
            return null;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public static String GetType(parameter parameter)
        {
            if (parameter.type != null)
            {
                return parameter.type;
            }
            if (parameter.Items != null)
            {
                type type = parameter.Items[0];
                if (type.list)
                {
                    return "list";
                }
                return type.type1;
            }
            return null;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public static String GetType(result result)
        {
            if (result == null)
            {
                return null;
            }
            if (result.type != null)
            {
                return result.type;
            }
            if (result.Items !=null)
            {
                return result.Items[0].type1;
            }
            return null;
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

            // Search for a known class or enumeration
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
				return result;
            }
            else if (typeEnumeration != null)
            {
                result = NamingHelper.GenerateDotNetName(this.Prefix, typeEnumeration.name);
				return result;
            }

            if (anyType)
            {
                // Search for a common type
                switch (type)
                {
                    case "void":
                        result = "void";
                        break;
                    case "boolean":
                        result = "Boolean";
                        break;
                    case "integer":
                    case "unsigned integer":
                        result = "NSInteger";
                        break;
                    case "double integer":
                        result = "Int64";
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
                    case "file":
                        result = "NSURL";
                        break;
                    case "specifier":
                    case "location specifier":
                        result = "SBObject";
                        break;
                    default:
                        result = "Id";
                        break;
                }
            }

            return result;
        }
    }
}