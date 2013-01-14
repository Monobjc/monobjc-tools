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

namespace Monobjc.Tools.Generator.OpenGL
{
    internal class ParsedParameterTuple
    {
        public ParsedParameterTuple(String i1, String i2, String i3, bool i4)
        {
            this.Name = i1;
            this.Type = i2;
            this.Indirection = i3;
            this.IsConst = i4;
        }

        public String Name { get; set; }

        public String Type { get; private set; }

        public String Indirection { get; private set; }

        public bool IsConst { get; private set; }
    }
}