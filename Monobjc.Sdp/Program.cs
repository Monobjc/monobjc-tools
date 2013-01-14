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
using Monobjc.Tools.Sdp.Generation;
using NDesk.Options;
using System.IO;

namespace Monobjc.Tools.Sdp
{
    static class Program
    {
        static void Main(string[] args)
        {
            String inputFile = null;
            String prefix = null;
            String language = "CSharp";

            // Create an option set
            OptionSet p = new OptionSet().
                Add("h|help", v => Usage()).
                Add("i=|input=", v => inputFile = v).
                Add("p=|prefix=", v => prefix = v).
                Add("l=|language=", v => language = v);
            p.Parse(args);

            if (inputFile == null)
            {
                Usage();
            }
            if (prefix == null)
            {
				prefix = inputFile.Substring(0, inputFile.Length - ".sdef".Length);
            }

            Generator generator = Generator.CreateGenerator(language);
            generator.Generate(prefix, inputFile);
        }

        static void Usage()
        {
            Console.WriteLine();
            Console.WriteLine("Monobjc Sdp Generator");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("monobjc-sdp [--help] --input=XXX --prefix=XXX [--language=XXX]");
            Console.WriteLine();
            Console.WriteLine("\t-h|--help           : Display this help");
            Console.WriteLine("\t-i|--input=value    : The sdef input file to parse");
            Console.WriteLine("\t-p|--prefix=value   : The prefix to use for the generation");
            Console.WriteLine("\t-l|--language=value : The language to use for the generation (CSharp or VBSharp)");
            Console.WriteLine();

            Environment.Exit(-1);
        }
    }
}
