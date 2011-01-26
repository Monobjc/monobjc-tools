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
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Monobjc.Tools.Generator.Model.Database;
using Monobjc.Tools.Generator.Tasks;
using Monobjc.Tools.Generator.Tasks.Conversion;
using Monobjc.Tools.Generator.Tasks.General;
using Monobjc.Tools.Generator.Tasks.Generation;
using Monobjc.Tools.Generator.Tasks.Output;
using Monobjc.Tools.Generator.Tasks.Patching;
using Monobjc.Tools.Generator.Utilities;
using NDesk.Options;

namespace Monobjc.Tools.Generator
{
    internal class Program
    {
        public static void Help()
        {
            Console.WriteLine();
            Console.WriteLine("Monobjc Code Generator");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("MonobjcGenerate [--help] [--tasks=XXX] [--dir=YYY]");
            Console.WriteLine();
            Console.WriteLine("\t--help        : Display this help");
            Console.WriteLine("\t--tasks=value : A comma separated list of task to execute");
            Console.WriteLine("\t                The available tasks are: download, extract, generate");
            Console.WriteLine("\t--dir=value   : The directory where to copy generated code");
            Console.WriteLine();
        }

        public static void Main(string[] args)
        {
            int verbose = 0;
            String tasks = null;
            String targetDir = null;

            // Create an option set
            OptionSet p = new OptionSet().
                Add("h|help", v => tasks = "help").
                Add("v", v => ++verbose).
                Add("t=|tasks=", v => tasks = v).
                Add("d=|dir=", v => targetDir = v);
            p.Parse(args);

            // Create an execution context
            TaskContext context = new TaskContext();
            context.Settings = new NameValueCollection(ConfigurationManager.AppSettings);
            context.TypeManager = new TypeManager();

            // Create the execution manager
            TaskManager manager = new TaskManager();
            manager.Context = context;
            manager.AddTask(new LoadTask("Load"));

            // Prepare each task
            tasks = tasks ?? String.Empty;
            foreach (String task in tasks.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct())
            {
                switch (task)
                {
                    case "update":
                        manager.AddTask(new UpdateTask("Update"));
                        break;
                    case "validate":
                        manager.AddTask(new ValidateTask("Validate"));
                        break;
                    case "download":
                        manager.AddTask(new DownloadTask("Download"));
                        break;
                    case "extract":
                        manager.AddTask(new CleanClassicTask("Clean Classic HTML"));
                        manager.AddTask(new ReplaceTextTask("Patch HTML", EntryFolderType.Html));
                        manager.AddTask(new Html2XhtmlTask("HTML 2 XHTML"));
                        manager.AddTask(new ReplaceTextTask("Patch XHTML", EntryFolderType.Xhtml));
                        manager.AddTask(new DeprecateTask("Deprecate"));
                        manager.AddTask(new Xhtml2XmlTask("XHTML 2 XML"));
                        manager.AddTask(new CSharp2XmlTask("Code 2 XML"));
                        manager.AddTask(new DeprecateTask("Deprecate"));
                        manager.AddTask(new PatchXmlTask("Patch XML"));
                        break;
                    case "generate":
                        manager.AddTask(new GenerateCodeTask("Generate"));
                        manager.AddTask(new ReplaceTextTask("Patch Generated Code 1/3", EntryFolderType.Generated, ".cs"));
                        manager.AddTask(new ReplaceTextTask("Patch Generated Code 2/3", EntryFolderType.Generated, ".Class.cs"));
                        manager.AddTask(new ReplaceTextTask("Patch Generated Code 3/3", EntryFolderType.Generated, ".Constants.cs"));
                        break;
                    case "copy":
                        if (String.IsNullOrEmpty(targetDir))
                        {
                            Console.WriteLine("Directory must be provided");
                            Help();
                            Environment.Exit(-1);
                        }
                        manager.AddTask(new CopyInPlaceTask("Coyy In Place", targetDir));
                        manager.AddTask(new GenerateInfoTask("Generate Assembly Info"));
                        break;
                    case "analyze":
                        manager.AddTask(new DumpStaticInitializersTask("Search Static Initializers"));
                        break;

                    //
                    // This code is reserved when developing new task
                    //
                    case "dev":
                        // ...
                        break;
                    //
                    // This code is reserved when developing new task
                    //

                    default:
                        Help();
                        Environment.Exit(-1);
                        break;
                }
            }

            manager.Execute();
        }
    }
}