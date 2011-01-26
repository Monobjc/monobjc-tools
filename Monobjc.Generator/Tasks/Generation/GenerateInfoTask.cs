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
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using Monobjc.Tools.Generator.Generators;
using Monobjc.Tools.Generator.Model.Configuration;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Tasks.Generation
{
    public class GenerateInfoTask : BaseTask
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "GenerateInfoTask" /> class.
        /// </summary>
        /// <param name = "name">The name.</param>
        public GenerateInfoTask(String name) : base(name) {}

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.DisplayBanner();

            String outputDirectory = this.Settings["TargetDir"];
            if (String.IsNullOrEmpty(outputDirectory))
            {
                return;
            }

            this.LoadLicense();
            using (StreamReader reader = new StreamReader(this.Settings["Entities"]))
            {
                XmlSerializer serializer = new XmlSerializer(typeof (Entities));
                Entities entities = (Entities) serializer.Deserialize(reader);

                IEnumerable<string> assemblies = (from f in entities.Items.Cast<EntitiesFramework>()
                                                  select f.assembly).Distinct();

                foreach (String assembly in assemblies)
                {
                    String path = Path.Combine(outputDirectory, assembly);
                    path = Path.Combine(path, "Properties");
                    path = Path.Combine(path, "AssemblyInfo.cs");

                    Console.WriteLine("Generating " + path + "...");

                    String framework = assembly.Split('.')[1];
                    byte[] bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(assembly));

                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        writer.WriteLineFormat(0, GenerationHelper.License);

                        writer.WriteLineFormat(0, "using System;");
                        writer.WriteLineFormat(0, "using System.Reflection;");
                        writer.WriteLineFormat(0, "using System.Resources;");
                        writer.WriteLineFormat(0, "using System.Runtime.CompilerServices;");
                        writer.WriteLineFormat(0, "using System.Runtime.InteropServices;");
                        writer.WriteLineFormat(0, String.Empty);
                        writer.WriteLineFormat(0, "[assembly: AssemblyTitle(\"Monobjc Bridge - {0} Library\")]", framework);
                        writer.WriteLineFormat(0, "[assembly: AssemblyDescription(\"Monobjc Bridge {0} Library\")]", framework);
                        writer.WriteLineFormat(0, "[assembly: AssemblyCompany(\"Monobjc Project\")]");
                        writer.WriteLineFormat(0, "[assembly: AssemblyProduct(\"Monobjc Bridge Project\")]");
                        writer.WriteLineFormat(0, "[assembly: AssemblyCopyright(\"Copyright (c) Monobjc Project 2007-2011 - Licensed under MIT License\")]");
                        writer.WriteLineFormat(0, "[assembly: AssemblyTrademark(\"\")]");
                        writer.WriteLineFormat(0, "[assembly: AssemblyCulture(\"\")]");
                        writer.WriteLineFormat(0, "[assembly: NeutralResourcesLanguage(\"en-US\")]");
                        //writer.WriteLineFormat(0, "[assembly: CLSCompliant(true)]");
                        writer.WriteLineFormat(0, "[assembly: ComVisible(false)]");
                        writer.WriteLineFormat(0, "[assembly: Guid(\"{0}\")]", new Guid(bytes));
                        writer.WriteLineFormat(0, String.Empty);
                        writer.WriteLineFormat(0, "#if TESTING");
                        writer.WriteLineFormat(0, "[assembly: InternalsVisibleTo(\"{0}.Tests\")]", assembly);
                        writer.WriteLineFormat(0, "#endif");
                    }
                }
            }
        }
    }
}