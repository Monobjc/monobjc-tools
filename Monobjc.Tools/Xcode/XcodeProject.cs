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

namespace Monobjc.Tools.Xcode
{
    public class XcodeProject
    {
        public XcodeProject(String dir, string name)
        {
            this.Dir = dir;
            this.Name = name;
            this.Document = new PBXDocument();
        }

        public String Dir { get; private set; }

        public String Name { get; private set; }

        public PBXDocument Document { get; private set; }

        public PBXGroup AddGroup(String groups)
        {
            var parts = groups.Split('/').ToList();
            PBXGroup group = this.Document.Project.MainGroup;
            foreach (string part in parts)
            {
                PBXGroup g = group.FindGroup(part);
                if (g == null)
                {
                    g = new PBXGroup(part);
                    group.AddChild(g);
                }
                group = g;
            }
            return group;
        }

        public void RemoveGroup(String groups)
        {
            var parts = groups.Split('/').ToList();
            String last = parts.Last();
            parts.RemoveAt(parts.Count - 1);

            PBXGroup g;
            PBXGroup group = this.Document.Project.MainGroup;
            foreach (string part in parts)
            {
                g = group.FindGroup(part);
                if (g == null)
                {
                    g = new PBXGroup(part);
                    group.AddChild(g);
                }
                group = g;
            }

            g = group.FindGroup(last);
            if (g!=null)
            {
                group.RemoveChild(g);
            }
        }

        public PBXFileReference AddFile(String groups, String file)
        {
            PBXGroup group = this.AddGroup(groups);
            String path = Path.GetFullPath(file);
            String baseDir = Path.GetFullPath(this.Dir);

            PBXFileReference fileReference = new PBXFileReference(Path.GetFileName(path));
            if (path.StartsWith(baseDir))
            {
                path = path.Substring(baseDir.Length + 1);
                fileReference.SourceTree = PBXSourceTree.Group;
            }
            else
            {
                fileReference.SourceTree = PBXSourceTree.Absolute;
            }
            fileReference.Path = path;
            fileReference.LastKnownFileType = GetFileType(fileReference.Name);

            group.AddChild(fileReference);

            return fileReference;
        }

        private PBXFileType GetFileType(String name)
        {
            String ext = Path.GetExtension(name);
            switch (ext)
            {
                case ".c":
                    return PBXFileType.SourcecodeC;
                case ".cpp":
                case ".cxx":
                    return PBXFileType.SourcecodeCppCpp;
                case ".h":
                    return PBXFileType.SourcecodeCH;
                case ".hpp":
                case ".hxx":
                    return PBXFileType.SourcecodeCppH;
                case ".m":
                    return PBXFileType.SourcecodeCObjc;
                case ".xib":
                    return PBXFileType.FileXib;
            }
            return PBXFileType.None;
        }

        public void RemoveFile(String path, String file)
        {
            String[] parts = path.Split('/');
        }

        public void AddFramework(String path, String framework)
        {
            String[] parts = path.Split('/');
        }

        public void RemoveFramework(String path, String framework)
        {
            String[] parts = path.Split('/');
        }

        public void Save()
        {
            String folder = Path.Combine(this.Dir, this.Name + ".xcodeproj");
            Directory.CreateDirectory(folder);
            String file = Path.Combine(folder, "project.pbxproj");
            this.Document.WriteToFile(file);
        }
    }
}