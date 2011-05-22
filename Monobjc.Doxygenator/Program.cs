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
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Monobjc.Tools.Doxygenator.Model.Doxygen;

namespace Monobjc.Tools.Doxygenator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            String inputFolder = null;
            String outputFolder = null;

            if (args.Length > 1)
            {
                inputFolder = args[0];
            }
            if (args.Length > 2)
            {
                outputFolder = args[1];
            }
            inputFolder = inputFolder ?? "Input";
            outputFolder = outputFolder ?? "Output";

            // Clear the output
            if (Directory.Exists(outputFolder))
            {
                Directory.Delete(outputFolder, true);
            }

            // Read the index files
            String indexFile = Path.Combine(inputFolder, "index.xml");
            DoxygenIndexType index = null;
            using (StreamReader reader = new StreamReader(indexFile))
            {
                using (XmlReader xmlReader = XmlReader.Create(reader))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(DoxygenIndexType));
                    index = (DoxygenIndexType)serializer.Deserialize(xmlReader);
                }
            }

            // Output the XML fragment for inclusion in the entities
            // and copy files into the folder structure
            GenerateFragment(inputFolder, outputFolder, index);
        }

        /// <summary>
        ///   Generate an XML fragment ready to be included in the entities list of the generator.
        /// </summary>
        private static void GenerateFragment(String inputFolder, String outputFolder, DoxygenIndexType index)
        {
            XElement root = new XElement("Framework");

            String classOutput = Path.Combine(outputFolder, "C");
            Directory.CreateDirectory(classOutput);
            XElement classesElement = new XElement("Classes");
            foreach (CompoundType compound in index.compound.Where(c => c.kind == CompoundKind.@class))
            {
                XElement element = new XElement("Class");
                String name = compound.name;
                String categoryName = name.Replace('(', '_').Trim(')');
                var pos = name.IndexOf("(");
                String className = (pos != -1) ? name.Substring(0, pos) : name;
                element.Add(new XAttribute("name", categoryName));
                if (className != categoryName)
                {
                    XElement additionElement = new XElement("Patch");
                    additionElement.SetValue("AdditionFor=" + className);
                    element.Add(additionElement);
                }
                classesElement.Add(element);

                String inputFile = Path.Combine(inputFolder, compound.refid + ".xml");
                String outputFile = Path.Combine(classOutput, categoryName);
                File.Copy(inputFile, outputFile, true);
            }
            foreach (CompoundType compound in index.compound.Where(c => c.kind == CompoundKind.category))
            {
                XElement element = new XElement("Class");
                String name = compound.name;
                String categoryName = name.Replace('(', '_').Trim(')');
                String className = name.Substring(0, name.IndexOf("("));
                element.Add(new XAttribute("name", categoryName));
                XElement additionElement = new XElement("Patch");
                additionElement.SetValue("AdditionFor=" + className);
                element.Add(additionElement);
                classesElement.Add(element);

                String inputFile = Path.Combine(inputFolder, compound.refid + ".xml");
                String outputFile = Path.Combine(classOutput, categoryName);
                File.Copy(inputFile, outputFile, true);
            }
            foreach (CompoundType compound in index.compound.Where(c => c.kind == CompoundKind.file))
            {
                XElement element = new XElement("Class");
                String name = compound.name;
                if (!name.EndsWith(".h"))
                {
                    continue;
                }
                name = name.Replace("+", "_").Replace(".h", String.Empty);
                String className = name + "_Definitions";
                element.Add(new XAttribute("name", className));
                XElement additionElement = new XElement("Patch");
                additionElement.SetValue("AdditionFor=" + name);
                element.Add(additionElement);
                classesElement.Add(element);

                String inputFile = Path.Combine(inputFolder, compound.refid + ".xml");
                String outputFile = Path.Combine(classOutput, className);
                File.Copy(inputFile, outputFile, true);
            }
            root.Add(classesElement);

            String protocolOutput = Path.Combine(outputFolder, "P");
            Directory.CreateDirectory(protocolOutput);
            XElement protocolsElement = new XElement("Protocols");
            foreach (CompoundType compound in index.compound.Where(c => c.kind == CompoundKind.protocol))
            {
                XElement element = new XElement("Protocol");
                String name = compound.name;
                if (name.EndsWith("-p"))
                {
                    name = name.Replace("-p", String.Empty);
                }
                element.Add(new XAttribute("name", name));
                protocolsElement.Add(element);

                String inputFile = Path.Combine(inputFolder, compound.refid + ".xml");
                String outputFile = Path.Combine(protocolOutput, name);
                File.Copy(inputFile, outputFile, true);
            }
            root.Add(protocolsElement);

            String structureOutput = Path.Combine(outputFolder, "S");
            Directory.CreateDirectory(structureOutput);
            XElement structuresElement = new XElement("Structures");
            foreach (CompoundType compound in index.compound.Where(c => c.kind == CompoundKind.@struct))
            {
                XElement element = new XElement("Structure");
                String name = compound.name;
                name = name.TrimStart('_');
                element.Add(new XAttribute("name", name));
                structuresElement.Add(element);

                String inputFile = Path.Combine(inputFolder, compound.refid + ".xml");
                String outputFile = Path.Combine(structureOutput, name);
                File.Copy(inputFile, outputFile, true);
            }
            root.Add(structuresElement);

            // Output the fragment in a file
            Directory.CreateDirectory(outputFolder);
            String fragmentFile = Path.Combine(outputFolder, "fragment.xml");
            File.WriteAllText(fragmentFile, root.ToString());
        }
    }
}