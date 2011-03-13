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
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Monobjc.Tools.Generator.Model.Configuration
{
    /// <remarks />
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Mappings
    {
        private MappingsMapping[] itemsField;

        /// <remarks />
        [XmlElement("Mapping", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public MappingsMapping[] Items
        {
            get { return this.itemsField; }
            set { this.itemsField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class MappingsMapping
    {
        private string typeField;

        private string valueField;

        /// <remarks />
        [XmlAttribute]
        public string type
        {
            get { return this.typeField; }
            set { this.typeField = value; }
        }

        /// <remarks />
        [XmlText]
        public string Value
        {
            get { return this.valueField; }
            set { this.valueField = value; }
        }
    }
}