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
using System.Text.RegularExpressions;
using Monobjc.Tools.Generator.Model.Configuration;
using Monobjc.Tools.Generator.Model.Database;

namespace Monobjc.Tools.Generator.Tasks.Patching
{
    public class CleanClassicTask : ReplaceTextTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "CleanClassicTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public CleanClassicTask(String name) : base(name, EntryFolderType.Html) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            Replace[] replacements = new[]
                                         {
                                             new Replace {Source = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"", With = "<!DOCTYPE html>"},
                                             new Replace {Source = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Frameset//EN\"", With = "<!DOCTYPE html>"},
                                             new Replace {Source = "\"http://www.w3.org/TR/1998/REC-html40-19980424/loose.dtd\">", With = String.Empty},
                                             new Replace {Source = "\"http://www.w3.org/TR/1999/REC-html401-19991224/frameset.dtd\">", With = String.Empty},
                                             new Replace {Source = "<hr>", With = "<hr/>"},
                                             new Replace {Source = "<br>", With = "<br/>"},
                                             new Replace {Source = "<p><!-- begin abstract --><p>", With = "<p>"},
                                             new Replace {Source = "</p>\n<!-- end abstract --></p>", With = "</p>"},
                                         };

            foreach (Entry entry in this.Entries)
            {
                if (entry.PageStyle != PageStyle.Classic)
                {
                    continue;
                }

                this.Patch(entry, replacements);
                this.PatchDiscussion(entry);
            }
        }

        private const String DISCUSSION_BEGIN = "<!-- begin discussion -->";
        private const String DISCUSSION_END = "<!-- end discussion -->";
        private static readonly Regex DISCUSSION_OPENING_REGEX = new Regex(@"(?m-isnx:)(<p>)", RegexOptions.Multiline);
        private static readonly Regex DISCUSSION_CLOSING_REGEX = new Regex(@"(?m-isnx:)(</p>)", RegexOptions.Multiline);

        protected void PatchDiscussion(Entry entry)
        {
            if (entry != null)
            {
                String file = entry[this.Type];
                if (!String.IsNullOrEmpty(this.Extension))
                {
                    file += this.Extension;
                }
                if (!File.Exists(file))
                {
                    return;
                }

                String contents = File.ReadAllText(file);
                bool modified = false;

                // Search for discussion parts
                int index = 0;
                int beginning = 0;
                int ending = 0;
                while ((beginning = contents.IndexOf(DISCUSSION_BEGIN, index)) != -1)
                {
                    ending = contents.IndexOf(DISCUSSION_END, beginning);
                    String content = contents.Substring(beginning, ending + DISCUSSION_END.Length - beginning);
                    String replacement = content;

                    // Count opening and closing paragraphs
                    int opening = DISCUSSION_OPENING_REGEX.Matches(replacement).Count;
                    int closing = DISCUSSION_CLOSING_REGEX.Matches(replacement).Count;

                    if (opening < closing)
                    {
                        throw new NotSupportedException();
                        modified |= true;
                    }
                    if (opening > closing)
                    {
                        replacement = replacement.Replace("<!-- begin discussion -->", String.Empty);
                        replacement = replacement.Replace("<!-- end discussion -->", String.Empty);
                        for (int i = closing; i < opening; i++)
                        {
                            replacement += "</p>";
                        }
                        replacement = "<!-- begin discussion -->" + replacement;
                        replacement = replacement + "<!-- end discussion -->";

                        contents = contents.Replace(content, replacement);
                        modified |= true;
                    }

                    index = beginning + replacement.Length;
                }

                if (modified)
                {
                    Console.WriteLine("Patching '{0}'...", entry.Name);
                    File.WriteAllText(file, contents);
                }
            }
        }
    }
}