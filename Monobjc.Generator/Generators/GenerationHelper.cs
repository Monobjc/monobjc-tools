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
using System.Reflection;

namespace Monobjc.Tools.Generator.Generators
{
    public static class GenerationHelper
    {
        private static String toolName;
        private static String toolVersion;

        /// <summary>
        ///   Loads the license.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        public static void LoadLicense(NameValueCollection settings)
        {
            if (License == null)
            {
                License = String.IsNullOrEmpty(settings["License"]) ? "/* License goes here */" : File.ReadAllText(settings["License"]);
            }
        }

        /// <summary>
        ///   Gets or sets the License txt.
        /// </summary>
        /// <value>The License text.</value>
        public static String License { get; set; }

        /// <summary>
        ///   Gets the name of the tool.
        /// </summary>
        /// <value>The name of the tool.</value>
        public static String ToolName
        {
            get { return toolName ?? (toolName = Assembly.GetEntryAssembly().GetName().Name); }
        }

        /// <summary>
        ///   Gets the tool version.
        /// </summary>
        /// <value>The tool version.</value>
        public static String ToolVersion
        {
            get { return toolVersion ?? (toolVersion = Assembly.GetEntryAssembly().GetName().Version.ToString()); }
        }
    }
}