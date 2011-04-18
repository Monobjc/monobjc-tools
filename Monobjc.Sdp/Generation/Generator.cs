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
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Monobjc.Tools.Sdp.Model;

namespace Monobjc.Tools.Sdp.Generation
{
    /// <summary>
    /// Generator base-class.
    /// </summary>
    public abstract class Generator
    {
        /// <summary>
        /// Creates the generator.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>A generator.</returns>
        public static Generator CreateGenerator(String language)
        {
            switch (language)
            {
                case "CSharp":
                    return new CSharpGenerator();
                default:
                    throw new NotSupportedException("Unsupported language: " + language);
            }
        }

        /// <summary>
        /// Generates a wrapper.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        public void Generate(String prefix, String input, String output)
        {
            if (!File.Exists(input))
            {
                throw new ArgumentException("Input file not found: " + input);
            }
            using (TextReader reader = new StreamReader(input))
            {
                using (TextWriter writer = new StreamWriter(output))
                {
                    this.Generate(prefix, reader, writer);
                }
            }
        }

        /// <summary>
        /// Generates a wrapper.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="writer">The writer.</param>
        public void Generate(String prefix, TextReader reader, TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (dictionary));
            dictionary dictionary = serializer.Deserialize(reader) as dictionary;

            if (dictionary == null)
            {
                return;
            }

            IEnumerable<@class> classes = dictionary.suite.Where(s => s.Items != null).SelectMany(s => s.Items).Where(i => i is @class).Cast<@class>();
            IEnumerable<command> commands = dictionary.suite.Where(s => s.Items != null).SelectMany(s => s.Items).Where(i => i is command).Cast<command>();
            IEnumerable<enumeration> enumerations = dictionary.suite.Where(s => s.Items != null).SelectMany(s => s.Items).Where(i => i is enumeration).Cast<enumeration>();
            GenerationContext context = new GenerationContext(prefix, classes, commands, enumerations);

            String contents = this.Generate(context);
            writer.Write(contents);
        }

        /// <summary>
        /// Generates a wrapper.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected abstract String Generate(GenerationContext context);
    }
}