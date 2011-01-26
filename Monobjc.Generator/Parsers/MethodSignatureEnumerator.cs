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
using System.Collections;
using System.Collections.Generic;

namespace Monobjc.Tools.Generator.Parsers
{
    /// <summary>
    ///   Parse the method signature and expose it as an enumerator.
    /// </summary>
    public sealed class MethodSignatureEnumerator : IEnumerator<String>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "MethodSignatureEnumerator" /> class.
        /// </summary>
        /// <param name = "signature">The signature.</param>
        public MethodSignatureEnumerator(String signature)
        {
            this.Signature = signature;
            this.Reset();
        }

        /// <summary>
        ///   Gets or sets the signature.
        /// </summary>
        /// <value>The signature.</value>
        private String Signature { get; set; }

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
            int openingIndex = this.Signature.IndexOf('(', this.Index);
            if (openingIndex < 0)
            {
                return false;
            }

            int count = 0;
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
                    if (count == 0)
                    {
                        break;
                    }
                    count--;
                }
            }

            this.CurrentToken = this.Signature.Substring(openingIndex + 1, closingIndex - openingIndex - 2);
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
            this.Index = 0;
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