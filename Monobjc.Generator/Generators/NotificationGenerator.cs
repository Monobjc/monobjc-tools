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
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Generators
{
    /// <summary>
    ///   Code generator for properties.
    /// </summary>
    public class NotificationGenerator : BaseGenerator
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "NotificationGenerator" /> class.
        /// </summary>
        /// <param name = "writer">The writer.</param>
        /// <param name = "statistics">The statistics.</param>
        public NotificationGenerator(StreamWriter writer, GenerationStatistics statistics) : base(writer, statistics) {}

        /// <summary>
        ///   Generates the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        public void Generate(String framework, NotificationEntity entity)
        {
            // Don't generate if required
            if (!entity.Generate)
            {
                return;
            }

            // Append static condition if needed
            this.AppendStartCondition(entity);

            // Append property comments
            this.Writer.WriteLineFormat(2, "/// <summary>");
            foreach (String line in entity.Summary)
            {
                this.Writer.WriteLineFormat(2, "/// <para>{0}</para>", line);
            }
            this.AppendAvailability(2, entity);
            this.Writer.WriteLineFormat(2, "/// </summary>");

            // Print the notification
            this.Writer.WriteLineFormat(2, "public static readonly NSString {1} = ObjectiveCRuntime.GetExtern<NSString>(\"{0}\", \"{1}\");", framework, entity.Name);

            // Append static condition if needed
            this.AppendEndCondition(entity);

            // Update statistics
            this.Statistics.Notifications++;
        }
    }
}