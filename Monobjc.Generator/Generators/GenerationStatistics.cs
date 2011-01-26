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
using System.Text;

namespace Monobjc.Tools.Generator.Generators
{
    /// <summary>
    ///   Statistics collector.
    /// </summary>
    public class GenerationStatistics
    {
        public int Types { get; set; }

        public int Classes { get; set; }

        public int Protocols { get; set; }

        public int Enumerations { get; set; }

        public int Structures { get; set; }

        public int Methods { get; set; }

        public int Properties { get; set; }

        public int Functions { get; set; }

        public int Constants { get; set; }

        public int Notifications { get; set; }

        /// <summary>
        ///   Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///   A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine();
            builder.AppendLine("Generation Statistics");
            builder.AppendLine();
            builder.AppendFormat("Types         : {0}", this.Types);
            builder.AppendLine();
            builder.AppendFormat("Classes       : {0}", this.Classes);
            builder.AppendLine();
            builder.AppendFormat("Protocols     : {0}", this.Protocols);
            builder.AppendLine();
            builder.AppendFormat("Enumerations  : {0}", this.Enumerations);
            builder.AppendLine();
            builder.AppendFormat("Methods       : {0}", this.Methods);
            builder.AppendLine();
            builder.AppendFormat("Properties    : {0}", this.Properties);
            builder.AppendLine();
            builder.AppendFormat("Functions     : {0}", this.Functions);
            builder.AppendLine();
            builder.AppendFormat("Constants     : {0}", this.Constants);
            builder.AppendLine();
            builder.AppendFormat("Notifications : {0}", this.Notifications);
            builder.AppendLine();

            return builder.ToString();
        }
    }
}