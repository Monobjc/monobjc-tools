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
namespace Monobjc.Tools.Doxygenator.Model.Doxygen
{
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("doxygenindex", Namespace="", IsNullable=false)]
    public partial class DoxygenIndexType {
        
        private CompoundType[] compoundField;
        
        private string versionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("compound", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CompoundType[] compound {
            get {
                return this.compoundField;
            }
            set {
                this.compoundField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CompoundType {
        
        private string nameField;
        
        private MemberType[] memberField;
        
        private string refidField;
        
        private CompoundKind kindField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("member", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MemberType[] member {
            get {
                return this.memberField;
            }
            set {
                this.memberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string refid {
            get {
                return this.refidField;
            }
            set {
                this.refidField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public CompoundKind kind {
            get {
                return this.kindField;
            }
            set {
                this.kindField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MemberType {
        
        private string nameField;
        
        private string refidField;
        
        private MemberKind kindField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string refid {
            get {
                return this.refidField;
            }
            set {
                this.refidField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public MemberKind kind {
            get {
                return this.kindField;
            }
            set {
                this.kindField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    public enum MemberKind {
        
        /// <remarks/>
        define,
        
        /// <remarks/>
        property,
        
        /// <remarks/>
        @event,
        
        /// <remarks/>
        variable,
        
        /// <remarks/>
        typedef,
        
        /// <remarks/>
        @enum,
        
        /// <remarks/>
        enumvalue,
        
        /// <remarks/>
        function,
        
        /// <remarks/>
        signal,
        
        /// <remarks/>
        prototype,
        
        /// <remarks/>
        friend,
        
        /// <remarks/>
        dcop,
        
        /// <remarks/>
        slot,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    public enum CompoundKind {
        
        /// <remarks/>
        @class,
        
        /// <remarks/>
        @struct,
        
        /// <remarks/>
        union,
        
        /// <remarks/>
        @interface,
        
        /// <remarks/>
        protocol,
        
        /// <remarks/>
        category,
        
        /// <remarks/>
        exception,
        
        /// <remarks/>
        file,
        
        /// <remarks/>
        @namespace,
        
        /// <remarks/>
        group,
        
        /// <remarks/>
        page,
        
        /// <remarks/>
        example,
        
        /// <remarks/>
        dir,
    }
}
