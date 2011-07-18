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
using System.IO;
using System.Text;
using Monobjc.Tools.Properties;
using Monobjc.Tools.PropertyList;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Generators
{
    public class InfoPListGenerator
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "InfoPListGenerator" /> class.
        /// </summary>
        public InfoPListGenerator()
        {
            this.TargetOSVersion = MacOSVersion.MacOS105;
        }

        /// <summary>
        ///   Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string ApplicationName { get; set; }

        /// <summary>
        ///   Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public String Icon { get; set; }

        /// <summary>
        ///   Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public String Identifier { get; set; }

        /// <summary>
        ///   Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public String Version { get; set; }

        /// <summary>
        ///   Gets or sets the target OS version.
        /// </summary>
        /// <value>The target OS version.</value>
        public MacOSVersion TargetOSVersion { get; set; }

        /// <summary>
        ///   Gets or sets the main nib file.
        /// </summary>
        /// <value>The main nib file.</value>
        public String MainNibFile { get; set; }

        /// <summary>
        ///   Gets or sets the principal class.
        /// </summary>
        /// <value>The principal class.</value>
        public String PrincipalClass { get; set; }

        /// <summary>
        ///   Gets or sets the content of the Info.plist file.
        ///   <para>On input, allows to set the template to use.</para>
        /// </summary>
        /// <value>The content.</value>
        public String Content { get; set; }

        /// <summary>
        ///   Generates the Info.plist content.
        /// </summary>
        public void WriteTo(string path)
        {
            if (this.Content == null)
            {
                // Load template
                this.Content = Resources.InfoPlist;
            }

            // Parse the template content
            PListDocument document = PListDocument.LoadFromXml(this.Content);
            PList list = document.Root;
            PListDict dict = list.Dict;

            // Set default values
            this.Identifier = this.Identifier ?? "net.monobjc.app";
            this.Version = this.Version ?? "1.0";
            this.MainNibFile = this.MainNibFile ?? "MainMenu";
            this.PrincipalClass = this.PrincipalClass ?? "NSApplication";

            // Replace template values
            Replace(dict, "CFBundleExecutable", this.ApplicationName);
            Replace(dict, "CFBundleIconFile", this.Icon);
            Replace(dict, "CFBundleIdentifier", this.Identifier);
            Replace(dict, "CFBundleName", this.ApplicationName);
            Replace(dict, "CFBundleShortVersionString", this.Version);
            switch (this.TargetOSVersion)
            {
                case MacOSVersion.MacOS105:
                    Replace(dict, "LSMinimumSystemVersion", "10.5");
                    break;
                case MacOSVersion.MacOS106:
                    Replace(dict, "LSMinimumSystemVersion", "10.6");
                    break;
                case MacOSVersion.MacOS107:
                    Replace(dict, "LSMinimumSystemVersion", "10.7");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Replace(dict, "NSMainNibFile", this.MainNibFile);
            Replace(dict, "NSPrincipalClass", this.PrincipalClass);

            // Resassign content
            this.Content = document.ToString();

            File.WriteAllText(path, this.Content, Encoding.UTF8);
        }

        private static void Replace(PListDict dict, String key, String value)
        {
            if (value == null)
            {
                if (dict.ContainsKey(key))
                {
                    dict.Remove(key);
                }
                return;
            }

            if (!dict.ContainsKey(key))
            {
                dict[key] = new PListString(value);
                return;
            }

            PListString pListString = dict[key] as PListString;
            if (pListString == null)
            {
                dict[key] = new PListString(value);
                return;
            }

            String currentValue = pListString.Value;
            if (currentValue.StartsWith("${") && currentValue.EndsWith("}"))
            {
                dict[key] = new PListString(value);
                return;
            }
        }
    }
}