//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2014 - Laurent Etiemble
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
using Monobjc.Tools.Generator.Model;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Generators
{
	/// <summary>
	///   Code generator for properties.
	/// </summary>
	public class ConstantGenerator : BaseGenerator
	{
		public override void Generate (BaseEntity entity)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		///   Generates the specified entity.
		/// </summary>
		/// <param name = "framework">The framework.</param>
		/// <param name = "entity">The entity.</param>
		public void Generate (String framework, ConstantEntity entity)
		{
			// Don't generate if required
			if (!entity.Generate) {
				return;
			}

			// Append static condition if needed
			this.AppendStartCondition (entity);

			// Append property comments
			this.Writer.WriteLineFormat (2, "/// <summary>");
			foreach (String line in entity.Summary) {
				this.Writer.WriteLineFormat (2, "/// <para>{0}</para>", line.EscapeAll ());
			}
			this.AppendAvailability (2, entity);
			this.Writer.WriteLineFormat (2, "/// </summary>");

			if (entity.Static) {
				// Print the static constant
				this.Writer.WriteLineFormat (2, "public static readonly {0} {1} = {2};", entity.Type, entity.Name, entity.Value);
			} else {
				// Print the extern constant
				this.Writer.WriteLineFormat (2, "public static readonly {0} {2} = ObjectiveCRuntime.GetExtern<{0}>(\"{1}\", \"{2}\");", entity.Type, framework, entity.Name);
			}

			// Append static condition if needed
			this.AppendEndCondition (entity);

			// Update statistics
			this.Statistics.Constants++;
		}
	}
}