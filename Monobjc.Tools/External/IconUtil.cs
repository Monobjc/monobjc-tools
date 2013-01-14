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
using System.Globalization;
using System.IO;
using Monobjc.Tools.Utilities;

namespace Monobjc.Tools.External
{
	/// <summary>
	///   Wrapper class around the <c>iconutil</c> command line tool.
	/// </summary>
	public static class IconUtil
	{
		/// <summary>
		///   Cat multiple images
		/// </summary>
		/// <param name = "mask">The permission mask.</param>
		/// <param name = "file">The file.</param>
		/// <returns>The result of the command.</returns>
		public static void Convert (IconUtilType inputType, String intputFile, TextWriter outputWriter = null, TextWriter errorWriter = null)
		{
			String type;
			switch(inputType) {
			case IconUtilType.Icns:
				type = "icns";
				break;
			case IconUtilType.Iconset:
				type = "iconset";
				break;
			default:
				throw new ArgumentException("Type must be either 'Icns' or 'Iconset'", "inputType");
			}
			String arguments = String.Format (CultureInfo.InvariantCulture, "--convert {0} \"{1}\"", type, intputFile);
			ProcessHelper helper = new ProcessHelper (Executable, arguments);
			helper.OutputWriter = outputWriter;
			helper.ErrorWriter = errorWriter;
			helper.Execute ();
		}

		private static string Executable {
			get { return "iconutil"; }
		}
	}

	/// <summary>
	/// Types used by the <see cref="IconUtil"/>
	/// </summary>
	public enum IconUtilType
	{
		/// <summary>
		/// No type.
		/// </summary>
		None,
		/// <summary>
		/// The icns type.
		/// </summary>
		Icns,
		/// <summary>
		/// The iconset type.
		/// </summary>
		Iconset,
	}
}