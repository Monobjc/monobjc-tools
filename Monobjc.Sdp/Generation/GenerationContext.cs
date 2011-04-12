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
        ///   Gets or sets the prefix.
        /// </summary>
        /// <value>The prefix.</value>
        public String Prefix { get; set; }

        /// <summary>
        ///   Gets or sets the classes.
        /// </summary>
        /// <value>The classes.</value>
        public IEnumerable<@class> Classes { get; set; }

        /// <summary>
        ///   Gets or sets the commands.
        /// </summary>
        /// <value>The commands.</value>
        public IEnumerable<command> Commands { get; set; }

        /// <summary>
        ///   Gets or sets the enumerations.
        /// </summary>
        /// <value>The enumerations.</value>
        public IEnumerable<enumeration> Enumerations { get; set; }

        /// <summary>
        ///   Converts the type.
        /// </summary>
        /// <param name = "type">The type.</param>
        /// <returns></returns>
        public String ConvertType(String type)
        {
            String result = type;
            @class typeCls = this.Classes.FirstOrDefault(c => String.Equals(c.name, type));
            enumeration typeEnumeration = this.Enumerations.FirstOrDefault(e => String.Equals(e.name, type));
            if (typeCls != null || typeEnumeration != null)
            {
                result = NamingHelper.GenerateDotNetName(this.Prefix, type);
            }
            else
            {
                switch (result)
                {
                    case "boolean":
                        result = "bool";
                        break;
                    case "integer":
                        result = "NSInteger";
                        break;
                    case "double integer":
                        result = "long";
                        break;
                    case "real":
                        result = "double";
                        break;
                    case "list":
                        result = "NSArray";
                        break;
                    case "data":
                    case "tdta":
                    case "picture":
                        result = "NSData";
                        break;
                    case "date":
                        result = "NSDate";
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
                        result = "SBObject";
                        break;
                    default:
                        result = "###";
                        break;
                }
            }
            return result;
        }
    }
}