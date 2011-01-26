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
using Monobjc.Tools.Generator.Model.Entities;

namespace Monobjc.Tools.Generator.Utilities
{
    /// <summary>
    ///   Helper class for URL generation.
    /// </summary>
    internal static class UrlGenerator
    {
        /// <summary>
        ///   Generates a combination of URLs.
        /// </summary>
        /// <param name = "nature">The nature.</param>
        /// <param name = "name">The name.</param>
        /// <param name = "file">The file.</param>
        /// <returns></returns>
        public static IEnumerable<string> GenerateURL(String nature, String name, String file)
        {
            List<String> result = new List<string>(64);

            const string baseURL = "http://developer.apple.com/library/mac/documentation/";
            string[] prefixes = new[] {"Cocoa", "AppKit", "CoreLocation", "UserExperience", "GraphicsImaging", "QuickTime", "MusicAudio", "Quartz", "Security",};
            string[] frameworks = new[] {"Foundation", "ApplicationKit", "AddressBook", "QuartzFramework", "QTKitFramework", "QuartzCoreFramework", "SecurityFoundationFramework", "SecurityInterfaceFramework", "WebKit",};

            for (int i = 0; i < prefixes.Length; i++)
            {
                // Try different frameworks
                for (int j = 0; j < frameworks.Length; j++)
                {
                    if (TypedEntity.CLASS_NATURE.Equals(nature))
                    {
                        if (!file.Equals(name))
                        {
                            result.Add(String.Format("{0}{1}/Reference/{2}_Class/{2}_Reference.html", baseURL, prefixes[i], file));
                        }
                        result.Add(String.Format("{0}{1}/Reference/{2}/Classes/{3}_Class/Reference/Reference.html", baseURL, prefixes[i], frameworks[j], name));
                        result.Add(String.Format("{0}{1}/Reference/{2}/Classes/{3}_Class/Introduction/Introduction.html", baseURL, prefixes[i], frameworks[j], name));
                        result.Add(String.Format("{0}{1}/Reference/{2}/Classes/{3}/Reference/Reference.html", baseURL, prefixes[i], frameworks[j], name));
                        result.Add(String.Format("{0}{1}/Reference/{2}/Classes/{3}_Class/{3}.html", baseURL, prefixes[i], frameworks[j], name));
                        result.Add(String.Format("{0}{1}/Reference/{2}/Classes/{3}_Class/Reference/{3}.html", baseURL, prefixes[i], frameworks[j], name));
                    }
                    else if (TypedEntity.PROTOCOL_NATURE.Equals(nature))
                    {
                        if (!file.Equals(name))
                        {
                            result.Add(String.Format("{0}{1}/Reference/{2}/Protocols/{3}_Protocol/Reference/Reference.html", baseURL, prefixes[i], frameworks[j], file));
                        }
                        result.Add(String.Format("{0}{1}/Reference/{2}/Protocols/{3}_Protocol/Reference/Reference.html", baseURL, prefixes[i], frameworks[j], name));
                        result.Add(String.Format("{0}{1}/Reference/{2}/Protocols/{3}_Protocol/Reference/{3}.html", baseURL, prefixes[i], frameworks[j], name));
                    }
                    else
                    {
                        // Skip for now
                    }
                }

                // Try direct URL
                if (TypedEntity.CLASS_NATURE.Equals(nature))
                {
                    if (!file.Equals(name))
                    {
                        result.Add(String.Format("{0}{1}/Reference/{2}_Class/{2}_Reference.html", baseURL, prefixes[i], file));
                    }
                    result.Add(String.Format("{0}{1}/Reference/{2}_Class/Reference/Reference.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Class/Introduction/Introduction.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Class/Reference/{2}.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Class/{2}/{2}.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Class/{2}_Reference.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Class/{2}_Ref.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Class/Reference.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Class/{2}.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}/{2}_Reference.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}/Introduction/Introduction.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Ref/Introduction/Introduction.html", baseURL, prefixes[i], name));
                }
                else if (TypedEntity.PROTOCOL_NATURE.Equals(nature))
                {
                    if (!file.Equals(name))
                    {
                        result.Add(String.Format("{0}{1}/Reference/{2}_Protocol/Reference/Reference.html", baseURL, prefixes[i], file));
                    }
                    result.Add(String.Format("{0}{1}/Reference/{2}_Protocol/Reference/Reference.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Protocol/Introduction/Introduction.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Protocol/Reference/{2}.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Protocol/{2}_Reference.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}_Protocol/{2}/{2}.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}/{2}_Ref.html", baseURL, prefixes[i], name));
                    result.Add(String.Format("{0}{1}/Reference/{2}ProtocolRef/Introduction/Introduction.html", baseURL, prefixes[i], name));
                }
                else
                {
                    // Skip for now
                }
            }

            return result;
        }
    }
}