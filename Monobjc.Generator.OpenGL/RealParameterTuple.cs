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

namespace Monobjc.Tools.Generator.OpenGL
{
    internal class RealParameterTuple : Tuple<string, string, bool, bool>
    {
        public RealParameterTuple(String i1, String i2, bool i3, bool i4) : base(i1, i2, i3, i4) {}

        public String Name
        {
            get { return this.Item1; }
        }

        public String Type
        {
            get { return this.Item2; }
        }

        public bool NeedMarshalling
        {
            get { return this.Item3; }
        }

        public bool NeedUnmarshalling
        {
            get { return this.Item4; }
        }
    }
}