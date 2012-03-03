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

namespace Monobjc.Tools
{
    public class DmgCreator
    {
        private readonly DmgOptions options;

        public DmgCreator(DmgOptions options)
        {
            this.options = options;
        }

        public void Create(String destinationDir) {}
    }

    public class DmgOptions
    {
        public float WindowX { get; set; }

        public float WindowY { get; set; }

        public float WindowWidth { get; set; }

        public float WindowHeight { get; set; }

        public float IconSize { get; set; }

        public String ImageFile { get; set; }

        public String SourceFolder { get; set; }

        public String VolumeName { get; set; }

        public String EulaFile { get; set; }
    }
}