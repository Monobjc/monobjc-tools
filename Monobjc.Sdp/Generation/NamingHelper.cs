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
using System.Linq;
using Monobjc.Tools.Sdp.Model;
using System.Collections.Generic;

namespace Monobjc.Tools.Sdp.Generation
{
    public static class NamingHelper
    {
        /// <summary>
        ///   Converts a enumeration value to an integer.
        /// </summary>
        /// <param name = "enumValue">The enum value.</param>
        /// <returns>An unsigned integer</returns>
        public static uint ToUInt32(String enumValue)
        {
            uint result;
            if (!UInt32.TryParse(enumValue, out result))
            {
                result = FourCharToInt(enumValue);
            }
            return result;
        }

        /// <summary>
        ///   Converts a four-char value to an unsigned integer.
        /// </summary>
        /// <param name = "fourCharValue">The four char value.</param>
        /// <returns>An unsigned integer</returns>
        public static uint FourCharToInt(String fourCharValue)
        {
            if (fourCharValue.Length != 4)
            {
                throw new ArgumentException();
            }
            return fourCharValue.Aggregate(0u, (i, c) => (i*256) + c);
        }

        /// <summary>
        ///   Generates a valid DotNet name.
        /// </summary>
        /// <param name = "prefix">The prefix.</param>
        /// <param name = "value">The value.</param>
        /// <returns>A valid DotNet name.</returns>
        public static String GenerateDotNetName(String prefix, String value)
        {
            value = value.Replace('-', ' ');
            value = value.Replace('&', ' ');
            String[] parts = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Aggregate(prefix, (s, part) => s += (part.Substring(0, 1).ToUpper() + part.Substring(1)));
        }

        /// <summary>
        ///   Generates a valid Objective-C name.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>A valid DotNet name.</returns>
        public static String GenerateObjCName(String value)
        {
            String[] parts = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            String result = parts[0];
            return parts.Skip(1).Aggregate(result, (s, part) => s += (part.Substring(0, 1).ToUpper() + part.Substring(1)));
        }

        /// <summary>
        /// Generates the name of the DotNet method name.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A method name.</returns>
        public static String GenerateDotNetMethodsName(command command)
        {
            List<String> parts = new List<String>();
            parts.Add(GenerateDotNetName(String.Empty, command.name));

            if (command.parameter != null)
            {
                foreach (parameter parameter in command.parameter)
                {
                    parts.Add(GenerateDotNetName(String.Empty, parameter.name));
                }
            }

            return String.Join(String.Empty, parts.ToArray());
        }

        /// <summary>
        /// Generates a valid Objective-C selector.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="isApplication">if set to <c>true</c>, then the command is for an application class.</param>
        /// <returns>A valid Objective-C selector.</returns>
        public static String GenerateObjCSelector(command command, bool isApplication)
        {
            List<String> parts = new List<String>();
            parts.Add(GenerateObjCName(command.name));

            bool first = true;
            if (isApplication && command.directparameter != null && command.directparameter.type != "specifier")
            {
                parts.Add(":");
                first = false;
            }
            if (command.parameter != null)
            {
                foreach (parameter parameter in command.parameter)
                {
                    if (first)
                    {
                        first = false;
                        parts.Add(GenerateDotNetName(String.Empty, parameter.name) + ":");
                    }
                    else
                    {
                        parts.Add(GenerateObjCName(parameter.name) + ":");
                    }
                }
            }

            return String.Join(String.Empty, parts.ToArray());
        }
    }
}