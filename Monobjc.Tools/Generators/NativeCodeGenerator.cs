//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2012 - Laurent Etiemble
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
        private const String KEY_INPUT_SIZE = "INPUT_SIZE";
        private const String KEY_OUTPUT_SIZE = "OUTPUT_SIZE";

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
		/// Gets or sets a value indicating whether this instance use Receigen.
		/// </summary>
		/// <value>
		/// <c>true</c> to use Receigen; otherwise, <c>false</c>.
		/// </value>
		public bool UseReceigen { get; set; }
		
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
        ///   Gets or sets the folder for developer tools.
        /// </summary>
        /// <value>The folder for developer tools.</value>
        public String DeveloperToolsFolder { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the data file will be compressed.
		/// </summary>
		/// <value>
		/// <c>true</c> to compress; otherwise, <c>false</c>.
		/// </value>
		public bool Compress { get; set; }

		/// <summary>
		/// Gets or sets the native compiler.
		/// </summary>
		/// <value>
		/// The native compiler.
		/// </value>
		public String NativeCompiler { get; set; }
		
        /// <summary>
        ///   Generates the native executable.
        /// </summary>
        /// <param name = "directory">The directory.</param>
        /// <returns></returns>
        public String Generate(String directory)
        {
            NativeContext nativeContext = new NativeContext(this.TargetOSVersion, this.TargetArchitecture);
			if (this.NativeCompiler == null) {
				this.NativeCompiler = nativeContext.Compiler;
			}

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
                String name = Path.GetFileName(assembly);

                this.Logger.LogInfo("Processing static library " + name);

                Dictionary<string, string> bundle = CreateBundle(creator, assembly, false);
                assemblies.Add(bundle);

                String config = assembly + ".config";
                if (File.Exists(config))
                {
                    this.Logger.LogInfo("Processing configuration for " + name);

                    bundle = CreateBundle(creator, config, true);
                    bundle[KEY_ASSEMBLY] = name;
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
            String mainSource = Path.Combine(directory, "main.m");
            this.GenerateMainSource(mainSource, mainImage, assemblies, configurations, machineConfig);

            // Dump the header file
            nativeContext.WriteHeader(directory);

            // Dump the library file
            nativeContext.WriteLibrary(directory);
			
			// Stage 1: Preparation of common properties
            String mainObject = Path.Combine(directory, "main.o");
            String executableFile = Path.Combine(directory, Path.GetFileNameWithoutExtension(mainImage));
			String nativeOptions = String.Format(" {0} {1} ", nativeContext.ArchitectureFlags, nativeContext.SDKFlags);
			
			// Stage 2: Compilation
			this.Compile(directory, mainSource, mainObject, nativeOptions);
			
			// Stage 3: Linkage
			this.Link(directory, mainObject, executableFile, nativeOptions, assemblies, configurations, machineConfig);
			
            this.Logger.LogInfo("Embedding done");

            return executableFile;
        }

        private void GenerateMainSource(string mainSource, string mainImage, IEnumerable<Dictionary<string, string>> assemblies, IEnumerable<Dictionary<string, string>> configurations, Dictionary<string, string> machineConfig)
        {
            using (StreamWriter sourceWriter = new StreamWriter(mainSource))
            {
                // Output header for the main source
                sourceWriter.WriteLine("// This source code was produced by the Monobjc Tools, DO NOT EDIT !!!");
                sourceWriter.WriteLine();
                sourceWriter.WriteLine("#include <stdlib.h>");
				if (this.Compress) {
	                sourceWriter.WriteLine("#include <zlib.h>");
				}
                sourceWriter.WriteLine("#include <mono/metadata/assembly.h>");
                sourceWriter.WriteLine("#include \"monobjc.h\"");
                sourceWriter.WriteLine();
				sourceWriter.WriteLine("#ifdef RECEIGEN");
				sourceWriter.WriteLine("#define RUNNER mono_main");
				sourceWriter.WriteLine("#include \"receigen.h\"");
				sourceWriter.WriteLine("#endif");
                sourceWriter.WriteLine();

				// Output the custom structure
                sourceWriter.WriteLine("// Structure for MonoBundledAssemblyExt");
                sourceWriter.WriteLine("typedef struct {");
				sourceWriter.WriteLine("\tconst char *name;");
				sourceWriter.WriteLine("\tconst unsigned char *data;");
				sourceWriter.WriteLine("\tconst unsigned int size;");
                sourceWriter.WriteLine("\tconst unsigned char *zip_data;");
                sourceWriter.WriteLine("\tconst unsigned int zip_size;");
                sourceWriter.WriteLine("} MonoBundledAssemblyExt;");
                sourceWriter.WriteLine();
				
                // Output the symbols for the assemblies
                this.Logger.LogInfo("Output symbols for assemblies (" + assemblies.Count() + ")");
                sourceWriter.WriteLine("// Symbols for assemblies");
                foreach (Dictionary<string, string> dictionary in assemblies)
                {
                    sourceWriter.WriteLine("extern const unsigned char {0}[];", dictionary[KEY_SYMBOL]);
                }
                sourceWriter.WriteLine();

                // Output the symbols for the configuration
                this.Logger.LogInfo("Output symbols for configuration (" + configurations.Count() + ")");
                sourceWriter.WriteLine("// Symbols for configurations");
                foreach (Dictionary<string, string> dictionary in configurations)
                {
                    sourceWriter.WriteLine("extern const char {0}[];", dictionary[KEY_SYMBOL]);
                }
                sourceWriter.WriteLine();

                // Output the symbol for the machine configuration
                sourceWriter.WriteLine("// Symbol for machine configuration");
                this.Logger.LogInfo("Output symbol for machine configuration");
                if (machineConfig != null)
                {
                    sourceWriter.WriteLine("extern const char {0}[];", machineConfig[KEY_SYMBOL]);
                }
                sourceWriter.WriteLine();
				
                // Output the bundle for each assembly
                this.Logger.LogInfo("Output bundle for assemblies (" + assemblies.Count() + ")");
                sourceWriter.WriteLine("// Bundle for assemblies");
                foreach (Dictionary<string, string> dictionary in assemblies)
                {
					if (this.Compress) {
    	                sourceWriter.WriteLine("static MonoBundledAssemblyExt bundle_{0} = {{ \"{1}\", NULL, {2}, {0}, {3} }};", dictionary[KEY_SYMBOL], dictionary[KEY_NAME], dictionary[KEY_INPUT_SIZE], dictionary[KEY_OUTPUT_SIZE]);
					} else {
	                    sourceWriter.WriteLine("static MonoBundledAssemblyExt bundle_{0} = {{ \"{1}\", {0}, {2}, NULL, 0 }};", dictionary[KEY_SYMBOL], dictionary[KEY_NAME], dictionary[KEY_INPUT_SIZE]);
					}
                }
                sourceWriter.WriteLine();
				
				// Output the inflate function
				if (this.Compress) {
	                sourceWriter.WriteLine("int bundle_inflate(MonoBundledAssemblyExt *bundle) {");
	                sourceWriter.WriteLine("\tif (!bundle->zip_data) {");
	                sourceWriter.WriteLine("\t\treturn 0;");
	                sourceWriter.WriteLine("\t}");
	                sourceWriter.WriteLine();
					sourceWriter.WriteLine("\tbundle->data = (const unsigned char *) malloc(bundle->size);");
	                sourceWriter.WriteLine();
					sourceWriter.WriteLine("\tint ret;");
					sourceWriter.WriteLine("\tz_stream strm = { 0 };");
					sourceWriter.WriteLine("\tret = inflateInit(&strm);");
	                sourceWriter.WriteLine("\tif (ret != Z_OK) {");
	                sourceWriter.WriteLine("\t\treturn ret;");
	                sourceWriter.WriteLine("\t}");
	                sourceWriter.WriteLine();
	                sourceWriter.WriteLine("\tstrm.next_in = (Bytef *) bundle->zip_data;");
	                sourceWriter.WriteLine("\tstrm.avail_in = (uInt) bundle->zip_size;");
	                sourceWriter.WriteLine();
	                sourceWriter.WriteLine("\twhile(1) {");
	                sourceWriter.WriteLine("\t\tstrm.next_out = (Bytef *) bundle->data;");
	                sourceWriter.WriteLine("\t\tstrm.avail_out = (uInt) bundle->size;");
	                sourceWriter.WriteLine("\t\tret = inflate(&strm, Z_NO_FLUSH);");
	                sourceWriter.WriteLine("\t\tif (ret == Z_STREAM_END) {");
	                sourceWriter.WriteLine("\t\t\tbreak;");
	                sourceWriter.WriteLine("\t\t}");
	                sourceWriter.WriteLine("\t\tif (ret != Z_OK) {");
	                sourceWriter.WriteLine("\t\t\treturn ret;");
	                sourceWriter.WriteLine("\t\t}");
	                sourceWriter.WriteLine("\t}");
	                sourceWriter.WriteLine();
					sourceWriter.WriteLine("\tret = inflateEnd(&strm);");
	                sourceWriter.WriteLine("\tif (ret != Z_OK) {");
	                sourceWriter.WriteLine("\t\treturn ret;");
	                sourceWriter.WriteLine("\t}");
	                sourceWriter.WriteLine();
	                sourceWriter.WriteLine("\t\treturn 0;");
	                sourceWriter.WriteLine("}");
	                sourceWriter.WriteLine();
				}
				
                // Output the main image
                sourceWriter.WriteLine("static char *image_name = \"{0}\";", mainImage);
                sourceWriter.WriteLine();

                // Output the main method
                sourceWriter.WriteLine("int main(int argc, char* argv[]) {");
                sourceWriter.WriteLine("\tint i;");
                sourceWriter.WriteLine();

				int i = 0;
                sourceWriter.WriteLine("\tMonoBundledAssemblyExt *bundled[{0}];", assemblies.Count() + 1);
                foreach (Dictionary<string, string> dictionary in assemblies)
                {
					if (this.Compress) {
		                sourceWriter.WriteLine("\tbundle_inflate(&bundle_{1});", i, dictionary[KEY_SYMBOL]);
					}
	                sourceWriter.WriteLine("\tbundled[{0}] = &bundle_{1};", i, dictionary[KEY_SYMBOL]);
					i++;
                }
                sourceWriter.WriteLine("\tbundled[{0}] = NULL;", i);
                sourceWriter.WriteLine("\t");
				
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
                sourceWriter.WriteLine("\tmono_register_bundled_assemblies((const MonoBundledAssembly **) bundled);");
                sourceWriter.WriteLine();

                sourceWriter.WriteLine("\t// Invoke the Mono runtime");
				sourceWriter.WriteLine("#ifdef RECEIGEN");
                sourceWriter.WriteLine("\treturn CheckReceiptAndRun(argc + 1, newargs);");
				sourceWriter.WriteLine("#else");
                sourceWriter.WriteLine("\treturn mono_main(argc + 1, newargs);");
				sourceWriter.WriteLine("#endif");
                sourceWriter.WriteLine("}");
            }
        }
		
		private void Compile(String directory, String sourceFile, String objectFile, String nativeOptions)
		{
            // Build the command line
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" -Os -gdwarf-2 {0} -I\"{1}\" ", nativeOptions, directory);
            builder.AppendFormat(" -c \"{0}\" -o \"{1}\" ", sourceFile, objectFile);

			if (this.UseReceigen) {
	            builder.AppendFormat(" -DRECEIGEN ");
			}
			
            // Add the pkg-config flags for Mono
            using (ProcessHelper helper = new ProcessHelper("pkg-config", String.Format("{0} {1}", "--cflags", "mono-2")))
            {
                helper.Logger = this.Logger;
                String result = helper.Execute();
                result = result.Replace("\n", String.Empty);
                builder.Append(result);
            }
			
			// TODO: I18N
            this.Logger.LogInfo("Compiling...");
            this.Logger.LogDebug(String.Format("Arguments: '{0}'", builder.ToString()));
            using (ProcessHelper helper = new ProcessHelper(this.NativeCompiler, builder.ToString()))
            {
                helper.Logger = this.Logger;
                helper.Execute();
            }
		}
		
		private void Link(String directory, String objectFile, String outputFile, String nativeOptions, List<Dictionary<String, String>> assemblies, List<Dictionary<String, String>> configurations, Dictionary<String, String> machineConfig)
		{
            // Build the command line
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" {0} -L\"{1}\" ", nativeOptions, directory);
			
            // Add the pkg-config flags for Mono
            using (ProcessHelper helper = new ProcessHelper("pkg-config", String.Format("{0} {1}", "--libs", "mono-2")))
            {
                helper.Logger = this.Logger;
                String result = helper.Execute();
                result = result.Replace("\n", String.Empty);
                builder.Append(result);
            }
			
            // Add zlib shared library
			if (this.Compress) {
	            builder.Append(" -lz ");
			}

            // Add Monobjc shared library
            builder.AppendFormat(" -lmonobjc ");
			
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
			
			// Append required framework for Receigen
			if (this.UseReceigen) {
                builder.AppendFormat(" -framework {0}", "AppKit");
                builder.AppendFormat(" -framework {0}", "Foundation");
                builder.AppendFormat(" -framework {0}", "Security");
                builder.AppendFormat(" -framework {0}", "IOKit");
			}
			
            builder.AppendFormat(" -o \"{0}\" \"{1}\" ", outputFile, objectFile);
			
			// TODO: I18N
            this.Logger.LogInfo("Linking...");
            this.Logger.LogDebug(String.Format("Arguments: '{0}'", builder.ToString()));
            using (ProcessHelper helper = new ProcessHelper(this.NativeCompiler, builder.ToString()))
            {
                helper.Logger = this.Logger;
                helper.Execute();
            }
		}
		
        private Dictionary<string, string> CreateBundle(DataLibraryCreator creator, string assembly, bool isText)
        {
            Dictionary<String, String> bundle = new Dictionary<String, String>();

            // Create the static library for the assembly
			creator.Compress = this.Compress && !isText;
            creator.CreateStaticLibrary(assembly, isText);
            bundle[KEY_FILE] = assembly;
            bundle[KEY_NAME] = Path.GetFileName(assembly);
            bundle[KEY_SYMBOL] = creator.SymbolName;
            bundle[KEY_LIBRARY] = creator.OutputFile;
            bundle[KEY_INPUT_SIZE] = creator.InputSize.ToString();
            bundle[KEY_OUTPUT_SIZE] = creator.OutputSize.ToString();

            return bundle;
        }
    }
}