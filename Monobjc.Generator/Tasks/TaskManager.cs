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
using System.Collections.Generic;

namespace Monobjc.Tools.Generator.Tasks
{
    /// <summary>
    ///   Class that
    /// </summary>
    public class TaskManager : BaseTask
    {
        private readonly List<ITask> tasks;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TaskManager" /> class.
        /// </summary>
        public TaskManager()
            : base("Manager")
        {
            this.tasks = new List<ITask>();
        }

        /// <summary>
        ///   Adds the given task.
        /// </summary>
        /// <param name = "task">The task to add.</param>
        public void AddTask(ITask task)
        {
            task.Context = this.Context;
            this.tasks.Add(task);
        }

        /// <summary>
        ///   Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.Context.Load();
            foreach (ITask task in this.tasks)
            {
                task.Execute();
            }
            this.Context.Save();
        }
    }
}