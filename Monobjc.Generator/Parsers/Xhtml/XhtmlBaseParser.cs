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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml
{
    /// <summary>
    ///   Base class for XHTML parsing.
    /// </summary>
    public abstract class XhtmlBaseParser : BaseParser
    {
        protected static readonly Regex ENUMERATION_REGEX = new Regex(@"(typedef )?enum( ?[_A-z]+)? ?\{(.+)\};?( ?typedef)?( ?[A-z0-9_]+ ?)?([A-z]+)?");
        protected static readonly Regex CONSTANT_REGEX = new Regex(@"(id ?|unsigned ?|double ?|float ?\*?|NSString ?\*? ?|CFStringRef ?|CIFormat|CATransform3D|CLLocationDistance ?)([A-z0-9]+)$");
        protected static readonly Regex PARAMETER_REGEX = new Regex(@"(const )?([0-9A-z]+ ?\*{0,2} ?)([0-9A-z]+)");

        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlBaseParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        protected XhtmlBaseParser(NameValueCollection settings, TypeManager typeManager) : base(settings, typeManager) {}

        /// <summary>
        ///   Gets the name of the method.
        /// </summary>
        /// <param name = "methodEntity">The method entity.</param>
        /// <returns></returns>
        internal static String GetMethodName(MethodEntity methodEntity)
        {
            String[] parts = methodEntity.Selector.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder builder = new StringBuilder();
            foreach (String part in parts)
            {
                String token = part.UpperCaseFirstLetter();
                builder.Append(token);
            }
            return builder.ToString();
        }

        protected bool SplitEnumeration(string declaration, ref string name, ref string type, ref string values)
        {
            Match r = ENUMERATION_REGEX.Match(declaration);
            if (r.Success)
            {
                String v2 = r.Groups[2].Value.Trim();
                String v5 = r.Groups[5].Value.Trim();
                String v6 = r.Groups[6].Value.Trim();

                values = r.Groups[3].Value.Trim();

                // Name can be before enumeration values
                if (!String.IsNullOrEmpty(v5) && !String.IsNullOrEmpty(v6))
                {
                    type = v5;
                    name = v6;
                }
                else if (!String.IsNullOrEmpty(v5) && String.IsNullOrEmpty(v6))
                {
                    name = v5;
                }
                else if (!String.IsNullOrEmpty(v2) && !String.IsNullOrEmpty(v5))
                {
                    name = v5;
                }
                else if (!String.IsNullOrEmpty(v2) && String.IsNullOrEmpty(v6))
                {
                    name = v2;
                }

                // Clean results
                name = name.Trim(';');

                // Make sure name is ok
                name = " -:—".Aggregate(name, (current, c) => current.Replace(c, '_'));

                bool isOut;
                bool isByRef;
                bool isBlock;
                type = this.TypeManager.ConvertType(type, out isOut, out isByRef, out isBlock);

                //Console.WriteLine("Enumeration found '{0}' of type '{1}'", name, type);

                return true;
            }

            Console.WriteLine("FAILED to parse enum '{0}'", declaration);
            return false;
        }

        /// <summary>
        ///   Converts a four-char value to an unsigned integer.
        /// </summary>
        /// <param name = "fourCharValue">The four char value.</param>
        /// <returns>An unsigned integer</returns>
        protected static uint FourCharToInt(String fourCharValue)
        {
            if (fourCharValue.Length != 4)
            {
                throw new ArgumentException();
            }
            return fourCharValue.Aggregate(0u, (i, c) => (i * 256) + c);
        }
    }
}