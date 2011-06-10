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
using System.Globalization;
using System.IO;
using System.Linq;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Generators
{
    /// <summary>
    ///   <para>Class that embeds an arbitrary file into a static library.</para>
    ///   <para>The static library then export two symbols: one for the data and one for the size.</para>
    /// </summary>
    public class DataLibraryCreator
    {
        /// <summary>
        ///   Template for assembly file
        /// </summary>
        private const String TEMPLATE = ".section __DATA,__data\n" +
                                        "\t.globl _{0}\n" +
                                        "\t_{0}:\n" +
                                        "\t.space {1},{2}\n" +
                                        "\t.long 0xfeedface\n" +
                                        "\t.globl _{0}_size\n" +
                                        "\t_{0}_size:\n" +
                                        "\t.long {1}\n";

        /// <summary>
        ///   Meaning of life (HGTG)...
        /// </summary>
        private const byte SPACER_BYTE = 0x42;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DataLibraryCreator" /> class.
        /// </summary>
        public DataLibraryCreator()
        {
            this.Logger = new NullLogger();
        }

        /// <summary>
        ///   Gets or sets the architecture flags.
        /// </summary>
        /// <value>The architecture flags.</value>
        public String ArchitectureFlags { get; set; }

        /// <summary>
        ///   Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public IExecutionLogger Logger { get; set; }

        /// <summary>
        ///   Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public String OutputDirectory { get; set; }

        /// <summary>
        ///   Gets or sets the output file.
        /// </summary>
        /// <value>The output file.</value>
        public String OutputFile { get; private set; }

        /// <summary>
        ///   Gets or sets the name of the symbol.
        /// </summary>
        /// <value>The name of the symbol.</value>
        public String SymbolName { get; private set; }

        public void CreateStaticLibrary(String dataFile, bool needZeroEnd)
        {
            // Generate the pretty name
            this.SymbolName = GetSymbolName(dataFile);

            // If we need a zero at the end (for text files), add 1 byte
            long size = new FileInfo(dataFile).Length;
            long contentSize = size + (needZeroEnd ? 1L : 0L);

            this.Logger.LogInfo("Embedding '" + dataFile + "'...");

            // Compute the names
            String sFile = Path.Combine(this.OutputDirectory, this.SymbolName + ".s");
            String oFile = Path.Combine(this.OutputDirectory, this.SymbolName + ".o");
            String aFile = Path.Combine(this.OutputDirectory, this.SymbolName + ".a");
            this.OutputFile = Path.Combine(this.OutputDirectory, "lib" + this.SymbolName + ".a");

            // (1) Create the assembly source file
            this.Logger.LogDebug("Create assembly file '" + Path.GetFileName(sFile) + "'...");
            String content = String.Format(CultureInfo.CurrentCulture, TEMPLATE, this.SymbolName, contentSize, SPACER_BYTE);
            File.WriteAllText(sFile, content);

            // (2) Create the object file
            this.Logger.LogDebug("Create object file '" + Path.GetFileName(oFile) + "'...");
            using (ProcessHelper helper = new ProcessHelper("gcc", string.Format("{0} -c -o {1} {2}", this.ArchitectureFlags ?? String.Empty, oFile, sFile)))
            {
                helper.Logger = this.Logger;
                helper.Execute();
            }

            // (3) Create the static library
            this.Logger.LogDebug("Create library file '" + Path.GetFileName(aFile) + "'...");
            using (ProcessHelper helper = new ProcessHelper("libtool", string.Format("-o {0} {1}", aFile, oFile)))
            {
                helper.Logger = this.Logger;
                helper.Execute();
            }

            // (4) Swap binary content
            this.Logger.LogDebug("Swaping content to '" + Path.GetFileName(this.OutputFile) + "'...");

            // Not quite memory-efficient, but simpler to code
            byte[] fileBuffer = File.ReadAllBytes(dataFile);
            byte[] outputBuffer = File.ReadAllBytes(aFile);

            // Search for the beginning and the end of the spacer zone
            int start = Locate(outputBuffer, new[] {SPACER_BYTE, SPACER_BYTE, SPACER_BYTE, SPACER_BYTE});

            // Insert the data file content into the static library
            Array.Copy(fileBuffer, 0, outputBuffer, start, size);

            // Put a zero at the end if required
            if (needZeroEnd)
            {
                outputBuffer[start + size] = 0;
            }

            // Write the result on the disk
            File.WriteAllBytes(this.OutputFile, outputBuffer);
        }

        /// <summary>
        ///   Gets the pretty name of a file. The name will be ready for inclusion in source code.
        /// </summary>
        /// <param name = "filename">The filename.</param>
        /// <returns></returns>
        private static String GetSymbolName(String filename)
        {
            String name = Path.GetFileName(filename);
            return "-. ".Aggregate(name, (current, c) => current.Replace(c, '_'));
        }

        /// <summary>
        ///   Locates the specified pattern in the buffer.
        /// </summary>
        /// <param name = "buffer">The buffer.</param>
        /// <param name = "pattern">The pattern.</param>
        /// <returns>The position of the first occurence of the pattern in the buffer.</returns>
        private static int Locate(byte[] buffer, byte[] pattern)
        {
            for (int i = 0; i < buffer.Length - pattern.Length; i++)
            {
                if (!Match(buffer, pattern, i))
                {
                    continue;
                }
                return i;
            }
            return -1;
        }

        /// <summary>
        ///   Test to see if the buffer match the pattern at the given position.
        /// </summary>
        /// <param name = "buffer">The buffer.</param>
        /// <param name = "pattern">The pattern.</param>
        /// <param name = "position">The position.</param>
        /// <returns>True if there is a match.</returns>
        private static bool Match(byte[] buffer, byte[] pattern, int position)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (buffer[position + i] != pattern[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}