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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Monobjc.Tools.Generator.OpenGL
{
    internal class Program
    {
        private static readonly Regex REGEX_DEFINE = new Regex(@"#define (.+)\s+(.+)");
        private static readonly Regex REGEX_EXTERN = new Regex(@"extern (.+) (.+)\s?\((.+)\);");
        private static readonly Regex REGEX_PARAMETER = new Regex(@"(const\s)?([\w]+)([\s\*]*)(\w+)?");
        private static readonly String[] KEYWORDS = new String[] { "base", "in", "out", "object", "params", "ref", "string" };

        private static readonly IList<String> defineStatements = new List<string>();
        private static readonly Stack<String> ifdefStatements = new Stack<string>();
        private static bool skip;

        private static void Main(string[] args)
        {
            // Parse the input file line by line
            String input = args.Length > 0 ? args[0] : "glu.h";

            String outputConstants = Path.ChangeExtension(input, ".Constants.cs");
            String outputMethods = Path.ChangeExtension(input, "Methods.cs");

            using (StreamWriter writerConstants = new StreamWriter(outputConstants))
            {
                using (StreamWriter writerMethods = new StreamWriter(outputMethods))
                {
                    StreamWriter[] writers = new[] { writerConstants, writerMethods };
                    foreach (StreamWriter writer in writers)
                    {
                        writer.WriteLine("using System;");
                        writer.WriteLine("using System.Linq;");
                        writer.WriteLine("using System.Runtime.InteropServices;");
                        writer.WriteLine("using Monobjc;");
                        writer.WriteLine("using Monobjc.Foundation;");
                        writer.WriteLine();
                        writer.WriteLine("namespace Monobjc.OpenGL");
                        writer.WriteLine("{");
                        writer.WriteLine("public partial class GL");
                        writer.WriteLine("{");
                    }

                    String[] lines = File.ReadAllLines(input);
                    foreach (String line in lines)
                    {
                        ProcessConstants(writerConstants, line);
                        ProcessMethods(writerMethods, line);
                    }

                    foreach (StreamWriter writer in writers)
                    {
                        writer.WriteLine("}");
                        writer.WriteLine("}");
                    }
                }
            }
        }

        private static void ProcessConstants(StreamWriter writer, String line)
        {
            if (line.StartsWith("#define"))
            {
                Match m = REGEX_DEFINE.Match(line);
                if (m.Success)
                {
                    String name = m.Groups[1].Value.Trim();
                    String value = m.Groups[2].Value.Trim();
                    if (defineStatements.Contains(name))
                    {
                        writer.Write("//");
                    }
                    else
                    {
                        defineStatements.Add(name);
                    }
                    writer.WriteLine(String.Format("public const uint {0} = {1};", name, value));
                    return;
                }
            }
            if (line.StartsWith("typedef"))
            {
                return;
            }
            if (line.StartsWith("#else"))
            {
                return;
            }
            if (line.Contains("GL_GLEXT_FUNCTION_POINTER"))
            {
                return;
            }
            if (line.StartsWith("extern"))
            {
                return;
            }
            writer.WriteLine(line);
        }

        private static void ProcessMethods(StreamWriter writer, String line)
        {
            if (line.StartsWith("#define"))
            {
                return;
            }
            if (line.StartsWith("typedef"))
            {
                return;
            }

            if (line.StartsWith("#if"))
            {
                ifdefStatements.Push(line);
                if (line.Contains("GL_GLEXT_FUNCTION_POINTER"))
                {
                    skip = true;
                    return;
                }
            }
            if (line.StartsWith("#else"))
            {
                if (ifdefStatements.Peek().Contains("GL_GLEXT_FUNCTION_POINTER"))
                {
                    skip = false;
                }
                return;
            }
            if (line.StartsWith("#endif"))
            {
                String previous = ifdefStatements.Pop();
                if (previous.Contains("GL_GLEXT_FUNCTION_POINTER"))
                {
                    skip = false;
                    return;
                }
                line = "#endif";
            }

            if (line.StartsWith("/*") && line.EndsWith("*/"))
            {
                return;
            }

            if (skip)
            {
                return;
            }

            if (line.StartsWith("extern"))
            {
                Match m = REGEX_EXTERN.Match(line);
                if (m.Success)
                {
                    String returnType = m.Groups[1].Value.Trim();
                    String method = m.Groups[2].Value.Trim();
                    String parameters = m.Groups[3].Value;

                    // Gather the parameters
                    ParsedParameterTuples parameterTuples = new ParsedParameterTuples();
                    foreach (String parameter in parameters.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        m = REGEX_PARAMETER.Match(parameter);
                        if (m.Success)
                        {
                            bool constant = m.Groups[1].Value.Trim() == "const";
                            String type = m.Groups[2].Value;
                            String indirection = m.Groups[3].Value.Replace(" ", String.Empty);
                            String name = m.Groups[4].Value;
                            if (String.IsNullOrEmpty(name))
                            {
                                name = "p" + parameterTuples.Count;
                            }

                            ParsedParameterTuple tuple = new ParsedParameterTuple(name, type, indirection, constant);
                            parameterTuples.Add(tuple);
                        }
                        else
                        {
                            Console.WriteLine("[WARN] Cannot parse " + line);
                            return;
                            //throw new NotSupportedException("Unable to parse: '" + parameter + "'");
                        }
                    }

                    // Change return type if needed
                    if (returnType == "const GLubyte *" ||
                        returnType == "GLvoid*" ||
                        returnType == "GLvoid *")
                    {
                        returnType = "IntPtr";
                    }

                    // Change parameter names if needed
                    ParsedParameterTuple parsedParameterTuple = parameterTuples.Find(p => p.Type == "void");
                    if (parsedParameterTuple != null)
                    {
                        parameterTuples.Remove(parsedParameterTuple);
                    }
                    foreach (ParsedParameterTuple parameterTuple in parameterTuples)
                    {
                        if (KEYWORDS.Contains(parameterTuple.Name))
                        {
                            parameterTuple.Name = "@" + parameterTuple.Name;
                        }
                    }

                    // Count the parameter that are pointers
                    int pointerCount = (from p in parameterTuples
                                        where !String.IsNullOrEmpty(p.Indirection)
                                        select p).Count();

                    if (pointerCount == 0)
                    {
                        OutputMethod(writer, line, method, returnType, parameterTuples.Select(p => new RealParameterTuple(p.Name, p.Type, false, false)));
                    }
                    else
                    {
                        IList<IList<RealParameterTuple>> realParameters = new List<IList<RealParameterTuple>>();
                        IList<RealParameterTuple> currentParameters = new List<RealParameterTuple>();
                        for (int i = 0; i < parameterTuples.Count; i++)
                        {
                            realParameters.Add(GetTuples(parameterTuples[i]));
                            currentParameters.Add(new RealParameterTuple("", "", false, false));
                        }

                        OutputMethod(writer, line, method, returnType, realParameters, 0, currentParameters);
                    }

                    return;
                }
            }
            writer.WriteLine(line);
        }

        private static RealParameterTuples GetTuples(ParsedParameterTuple parameter)
        {
            RealParameterTuples tuples = new RealParameterTuples();
            if (parameter.Type == "GLvoid" ||
                parameter.Type == "GLUnurbs" ||
                parameter.Type == "GLUquadric" ||
                parameter.Type == "GLUtesselator")
            {
                tuples.Add(new RealParameterTuple(parameter.Name, "IntPtr", false, false));
            }
            else if (!String.IsNullOrEmpty(parameter.Indirection))
            {
                if (parameter.IsConst)
                {
                    tuples.Add(new RealParameterTuple(parameter.Name, "IntPtr", false, false));
                    tuples.Add(new RealParameterTuple(parameter.Name, parameter.Type + "[]", true, false));
                    tuples.Add(new RealParameterTuple(parameter.Name, "ref " + parameter.Type, true, false));
                }
                else
                {
                    tuples.Add(new RealParameterTuple(parameter.Name, "IntPtr", false, false));
                    tuples.Add(new RealParameterTuple(parameter.Name, "out " + parameter.Type, false, true));
                }
            }
            else
            {
                tuples.Add(new RealParameterTuple(parameter.Name, parameter.Type, false, false));
            }
            return tuples;
        }

        private static void OutputMethod(StreamWriter writer, String line, String method, String returnType, IList<IList<RealParameterTuple>> allParameterTuples, int index, IList<RealParameterTuple> parameterTuples)
        {
            bool isLast = (index == allParameterTuples.Count - 1);
            IList<RealParameterTuple> currentTuples = allParameterTuples[index];
            for (int i = 0; i < currentTuples.Count; i++)
            {
                parameterTuples[index] = currentTuples[i];
                if (isLast)
                {
                    OutputMethod(writer, line, method, returnType, parameterTuples);
                }
                else
                {
                    OutputMethod(writer, line, method, returnType, allParameterTuples, index + 1, parameterTuples);
                }
            }
        }

        private static void OutputMethod(StreamWriter writer, String line, String method, String returnType, IEnumerable<RealParameterTuple> parameters)
        {
            bool hasReturn = (returnType != "void");
            bool hasBody = parameters.Any(p => p.NeedMarshalling || p.NeedUnmarshalling);

            // Create the parameter list
            String parameterString = String.Join(", ", parameters.Select(p => String.Format("{0} {1}", p.Type, p.Name)).ToArray());

            // Output documentation
            writer.WriteLine("/// <summary>");
            writer.WriteLine(String.Format("/// <para>Original signature is '{0}'</para>", line.Trim()));
            writer.WriteLine("/// </summary>");

            // Output the method
            if (!hasBody)
            {
                writer.WriteLine(String.Format("[DllImport(OPENGL, EntryPoint = \"{0}\")]", method));
            }
            writer.Write(String.Format("public static {0} {1} {2}({3})", hasBody ? String.Empty : "extern", returnType, method, parameterString));
            if (!hasBody)
            {
                writer.WriteLine(";");
            }
            else
            {
                writer.WriteLine();
                writer.WriteLine("{");

                // Marshal the parameters
                for (int i = 0; i < parameters.Count(); i++)
                {
                    OutputAllocation(writer, parameters.ElementAt(i), i);
                    OutputMarshalling(writer, parameters.ElementAt(i), i);
                }

                // Output the call
                if (hasReturn)
                {
                    writer.Write("{0} result = ", returnType);
                }
                writer.Write("{0}(", method);
                for (int i = 0; i < parameters.Count(); i++)
                {
                    if (i > 0)
                    {
                        writer.Write(", ");
                    }
                    RealParameterTuple parameter = parameters.ElementAt(i);
                    if (parameter.NeedMarshalling || parameter.NeedUnmarshalling)
                    {
                        writer.Write("__local{0}", i + 1);
                    }
                    else
                    {
                        writer.Write(parameter.Name);
                    }
                }
                writer.WriteLine(");");

                // Unmarshal the parameters
                for (int i = 0; i < parameters.Count(); i++)
                {
                    OutputUnmarshalling(writer, parameters.ElementAt(i), i);
                    OutputDeallocation(writer, parameters.ElementAt(i), i);
                }

                if (hasReturn)
                {
                    writer.Write("return result;");
                }

                writer.WriteLine("}");
            }
            writer.WriteLine();
        }

        private static void OutputAllocation(StreamWriter writer, RealParameterTuple parameter, int index)
        {
            if (!parameter.Item3 && !parameter.Item4)
            {
                return;
            }

            // Get the type
            int position = index + 1;
            String type = parameter.Item2;
            bool isRef = type.StartsWith("ref ");
            bool isOut = type.StartsWith("out ");
            bool isArray = type.EndsWith("[]");

            if (isRef)
            {
                type = type.Replace("ref ", String.Empty);
            }
            if (isOut)
            {
                type = type.Replace("out ", String.Empty);
            }
            if (isArray)
            {
                type = type.Replace("[]", String.Empty);
            }

            if (isRef || isOut)
            {
                writer.Write("IntPtr __local{0} = Marshal.AllocHGlobal(Marshal.SizeOf(typeof({1})));", position, type);
                writer.WriteLine();
            }
            if (isArray)
            {
                writer.Write("IntPtr __local{0} = Marshal.AllocHGlobal(Marshal.SizeOf(typeof({1})) * {2}.Length);", position, type, parameter.Item1);
                writer.WriteLine();
            }
        }

        private static void OutputMarshalling(StreamWriter writer, RealParameterTuple parameter, int index)
        {
            if (!parameter.Item3)
            {
                return;
            }

            // Get the type
            int position = index + 1;
            String type = parameter.Item2;
            bool isRef = type.StartsWith("ref ");
            bool isArray = type.EndsWith("[]");

            if (isRef)
            {
                type = type.Replace("ref ", String.Empty);
            }
            if (isArray)
            {
                type = type.Replace("[]", String.Empty);
            }

            if (isRef)
            {
                switch (type)
                {
                    case "GLboolean":
                    case "GLubyte":
                        writer.Write("Marshal.WriteByte(__local{0}, {1});", position, parameter.Item1);
                        break;
                    case "GLchar":
                    case "GLbyte":
                    case "GLcharARB":
                        writer.Write("Marshal.WriteByte(__local{0}, (GLubyte) {1});", position, parameter.Item1);
                        break;
                    case "GLshort":
                        writer.Write("Marshal.WriteInt16(__local{0}, {1});", position, parameter.Item1);
                        break;
                    case "GLushort":
                    case "GLhalfARB":
                    case "GLhalf":
                        writer.Write("Marshal.WriteInt16(__local{0}, (GLshort) {1});", position, parameter.Item1);
                        break;
                    case "GLsizei":
                    case "GLint":
                        writer.Write("Marshal.WriteInt32(__local{0}, {1});", position, parameter.Item1);
                        break;
                    case "GLuint":
                    case "GLenum":
                        writer.Write("Marshal.WriteInt32(__local{0}, (GLint) {1});", position, parameter.Item1);
                        break;
                    case "GLintptr":
                    case "GLsizeiptr":
                    case "GLhandleARB":
                    case "GLintptrARB":
                    case "GLsizeiptrARB":
                        writer.Write("Marshal.WriteIntPtr(__local{0}, {1});", position, parameter.Item1);
                        break;
                    case "GLfloat":
                    case "GLclampf":
                    case "GLdouble":
                    case "GLclampd":
                        writer.Write("Marshal.StructureToPtr({1}, __local{0}, false);", position, parameter.Item1);
                        break;
                    default:
                        writer.Write("Not Supported: {0}", parameter.Type);
                        break;
                }
                writer.WriteLine();
            }
            if (isArray)
            {
                switch (type)
                {
                    case "GLchar":
                    case "GLbyte":
                    case "GLcharARB":
                        writer.Write("GLubyte[] __array{0} = Array.ConvertAll({1}, item => (GLubyte) item);", position, parameter.Item1);
                        writer.WriteLine();
                        writer.Write("Marshal.Copy(__array{0}, 0, __local{0}, __array{0}.Length);", position);
                        break;
                    case "GLushort":
                    case "GLhalfARB":
                    case "GLhalf":
                        writer.Write("GLshort[] __array{0} = Array.ConvertAll({1}, item => (GLshort) item);", position, parameter.Item1);
                        writer.WriteLine();
                        writer.Write("Marshal.Copy(__array{0}, 0, __local{0}, __array{0}.Length);", position);
                        break;
                    case "GLenum":
                    case "GLuint":
                        writer.Write("GLint[] __array{0} = Array.ConvertAll({1}, item => (GLint) item);", position, parameter.Item1);
                        writer.WriteLine();
                        writer.Write("Marshal.Copy(__array{0}, 0, __local{0}, __array{0}.Length);", position);
                        break;
                    default:
                        writer.Write("Marshal.Copy({1}, 0, __local{0}, {1}.Length);", position, parameter.Item1);
                        break;
                }

                writer.WriteLine();
            }
        }

        private static void OutputUnmarshalling(StreamWriter writer, RealParameterTuple parameter, int index)
        {
            if (!parameter.Item4)
            {
                return;
            }

            // Get the type
            int position = index + 1;
            String type = parameter.Item2;
            bool isOut = type.StartsWith("out ");
            bool isArray = type.EndsWith("[]");

            if (isOut)
            {
                type = type.Replace("out ", String.Empty);
            }
            if (isArray)
            {
                type = type.Replace("[]", String.Empty);
            }

            if (isOut)
            {
                switch (type)
                {
                    case "GLubyte":
                        writer.Write("{1} = Marshal.ReadByte(__local{0});", position, parameter.Item1);
                        break;
                    case "GLboolean":
                    case "GLchar":
                    case "GLbyte":
                    case "GLcharARB":
                        writer.Write("{1} = ({2}) Marshal.ReadByte(__local{0});", position, parameter.Item1, type);
                        break;
                    case "GLshort":
                        writer.Write("{1} = Marshal.ReadInt16(__local{0});", position, parameter.Item1);
                        break;
                    case "GLushort":
                    case "GLhalfARB":
                    case "GLhalf":
                        writer.Write("{1} = ({2}) Marshal.ReadInt16(__local{0});", position, parameter.Item1, type);
                        break;
                    case "GLsizei":
                    case "GLint":
                        writer.Write("{1} = ({2}) Marshal.ReadInt32(__local{0});", position, parameter.Item1, type);
                        break;
                    case "GLuint":
                    case "GLenum":
                        writer.Write("{1} = ({2}) Marshal.ReadInt32(__local{0});", position, parameter.Item1, type);
                        break;
                    case "GLintptr":
                    case "GLsizeiptr":
                    case "GLhandleARB":
                    case "GLintptrARB":
                    case "GLsizeiptrARB":
                        writer.Write("{1} = ({2}) Marshal.ReadIntPtr(__local{0});", position, parameter.Item1, type);
                        break;
                    case "GLfloat":
                    case "GLclampf":
                    case "GLdouble":
                    case "GLclampd":
                        writer.Write("{1} = ({2}) Marshal.PtrToStructure(__local{0}, typeof({2}));", position, parameter.Item1, type);
                        break;
                    default:
                        writer.Write("Not Supported: {0}", parameter.Type);
                        break;
                }
                writer.WriteLine();
            }
            if (isArray)
            {
                // TODO
            }
        }

        private static void OutputDeallocation(StreamWriter writer, RealParameterTuple parameter, int index)
        {
            if (!parameter.Item3 && !parameter.Item4)
            {
                return;
            }

            writer.Write("Marshal.FreeHGlobal(__local{0});", index + 1);
            writer.WriteLine();
        }
    }
}