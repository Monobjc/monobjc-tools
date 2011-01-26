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
using System.Collections.Specialized;
using System.IO;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers
{
    public abstract class BaseParser : IBaseParser
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "BaseParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        protected BaseParser(NameValueCollection settings, TypeManager typeManager)
        {
            this.Settings = settings;
            this.TypeManager = typeManager;
        }

        /// <summary>
        ///   Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public NameValueCollection Settings { get; private set; }

        /// <summary>
        ///   Gets or sets the type manager.
        /// </summary>
        /// <value>The type manager.</value>
        public TypeManager TypeManager { get; private set; }

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "file">The file.</param>
        public void Parse(BaseEntity entity, String file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                Parse(entity, reader);
            }
        }

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "reader">The reader.</param>
        public abstract void Parse(BaseEntity entity, TextReader reader);
    }
}