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
    [XmlRoot(Namespace = "", IsNullable = true)]
    public class Patch
    {
        private string valueField;

        /// <remarks />
        [XmlText]
        public string Value
        {
            get { return this.valueField; }
            set { this.valueField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Replace
    {
        private string sourceField;

        private string withField;

        private string typeField;

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Source
        {
            get { return this.sourceField; }
            set { this.sourceField = value; }
        }

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string With
        {
            get { return this.withField; }
            set { this.withField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string type
        {
            get { return this.typeField; }
            set { this.typeField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Entities
    {
        private object[] itemsField;

        /// <remarks />
        [XmlElement("Framework", typeof (EntitiesFramework), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("Patch", typeof (Patch), IsNullable = true)]
        [XmlElement("Replace", typeof (Replace))]
        public object[] Items
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
    public class EntitiesFramework
    {
        private EntitiesFrameworkType[] typesField;

        private EntitiesFrameworkClass[] classesField;

        private EntitiesFrameworkProtocol[] protocolsField;

        private EntitiesFrameworkEnumeration[] enumerationsField;

        private EntitiesFrameworkStructure[] structuresField;

        private string nameField;

        private string usingsField;

        private string assemblyField;

        /// <remarks />
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Type", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public EntitiesFrameworkType[] Types
        {
            get { return this.typesField; }
            set { this.typesField = value; }
        }

        /// <remarks />
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Class", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public EntitiesFrameworkClass[] Classes
        {
            get { return this.classesField; }
            set { this.classesField = value; }
        }

        /// <remarks />
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Protocol", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public EntitiesFrameworkProtocol[] Protocols
        {
            get { return this.protocolsField; }
            set { this.protocolsField = value; }
        }

        /// <remarks />
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Enumeration", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public EntitiesFrameworkEnumeration[] Enumerations
        {
            get { return this.enumerationsField; }
            set { this.enumerationsField = value; }
        }

        /// <remarks />
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Structure", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public EntitiesFrameworkStructure[] Structures
        {
            get { return this.structuresField; }
            set { this.structuresField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string usings
        {
            get { return this.usingsField; }
            set { this.usingsField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string assembly
        {
            get { return this.assemblyField; }
            set { this.assemblyField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class EntitiesFrameworkType
    {
        private Patch[] patchField;

        private Replace[] replaceField;

        private string nameField;

        /// <remarks />
        [XmlElement("Patch", IsNullable = true)]
        public Patch[] Patch
        {
            get { return this.patchField; }
            set { this.patchField = value; }
        }

        /// <remarks />
        [XmlElement("Replace")]
        public Replace[] Replace
        {
            get { return this.replaceField; }
            set { this.replaceField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class EntitiesFrameworkClass
    {
        private Patch[] patchField;

        private Replace[] replaceField;

        private string nameField;

        /// <remarks />
        [XmlElement("Patch", IsNullable = true)]
        public Patch[] Patch
        {
            get { return this.patchField; }
            set { this.patchField = value; }
        }

        /// <remarks />
        [XmlElement("Replace")]
        public Replace[] Replace
        {
            get { return this.replaceField; }
            set { this.replaceField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class EntitiesFrameworkProtocol
    {
        private Patch[] patchField;

        private Replace[] replaceField;

        private string nameField;

        /// <remarks />
        [XmlElement("Patch", IsNullable = true)]
        public Patch[] Patch
        {
            get { return this.patchField; }
            set { this.patchField = value; }
        }

        /// <remarks />
        [XmlElement("Replace")]
        public Replace[] Replace
        {
            get { return this.replaceField; }
            set { this.replaceField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class EntitiesFrameworkEnumeration
    {
        private Patch[] patchField;

        private Replace[] replaceField;

        private string nameField;

        /// <remarks />
        [XmlElement("Patch", IsNullable = true)]
        public Patch[] Patch
        {
            get { return this.patchField; }
            set { this.patchField = value; }
        }

        /// <remarks />
        [XmlElement("Replace")]
        public Replace[] Replace
        {
            get { return this.replaceField; }
            set { this.replaceField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "2.0.50727.3038")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class EntitiesFrameworkStructure
    {
        private Patch[] patchField;

        private Replace[] replaceField;

        private string nameField;

        /// <remarks />
        [XmlElement("Patch", IsNullable = true)]
        public Patch[] Patch
        {
            get { return this.patchField; }
            set { this.patchField = value; }
        }

        /// <remarks />
        [XmlElement("Replace")]
        public Replace[] Replace
        {
            get { return this.replaceField; }
            set { this.replaceField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }
    }
}