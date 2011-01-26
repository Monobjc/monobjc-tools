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
using System.Net;

namespace Monobjc.Tools.Generator.Utilities
{
    /// <summary>
    ///   Helper class for validation and download.
    /// </summary>
    internal static class DownloadHelper
    {
        public static bool Download(String url, String output)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.AllowAutoRedirect = true;
            request.Method = (output != null) ? "GET" : "HEAD";
            try
            {
                //Console.Write("Probing " + url + "... ");
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (output != null)
                        {
                            using (StreamReader contentReader = new StreamReader(response.GetResponseStream()))
                            {
                                String contents = contentReader.ReadToEnd();
                                File.WriteAllText(output, contents);
                            }
                        }
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while downloading: " + e.Message);
            }
            return false;
        }

        public static bool Validate(String url)
        {
            return Download(url, null);
        }
    }
}