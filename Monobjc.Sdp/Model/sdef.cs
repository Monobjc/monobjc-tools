using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace Monobjc.Tools.Sdp.Model {
    
    public partial class dictionary {
        
		public dictionary(XElement element)
		{
			this.documentation = new List<documentation>(element.Elements("documentation").Select(e => new documentation(e)));
			this.suite = new List<suite>(element.Elements("suite").Select(e => new suite(e)));
			this.title = (String) element.Attribute("title");
		}
		
        public IList<documentation> documentation {
			get; set;
        }
        
        public IList<suite> suite {
			get; set;
        }
        
        public string title {
			get; set;
        }
    }
    
    public partial class documentation {
        
		public documentation(XElement element)
		{
			this.Items = new List<html>(element.Elements("html").Select(e => new html(e)));
		}
		
        public IList<html> Items {
			get; set;
        }
    }
    
    public partial class html {
        
		public html(XElement element)
		{
			this.Text = new List<String>(element.Nodes().OfType<XText>().Select(n => n.Value));
		}
		
        public IList<String> Text {
			get; set;
        }
    }
    
    public partial class suite {
        
		public suite(XElement element)
		{
			this.Item = element.Element("cocoa") != null ? new cocoa(element.Element("cocoa")) : null;
			this.@class = new List<@class>(element.Elements("class").Select(e => new @class(e)));
			this.classextension = new List<classextension>(element.Elements("class-extension").Select(e => new classextension(e)));
			this.command = new List<command>(element.Elements("command").Select(e => new command(e)));
			this.documentation = new List<documentation>(element.Elements("documentation").Select(e => new documentation(e)));
			this.enumeration = new List<enumeration>(element.Elements("enumeration").Select(e => new enumeration(e)));
			this.@event = new List<@event>(element.Elements("event").Select(e => new @event(e)));
			this.recordtype = new List<recordtype>(element.Elements("record-type").Select(e => new recordtype(e)));
			this.valuetype = new List<valuetype>(element.Elements("value-type").Select(e => new valuetype(e)));
			this.name = (String) element.Attribute("name");
			this.code = (String) element.Attribute("code");
			this.description = (String) element.Attribute("description");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
		}
		
        public cocoa Item {
			get; set;
        }
        
        public IList<@class> @class {
			get; set;
        }
        
        public IList<classextension> classextension {
			get; set;
        }
        
        public IList<command> command {
			get; set;
        }
        
        public IList<documentation> documentation {
			get; set;
        }
        
        public IList<enumeration> enumeration {
			get; set;
        }
        
        public IList<@event> @event {
			get; set;
        }
        
        public IList<recordtype> recordtype {
			get; set;
        }
        
        public IList<valuetype> valuetype {
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
			this.contents = new List<contents>(element.Elements("contents").Select(e => new contents(e)));
			this.documentation = new List<documentation>(element.Elements("documentation").Select(e => new documentation(e)));
			this.element = new List<element>(element.Elements("element").Select(e => new element(e)));
			this.property = new List<property>(element.Elements("property").Select(e => new property(e)));
			this.respondsto = new List<respondsto>(element.Elements("respondsto").Select(e => new respondsto(e)));
			this.synonym = new List<synonym>(element.Elements("synonym").Select(e => new synonym(e)));
			this.xref = new List<xref>(element.Elements("xref").Select(e => new xref(e)));
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
        
		public IList<contents> contents {
			get; set;
		}
		
		public IList<documentation> documentation {
			get; set;
		}
		
		public IList<element> element {
			get; set;
		}
		
		public IList<property> property {
			get; set;
		}
		
		public IList<respondsto> respondsto {
			get; set;
		}
		
		public IList<synonym> synonym {
			get; set;
		}
		
		public IList<xref> xref {
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
			this.Items = new List<type>(element.Elements("type").Select(e => new type(e)));
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
        
        public IList<type> Items {
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
			this.accessor = new List<accessor>(element.Elements("accessor").Select(e => new accessor(e)));
			this.type = (String) element.Attribute("type");
			this.access = (String) element.Attribute("access");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
		}
		
        public cocoa Item {
			get; set;
        }
        
        public IList<accessor> accessor {
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
			this.documentation = new List<documentation>(element.Elements("documentation").Select(e => new documentation(e)));
			this.synonym = new List<synonym>(element.Elements("synonym").Select(e => new synonym(e)));
			this.Items = new List<type>(element.Elements("type").Select(e => new type(e)));
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
        
        public IList<documentation> documentation {
			get; set;
        }
        
        public IList<synonym> synonym {
			get; set;
        }
        
        public IList<type> Items {
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
			this.contents = new List<contents>(element.Elements("contents").Select(e => new contents(e)));
			this.documentation = new List<documentation>(element.Elements("documentation").Select(e => new documentation(e)));
			this.element = new List<element>(element.Elements("element").Select(e => new element(e)));
			this.property = new List<property>(element.Elements("property").Select(e => new property(e)));
			this.respondsto = new List<respondsto>(element.Elements("respondsto").Select(e => new respondsto(e)));
			this.synonym = new List<synonym>(element.Elements("synonym").Select(e => new synonym(e)));
			this.xref = new List<xref>(element.Elements("xref").Select(e => new xref(e)));
			this.id = (String) element.Attribute("id");
			this.extends = (String) element.Attribute("extends");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
		}
        
        public cocoa Item {
			get; set;
        }
        
        public IList<contents> contents {
			get; set;
        }
        
        public IList<documentation> documentation {
			get; set;
        }
        
        public IList<element> element {
			get; set;
        }
        
        public IList<property> property {
			get; set;
        }
        
        public IList<respondsto> respondsto {
			get; set;
        }
        
        public IList<synonym> synonym {
			get; set;
        }
        
        public IList<xref> xref {
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
			this.synonym = new List<synonym>(element.Elements("synonym").Select(e => new synonym(e)));
			this.directparameter = element.Element("direct-parameter") != null ? new directparameter(element.Element("direct-parameter")) : null;
			this.parameter = new List<parameter>(element.Elements("parameter").Select(e => new parameter(e)));
			this.result = element.Element("result") != null ? new result(element.Element("result")) : null;
			this.documentation = new List<documentation>(element.Elements("documentation").Select(e => new documentation(e)));
			this.xref = new List<xref>(element.Elements("xref").Select(e => new xref(e)));
			this.name = (String) element.Attribute("name");
			this.id = (String) element.Attribute("id");
			this.code = (String) element.Attribute("code");
			this.description = (String) element.Attribute("description");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
		}
        
        public cocoa Item {
			get; set;
        }
        
        public IList<synonym> synonym {
			get; set;
        }
        
        public directparameter directparameter {
			get; set;
        }
        
        public IList<parameter> parameter {
			get; set;
        }
        
        public result result {
			get; set;
        }
        
        public IList<documentation> documentation {
			get; set;
        }
        
        public IList<xref> xref {
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
			this.Items = new List<type>(element.Elements("type").Select(e => new type(e)));
			this.type = (String) element.Attribute("type");
			this.optional = "yes".Equals((String) element.Attribute("optional"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
		}
        
        public IList<type> Items {
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
			this.Items = new List<type>(element.Elements("type").Select(e => new type(e)));
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
        
        public IList<type> Items {
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
			this.Items = new List<type>(element.Elements("type").Select(e => new type(e)));
			this.type = (String) element.Attribute("type");
			this.description = (String) element.Attribute("description");
		}
        
        public IList<type> Items {
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
			this.documentation = new List<documentation>(element.Elements("documentation").Select(e => new documentation(e)));
			this.enumerator = new List<enumerator>(element.Elements("enumerator").Select(e => new enumerator(e)));
			this.xref = new List<xref>(element.Elements("xref").Select(e => new xref(e)));
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
        
        public IList<documentation> documentation {
			get; set;
        }
        
        public IList<enumerator> enumerator {
			get; set;
        }
        
        public IList<xref> xref {
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
			this.synonym = new List<synonym>(element.Elements("synonym").Select(e => new synonym(e)));
			this.documentation = new List<documentation>(element.Elements("documentation").Select(e => new documentation(e)));
			this.name = (String) element.Attribute("name");
			this.code = (String) element.Attribute("code");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
			this.description = (String) element.Attribute("description");
		}
        
        public cocoa Item {
			get; set;
        }
        
        public IList<synonym> synonym {
			get; set;
        }
        
        public IList<documentation> documentation {
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
			this.synonym = new List<synonym>(element.Elements("synonym").Select(e => new synonym(e)));
			this.documentation = new List<documentation>(element.Elements("documentation").Select(e => new documentation(e)));
			this.directparameter = element.Element("direct-parameter") != null ? new directparameter(element.Element("direct-parameter")) : null;
			this.parameter = new List<parameter>(element.Elements("parameter").Select(e => new parameter(e)));
			this.result = element.Element("result") != null ? new result(element.Element("result")) : null;
			this.xref = new List<xref>(element.Elements("xref").Select(e => new xref(e)));
			this.id = (String) element.Attribute("id");
			this.name = (String) element.Attribute("name");
			this.code = (String) element.Attribute("code");
			this.description = (String) element.Attribute("description");
			this.hidden = "yes".Equals((String) element.Attribute("hidden"), StringComparison.OrdinalIgnoreCase);
		}
        
        public cocoa Item {
			get; set;
        }
        
        public IList<synonym> synonym {
			get; set;
        }
        
        public IList<documentation> documentation {
			get; set;
        }
        
        public directparameter directparameter {
			get; set;
        }
        
        public IList<parameter> parameter {
			get; set;
        }
        
        public result result {
			get; set;
        }
        
        public IList<xref> xref {
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
			this.synonym = new List<synonym>(element.Elements("synonym").Select(e => new synonym(e)));
			this.documentation = new List<documentation>(element.Elements("documentation").Select(e => new documentation(e)));
			this.property = new List<property>(element.Elements("property").Select(e => new property(e)));
			this.xref = new List<xref>(element.Elements("xref").Select(e => new xref(e)));
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
        
        public IList<synonym> synonym {
			get; set;
        }
        
        public IList<documentation> documentation {
			get; set;
        }
        
        public IList<property> property {
			get; set;
        }
        
        public IList<xref> xref {
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
			this.synonym = new List<synonym>(element.Elements("synonym").Select(e => new synonym(e)));
			this.documentation = new List<documentation>(element.Elements("documentation").Select(e => new documentation(e)));
			this.xref = new List<xref>(element.Elements("xref").Select(e => new xref(e)));
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
        
        public IList<synonym> synonym {
			get; set;
        }
        
        public IList<documentation> documentation {
			get; set;
        }
        
        public IList<xref> xref {
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
