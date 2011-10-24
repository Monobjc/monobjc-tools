using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace Monobjc.Tools.Sdp.Model {
    
    public partial class dictionary {
        
		public dictionary(XElement element)
		{
			this.documentation = element.Elements("documentation").Select(e => new documentation(e)).ToArray();
			this.suite = element.Elements("suite").Select(e => new suite(e)).ToArray();
			this.title = (String) element.Attribute("title");
		}
		
        public documentation[] documentation {
			get; set;
        }
        
        public suite[] suite {
			get; set;
        }
        
        public string title {
			get; set;
        }
    }
    
    public partial class documentation {
        
		public documentation(XElement element)
		{
			this.Items = element.Elements("html").Select(e => new html(e)).ToArray();
		}
		
        public html[] Items {
			get; set;
        }
    }
    
    public partial class html {
        
		public html(XElement element)
		{
			this.Text = element.Nodes().OfType<XText>().Select(n => n.Value).ToArray();
		}
		
        public string[] Text {
			get; set;
        }
    }
    
    public partial class suite {
        
		public suite(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.@class = element.Elements("class").Select(e => new @class(e)).ToArray();
			this.classextension = element.Elements("class-extension").Select(e => new classextension(e)).ToArray();
			this.command = element.Elements("command").Select(e => new command(e)).ToArray();
			this.documentation = element.Elements("documentation").Select(e => new documentation(e)).ToArray();
			this.enumeration = element.Elements("enumeration").Select(e => new enumeration(e)).ToArray();
			this.@event = element.Elements("event").Select(e => new @event(e)).ToArray();
			this.recordtype = element.Elements("record-type").Select(e => new recordtype(e)).ToArray();
			this.valuetype = element.Elements("value-type").Select(e => new valuetype(e)).ToArray();
			this.name = (String) element.Attribute("name");
			this.code = (String) element.Attribute("code");
			this.description = (String) element.Attribute("description");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
		}
		
        public cocoa Item {
			get; set;
        }
        
        public @class[] @class {
			get; set;
        }
        
        public classextension[] classextension {
			get; set;
        }
        
        public command[] command {
			get; set;
        }
        
        public documentation[] documentation {
			get; set;
        }
        
        public enumeration[] enumeration {
			get; set;
        }
        
        public @event[] @event {
			get; set;
        }
        
        public recordtype[] recordtype {
			get; set;
        }
        
        public valuetype[] valuetype {
			get; set;
        }
        
        public string name {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public string description {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
    }
    
    public partial class cocoa {
        
		public cocoa(XElement element)
		{
			this.name = (String) element.Attribute("name");
			this.@class = (String) element.Attribute("class");
			this.key = (String) element.Attribute("key");
			this.method = (String) element.Attribute("method");
			this.insertatbeginning = "yes".Equals((String) element.Attribute("insert-at-beginning"), StringComparison.OrdinalIgnoreCase);
			this.booleanvalue = "yes".Equals((String) element.Attribute("boolean-value"), StringComparison.OrdinalIgnoreCase);
			this.integervalue = (String) element.Attribute("integer-value");
			this.stringvalue = (String) element.Attribute("string-value");
		}
		
        public string name {
			get; set;
        }
        
        public string @class {
			get; set;
        }
        
        public string key {
			get; set;
        }
        
        public string method {
			get; set;
        }
        
        public bool insertatbeginning {
			get; set;
        }
        
        public bool booleanvalue {
			get; set;
        }
        
        public string integervalue {
			get; set;
        }
        
        public string stringvalue {
			get; set;
        }
    }
    
    public partial class @class {
        
		public @class(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.contents = element.Elements("contents").Select(e => new contents(e)).ToArray();
			this.documentation = element.Elements("documentation").Select(e => new documentation(e)).ToArray();
			this.element = element.Elements("element").Select(e => new element(e)).ToArray();
			this.property = element.Elements("property").Select(e => new property(e)).ToArray();
			this.respondsto = element.Elements("respondsto").Select(e => new respondsto(e)).ToArray();
			this.synonym = element.Elements("synonym").Select(e => new synonym(e)).ToArray();
			this.xref = element.Elements("xref").Select(e => new xref(e)).ToArray();
			this.name = (String) element.Attribute("name");
			this.id = (String) element.Attribute("id");
			this.code = (String) element.Attribute("code");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.plural = (String) element.Attribute("plural");
			this.inherits = (String) element.Attribute("inherits");
			this.description = (String) element.Attribute("description");
		}
		
        public cocoa Item {
			get; set;
        }
        
		public contents[] contents {
			get; set;
		}
		
		public documentation[] documentation {
			get; set;
		}
		
		public element[] element {
			get; set;
		}
		
		public property[] property {
			get; set;
		}
		
		public respondsto[] respondsto {
			get; set;
		}
		
		public synonym[] synonym {
			get; set;
		}
		
		public xref[] xref {
			get; set;
		}
		
        public string name {
			get; set;
        }
        
        public string id {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
        
        public string plural {
			get; set;
        }
        
        public string inherits {
			get; set;
        }
        
        public string description {
			get; set;
        }
    }
    
    public partial class contents {
        
		public contents(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.Items = element.Elements("type").Select(e => new type(e)).ToArray();
			this.name = (String) element.Attribute("name");
			this.code = (String) element.Attribute("code");
			this.type = (String) element.Attribute("type");
			this.access = (String) element.Attribute("access");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
		}
        
        public cocoa Item {
			get; set;
        }
        
        public type[] Items {
			get; set;
        }
        
        public string name {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public string type {
			get; set;
        }
        
        public string access {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
        
        public string description {
			get; set;
        }
    }
    
    public partial class type {
        
		public type(XElement element)
		{
			this.type1 = (String) element.Attribute("type");
			this.list = "yes".Equals((String) element.Attribute("list"), StringComparison.OrdinalIgnoreCase);
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
		}
		
        public string type1 {
			get; set;
        }
        
        public bool list {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
    }
    
    public partial class element {
        
		public element(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.accessor = element.Elements("accessor").Select(e => new accessor(e)).ToArray();
			this.type = (String) element.Attribute("type");
			this.access = (String) element.Attribute("access");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
		}
		
        public cocoa Item {
			get; set;
        }
        
        public accessor[] accessor {
			get; set;
        }
        
        public string type {
			get; set;
        }
        
        public string access {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
        
        public string description {
			get; set;
        }
    }
    
    public partial class accessor {
        
		public accessor(XElement element)
		{
			this.style = (String) element.Attribute("style");
		}
		
        public string style {
			get; set;
        }
    }
    
    public partial class property {
        
		public property(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.documentation = element.Elements("documentation").Select(e => new documentation(e)).ToArray();
			this.synonym = element.Elements("synonym").Select(e => new synonym(e)).ToArray();
			this.Items = element.Elements("type").Select(e => new type(e)).ToArray();
			this.name = (String) element.Attribute("name");
			this.code = (String) element.Attribute("code");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.type = (String) element.Attribute("type");
			this.access = (String) element.Attribute("access");
			this.inproperties = "yes".Equals((String) element.Attribute("in-properties"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
		}
        
        public cocoa Item {
			get; set;
        }
        
        public documentation[] documentation {
			get; set;
        }
        
        public synonym[] synonym {
			get; set;
        }
        
        public type[] Items {
			get; set;
        }
        
        public string name {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
        
        public string type {
			get; set;
        }
        
        public string access {
			get; set;
        }
        
        public bool inproperties {
			get; set;
        }
        
        public string description {
			get; set;
        }
    }
    
    public partial class synonym {
        
		public synonym(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.name = (String) element.Attribute("name");
			this.code = (String) element.Attribute("code");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
		}
        
        public cocoa Item {
			get; set;
        }
        
        public string name {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
    }
    
    public partial class respondsto {
        
		public respondsto(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.command = (String) element.Attribute("command");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.name = (String) element.Attribute("name");
		}
        
        public cocoa Item {
			get; set;
        }
        
        public string command {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
        
        public string name {
			get; set;
        }
    }
    
    public partial class xref {
        
		public xref(XElement element)
		{
			this.target = (String) element.Attribute("target");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
		}
        
        public string target {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
    }
    
    public partial class classextension {
        
		public classextension(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.contents = element.Elements("contents").Select(e => new contents(e)).ToArray();
			this.documentation = element.Elements("documentation").Select(e => new documentation(e)).ToArray();
			this.element = element.Elements("element").Select(e => new element(e)).ToArray();
			this.property = element.Elements("property").Select(e => new property(e)).ToArray();
			this.respondsto = element.Elements("respondsto").Select(e => new respondsto(e)).ToArray();
			this.synonym = element.Elements("synonym").Select(e => new synonym(e)).ToArray();
			this.xref = element.Elements("xref").Select(e => new xref(e)).ToArray();
			this.id = (String) element.Attribute("id");
			this.extends = (String) element.Attribute("extends");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
		}
        
        public cocoa Item {
			get; set;
        }
        
        public contents[] contents {
			get; set;
        }
        
        public documentation[] documentation {
			get; set;
        }
        
        public element[] element {
			get; set;
        }
        
        public property[] property {
			get; set;
        }
        
        public respondsto[] respondsto {
			get; set;
        }
        
        public synonym[] synonym {
			get; set;
        }
        
        public xref[] xref {
			get; set;
        }
        
        public string id {
			get; set;
        }
        
        public string extends {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
        
        public string description {
			get; set;
        }
    }
    
    public partial class command {
        
		public command(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.synonym = element.Elements("synonym").Select(e => new synonym(e)).ToArray();
			this.directparameter = element.Element("direct-parameter") != null ? new directparameter(element.Element("direct-parameter")) : null;
			this.parameter = element.Elements("parameter").Select(e => new parameter(e)).ToArray();
			this.result = element.Element("result") != null ? new result(element.Element("result")) : null;
			this.documentation = element.Elements("documentation").Select(e => new documentation(e)).ToArray();
			this.xref = element.Elements("xref").Select(e => new xref(e)).ToArray();
			this.name = (String) element.Attribute("name");
			this.id = (String) element.Attribute("id");
			this.code = (String) element.Attribute("code");
			this.description = (String) element.Attribute("description");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
		}
        
        public cocoa Item {
			get; set;
        }
        
        public synonym[] synonym {
			get; set;
        }
        
        public directparameter directparameter {
			get; set;
        }
        
        public parameter[] parameter {
			get; set;
        }
        
        public result result {
			get; set;
        }
        
        public documentation[] documentation {
			get; set;
        }
        
        public xref[] xref {
			get; set;
        }
        
        public string name {
			get; set;
        }
        
        public string id {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public string description {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
    }
    
    public partial class directparameter {
        
		public directparameter(XElement element)
		{
			this.Items = element.Elements("type").Select(e => new type(e)).ToArray();
			this.type = (String) element.Attribute("type");
			this.optional = "yes".Equals((String) element.Attribute("optional"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
		}
        
        public type[] Items {
			get; set;
        }
        
        public string type {
			get; set;
        }
        
        public bool optional {
			get; set;
        }
        
        public string description {
			get; set;
        }
    }
    
    public partial class parameter {
        
		public parameter(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.Items = element.Elements("type").Select(e => new type(e)).ToArray();
			this.name = (String) element.Attribute("name");
			this.code = (String) element.Attribute("code");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.type = (String) element.Attribute("type");
			this.optional = "yes".Equals((String) element.Attribute("optional"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
		}
        
        public cocoa Item {
			get; set;
        }
        
        public type[] Items {
			get; set;
        }
        
        public string name {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
        
        public string type {
			get; set;
        }
        
        public bool optional {
			get; set;
        }
        
        public string description {
			get; set;
        }
    }
    
    public partial class result {
        
		public result(XElement element)
		{
			this.Items = element.Elements("type").Select(e => new type(e)).ToArray();
			this.type = (String) element.Attribute("type");
			this.description = (String) element.Attribute("description");
		}
        
        public type[] Items {
			get; set;
        }
        
        public string type {
			get; set;
        }
        
        public string description {
			get; set;
        }
    }
    
    public partial class enumeration {
        
		public enumeration(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.documentation = element.Elements("documentation").Select(e => new documentation(e)).ToArray();
			this.enumerator = element.Elements("enumerator").Select(e => new enumerator(e)).ToArray();
			this.xref = element.Elements("xref").Select(e => new xref(e)).ToArray();
			this.name = (String) element.Attribute("name");
			this.id = (String) element.Attribute("id");
			this.code = (String) element.Attribute("code");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
			this.inline = (String) element.Attribute("inline");
		}
        
        public cocoa Item {
			get; set;
        }
        
        public documentation[] documentation {
			get; set;
        }
        
        public enumerator[] enumerator {
			get; set;
        }
        
        public xref[] xref {
			get; set;
        }
        
        public string name {
			get; set;
        }
        
        public string id {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
        
        public string description {
			get; set;
        }
        
        public string inline {
			get; set;
        }
    }
    
    public partial class enumerator {
        
		public enumerator(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.synonym = element.Elements("synonym").Select(e => new synonym(e)).ToArray();
			this.documentation = element.Elements("documentation").Select(e => new documentation(e)).ToArray();
			this.name = (String) element.Attribute("name");
			this.code = (String) element.Attribute("code");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
		}
        
        public cocoa Item {
			get; set;
        }
        
        public synonym[] synonym {
			get; set;
        }
        
        public documentation[] documentation {
			get; set;
        }
        
        public string name {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
        
        public string description {
			get; set;
        }
    }
    
    public partial class @event {
        
		public @event(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.synonym = element.Elements("synonym").Select(e => new synonym(e)).ToArray();
			this.documentation = element.Elements("documentation").Select(e => new documentation(e)).ToArray();
			this.directparameter = element.Element("direct-parameter") != null ? new directparameter(element.Element("direct-parameter")) : null;
			this.parameter = element.Elements("parameter").Select(e => new parameter(e)).ToArray();
			this.result = element.Element("result") != null ? new result(element.Element("result")) : null;
			this.xref = element.Elements("xref").Select(e => new xref(e)).ToArray();
			this.id = (String) element.Attribute("id");
			this.name = (String) element.Attribute("name");
			this.code = (String) element.Attribute("code");
			this.description = (String) element.Attribute("description");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
		}
        
        public cocoa Item {
			get; set;
        }
        
        public synonym[] synonym {
			get; set;
        }
        
        public documentation[] documentation {
			get; set;
        }
        
        public directparameter directparameter {
			get; set;
        }
        
        public parameter[] parameter {
			get; set;
        }
        
        public result result {
			get; set;
        }
        
        public xref[] xref {
			get; set;
        }
        
        public string name {
			get; set;
        }
        
        public string id {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public string description {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
    }
    
    public partial class recordtype {
        
		public recordtype(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.synonym = element.Elements("synonym").Select(e => new synonym(e)).ToArray();
			this.documentation = element.Elements("documentation").Select(e => new documentation(e)).ToArray();
			this.property = element.Elements("property").Select(e => new property(e)).ToArray();
			this.xref = element.Elements("xref").Select(e => new xref(e)).ToArray();
			this.name = (String) element.Attribute("name");
			this.id = (String) element.Attribute("id");
			this.code = (String) element.Attribute("code");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.plural = (String) element.Attribute("plural");
			this.description = (String) element.Attribute("description");
		}
        
        public cocoa Item {
			get; set;
        }
        
        public synonym[] synonym {
			get; set;
        }
        
        public documentation[] documentation {
			get; set;
        }
        
        public property[] property {
			get; set;
        }
        
        public xref[] xref {
			get; set;
        }
        
        public string name {
			get; set;
        }
        
        public string id {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
        
        public string plural {
			get; set;
        }
        
        public string description {
			get; set;
        }
    }
    
    public partial class valuetype {
        
		public valuetype(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.synonym = element.Elements("synonym").Select(e => new synonym(e)).ToArray();
			this.documentation = element.Elements("documentation").Select(e => new documentation(e)).ToArray();
			this.xref = element.Elements("xref").Select(e => new xref(e)).ToArray();
			this.name = (String) element.Attribute("name");
			this.id = (String) element.Attribute("id");
			this.code = (String) element.Attribute("code");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.plural = (String) element.Attribute("plural");
			this.description = (String) element.Attribute("description");
		}
        
        public cocoa Item {
			get; set;
        }
        
        public synonym[] synonym {
			get; set;
        }
        
        public documentation[] documentation {
			get; set;
        }
        
        public xref[] xref {
			get; set;
        }
        
        public string name {
			get; set;
        }
        
        public string id {
			get; set;
        }
        
        public string code {
			get; set;
        }
        
        public bool hidden {
			get; set;
        }
        
        public string plural {
			get; set;
        }
        
        public string description {
			get; set;
        }
    }
}
