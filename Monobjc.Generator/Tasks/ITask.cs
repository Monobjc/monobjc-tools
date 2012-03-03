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

namespace Monobjc.Tools.Generator.Tasks
{
    /// <summary>
    ///   A unit of work.
    /// </summary>
    public interface ITask
    {
        /// <summary>
        ///   Gets the name.
        /// </summary>
        /// <value>The name.</value>
        String Name { get; }

        /// <summary>
        ///   Gets or sets the task context.
        /// </summary>
        /// <value>The task context.</value>
        TaskContext Context { get; set; }

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        void Execute();
    }
}