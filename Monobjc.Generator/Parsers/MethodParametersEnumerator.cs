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
using System.Collections;
using System.Collections.Generic;

namespace Monobjc.Tools.Generator.Parsers
{
    /// <summary>
    ///   Parse the method parameters and expose them as an enumerator.
    /// </summary>
    public sealed class MethodParametersEnumerator : IEnumerator<String>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "MethodParametersEnumerator" /> class.
        /// </summary>
        /// <param name = "signature">The signature.</param>
        /// <param name = "parameterNames">if set to <c>true</c> [parameter names].</param>
        public MethodParametersEnumerator(String signature, bool parameterNames)
        {
            this.Signature = signature;
            this.ParameterNames = parameterNames;
            this.Reset();
        }

        /// <summary>
        ///   Gets or sets the signature.
        /// </summary>
        /// <value>The signature.</value>
        private String Signature { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [parameter names].
        /// </summary>
        /// <value><c>true</c> if [parameter names]; otherwise, <c>false</c>.</value>
        private bool ParameterNames { get; set; }

        /// <summary>
        ///   Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        private int Index { get; set; }

        /// <summary>
        ///   Gets or sets the current token.
        /// </summary>
        /// <value>The current token.</value>
        private String CurrentToken { get; set; }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {}

        /// <summary>
        ///   Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        ///   true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        /// </returns>
        /// <exception cref = "T:System.InvalidOperationException">
        ///   The collection was modified after the enumerator was created.
        /// </exception>
        public bool MoveNext()
        {
            if (this.Index < 0)
            {
                return false;
            }

            int openingIndex = this.Signature.IndexOf('(', this.Index);
            if (openingIndex < 0)
            {
                return false;
            }

            int count = 1;
            int closingIndex = openingIndex + 1;
            while (true)
            {
                char c = this.Signature[closingIndex++];
                if (c == '(')
                {
                    count++;
                }
                if (c == ')')
                {
					count--;
					if (count == 0)
                    {
                        break;
                    }
                }
            }

            if (this.ParameterNames)
            {
                int spaceIndex = this.Signature.IndexOf(' ', closingIndex + 1);
                spaceIndex = (spaceIndex == -1) ? this.Signature.Length : spaceIndex;
                this.CurrentToken = this.Signature.Substring(closingIndex, spaceIndex - closingIndex);
            }
            else
            {
                this.CurrentToken = this.Signature.Substring(openingIndex + 1, closingIndex - openingIndex - 2);
            }
            this.Index = closingIndex + 1;

            return true;
        }

        /// <summary>
        ///   Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref = "T:System.InvalidOperationException">
        ///   The collection was modified after the enumerator was created.
        /// </exception>
        public void Reset()
        {
            this.CurrentToken = null;

            // Set the index after the return type
			int index = 0;
			int count = 0;
			bool found = false;
			while (!found && index < this.Signature.Length)
			{
				char c = this.Signature[index++];
				if (c == '(')
				{
					count++;
				}
				if (c == ')')
				{
					count--;
					if (count == 0)
					{
						found = true;
						break;
					}
				}
			}

			this.Index = index;
        }

        /// <summary>
        ///   Gets the current.
        /// </summary>
        /// <value>The current.</value>
        public string Current
        {
            get { return this.CurrentToken; }
        }

        /// <summary>
        ///   Gets the current.
        /// </summary>
        /// <value>The current.</value>
        object IEnumerator.Current
        {
            get { return this.Current; }
        }
    }
}