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
using System.Text;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.Generators
{
    public class NativeCodeGenerator
    {
        private const String KEY_FILE = "FILE";
        private const String KEY_NAME = "NAME";
        private const String KEY_SYMBOL = "SYMBOL";
        private const String KEY_LIBRARY = "LIBRARY";
        private const String KEY_ASSEMBLY = "ASSEMBLY";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "NativeCodeGenerator" /> class.
        /// </summary>
        public NativeCodeGenerator()
        {
            this.Logger = new NullLogger();
            this.TargetOSVersion = MacOSVersion.MacOS105;
        }

        /// <summary>
        ///   Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public IExecutionLogger Logger { get; set; }

        /// <summary>
        ///   Gets or sets the assemblies.
        /// </summary>
        /// <value>The assemblies.</value>
        public IList<String> Assemblies { get; set; }

        /// <summary>
        ///   Gets or sets the machine configuration.
        /// </summary>
        /// <value>The machine configuration.</value>
        public String MachineConfiguration { get; set; }

        /// <summary>
        ///   Gets or sets the target OS version.
        /// </summary>
        /// <value>The target OS version.</value>
        public MacOSVersion TargetOSVersion { get; set; }

        /// <summary>
        ///   Gets or sets the target architecture.
        /// </summary>
        /// <value>The target architecture.</value>
        public MacOSArchitecture TargetArchitecture { get; set; }

        /// <summary>
        ///   Generates the native executable.
        /// </summary>
        /// <param name = "directory">The directory.</param>
        /// <returns></returns>
        public String Generate(String directory)
        {
            NativeContext nativeContext = new NativeContext(this.TargetOSVersion, this.TargetArchitecture);

            // We embed:
            // - the assemblies (with their configuration)
            // - the machine configuration
            //
            // For every embedded assembly, we store:
            // - the name of the assembly
            // - the pretty name of the assembly (for inclusion in source code)
            // - the path to the static library that embed the assembly
            // - the path to the static library that embed the assembly configuration file
            List<Dictionary<String, String>> assemblies = new List<Dictionary<string, string>>();
            List<Dictionary<String, String>> configurations = new List<Dictionary<string, string>>();

            // Create an instance of creator
            DataLibraryCreator creator = new DataLibraryCreator();
            creator.Logger = this.Logger;
            creator.ArchitectureFlags = nativeContext.ArchitectureFlags;
            creator.OutputDirectory = directory;

            this.Logger.LogInfo("Creating static libraries...");

            // For each assembly and its config, generate a native library);
            foreach (String assembly in this.Assemblies)
            {
                assemblies.Add(CreateBundle(creator, assembly, false));

                String config = assembly + ".config";
                if (File.Exists(config))
                {
                    Dictionary<string, string> bundle = CreateBundle(creator, config, true);
                    bundle[KEY_ASSEMBLY] = Path.GetFileName(assembly);
                    configurations.Add(bundle);
                }
            }

            // Store the main image
            String mainImage = assemblies[0][KEY_NAME];

            // Sort all the symbols
            assemblies.Sort((D1, D2) => D1[KEY_SYMBOL].CompareTo(D2[KEY_SYMBOL]));
            configurations.Sort((D1, D2) => D1[KEY_SYMBOL].CompareTo(D2[KEY_SYMBOL]));

            // Generate a native library for the machine configuration
            Dictionary<String, String> machineConfig = null;
            if (this.MachineConfiguration != null)
            {
                machineConfig = CreateBundle(creator, this.MachineConfiguration, true);
            }

            this.Logger.LogInfo("Creating source file...");

            // Generate the main source file
            String mainSource = Path.Combine(directory, "main.c");
            GenerateMainSource(mainSource, mainImage, assemblies, configurations, machineConfig);

            // Dump the header file
            nativeContext.WriteHeader(directory);

            // Dump the library file
            nativeContext.WriteLibrary(directory);

            // Compute the resulting executable file
            String executableFile = Path.Combine(directory, Path.GetFileNameWithoutExtension(mainImage));

            // Build the compilation command line
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("-g -O -I{0} -L{0}", directory);
            builder.AppendFormat(" {0}", nativeContext.ArchitectureFlags);
            builder.AppendFormat(" {0}", nativeContext.SDKFlags);
            builder.AppendFormat(" -o {0} ", executableFile);

            this.Logger.LogInfo("Querying for Mono flags...");

            // Add the pkg-config flags for Mono
            using (ProcessHelper helper = new ProcessHelper("pkg-config", string.Format("{0} {1}", "--cflags --libs", "mono-2")))
            {
                helper.Logger = this.Logger;
                String result = helper.Execute();
                result = result.Replace("\n", String.Empty);
                builder.Append(result);
            }

            // Add Monobjc shared library
            builder.AppendFormat(" -l{0}", "monobjc");

            // Add all the static libraries
            foreach (Dictionary<string, string> dictionary in assemblies)
            {
                builder.AppendFormat(" -l{0}", dictionary[KEY_SYMBOL]);
            }
            foreach (Dictionary<string, string> dictionary in configurations)
            {
                builder.AppendFormat(" -l{0}", dictionary[KEY_SYMBOL]);
            }
            if (machineConfig != null)
            {
                builder.AppendFormat(" -l{0}", machineConfig[KEY_SYMBOL]);
            }

            builder.AppendFormat(" {0}", Path.Combine(directory, "main.c"));

            this.Logger.LogInfo("Compiling...");
            using (ProcessHelper helper = new ProcessHelper(nativeContext.Compiler, builder.ToString()))
            {
                helper.Logger = this.Logger;
                helper.Execute();
            }

            this.Logger.LogInfo("Embedding done");

            return executableFile;
        }

        private static void GenerateMainSource(string mainSource, string mainImage, IEnumerable<Dictionary<string, string>> assemblies, IEnumerable<Dictionary<string, string>> configurations, Dictionary<string, string> machineConfig)
        {
            using (StreamWriter sourceWriter = new StreamWriter(mainSource))
            {
                // Output header for the main source
                sourceWriter.WriteLine("// This source code was produced by the Monobjc Tools, DO NOT EDIT !!!");
                sourceWriter.WriteLine();
                sourceWriter.WriteLine("#include <stdlib.h>");
                sourceWriter.WriteLine("#include <mono/metadata/assembly.h>");
                sourceWriter.WriteLine("#include \"monobjc.h\"");
                sourceWriter.WriteLine();

                // Output the symbols for the assemblies
                sourceWriter.WriteLine("// Symbol for assemblies");
                foreach (Dictionary<string, string> dictionary in assemblies)
                {
                    sourceWriter.WriteLine("extern const unsigned char {0}[];", dictionary[KEY_SYMBOL]);
                }
                sourceWriter.WriteLine();

                // Output the symbols for the configuration
                sourceWriter.WriteLine("// Symbol for configurations");
                foreach (Dictionary<string, string> dictionary in configurations)
                {
                    sourceWriter.WriteLine("extern const char {0}[];", dictionary[KEY_SYMBOL]);
                }
                sourceWriter.WriteLine();

                // Output the symbol for the machine configuration
                sourceWriter.WriteLine("// Symbol for machine configuration");
                if (machineConfig != null)
                {
                    sourceWriter.WriteLine("extern const char {0}[];", machineConfig[KEY_SYMBOL]);
                }
                sourceWriter.WriteLine();

                // Output the bundle for each assembly
                sourceWriter.WriteLine("// Bundle for assemblies");
                foreach (Dictionary<string, string> dictionary in assemblies)
                {
                    FileInfo fileInfo = new FileInfo(dictionary[KEY_FILE]);
                    sourceWriter.WriteLine("static const MonoBundledAssembly bundle_{0} = {{ \"{1}\", {0}, {2} }};", dictionary[KEY_SYMBOL], dictionary[KEY_NAME], fileInfo.Length);
                }
                sourceWriter.WriteLine();
                sourceWriter.WriteLine("static const MonoBundledAssembly *bundled[] = {");
                foreach (Dictionary<string, string> dictionary in assemblies)
                {
                    sourceWriter.WriteLine("\t&bundle_{0},", dictionary[KEY_SYMBOL]);
                }
                sourceWriter.WriteLine("\tNULL");
                sourceWriter.WriteLine("};");
                sourceWriter.WriteLine();

                // Output the main image
                sourceWriter.WriteLine("static char *image_name = \"{0}\";", mainImage);
                sourceWriter.WriteLine();

                // Output the main method
                sourceWriter.WriteLine("int main(int argc, char* argv[]) {");
                sourceWriter.WriteLine("\tint i;");
                sourceWriter.WriteLine();

                sourceWriter.WriteLine("\t// Shift the arguments to include the image name");
                sourceWriter.WriteLine("\tchar **newargs = (char **) malloc (sizeof(char *) * (argc + 2));");
                sourceWriter.WriteLine("\tnewargs [0] = argv[0];");
                sourceWriter.WriteLine("\tnewargs [1] = image_name;");
                sourceWriter.WriteLine("\tfor(i = 1; i < argc; i++) {");
                sourceWriter.WriteLine("\t\tnewargs[i+1] = argv [i];");
                sourceWriter.WriteLine("\t}");
                sourceWriter.WriteLine("\tnewargs[++i] = NULL;");
                sourceWriter.WriteLine();

                sourceWriter.WriteLine("\t// Registers the configurations and the assemblies");
                if (machineConfig != null)
                {
                    sourceWriter.WriteLine("\tmono_register_machine_config({0});", machineConfig[KEY_SYMBOL]);
                }
                foreach (Dictionary<string, string> dictionary in configurations)
                {
                    sourceWriter.WriteLine("\tmono_register_config_for_assembly(\"{0}\", {1});", dictionary[KEY_ASSEMBLY], dictionary[KEY_SYMBOL]);
                }
                sourceWriter.WriteLine("\tmono_register_bundled_assemblies(bundled);");
                sourceWriter.WriteLine();

                sourceWriter.WriteLine("\t// Invoke the Mono runtime");
                sourceWriter.WriteLine("\treturn mono_main(argc + 1, newargs);");
                sourceWriter.WriteLine("}");
            }
        }

        private static Dictionary<string, string> CreateBundle(DataLibraryCreator creator, string assembly, bool isText)
        {
            Dictionary<String, String> bundle = new Dictionary<String, String>();

            // Create the static library for the assembly
            creator.CreateStaticLibrary(assembly, isText);
            bundle[KEY_FILE] = assembly;
            bundle[KEY_NAME] = Path.GetFileName(assembly);
            bundle[KEY_SYMBOL] = creator.SymbolName;
            bundle[KEY_LIBRARY] = creator.OutputFile;

            return bundle;
        }
    }
}