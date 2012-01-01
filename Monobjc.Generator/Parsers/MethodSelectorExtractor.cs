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
using System.Text;

namespace Monobjc.Tools.Generator.Parsers
{
    public class MethodSelectorExtractor
    {
        private readonly String signature;
        private int index;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MethodSelectorExtractor" /> class.
        /// </summary>
        /// <param name = "signature">The signature.</param>
        public MethodSelectorExtractor(String signature)
        {
            this.signature = signature;
            this.index = 0;
        }

        /// <summary>
        ///   Extracts the selector.
        /// </summary>
        /// <returns>The selector.</returns>
        public String Extract()
        {
            int colonPos;
            StringBuilder builder = new StringBuilder();

            colonPos = this.signature.IndexOf(":", this.index);
            if (colonPos < 0)
            {
                // Special case of no-parameter method.
                int startIndex = this.signature.Length - 1;

                // Go back to capture selector part (skip characters)
                while (startIndex > 0 && this.signature[startIndex] != ' ' && this.signature[startIndex] != ')')
                {
                    startIndex--;
                }
                startIndex++;

                // Append the selector part
                builder.Append(this.signature.Substring(startIndex).Trim());
            }
            else
            {
                while (colonPos > 0)
                {
                    this.index = colonPos + 1;
                    int startIndex = colonPos - 1;

                    // Go back to the end of selector part (skip spaces)
                    while (startIndex > 0 && this.signature[startIndex] == ' ')
                    {
                        startIndex--;
                    }

                    // Go back to capture selector part (skip characters)
                    while (startIndex > 0 && this.signature[startIndex] != ' ' && this.signature[startIndex] != ')')
                    {
                        startIndex--;
                    }
                    startIndex++;

                    // Append the selector part
                    builder.Append(this.signature.Substring(startIndex, colonPos - startIndex).Trim());
                    builder.Append(":");

                    colonPos = this.signature.IndexOf(":", this.index);
                }
            }

            return builder.ToString();
        }
    }
}