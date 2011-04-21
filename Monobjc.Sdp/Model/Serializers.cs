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
namespace Monobjc.Tools.Sdp.Model
{

    public class XmlSerializationWriter1 : System.Xml.Serialization.XmlSerializationWriter {

        public void Write34_dictionary(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"dictionary", @"");
                return;
            }
            TopLevelElement();
            Write31_dictionary(@"dictionary", @"", ((global::Monobjc.Tools.Sdp.Model.dictionary)o), false, false);
        }

        public void Write35_suite(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"suite", @"");
                return;
            }
            TopLevelElement();
            Write30_suite(@"suite", @"", ((global::Monobjc.Tools.Sdp.Model.suite)o), false, false);
        }

        public void Write36_cocoa(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"cocoa", @"");
                return;
            }
            TopLevelElement();
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o), false, false);
        }

        public void Write37_yorn(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"yorn", @"");
                return;
            }
            WriteElementString(@"yorn", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o)));
        }

        public void Write38_cocoaBooleanvalue(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"cocoaBooleanvalue", @"");
                return;
            }
            WriteElementString(@"cocoaBooleanvalue", @"", Write4_cocoaBooleanvalue(((global::Monobjc.Tools.Sdp.Model.cocoaBooleanvalue)o)));
        }

        public void Write39_class(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"class", @"");
                return;
            }
            TopLevelElement();
            Write25_class(@"class", @"", ((global::Monobjc.Tools.Sdp.Model.@class)o), false, false);
        }

        public void Write40_contents(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"contents", @"");
                return;
            }
            TopLevelElement();
            Write16_contents(@"contents", @"", ((global::Monobjc.Tools.Sdp.Model.contents)o), false, false);
        }

        public void Write41_type(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"type", @"");
                return;
            }
            TopLevelElement();
            Write8_type(@"type", @"", ((global::Monobjc.Tools.Sdp.Model.type)o), false, false);
        }

        public void Write42_contentsAccess(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"contentsAccess", @"");
                return;
            }
            WriteElementString(@"contentsAccess", @"", Write15_contentsAccess(((global::Monobjc.Tools.Sdp.Model.contentsAccess)o)));
        }

        public void Write43_documentation(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"documentation", @"");
                return;
            }
            TopLevelElement();
            Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)o), false, false);
        }

        public void Write44_element(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"element", @"");
                return;
            }
            TopLevelElement();
            Write20_element(@"element", @"", ((global::Monobjc.Tools.Sdp.Model.element)o), false, false);
        }

        public void Write45_accessor(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"accessor", @"");
                return;
            }
            TopLevelElement();
            Write18_accessor(@"accessor", @"", ((global::Monobjc.Tools.Sdp.Model.accessor)o), false, false);
        }

        public void Write46_accessortype(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"accessor-type", @"");
                return;
            }
            WriteElementString(@"accessor-type", @"", Write17_accessortype(((global::Monobjc.Tools.Sdp.Model.accessortype)o)));
        }

        public void Write47_elementAccess(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"elementAccess", @"");
                return;
            }
            WriteElementString(@"elementAccess", @"", Write19_elementAccess(((global::Monobjc.Tools.Sdp.Model.elementAccess)o)));
        }

        public void Write48_property(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"property", @"");
                return;
            }
            TopLevelElement();
            Write22_property(@"property", @"", ((global::Monobjc.Tools.Sdp.Model.property)o), false, false);
        }

        public void Write49_synonym(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"synonym", @"");
                return;
            }
            TopLevelElement();
            Write6_synonym(@"synonym", @"", ((global::Monobjc.Tools.Sdp.Model.synonym)o), false, false);
        }

        public void Write50_propertyAccess(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"propertyAccess", @"");
                return;
            }
            WriteElementString(@"propertyAccess", @"", Write21_propertyAccess(((global::Monobjc.Tools.Sdp.Model.propertyAccess)o)));
        }

        public void Write51_respondsto(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"responds-to", @"");
                return;
            }
            TopLevelElement();
            Write23_respondsto(@"responds-to", @"", ((global::Monobjc.Tools.Sdp.Model.respondsto)o), false, false);
        }

        public void Write52_xref(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"xref", @"");
                return;
            }
            TopLevelElement();
            Write12_xref(@"xref", @"", ((global::Monobjc.Tools.Sdp.Model.xref)o), false, false);
        }

        public void Write53_classextension(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"class-extension", @"");
                return;
            }
            TopLevelElement();
            Write24_classextension(@"class-extension", @"", ((global::Monobjc.Tools.Sdp.Model.classextension)o), false, false);
        }

        public void Write54_command(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"command", @"");
                return;
            }
            TopLevelElement();
            Write26_command(@"command", @"", ((global::Monobjc.Tools.Sdp.Model.command)o), false, false);
        }

        public void Write55_directparameter(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"direct-parameter", @"");
                return;
            }
            TopLevelElement();
            Write9_directparameter(@"direct-parameter", @"", ((global::Monobjc.Tools.Sdp.Model.directparameter)o), false, false);
        }

        public void Write56_parameter(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"parameter", @"");
                return;
            }
            TopLevelElement();
            Write10_parameter(@"parameter", @"", ((global::Monobjc.Tools.Sdp.Model.parameter)o), false, false);
        }

        public void Write57_result(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"result", @"");
                return;
            }
            TopLevelElement();
            Write11_result(@"result", @"", ((global::Monobjc.Tools.Sdp.Model.result)o), false, false);
        }

        public void Write58_enumeration(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"enumeration", @"");
                return;
            }
            TopLevelElement();
            Write28_enumeration(@"enumeration", @"", ((global::Monobjc.Tools.Sdp.Model.enumeration)o), false, false);
        }

        public void Write59_enumerator(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"enumerator", @"");
                return;
            }
            TopLevelElement();
            Write27_enumerator(@"enumerator", @"", ((global::Monobjc.Tools.Sdp.Model.enumerator)o), false, false);
        }

        public void Write60_event(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"event", @"");
                return;
            }
            TopLevelElement();
            Write13_event(@"event", @"", ((global::Monobjc.Tools.Sdp.Model.@event)o), false, false);
        }

        public void Write61_recordtype(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"record-type", @"");
                return;
            }
            TopLevelElement();
            Write29_recordtype(@"record-type", @"", ((global::Monobjc.Tools.Sdp.Model.recordtype)o), false, false);
        }

        public void Write62_valuetype(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"value-type", @"");
                return;
            }
            TopLevelElement();
            Write14_valuetype(@"value-type", @"", ((global::Monobjc.Tools.Sdp.Model.valuetype)o), false, false);
        }

        void Write14_valuetype(string n, string ns, global::Monobjc.Tools.Sdp.Model.valuetype o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.valuetype)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            WriteAttribute(@"id", @"", ((global::System.String)o.@id));
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"plural", @"", FromXmlNmTokens(((global::System.String)o.@plural)));
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.synonym[] a = (global::Monobjc.Tools.Sdp.Model.synonym[])o.@synonym;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write6_synonym(@"synonym", @"", ((global::Monobjc.Tools.Sdp.Model.synonym)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.documentation[] a = (global::Monobjc.Tools.Sdp.Model.documentation[])o.@documentation;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.xref[] a = (global::Monobjc.Tools.Sdp.Model.xref[])o.@xref;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write12_xref(@"xref", @"", ((global::Monobjc.Tools.Sdp.Model.xref)a[ia]), false, false);
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write12_xref(string n, string ns, global::Monobjc.Tools.Sdp.Model.xref o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.xref)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"target", @"", ((global::System.String)o.@target));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        string Write3_yorn(global::Monobjc.Tools.Sdp.Model.yorn v) {
            string s = null;
            switch (v) {
                case global::Monobjc.Tools.Sdp.Model.yorn.@yes: s = @"yes"; break;
                case global::Monobjc.Tools.Sdp.Model.yorn.@no: s = @"no"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Monobjc.Tools.Sdp.Model.yorn");
            }
            return s;
        }

        void Write7_documentation(string n, string ns, global::Monobjc.Tools.Sdp.Model.documentation o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.documentation)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            {
                global::System.String[] a = (global::System.String[])o.@html;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        WriteElementString(@"html", @"", ((global::System.String)a[ia]));
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write6_synonym(string n, string ns, global::Monobjc.Tools.Sdp.Model.synonym o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.synonym)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write5_cocoa(string n, string ns, global::Monobjc.Tools.Sdp.Model.cocoa o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.cocoa)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmToken(((global::System.String)o.@name)));
            WriteAttribute(@"class", @"", FromXmlNmToken(((global::System.String)o.@class)));
            WriteAttribute(@"key", @"", FromXmlNmToken(((global::System.String)o.@key)));
            WriteAttribute(@"method", @"", FromXmlNmToken(((global::System.String)o.@method)));
            if (o.@insertatbeginningSpecified) {
                WriteAttribute(@"insert-at-beginning", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@insertatbeginning)));
            }
            if (o.@booleanvalueSpecified) {
                WriteAttribute(@"boolean-value", @"", Write4_cocoaBooleanvalue(((global::Monobjc.Tools.Sdp.Model.cocoaBooleanvalue)o.@booleanvalue)));
            }
            WriteAttribute(@"integer-value", @"", FromXmlNmToken(((global::System.String)o.@integervalue)));
            WriteAttribute(@"string-value", @"", ((global::System.String)o.@stringvalue));
            if (o.@insertatbeginningSpecified) {
            }
            if (o.@booleanvalueSpecified) {
            }
            WriteEndElement(o);
        }

        string Write4_cocoaBooleanvalue(global::Monobjc.Tools.Sdp.Model.cocoaBooleanvalue v) {
            string s = null;
            switch (v) {
                case global::Monobjc.Tools.Sdp.Model.cocoaBooleanvalue.@YES: s = @"YES"; break;
                case global::Monobjc.Tools.Sdp.Model.cocoaBooleanvalue.@NO: s = @"NO"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Monobjc.Tools.Sdp.Model.cocoaBooleanvalue");
            }
            return s;
        }

        void Write29_recordtype(string n, string ns, global::Monobjc.Tools.Sdp.Model.recordtype o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.recordtype)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            WriteAttribute(@"id", @"", ((global::System.String)o.@id));
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"plural", @"", FromXmlNmTokens(((global::System.String)o.@plural)));
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.synonym[] a = (global::Monobjc.Tools.Sdp.Model.synonym[])o.@synonym;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write6_synonym(@"synonym", @"", ((global::Monobjc.Tools.Sdp.Model.synonym)a[ia]), false, false);
                    }
                }
            }
            {
                global::System.Object[] a = (global::System.Object[])o.@Items;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        global::System.Object ai = (global::System.Object)a[ia];
                        if ((object)(ai) != null){
                            if (ai is global::Monobjc.Tools.Sdp.Model.documentation) {
                                Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.property) {
                                Write22_property(@"property", @"", ((global::Monobjc.Tools.Sdp.Model.property)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.xref) {
                                Write12_xref(@"xref", @"", ((global::Monobjc.Tools.Sdp.Model.xref)ai), false, false);
                            }
                            else  if ((object)(ai) != null){
                                throw CreateUnknownTypeException(ai);
                            }
                        }
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write22_property(string n, string ns, global::Monobjc.Tools.Sdp.Model.property o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.property)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"type", @"", FromXmlNmTokens(((global::System.String)o.@type)));
            if (((global::Monobjc.Tools.Sdp.Model.propertyAccess)o.@access) != global::Monobjc.Tools.Sdp.Model.propertyAccess.@rw) {
                WriteAttribute(@"access", @"", Write21_propertyAccess(((global::Monobjc.Tools.Sdp.Model.propertyAccess)o.@access)));
            }
            if (o.@inpropertiesSpecified) {
                WriteAttribute(@"in-properties", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@inproperties)));
            }
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::System.Object[] a = (global::System.Object[])o.@Items;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        global::System.Object ai = (global::System.Object)a[ia];
                        if ((object)(ai) != null){
                            if (ai is global::Monobjc.Tools.Sdp.Model.synonym) {
                                Write6_synonym(@"synonym", @"", ((global::Monobjc.Tools.Sdp.Model.synonym)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.type) {
                                Write8_type(@"type", @"", ((global::Monobjc.Tools.Sdp.Model.type)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.documentation) {
                                Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)ai), false, false);
                            }
                            else  if ((object)(ai) != null){
                                throw CreateUnknownTypeException(ai);
                            }
                        }
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            if (o.@inpropertiesSpecified) {
            }
            WriteEndElement(o);
        }

        void Write8_type(string n, string ns, global::Monobjc.Tools.Sdp.Model.type o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.type)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"type", @"", FromXmlNmTokens(((global::System.String)o.@type1)));
            if (o.@listSpecified) {
                WriteAttribute(@"list", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@list)));
            }
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            if (o.@listSpecified) {
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        string Write21_propertyAccess(global::Monobjc.Tools.Sdp.Model.propertyAccess v) {
            string s = null;
            switch (v) {
                case global::Monobjc.Tools.Sdp.Model.propertyAccess.@r: s = @"r"; break;
                case global::Monobjc.Tools.Sdp.Model.propertyAccess.@w: s = @"w"; break;
                case global::Monobjc.Tools.Sdp.Model.propertyAccess.@rw: s = @"rw"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Monobjc.Tools.Sdp.Model.propertyAccess");
            }
            return s;
        }

        void Write13_event(string n, string ns, global::Monobjc.Tools.Sdp.Model.@event o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.@event)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            WriteAttribute(@"id", @"", ((global::System.String)o.@id));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.synonym[] a = (global::Monobjc.Tools.Sdp.Model.synonym[])o.@synonym;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write6_synonym(@"synonym", @"", ((global::Monobjc.Tools.Sdp.Model.synonym)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.documentation[] a = (global::Monobjc.Tools.Sdp.Model.documentation[])o.@documentation;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)a[ia]), false, false);
                    }
                }
            }
            Write9_directparameter(@"direct-parameter", @"", ((global::Monobjc.Tools.Sdp.Model.directparameter)o.@directparameter), false, false);
            {
                global::System.Object[] a = (global::System.Object[])o.@Items;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        global::System.Object ai = (global::System.Object)a[ia];
                        if ((object)(ai) != null){
                            if (ai is global::Monobjc.Tools.Sdp.Model.parameter) {
                                Write10_parameter(@"parameter", @"", ((global::Monobjc.Tools.Sdp.Model.parameter)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.documentation) {
                                Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)ai), false, false);
                            }
                            else  if ((object)(ai) != null){
                                throw CreateUnknownTypeException(ai);
                            }
                        }
                    }
                }
            }
            Write11_result(@"result", @"", ((global::Monobjc.Tools.Sdp.Model.result)o.@result), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.documentation[] a = (global::Monobjc.Tools.Sdp.Model.documentation[])o.@documentation1;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.xref[] a = (global::Monobjc.Tools.Sdp.Model.xref[])o.@xref;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write12_xref(@"xref", @"", ((global::Monobjc.Tools.Sdp.Model.xref)a[ia]), false, false);
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write11_result(string n, string ns, global::Monobjc.Tools.Sdp.Model.result o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.result)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"type", @"", FromXmlNmTokens(((global::System.String)o.@type1)));
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            {
                global::Monobjc.Tools.Sdp.Model.type[] a = (global::Monobjc.Tools.Sdp.Model.type[])o.@type;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write8_type(@"type", @"", ((global::Monobjc.Tools.Sdp.Model.type)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write10_parameter(string n, string ns, global::Monobjc.Tools.Sdp.Model.parameter o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.parameter)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"type", @"", FromXmlNmTokens(((global::System.String)o.@type1)));
            if (o.@optionalSpecified) {
                WriteAttribute(@"optional", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@optional)));
            }
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.type[] a = (global::Monobjc.Tools.Sdp.Model.type[])o.@type;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write8_type(@"type", @"", ((global::Monobjc.Tools.Sdp.Model.type)a[ia]), false, false);
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            if (o.@optionalSpecified) {
            }
            WriteEndElement(o);
        }

        void Write9_directparameter(string n, string ns, global::Monobjc.Tools.Sdp.Model.directparameter o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.directparameter)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"type", @"", FromXmlNmTokens(((global::System.String)o.@type1)));
            if (o.@optionalSpecified) {
                WriteAttribute(@"optional", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@optional)));
            }
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            {
                global::Monobjc.Tools.Sdp.Model.type[] a = (global::Monobjc.Tools.Sdp.Model.type[])o.@type;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write8_type(@"type", @"", ((global::Monobjc.Tools.Sdp.Model.type)a[ia]), false, false);
                    }
                }
            }
            if (o.@optionalSpecified) {
            }
            WriteEndElement(o);
        }

        void Write27_enumerator(string n, string ns, global::Monobjc.Tools.Sdp.Model.enumerator o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.enumerator)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.synonym[] a = (global::Monobjc.Tools.Sdp.Model.synonym[])o.@synonym;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write6_synonym(@"synonym", @"", ((global::Monobjc.Tools.Sdp.Model.synonym)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.documentation[] a = (global::Monobjc.Tools.Sdp.Model.documentation[])o.@documentation;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)a[ia]), false, false);
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write28_enumeration(string n, string ns, global::Monobjc.Tools.Sdp.Model.enumeration o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.enumeration)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            WriteAttribute(@"id", @"", ((global::System.String)o.@id));
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            WriteAttribute(@"inline", @"", ((global::System.String)o.@inline));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::System.Object[] a = (global::System.Object[])o.@Items;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        global::System.Object ai = (global::System.Object)a[ia];
                        if ((object)(ai) != null){
                            if (ai is global::Monobjc.Tools.Sdp.Model.enumerator) {
                                Write27_enumerator(@"enumerator", @"", ((global::Monobjc.Tools.Sdp.Model.enumerator)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.xref) {
                                Write12_xref(@"xref", @"", ((global::Monobjc.Tools.Sdp.Model.xref)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.documentation) {
                                Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)ai), false, false);
                            }
                            else  if ((object)(ai) != null){
                                throw CreateUnknownTypeException(ai);
                            }
                        }
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write26_command(string n, string ns, global::Monobjc.Tools.Sdp.Model.command o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.command)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            WriteAttribute(@"id", @"", ((global::System.String)o.@id));
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.synonym[] a = (global::Monobjc.Tools.Sdp.Model.synonym[])o.@synonym;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write6_synonym(@"synonym", @"", ((global::Monobjc.Tools.Sdp.Model.synonym)a[ia]), false, false);
                    }
                }
            }
            Write9_directparameter(@"direct-parameter", @"", ((global::Monobjc.Tools.Sdp.Model.directparameter)o.@directparameter), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.parameter[] a = (global::Monobjc.Tools.Sdp.Model.parameter[])o.@parameter;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write10_parameter(@"parameter", @"", ((global::Monobjc.Tools.Sdp.Model.parameter)a[ia]), false, false);
                    }
                }
            }
            Write11_result(@"result", @"", ((global::Monobjc.Tools.Sdp.Model.result)o.@result), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.documentation[] a = (global::Monobjc.Tools.Sdp.Model.documentation[])o.@documentation;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.xref[] a = (global::Monobjc.Tools.Sdp.Model.xref[])o.@xref;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write12_xref(@"xref", @"", ((global::Monobjc.Tools.Sdp.Model.xref)a[ia]), false, false);
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write24_classextension(string n, string ns, global::Monobjc.Tools.Sdp.Model.classextension o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.classextension)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"id", @"", ((global::System.String)o.@id));
            WriteAttribute(@"extends", @"", FromXmlNmTokens(((global::System.String)o.@extends)));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.contents[] a = (global::Monobjc.Tools.Sdp.Model.contents[])o.@contents;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write16_contents(@"contents", @"", ((global::Monobjc.Tools.Sdp.Model.contents)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.documentation[] a = (global::Monobjc.Tools.Sdp.Model.documentation[])o.@documentation;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.element[] a = (global::Monobjc.Tools.Sdp.Model.element[])o.@element;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write20_element(@"element", @"", ((global::Monobjc.Tools.Sdp.Model.element)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.property[] a = (global::Monobjc.Tools.Sdp.Model.property[])o.@property;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write22_property(@"property", @"", ((global::Monobjc.Tools.Sdp.Model.property)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.respondsto[] a = (global::Monobjc.Tools.Sdp.Model.respondsto[])o.@respondsto;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write23_respondsto(@"responds-to", @"", ((global::Monobjc.Tools.Sdp.Model.respondsto)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.synonym[] a = (global::Monobjc.Tools.Sdp.Model.synonym[])o.@synonym;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write6_synonym(@"synonym", @"", ((global::Monobjc.Tools.Sdp.Model.synonym)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.xref[] a = (global::Monobjc.Tools.Sdp.Model.xref[])o.@xref;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write12_xref(@"xref", @"", ((global::Monobjc.Tools.Sdp.Model.xref)a[ia]), false, false);
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write23_respondsto(string n, string ns, global::Monobjc.Tools.Sdp.Model.respondsto o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.respondsto)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"command", @"", FromXmlNmTokens(((global::System.String)o.@command)));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write20_element(string n, string ns, global::Monobjc.Tools.Sdp.Model.element o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.element)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"type", @"", FromXmlNmTokens(((global::System.String)o.@type)));
            if (((global::Monobjc.Tools.Sdp.Model.elementAccess)o.@access) != global::Monobjc.Tools.Sdp.Model.elementAccess.@rw) {
                WriteAttribute(@"access", @"", Write19_elementAccess(((global::Monobjc.Tools.Sdp.Model.elementAccess)o.@access)));
            }
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.accessor[] a = (global::Monobjc.Tools.Sdp.Model.accessor[])o.@accessor;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write18_accessor(@"accessor", @"", ((global::Monobjc.Tools.Sdp.Model.accessor)a[ia]), false, false);
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write18_accessor(string n, string ns, global::Monobjc.Tools.Sdp.Model.accessor o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.accessor)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"style", @"", Write17_accessortype(((global::Monobjc.Tools.Sdp.Model.accessortype)o.@style)));
            WriteEndElement(o);
        }

        string Write17_accessortype(global::Monobjc.Tools.Sdp.Model.accessortype v) {
            string s = null;
            switch (v) {
                case global::Monobjc.Tools.Sdp.Model.accessortype.@index: s = @"index"; break;
                case global::Monobjc.Tools.Sdp.Model.accessortype.@name: s = @"name"; break;
                case global::Monobjc.Tools.Sdp.Model.accessortype.@id: s = @"id"; break;
                case global::Monobjc.Tools.Sdp.Model.accessortype.@range: s = @"range"; break;
                case global::Monobjc.Tools.Sdp.Model.accessortype.@relative: s = @"relative"; break;
                case global::Monobjc.Tools.Sdp.Model.accessortype.@test: s = @"test"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Monobjc.Tools.Sdp.Model.accessortype");
            }
            return s;
        }

        string Write19_elementAccess(global::Monobjc.Tools.Sdp.Model.elementAccess v) {
            string s = null;
            switch (v) {
                case global::Monobjc.Tools.Sdp.Model.elementAccess.@r: s = @"r"; break;
                case global::Monobjc.Tools.Sdp.Model.elementAccess.@w: s = @"w"; break;
                case global::Monobjc.Tools.Sdp.Model.elementAccess.@rw: s = @"rw"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Monobjc.Tools.Sdp.Model.elementAccess");
            }
            return s;
        }

        void Write16_contents(string n, string ns, global::Monobjc.Tools.Sdp.Model.contents o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.contents)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            WriteAttribute(@"type", @"", FromXmlNmTokens(((global::System.String)o.@type1)));
            if (((global::Monobjc.Tools.Sdp.Model.contentsAccess)o.@access) != global::Monobjc.Tools.Sdp.Model.contentsAccess.@rw) {
                WriteAttribute(@"access", @"", Write15_contentsAccess(((global::Monobjc.Tools.Sdp.Model.contentsAccess)o.@access)));
            }
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.type[] a = (global::Monobjc.Tools.Sdp.Model.type[])o.@type;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write8_type(@"type", @"", ((global::Monobjc.Tools.Sdp.Model.type)a[ia]), false, false);
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        string Write15_contentsAccess(global::Monobjc.Tools.Sdp.Model.contentsAccess v) {
            string s = null;
            switch (v) {
                case global::Monobjc.Tools.Sdp.Model.contentsAccess.@r: s = @"r"; break;
                case global::Monobjc.Tools.Sdp.Model.contentsAccess.@w: s = @"w"; break;
                case global::Monobjc.Tools.Sdp.Model.contentsAccess.@rw: s = @"rw"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Monobjc.Tools.Sdp.Model.contentsAccess");
            }
            return s;
        }

        void Write25_class(string n, string ns, global::Monobjc.Tools.Sdp.Model.@class o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.@class)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", FromXmlNmTokens(((global::System.String)o.@name)));
            WriteAttribute(@"id", @"", ((global::System.String)o.@id));
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            WriteAttribute(@"plural", @"", FromXmlNmTokens(((global::System.String)o.@plural)));
            WriteAttribute(@"inherits", @"", FromXmlNmTokens(((global::System.String)o.@inherits)));
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::Monobjc.Tools.Sdp.Model.contents[] a = (global::Monobjc.Tools.Sdp.Model.contents[])o.@contents;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write16_contents(@"contents", @"", ((global::Monobjc.Tools.Sdp.Model.contents)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.element[] a = (global::Monobjc.Tools.Sdp.Model.element[])o.@element;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write20_element(@"element", @"", ((global::Monobjc.Tools.Sdp.Model.element)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.property[] a = (global::Monobjc.Tools.Sdp.Model.property[])o.@property;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write22_property(@"property", @"", ((global::Monobjc.Tools.Sdp.Model.property)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.respondsto[] a = (global::Monobjc.Tools.Sdp.Model.respondsto[])o.@respondsto;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write23_respondsto(@"responds-to", @"", ((global::Monobjc.Tools.Sdp.Model.respondsto)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.synonym[] a = (global::Monobjc.Tools.Sdp.Model.synonym[])o.@synonym;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write6_synonym(@"synonym", @"", ((global::Monobjc.Tools.Sdp.Model.synonym)a[ia]), false, false);
                    }
                }
            }
            {
                global::Monobjc.Tools.Sdp.Model.xref[] a = (global::Monobjc.Tools.Sdp.Model.xref[])o.@xref;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write12_xref(@"xref", @"", ((global::Monobjc.Tools.Sdp.Model.xref)a[ia]), false, false);
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write30_suite(string n, string ns, global::Monobjc.Tools.Sdp.Model.suite o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.suite)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"name", @"", ((global::System.String)o.@name));
            WriteAttribute(@"code", @"", ((global::System.String)o.@code));
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            if (o.@hiddenSpecified) {
                WriteAttribute(@"hidden", @"", Write3_yorn(((global::Monobjc.Tools.Sdp.Model.yorn)o.@hidden)));
            }
            Write5_cocoa(@"cocoa", @"", ((global::Monobjc.Tools.Sdp.Model.cocoa)o.@cocoa), false, false);
            {
                global::System.Object[] a = (global::System.Object[])o.@Items;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        global::System.Object ai = (global::System.Object)a[ia];
                        if ((object)(ai) != null){
                            if (ai is global::Monobjc.Tools.Sdp.Model.documentation) {
                                Write7_documentation(@"documentation", @"", ((global::Monobjc.Tools.Sdp.Model.documentation)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.command) {
                                Write26_command(@"command", @"", ((global::Monobjc.Tools.Sdp.Model.command)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.recordtype) {
                                Write29_recordtype(@"record-type", @"", ((global::Monobjc.Tools.Sdp.Model.recordtype)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.enumeration) {
                                Write28_enumeration(@"enumeration", @"", ((global::Monobjc.Tools.Sdp.Model.enumeration)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.valuetype) {
                                Write14_valuetype(@"value-type", @"", ((global::Monobjc.Tools.Sdp.Model.valuetype)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.@event) {
                                Write13_event(@"event", @"", ((global::Monobjc.Tools.Sdp.Model.@event)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.@class) {
                                Write25_class(@"class", @"", ((global::Monobjc.Tools.Sdp.Model.@class)ai), false, false);
                            }
                            else if (ai is global::Monobjc.Tools.Sdp.Model.classextension) {
                                Write24_classextension(@"class-extension", @"", ((global::Monobjc.Tools.Sdp.Model.classextension)ai), false, false);
                            }
                            else  if ((object)(ai) != null){
                                throw CreateUnknownTypeException(ai);
                            }
                        }
                    }
                }
            }
            if (o.@hiddenSpecified) {
            }
            WriteEndElement(o);
        }

        void Write31_dictionary(string n, string ns, global::Monobjc.Tools.Sdp.Model.dictionary o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Monobjc.Tools.Sdp.Model.dictionary)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"base", @"http://www.w3.org/XML/1998/namespace", ((global::System.String)o.@base));
            WriteAttribute(@"title", @"", ((global::System.String)o.@title));
            {
                global::Monobjc.Tools.Sdp.Model.suite[] a = (global::Monobjc.Tools.Sdp.Model.suite[])o.@suite;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write30_suite(@"suite", @"", ((global::Monobjc.Tools.Sdp.Model.suite)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        protected override void InitCallbacks() {
        }
    }

    public class XmlSerializationReader1 : System.Xml.Serialization.XmlSerializationReader {

        protected new string CollapseWhitespace(string value)
        {
            if (value == null)
                return null;
            return value.Trim();
        }

        public object Read34_dictionary()
        {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id3_dictionary && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read31_dictionary(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":dictionary");
            }
            return (object)o;
        }

        public object Read35_suite() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id4_suite && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read30_suite(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":suite");
            }
            return (object)o;
        }

        public object Read36_cocoa() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read5_cocoa(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":cocoa");
            }
            return (object)o;
        }

        public object Read37_yorn() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id6_yorn && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    {
                        o = Read3_yorn(Reader.ReadElementString());
                    }
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":yorn");
            }
            return (object)o;
        }

        public object Read38_cocoaBooleanvalue() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id7_cocoaBooleanvalue && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    {
                        o = Read4_cocoaBooleanvalue(Reader.ReadElementString());
                    }
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":cocoaBooleanvalue");
            }
            return (object)o;
        }

        public object Read39_class() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id8_class && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read25_class(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":class");
            }
            return (object)o;
        }

        public object Read40_contents() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id9_contents && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read16_contents(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":contents");
            }
            return (object)o;
        }

        public object Read41_type() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read8_type(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":type");
            }
            return (object)o;
        }

        public object Read42_contentsAccess() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id11_contentsAccess && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    {
                        o = Read15_contentsAccess(Reader.ReadElementString());
                    }
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":contentsAccess");
            }
            return (object)o;
        }

        public object Read43_documentation() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read7_documentation(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":documentation");
            }
            return (object)o;
        }

        public object Read44_element() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id13_element && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read20_element(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":element");
            }
            return (object)o;
        }

        public object Read45_accessor() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id14_accessor && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read18_accessor(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":accessor");
            }
            return (object)o;
        }

        public object Read46_accessortype() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id15_accessortype && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    {
                        o = Read17_accessortype(Reader.ReadElementString());
                    }
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":accessor-type");
            }
            return (object)o;
        }

        public object Read47_elementAccess() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id16_elementAccess && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    {
                        o = Read19_elementAccess(Reader.ReadElementString());
                    }
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":elementAccess");
            }
            return (object)o;
        }

        public object Read48_property() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id17_property && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read22_property(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":property");
            }
            return (object)o;
        }

        public object Read49_synonym() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id18_synonym && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read6_synonym(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":synonym");
            }
            return (object)o;
        }

        public object Read50_propertyAccess() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id19_propertyAccess && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    {
                        o = Read21_propertyAccess(Reader.ReadElementString());
                    }
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":propertyAccess");
            }
            return (object)o;
        }

        public object Read51_respondsto() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id20_respondsto && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read23_respondsto(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":responds-to");
            }
            return (object)o;
        }

        public object Read52_xref() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id21_xref && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read12_xref(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":xref");
            }
            return (object)o;
        }

        public object Read53_classextension() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id22_classextension && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read24_classextension(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":class-extension");
            }
            return (object)o;
        }

        public object Read54_command() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id23_command && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read26_command(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":command");
            }
            return (object)o;
        }

        public object Read55_directparameter() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id24_directparameter && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read9_directparameter(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":direct-parameter");
            }
            return (object)o;
        }

        public object Read56_parameter() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id25_parameter && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read10_parameter(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":parameter");
            }
            return (object)o;
        }

        public object Read57_result() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id26_result && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read11_result(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":result");
            }
            return (object)o;
        }

        public object Read58_enumeration() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id27_enumeration && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read28_enumeration(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":enumeration");
            }
            return (object)o;
        }

        public object Read59_enumerator() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id28_enumerator && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read27_enumerator(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":enumerator");
            }
            return (object)o;
        }

        public object Read60_event() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id29_event && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read13_event(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":event");
            }
            return (object)o;
        }

        public object Read61_recordtype() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id30_recordtype && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read29_recordtype(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":record-type");
            }
            return (object)o;
        }

        public object Read62_valuetype() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id31_valuetype && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read14_valuetype(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":value-type");
            }
            return (object)o;
        }

        global::Monobjc.Tools.Sdp.Model.valuetype Read14_valuetype(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.valuetype o;
            o = new global::Monobjc.Tools.Sdp.Model.valuetype();
            global::Monobjc.Tools.Sdp.Model.synonym[] a_1 = null;
            int ca_1 = 0;
            global::Monobjc.Tools.Sdp.Model.documentation[] a_2 = null;
            int ca_2 = 0;
            global::Monobjc.Tools.Sdp.Model.xref[] a_3 = null;
            int ca_3 = 0;
            bool[] paramsRead = new bool[11];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[4] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id36_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@id = CollapseWhitespace(Reader.Value);
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id39_plural && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@plural = ToXmlNmTokens(Reader.Value);
                    paramsRead[9] = true;
                }
                else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[10] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :id, :code, :hidden, :plural, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
                o.@documentation = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
                o.@xref = (global::Monobjc.Tools.Sdp.Model.xref[])ShrinkArray(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.xref), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations1 = 0;
            int readerCount1 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id18_synonym && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Monobjc.Tools.Sdp.Model.synonym[])EnsureArrayIndex(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym));a_1[ca_1++] = Read6_synonym(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_2 = (global::Monobjc.Tools.Sdp.Model.documentation[])EnsureArrayIndex(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation));a_2[ca_2++] = Read7_documentation(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id21_xref && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_3 = (global::Monobjc.Tools.Sdp.Model.xref[])EnsureArrayIndex(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.xref));a_3[ca_3++] = Read12_xref(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :synonym, :documentation, :xref");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :synonym, :documentation, :xref");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations1, ref readerCount1);
            }
            o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
            o.@documentation = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
            o.@xref = (global::Monobjc.Tools.Sdp.Model.xref[])ShrinkArray(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.xref), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.xref Read12_xref(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.xref o;
            o = new global::Monobjc.Tools.Sdp.Model.xref();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id41_target && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@target = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :target, :hidden");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations2 = 0;
            int readerCount2 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations2, ref readerCount2);
            }
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.yorn Read3_yorn(string s) {
            switch (s) {
                case @"yes": return global::Monobjc.Tools.Sdp.Model.yorn.@yes;
                case @"no": return global::Monobjc.Tools.Sdp.Model.yorn.@no;
                default: throw CreateUnknownConstantException(s, typeof(global::Monobjc.Tools.Sdp.Model.yorn));
            }
        }

        global::Monobjc.Tools.Sdp.Model.documentation Read7_documentation(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.documentation o;
            o = new global::Monobjc.Tools.Sdp.Model.documentation();
            global::System.String[] a_0 = null;
            int ca_0 = 0;
            bool[] paramsRead = new bool[1];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@html = (global::System.String[])ShrinkArray(a_0, ca_0, typeof(global::System.String), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations3 = 0;
            int readerCount3 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id42_html && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        {
                            a_0 = (global::System.String[])EnsureArrayIndex(a_0, ca_0, typeof(global::System.String));a_0[ca_0++] = Reader.ReadElementString();
                        }
                    }
                    else {
                        UnknownNode((object)o, @":html");
                    }
                }
                else {
                    UnknownNode((object)o, @":html");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations3, ref readerCount3);
            }
            o.@html = (global::System.String[])ShrinkArray(a_0, ca_0, typeof(global::System.String), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.synonym Read6_synonym(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.synonym o;
            o = new global::Monobjc.Tools.Sdp.Model.synonym();
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[4] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :code, :hidden");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations4 = 0;
            int readerCount4 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else {
                        UnknownNode((object)o, @":cocoa");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations4, ref readerCount4);
            }
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.cocoa Read5_cocoa(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.cocoa o;
            o = new global::Monobjc.Tools.Sdp.Model.cocoa();
            bool[] paramsRead = new bool[9];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmToken(Reader.Value);
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id8_class && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@class = ToXmlNmToken(Reader.Value);
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id43_key && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@key = ToXmlNmToken(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id44_method && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@method = ToXmlNmToken(Reader.Value);
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id45_insertatbeginning && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@insertatbeginning = Read3_yorn(Reader.Value);
                    o.@insertatbeginningSpecified = true;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id46_booleanvalue && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@booleanvalue = Read4_cocoaBooleanvalue(Reader.Value);
                    o.@booleanvalueSpecified = true;
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id47_integervalue && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@integervalue = ToXmlNmToken(Reader.Value);
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id48_stringvalue && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@stringvalue = Reader.Value;
                    paramsRead[8] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :class, :key, :method, :insert-at-beginning, :boolean-value, :integer-value, :string-value");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations5 = 0;
            int readerCount5 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations5, ref readerCount5);
            }
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.cocoaBooleanvalue Read4_cocoaBooleanvalue(string s) {
            switch (s) {
                case @"YES": return global::Monobjc.Tools.Sdp.Model.cocoaBooleanvalue.@YES;
                case @"NO": return global::Monobjc.Tools.Sdp.Model.cocoaBooleanvalue.@NO;
                default: throw CreateUnknownConstantException(s, typeof(global::Monobjc.Tools.Sdp.Model.cocoaBooleanvalue));
            }
        }

        global::Monobjc.Tools.Sdp.Model.recordtype Read29_recordtype(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.recordtype o;
            o = new global::Monobjc.Tools.Sdp.Model.recordtype();
            global::Monobjc.Tools.Sdp.Model.synonym[] a_1 = null;
            int ca_1 = 0;
            global::System.Object[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[10];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[3] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id36_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@id = CollapseWhitespace(Reader.Value);
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id39_plural && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@plural = ToXmlNmTokens(Reader.Value);
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[9] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :id, :code, :hidden, :plural, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
                o.@Items = (global::System.Object[])ShrinkArray(a_2, ca_2, typeof(global::System.Object), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations6 = 0;
            int readerCount6 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id18_synonym && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Monobjc.Tools.Sdp.Model.synonym[])EnsureArrayIndex(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym));a_1[ca_1++] = Read6_synonym(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id21_xref && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_2 = (global::System.Object[])EnsureArrayIndex(a_2, ca_2, typeof(global::System.Object));a_2[ca_2++] = Read12_xref(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id17_property && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_2 = (global::System.Object[])EnsureArrayIndex(a_2, ca_2, typeof(global::System.Object));a_2[ca_2++] = Read22_property(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_2 = (global::System.Object[])EnsureArrayIndex(a_2, ca_2, typeof(global::System.Object));a_2[ca_2++] = Read7_documentation(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :synonym, :xref, :property, :documentation");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :synonym, :xref, :property, :documentation");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations6, ref readerCount6);
            }
            o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
            o.@Items = (global::System.Object[])ShrinkArray(a_2, ca_2, typeof(global::System.Object), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.property Read22_property(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.property o;
            o = new global::Monobjc.Tools.Sdp.Model.property();
            global::System.Object[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[10];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@type = ToXmlNmTokens(Reader.Value);
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id49_access && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@access = Read21_propertyAccess(Reader.Value);
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id50_inproperties && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@inproperties = Read3_yorn(Reader.Value);
                    o.@inpropertiesSpecified = true;
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[9] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :code, :hidden, :type, :access, :in-properties, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@Items = (global::System.Object[])ShrinkArray(a_1, ca_1, typeof(global::System.Object), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations7 = 0;
            int readerCount7 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read7_documentation(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read8_type(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id18_synonym && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read6_synonym(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :documentation, :type, :synonym");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :documentation, :type, :synonym");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations7, ref readerCount7);
            }
            o.@Items = (global::System.Object[])ShrinkArray(a_1, ca_1, typeof(global::System.Object), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.type Read8_type(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.type o;
            o = new global::Monobjc.Tools.Sdp.Model.type();
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@type1 = ToXmlNmTokens(Reader.Value);
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id51_list && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@list = Read3_yorn(Reader.Value);
                    o.@listSpecified = true;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :type, :list, :hidden");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations8 = 0;
            int readerCount8 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations8, ref readerCount8);
            }
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.propertyAccess Read21_propertyAccess(string s) {
            switch (s) {
                case @"r": return global::Monobjc.Tools.Sdp.Model.propertyAccess.@r;
                case @"w": return global::Monobjc.Tools.Sdp.Model.propertyAccess.@w;
                case @"rw": return global::Monobjc.Tools.Sdp.Model.propertyAccess.@rw;
                default: throw CreateUnknownConstantException(s, typeof(global::Monobjc.Tools.Sdp.Model.propertyAccess));
            }
        }

        global::Monobjc.Tools.Sdp.Model.@event Read13_event(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.@event o;
            o = new global::Monobjc.Tools.Sdp.Model.@event();
            global::Monobjc.Tools.Sdp.Model.synonym[] a_1 = null;
            int ca_1 = 0;
            global::Monobjc.Tools.Sdp.Model.documentation[] a_2 = null;
            int ca_2 = 0;
            global::System.Object[] a_4 = null;
            int ca_4 = 0;
            global::Monobjc.Tools.Sdp.Model.documentation[] a_6 = null;
            int ca_6 = 0;
            global::Monobjc.Tools.Sdp.Model.xref[] a_7 = null;
            int ca_7 = 0;
            bool[] paramsRead = new bool[14];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[8] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[9] = true;
                }
                else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[10] = true;
                }
                else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[11] = true;
                }
                else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[12] = true;
                }
                else if (!paramsRead[13] && ((object) Reader.LocalName == (object)id36_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@id = CollapseWhitespace(Reader.Value);
                    paramsRead[13] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":code, :description, :hidden, http://www.w3.org/XML/1998/namespace, :name, :id");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
                o.@documentation = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
                o.@Items = (global::System.Object[])ShrinkArray(a_4, ca_4, typeof(global::System.Object), true);
                o.@documentation1 = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
                o.@xref = (global::Monobjc.Tools.Sdp.Model.xref[])ShrinkArray(a_7, ca_7, typeof(global::Monobjc.Tools.Sdp.Model.xref), true);
                return o;
            }
            Reader.ReadStartElement();
            int state = 0;
            Reader.MoveToContent();
            int whileIterations9 = 0;
            int readerCount9 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    switch (state) {
                    case 0:
                        if (((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                            o.@cocoa = Read5_cocoa(false, true);
                        }
                        state = 1;
                        break;
                    case 1:
                        if (((object) Reader.LocalName == (object)id18_synonym && (object) Reader.NamespaceURI == (object)id2_Item)) {
                            a_1 = (global::Monobjc.Tools.Sdp.Model.synonym[])EnsureArrayIndex(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym));a_1[ca_1++] = Read6_synonym(false, true);
                        }
                        else {
                            state = 2;
                        }
                        break;
                    case 2:
                        if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                            a_2 = (global::Monobjc.Tools.Sdp.Model.documentation[])EnsureArrayIndex(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation));a_2[ca_2++] = Read7_documentation(false, true);
                        }
                        else {
                            state = 3;
                        }
                        break;
                    case 3:
                        if (((object) Reader.LocalName == (object)id24_directparameter && (object) Reader.NamespaceURI == (object)id2_Item)) {
                            o.@directparameter = Read9_directparameter(false, true);
                        }
                        state = 4;
                        break;
                    case 4:
                        if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                            a_4 = (global::System.Object[])EnsureArrayIndex(a_4, ca_4, typeof(global::System.Object));a_4[ca_4++] = Read7_documentation(false, true);
                        }
                        else if (((object) Reader.LocalName == (object)id25_parameter && (object) Reader.NamespaceURI == (object)id2_Item)) {
                            a_4 = (global::System.Object[])EnsureArrayIndex(a_4, ca_4, typeof(global::System.Object));a_4[ca_4++] = Read10_parameter(false, true);
                        }
                        else {
                            state = 5;
                        }
                        break;
                    case 5:
                        if (((object) Reader.LocalName == (object)id26_result && (object) Reader.NamespaceURI == (object)id2_Item)) {
                            o.@result = Read11_result(false, true);
                        }
                        state = 6;
                        break;
                    case 6:
                        if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                            a_6 = (global::Monobjc.Tools.Sdp.Model.documentation[])EnsureArrayIndex(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.documentation));a_6[ca_6++] = Read7_documentation(false, true);
                        }
                        else {
                            state = 7;
                        }
                        break;
                    case 7:
                        if (((object) Reader.LocalName == (object)id21_xref && (object) Reader.NamespaceURI == (object)id2_Item)) {
                            a_7 = (global::Monobjc.Tools.Sdp.Model.xref[])EnsureArrayIndex(a_7, ca_7, typeof(global::Monobjc.Tools.Sdp.Model.xref));a_7[ca_7++] = Read12_xref(false, true);
                        }
                        else {
                            state = 8;
                        }
                        break;
                    default:
                        UnknownNode((object)o, null);
                        break;
                    }
                }
                else {
                    UnknownNode((object)o, null);
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations9, ref readerCount9);
            }
            o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
            o.@documentation = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
            o.@Items = (global::System.Object[])ShrinkArray(a_4, ca_4, typeof(global::System.Object), true);
            o.@documentation1 = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
            o.@xref = (global::Monobjc.Tools.Sdp.Model.xref[])ShrinkArray(a_7, ca_7, typeof(global::Monobjc.Tools.Sdp.Model.xref), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.result Read11_result(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.result o;
            o = new global::Monobjc.Tools.Sdp.Model.result();
            global::Monobjc.Tools.Sdp.Model.type[] a_0 = null;
            int ca_0 = 0;
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@type1 = ToXmlNmTokens(Reader.Value);
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :type, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@type = (global::Monobjc.Tools.Sdp.Model.type[])ShrinkArray(a_0, ca_0, typeof(global::Monobjc.Tools.Sdp.Model.type), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations10 = 0;
            int readerCount10 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Monobjc.Tools.Sdp.Model.type[])EnsureArrayIndex(a_0, ca_0, typeof(global::Monobjc.Tools.Sdp.Model.type));a_0[ca_0++] = Read8_type(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":type");
                    }
                }
                else {
                    UnknownNode((object)o, @":type");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations10, ref readerCount10);
            }
            o.@type = (global::Monobjc.Tools.Sdp.Model.type[])ShrinkArray(a_0, ca_0, typeof(global::Monobjc.Tools.Sdp.Model.type), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.parameter Read10_parameter(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.parameter o;
            o = new global::Monobjc.Tools.Sdp.Model.parameter();
            global::Monobjc.Tools.Sdp.Model.type[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[9];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@type1 = ToXmlNmTokens(Reader.Value);
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id52_optional && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@optional = Read3_yorn(Reader.Value);
                    o.@optionalSpecified = true;
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[8] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :code, :hidden, :type, :optional, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@type = (global::Monobjc.Tools.Sdp.Model.type[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.type), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations11 = 0;
            int readerCount11 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Monobjc.Tools.Sdp.Model.type[])EnsureArrayIndex(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.type));a_1[ca_1++] = Read8_type(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :type");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :type");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations11, ref readerCount11);
            }
            o.@type = (global::Monobjc.Tools.Sdp.Model.type[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.type), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.directparameter Read9_directparameter(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.directparameter o;
            o = new global::Monobjc.Tools.Sdp.Model.directparameter();
            global::Monobjc.Tools.Sdp.Model.type[] a_0 = null;
            int ca_0 = 0;
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@type1 = ToXmlNmTokens(Reader.Value);
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id52_optional && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@optional = Read3_yorn(Reader.Value);
                    o.@optionalSpecified = true;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :type, :optional, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@type = (global::Monobjc.Tools.Sdp.Model.type[])ShrinkArray(a_0, ca_0, typeof(global::Monobjc.Tools.Sdp.Model.type), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations12 = 0;
            int readerCount12 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Monobjc.Tools.Sdp.Model.type[])EnsureArrayIndex(a_0, ca_0, typeof(global::Monobjc.Tools.Sdp.Model.type));a_0[ca_0++] = Read8_type(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":type");
                    }
                }
                else {
                    UnknownNode((object)o, @":type");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations12, ref readerCount12);
            }
            o.@type = (global::Monobjc.Tools.Sdp.Model.type[])ShrinkArray(a_0, ca_0, typeof(global::Monobjc.Tools.Sdp.Model.type), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.enumerator Read27_enumerator(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.enumerator o;
            o = new global::Monobjc.Tools.Sdp.Model.enumerator();
            global::Monobjc.Tools.Sdp.Model.synonym[] a_1 = null;
            int ca_1 = 0;
            global::Monobjc.Tools.Sdp.Model.documentation[] a_2 = null;
            int ca_2 = 0;
            bool[] paramsRead = new bool[8];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[3] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :code, :hidden, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
                o.@documentation = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations13 = 0;
            int readerCount13 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id18_synonym && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Monobjc.Tools.Sdp.Model.synonym[])EnsureArrayIndex(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym));a_1[ca_1++] = Read6_synonym(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_2 = (global::Monobjc.Tools.Sdp.Model.documentation[])EnsureArrayIndex(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation));a_2[ca_2++] = Read7_documentation(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :synonym, :documentation");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :synonym, :documentation");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations13, ref readerCount13);
            }
            o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
            o.@documentation = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.enumeration Read28_enumeration(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.enumeration o;
            o = new global::Monobjc.Tools.Sdp.Model.enumeration();
            global::System.Object[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[9];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id36_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@id = CollapseWhitespace(Reader.Value);
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id53_inline && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@inline = Reader.Value;
                    paramsRead[8] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :id, :code, :hidden, :description, :inline");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@Items = (global::System.Object[])ShrinkArray(a_1, ca_1, typeof(global::System.Object), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations14 = 0;
            int readerCount14 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read7_documentation(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id21_xref && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read12_xref(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id28_enumerator && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read27_enumerator(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :documentation, :xref, :enumerator");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :documentation, :xref, :enumerator");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations14, ref readerCount14);
            }
            o.@Items = (global::System.Object[])ShrinkArray(a_1, ca_1, typeof(global::System.Object), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.command Read26_command(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.command o;
            o = new global::Monobjc.Tools.Sdp.Model.command();
            global::Monobjc.Tools.Sdp.Model.synonym[] a_1 = null;
            int ca_1 = 0;
            global::Monobjc.Tools.Sdp.Model.parameter[] a_3 = null;
            int ca_3 = 0;
            global::Monobjc.Tools.Sdp.Model.documentation[] a_5 = null;
            int ca_5 = 0;
            global::Monobjc.Tools.Sdp.Model.xref[] a_6 = null;
            int ca_6 = 0;
            bool[] paramsRead = new bool[13];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[7] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id36_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@id = CollapseWhitespace(Reader.Value);
                    paramsRead[9] = true;
                }
                else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[10] = true;
                }
                else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[11] = true;
                }
                else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[12] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :id, :code, :description, :hidden");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
                o.@parameter = (global::Monobjc.Tools.Sdp.Model.parameter[])ShrinkArray(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.parameter), true);
                o.@documentation = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_5, ca_5, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
                o.@xref = (global::Monobjc.Tools.Sdp.Model.xref[])ShrinkArray(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.xref), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations15 = 0;
            int readerCount15 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id18_synonym && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Monobjc.Tools.Sdp.Model.synonym[])EnsureArrayIndex(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym));a_1[ca_1++] = Read6_synonym(false, true);
                    }
                    else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id24_directparameter && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@directparameter = Read9_directparameter(false, true);
                        paramsRead[2] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id25_parameter && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_3 = (global::Monobjc.Tools.Sdp.Model.parameter[])EnsureArrayIndex(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.parameter));a_3[ca_3++] = Read10_parameter(false, true);
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id26_result && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@result = Read11_result(false, true);
                        paramsRead[4] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_5 = (global::Monobjc.Tools.Sdp.Model.documentation[])EnsureArrayIndex(a_5, ca_5, typeof(global::Monobjc.Tools.Sdp.Model.documentation));a_5[ca_5++] = Read7_documentation(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id21_xref && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_6 = (global::Monobjc.Tools.Sdp.Model.xref[])EnsureArrayIndex(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.xref));a_6[ca_6++] = Read12_xref(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :synonym, :direct-parameter, :parameter, :result, :documentation, :xref");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :synonym, :direct-parameter, :parameter, :result, :documentation, :xref");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations15, ref readerCount15);
            }
            o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
            o.@parameter = (global::Monobjc.Tools.Sdp.Model.parameter[])ShrinkArray(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.parameter), true);
            o.@documentation = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_5, ca_5, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
            o.@xref = (global::Monobjc.Tools.Sdp.Model.xref[])ShrinkArray(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.xref), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.classextension Read24_classextension(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.classextension o;
            o = new global::Monobjc.Tools.Sdp.Model.classextension();
            global::Monobjc.Tools.Sdp.Model.contents[] a_1 = null;
            int ca_1 = 0;
            global::Monobjc.Tools.Sdp.Model.documentation[] a_2 = null;
            int ca_2 = 0;
            global::Monobjc.Tools.Sdp.Model.element[] a_3 = null;
            int ca_3 = 0;
            global::Monobjc.Tools.Sdp.Model.property[] a_4 = null;
            int ca_4 = 0;
            global::Monobjc.Tools.Sdp.Model.respondsto[] a_5 = null;
            int ca_5 = 0;
            global::Monobjc.Tools.Sdp.Model.synonym[] a_6 = null;
            int ca_6 = 0;
            global::Monobjc.Tools.Sdp.Model.xref[] a_7 = null;
            int ca_7 = 0;
            bool[] paramsRead = new bool[13];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[8] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id36_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@id = CollapseWhitespace(Reader.Value);
                    paramsRead[9] = true;
                }
                else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id54_extends && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@extends = ToXmlNmTokens(Reader.Value);
                    paramsRead[10] = true;
                }
                else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[11] = true;
                }
                else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[12] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :id, :extends, :hidden, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@contents = (global::Monobjc.Tools.Sdp.Model.contents[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.contents), true);
                o.@documentation = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
                o.@element = (global::Monobjc.Tools.Sdp.Model.element[])ShrinkArray(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.element), true);
                o.@property = (global::Monobjc.Tools.Sdp.Model.property[])ShrinkArray(a_4, ca_4, typeof(global::Monobjc.Tools.Sdp.Model.property), true);
                o.@respondsto = (global::Monobjc.Tools.Sdp.Model.respondsto[])ShrinkArray(a_5, ca_5, typeof(global::Monobjc.Tools.Sdp.Model.respondsto), true);
                o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
                o.@xref = (global::Monobjc.Tools.Sdp.Model.xref[])ShrinkArray(a_7, ca_7, typeof(global::Monobjc.Tools.Sdp.Model.xref), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations16 = 0;
            int readerCount16 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id9_contents && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Monobjc.Tools.Sdp.Model.contents[])EnsureArrayIndex(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.contents));a_1[ca_1++] = Read16_contents(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_2 = (global::Monobjc.Tools.Sdp.Model.documentation[])EnsureArrayIndex(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation));a_2[ca_2++] = Read7_documentation(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id13_element && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_3 = (global::Monobjc.Tools.Sdp.Model.element[])EnsureArrayIndex(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.element));a_3[ca_3++] = Read20_element(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id17_property && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_4 = (global::Monobjc.Tools.Sdp.Model.property[])EnsureArrayIndex(a_4, ca_4, typeof(global::Monobjc.Tools.Sdp.Model.property));a_4[ca_4++] = Read22_property(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id20_respondsto && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_5 = (global::Monobjc.Tools.Sdp.Model.respondsto[])EnsureArrayIndex(a_5, ca_5, typeof(global::Monobjc.Tools.Sdp.Model.respondsto));a_5[ca_5++] = Read23_respondsto(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id18_synonym && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_6 = (global::Monobjc.Tools.Sdp.Model.synonym[])EnsureArrayIndex(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.synonym));a_6[ca_6++] = Read6_synonym(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id21_xref && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_7 = (global::Monobjc.Tools.Sdp.Model.xref[])EnsureArrayIndex(a_7, ca_7, typeof(global::Monobjc.Tools.Sdp.Model.xref));a_7[ca_7++] = Read12_xref(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :contents, :documentation, :element, :property, :responds-to, :synonym, :xref");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :contents, :documentation, :element, :property, :responds-to, :synonym, :xref");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations16, ref readerCount16);
            }
            o.@contents = (global::Monobjc.Tools.Sdp.Model.contents[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.contents), true);
            o.@documentation = (global::Monobjc.Tools.Sdp.Model.documentation[])ShrinkArray(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.documentation), true);
            o.@element = (global::Monobjc.Tools.Sdp.Model.element[])ShrinkArray(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.element), true);
            o.@property = (global::Monobjc.Tools.Sdp.Model.property[])ShrinkArray(a_4, ca_4, typeof(global::Monobjc.Tools.Sdp.Model.property), true);
            o.@respondsto = (global::Monobjc.Tools.Sdp.Model.respondsto[])ShrinkArray(a_5, ca_5, typeof(global::Monobjc.Tools.Sdp.Model.respondsto), true);
            o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
            o.@xref = (global::Monobjc.Tools.Sdp.Model.xref[])ShrinkArray(a_7, ca_7, typeof(global::Monobjc.Tools.Sdp.Model.xref), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.respondsto Read23_respondsto(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.respondsto o;
            o = new global::Monobjc.Tools.Sdp.Model.respondsto();
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id23_command && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@command = ToXmlNmTokens(Reader.Value);
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[4] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :command, :hidden, :name");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations17 = 0;
            int readerCount17 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else {
                        UnknownNode((object)o, @":cocoa");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations17, ref readerCount17);
            }
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.element Read20_element(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.element o;
            o = new global::Monobjc.Tools.Sdp.Model.element();
            global::Monobjc.Tools.Sdp.Model.accessor[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[7];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@type = ToXmlNmTokens(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id49_access && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@access = Read19_elementAccess(Reader.Value);
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[6] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :type, :access, :hidden, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@accessor = (global::Monobjc.Tools.Sdp.Model.accessor[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.accessor), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations18 = 0;
            int readerCount18 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id14_accessor && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Monobjc.Tools.Sdp.Model.accessor[])EnsureArrayIndex(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.accessor));a_1[ca_1++] = Read18_accessor(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :accessor");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :accessor");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations18, ref readerCount18);
            }
            o.@accessor = (global::Monobjc.Tools.Sdp.Model.accessor[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.accessor), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.accessor Read18_accessor(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.accessor o;
            o = new global::Monobjc.Tools.Sdp.Model.accessor();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id55_style && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@style = Read17_accessortype(Reader.Value);
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :style");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations19 = 0;
            int readerCount19 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations19, ref readerCount19);
            }
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.accessortype Read17_accessortype(string s) {
            switch (s) {
                case @"index": return global::Monobjc.Tools.Sdp.Model.accessortype.@index;
                case @"name": return global::Monobjc.Tools.Sdp.Model.accessortype.@name;
                case @"id": return global::Monobjc.Tools.Sdp.Model.accessortype.@id;
                case @"range": return global::Monobjc.Tools.Sdp.Model.accessortype.@range;
                case @"relative": return global::Monobjc.Tools.Sdp.Model.accessortype.@relative;
                case @"test": return global::Monobjc.Tools.Sdp.Model.accessortype.@test;
                default: throw CreateUnknownConstantException(s, typeof(global::Monobjc.Tools.Sdp.Model.accessortype));
            }
        }

        global::Monobjc.Tools.Sdp.Model.elementAccess Read19_elementAccess(string s) {
            switch (s) {
                case @"r": return global::Monobjc.Tools.Sdp.Model.elementAccess.@r;
                case @"w": return global::Monobjc.Tools.Sdp.Model.elementAccess.@w;
                case @"rw": return global::Monobjc.Tools.Sdp.Model.elementAccess.@rw;
                default: throw CreateUnknownConstantException(s, typeof(global::Monobjc.Tools.Sdp.Model.elementAccess));
            }
        }

        global::Monobjc.Tools.Sdp.Model.contents Read16_contents(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.contents o;
            o = new global::Monobjc.Tools.Sdp.Model.contents();
            global::Monobjc.Tools.Sdp.Model.type[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[9];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@type1 = ToXmlNmTokens(Reader.Value);
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id49_access && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@access = Read15_contentsAccess(Reader.Value);
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[8] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :code, :type, :access, :hidden, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@type = (global::Monobjc.Tools.Sdp.Model.type[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.type), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations20 = 0;
            int readerCount20 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id10_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Monobjc.Tools.Sdp.Model.type[])EnsureArrayIndex(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.type));a_1[ca_1++] = Read8_type(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :type");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :type");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations20, ref readerCount20);
            }
            o.@type = (global::Monobjc.Tools.Sdp.Model.type[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.type), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.contentsAccess Read15_contentsAccess(string s) {
            switch (s) {
                case @"r": return global::Monobjc.Tools.Sdp.Model.contentsAccess.@r;
                case @"w": return global::Monobjc.Tools.Sdp.Model.contentsAccess.@w;
                case @"rw": return global::Monobjc.Tools.Sdp.Model.contentsAccess.@rw;
                default: throw CreateUnknownConstantException(s, typeof(global::Monobjc.Tools.Sdp.Model.contentsAccess));
            }
        }

        global::Monobjc.Tools.Sdp.Model.@class Read25_class(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.@class o;
            o = new global::Monobjc.Tools.Sdp.Model.@class();
            global::Monobjc.Tools.Sdp.Model.contents[] a_1 = null;
            int ca_1 = 0;
            global::Monobjc.Tools.Sdp.Model.element[] a_2 = null;
            int ca_2 = 0;
            global::Monobjc.Tools.Sdp.Model.property[] a_3 = null;
            int ca_3 = 0;
            global::Monobjc.Tools.Sdp.Model.respondsto[] a_4 = null;
            int ca_4 = 0;
            global::Monobjc.Tools.Sdp.Model.synonym[] a_5 = null;
            int ca_5 = 0;
            global::Monobjc.Tools.Sdp.Model.xref[] a_6 = null;
            int ca_6 = 0;
            bool[] paramsRead = new bool[15];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[7] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = ToXmlNmTokens(Reader.Value);
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id36_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@id = CollapseWhitespace(Reader.Value);
                    paramsRead[9] = true;
                }
                else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[10] = true;
                }
                else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[11] = true;
                }
                else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id39_plural && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@plural = ToXmlNmTokens(Reader.Value);
                    paramsRead[12] = true;
                }
                else if (!paramsRead[13] && ((object) Reader.LocalName == (object)id56_inherits && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@inherits = ToXmlNmTokens(Reader.Value);
                    paramsRead[13] = true;
                }
                else if (!paramsRead[14] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[14] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :id, :code, :hidden, :plural, :inherits, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@contents = (global::Monobjc.Tools.Sdp.Model.contents[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.contents), true);
                o.@element = (global::Monobjc.Tools.Sdp.Model.element[])ShrinkArray(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.element), true);
                o.@property = (global::Monobjc.Tools.Sdp.Model.property[])ShrinkArray(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.property), true);
                o.@respondsto = (global::Monobjc.Tools.Sdp.Model.respondsto[])ShrinkArray(a_4, ca_4, typeof(global::Monobjc.Tools.Sdp.Model.respondsto), true);
                o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_5, ca_5, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
                o.@xref = (global::Monobjc.Tools.Sdp.Model.xref[])ShrinkArray(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.xref), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations21 = 0;
            int readerCount21 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id9_contents && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::Monobjc.Tools.Sdp.Model.contents[])EnsureArrayIndex(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.contents));a_1[ca_1++] = Read16_contents(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id13_element && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_2 = (global::Monobjc.Tools.Sdp.Model.element[])EnsureArrayIndex(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.element));a_2[ca_2++] = Read20_element(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id17_property && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_3 = (global::Monobjc.Tools.Sdp.Model.property[])EnsureArrayIndex(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.property));a_3[ca_3++] = Read22_property(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id20_respondsto && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_4 = (global::Monobjc.Tools.Sdp.Model.respondsto[])EnsureArrayIndex(a_4, ca_4, typeof(global::Monobjc.Tools.Sdp.Model.respondsto));a_4[ca_4++] = Read23_respondsto(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id18_synonym && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_5 = (global::Monobjc.Tools.Sdp.Model.synonym[])EnsureArrayIndex(a_5, ca_5, typeof(global::Monobjc.Tools.Sdp.Model.synonym));a_5[ca_5++] = Read6_synonym(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id21_xref && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_6 = (global::Monobjc.Tools.Sdp.Model.xref[])EnsureArrayIndex(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.xref));a_6[ca_6++] = Read12_xref(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :contents, :element, :property, :responds-to, :synonym, :xref");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :contents, :element, :property, :responds-to, :synonym, :xref");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations21, ref readerCount21);
            }
            o.@contents = (global::Monobjc.Tools.Sdp.Model.contents[])ShrinkArray(a_1, ca_1, typeof(global::Monobjc.Tools.Sdp.Model.contents), true);
            o.@element = (global::Monobjc.Tools.Sdp.Model.element[])ShrinkArray(a_2, ca_2, typeof(global::Monobjc.Tools.Sdp.Model.element), true);
            o.@property = (global::Monobjc.Tools.Sdp.Model.property[])ShrinkArray(a_3, ca_3, typeof(global::Monobjc.Tools.Sdp.Model.property), true);
            o.@respondsto = (global::Monobjc.Tools.Sdp.Model.respondsto[])ShrinkArray(a_4, ca_4, typeof(global::Monobjc.Tools.Sdp.Model.respondsto), true);
            o.@synonym = (global::Monobjc.Tools.Sdp.Model.synonym[])ShrinkArray(a_5, ca_5, typeof(global::Monobjc.Tools.Sdp.Model.synonym), true);
            o.@xref = (global::Monobjc.Tools.Sdp.Model.xref[])ShrinkArray(a_6, ca_6, typeof(global::Monobjc.Tools.Sdp.Model.xref), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.suite Read30_suite(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.suite o;
            o = new global::Monobjc.Tools.Sdp.Model.suite();
            global::System.Object[] a_1 = null;
            int ca_1 = 0;
            bool[] paramsRead = new bool[7];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[2] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id35_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id37_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@code = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id40_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id38_hidden && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@hidden = Read3_yorn(Reader.Value);
                    o.@hiddenSpecified = true;
                    paramsRead[6] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :name, :code, :description, :hidden");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@Items = (global::System.Object[])ShrinkArray(a_1, ca_1, typeof(global::System.Object), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations22 = 0;
            int readerCount22 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id5_cocoa && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        o.@cocoa = Read5_cocoa(false, true);
                        paramsRead[0] = true;
                    }
                    else if (((object) Reader.LocalName == (object)id29_event && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read13_event(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id31_valuetype && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read14_valuetype(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id22_classextension && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read24_classextension(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id8_class && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read25_class(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id23_command && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read26_command(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id12_documentation && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read7_documentation(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id27_enumeration && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read28_enumeration(false, true);
                    }
                    else if (((object) Reader.LocalName == (object)id30_recordtype && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_1 = (global::System.Object[])EnsureArrayIndex(a_1, ca_1, typeof(global::System.Object));a_1[ca_1++] = Read29_recordtype(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":cocoa, :event, :value-type, :class-extension, :class, :command, :documentation, :enumeration, :record-type");
                    }
                }
                else {
                    UnknownNode((object)o, @":cocoa, :event, :value-type, :class-extension, :class, :command, :documentation, :enumeration, :record-type");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations22, ref readerCount22);
            }
            o.@Items = (global::System.Object[])ShrinkArray(a_1, ca_1, typeof(global::System.Object), true);
            ReadEndElement();
            return o;
        }

        global::Monobjc.Tools.Sdp.Model.dictionary Read31_dictionary(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Monobjc.Tools.Sdp.Model.dictionary o;
            o = new global::Monobjc.Tools.Sdp.Model.dictionary();
            global::Monobjc.Tools.Sdp.Model.suite[] a_0 = null;
            int ca_0 = 0;
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id33_base && (object) Reader.NamespaceURI == (object)id34_Item)) {
                    o.@base = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id57_title && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@title = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @"http://www.w3.org/XML/1998/namespace, :title");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@suite = (global::Monobjc.Tools.Sdp.Model.suite[])ShrinkArray(a_0, ca_0, typeof(global::Monobjc.Tools.Sdp.Model.suite), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations23 = 0;
            int readerCount23 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id4_suite && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Monobjc.Tools.Sdp.Model.suite[])EnsureArrayIndex(a_0, ca_0, typeof(global::Monobjc.Tools.Sdp.Model.suite));a_0[ca_0++] = Read30_suite(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":suite");
                    }
                }
                else {
                    UnknownNode((object)o, @":suite");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations23, ref readerCount23);
            }
            o.@suite = (global::Monobjc.Tools.Sdp.Model.suite[])ShrinkArray(a_0, ca_0, typeof(global::Monobjc.Tools.Sdp.Model.suite), true);
            ReadEndElement();
            return o;
        }

        protected override void InitCallbacks() {
        }

        string id16_elementAccess;
        string id48_stringvalue;
        string id3_dictionary;
        string id10_type;
        string id24_directparameter;
        string id34_Item;
        string id55_style;
        string id22_classextension;
        string id33_base;
        string id13_element;
        string id21_xref;
        string id47_integervalue;
        string id41_target;
        string id12_documentation;
        string id7_cocoaBooleanvalue;
        string id32_CSharpGenerator;
        string id49_access;
        string id39_plural;
        string id18_synonym;
        string id17_property;
        string id26_result;
        string id29_event;
        string id2_Item;
        string id43_key;
        string id50_inproperties;
        string id51_list;
        string id4_suite;
        string id11_contentsAccess;
        string id28_enumerator;
        string id25_parameter;
        string id19_propertyAccess;
        string id46_booleanvalue;
        string id8_class;
        string id9_contents;
        string id45_insertatbeginning;
        string id1_Generator;
        string id40_description;
        string id38_hidden;
        string id53_inline;
        string id42_html;
        string id54_extends;
        string id27_enumeration;
        string id31_valuetype;
        string id23_command;
        string id35_name;
        string id6_yorn;
        string id30_recordtype;
        string id56_inherits;
        string id37_code;
        string id36_id;
        string id52_optional;
        string id57_title;
        string id14_accessor;
        string id20_respondsto;
        string id15_accessortype;
        string id44_method;
        string id5_cocoa;

        protected override void InitIDs() {
            id16_elementAccess = Reader.NameTable.Add(@"elementAccess");
            id48_stringvalue = Reader.NameTable.Add(@"string-value");
            id3_dictionary = Reader.NameTable.Add(@"dictionary");
            id10_type = Reader.NameTable.Add(@"type");
            id24_directparameter = Reader.NameTable.Add(@"direct-parameter");
            id34_Item = Reader.NameTable.Add(@"http://www.w3.org/XML/1998/namespace");
            id55_style = Reader.NameTable.Add(@"style");
            id22_classextension = Reader.NameTable.Add(@"class-extension");
            id33_base = Reader.NameTable.Add(@"base");
            id13_element = Reader.NameTable.Add(@"element");
            id21_xref = Reader.NameTable.Add(@"xref");
            id47_integervalue = Reader.NameTable.Add(@"integer-value");
            id41_target = Reader.NameTable.Add(@"target");
            id12_documentation = Reader.NameTable.Add(@"documentation");
            id7_cocoaBooleanvalue = Reader.NameTable.Add(@"cocoaBooleanvalue");
            id32_CSharpGenerator = Reader.NameTable.Add(@"CSharpGenerator");
            id49_access = Reader.NameTable.Add(@"access");
            id39_plural = Reader.NameTable.Add(@"plural");
            id18_synonym = Reader.NameTable.Add(@"synonym");
            id17_property = Reader.NameTable.Add(@"property");
            id26_result = Reader.NameTable.Add(@"result");
            id29_event = Reader.NameTable.Add(@"event");
            id2_Item = Reader.NameTable.Add(@"");
            id43_key = Reader.NameTable.Add(@"key");
            id50_inproperties = Reader.NameTable.Add(@"in-properties");
            id51_list = Reader.NameTable.Add(@"list");
            id4_suite = Reader.NameTable.Add(@"suite");
            id11_contentsAccess = Reader.NameTable.Add(@"contentsAccess");
            id28_enumerator = Reader.NameTable.Add(@"enumerator");
            id25_parameter = Reader.NameTable.Add(@"parameter");
            id19_propertyAccess = Reader.NameTable.Add(@"propertyAccess");
            id46_booleanvalue = Reader.NameTable.Add(@"boolean-value");
            id8_class = Reader.NameTable.Add(@"class");
            id9_contents = Reader.NameTable.Add(@"contents");
            id45_insertatbeginning = Reader.NameTable.Add(@"insert-at-beginning");
            id1_Generator = Reader.NameTable.Add(@"Generator");
            id40_description = Reader.NameTable.Add(@"description");
            id38_hidden = Reader.NameTable.Add(@"hidden");
            id53_inline = Reader.NameTable.Add(@"inline");
            id42_html = Reader.NameTable.Add(@"html");
            id54_extends = Reader.NameTable.Add(@"extends");
            id27_enumeration = Reader.NameTable.Add(@"enumeration");
            id31_valuetype = Reader.NameTable.Add(@"value-type");
            id23_command = Reader.NameTable.Add(@"command");
            id35_name = Reader.NameTable.Add(@"name");
            id6_yorn = Reader.NameTable.Add(@"yorn");
            id30_recordtype = Reader.NameTable.Add(@"record-type");
            id56_inherits = Reader.NameTable.Add(@"inherits");
            id37_code = Reader.NameTable.Add(@"code");
            id36_id = Reader.NameTable.Add(@"id");
            id52_optional = Reader.NameTable.Add(@"optional");
            id57_title = Reader.NameTable.Add(@"title");
            id14_accessor = Reader.NameTable.Add(@"accessor");
            id20_respondsto = Reader.NameTable.Add(@"responds-to");
            id15_accessortype = Reader.NameTable.Add(@"accessor-type");
            id44_method = Reader.NameTable.Add(@"method");
            id5_cocoa = Reader.NameTable.Add(@"cocoa");
        }
    }

    public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
        protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
            return new XmlSerializationReader1();
        }
        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
            return new XmlSerializationWriter1();
        }
    }

    public sealed class dictionarySerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"dictionary", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write34_dictionary(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read34_dictionary();
        }
    }

    public sealed class suiteSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"suite", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write35_suite(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read35_suite();
        }
    }

    public sealed class cocoaSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"cocoa", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write36_cocoa(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read36_cocoa();
        }
    }

    public sealed class yornSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"yorn", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write37_yorn(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read37_yorn();
        }
    }

    public sealed class cocoaBooleanvalueSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"cocoaBooleanvalue", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write38_cocoaBooleanvalue(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read38_cocoaBooleanvalue();
        }
    }

    public sealed class classSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"class", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write39_class(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read39_class();
        }
    }

    public sealed class contentsSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"contents", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write40_contents(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read40_contents();
        }
    }

    public sealed class typeSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"type", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write41_type(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read41_type();
        }
    }

    public sealed class contentsAccessSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"contentsAccess", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write42_contentsAccess(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read42_contentsAccess();
        }
    }

    public sealed class documentationSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"documentation", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write43_documentation(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read43_documentation();
        }
    }

    public sealed class elementSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"element", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write44_element(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read44_element();
        }
    }

    public sealed class accessorSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"accessor", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write45_accessor(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read45_accessor();
        }
    }

    public sealed class accessortypeSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"accessor-type", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write46_accessortype(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read46_accessortype();
        }
    }

    public sealed class elementAccessSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"elementAccess", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write47_elementAccess(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read47_elementAccess();
        }
    }

    public sealed class propertySerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"property", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write48_property(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read48_property();
        }
    }

    public sealed class synonymSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"synonym", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write49_synonym(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read49_synonym();
        }
    }

    public sealed class propertyAccessSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"propertyAccess", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write50_propertyAccess(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read50_propertyAccess();
        }
    }

    public sealed class respondstoSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"responds-to", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write51_respondsto(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read51_respondsto();
        }
    }

    public sealed class xrefSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"xref", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write52_xref(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read52_xref();
        }
    }

    public sealed class classextensionSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"class-extension", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write53_classextension(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read53_classextension();
        }
    }

    public sealed class commandSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"command", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write54_command(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read54_command();
        }
    }

    public sealed class directparameterSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"direct-parameter", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write55_directparameter(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read55_directparameter();
        }
    }

    public sealed class parameterSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"parameter", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write56_parameter(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read56_parameter();
        }
    }

    public sealed class resultSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"result", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write57_result(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read57_result();
        }
    }

    public sealed class enumerationSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"enumeration", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write58_enumeration(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read58_enumeration();
        }
    }

    public sealed class enumeratorSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"enumerator", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write59_enumerator(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read59_enumerator();
        }
    }

    public sealed class eventSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"event", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write60_event(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read60_event();
        }
    }

    public sealed class recordtypeSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"record-type", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write61_recordtype(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read61_recordtype();
        }
    }

    public sealed class valuetypeSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"value-type", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write62_valuetype(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read62_valuetype();
        }
    }

    public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
        public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReader1(); } }
        public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriter1(); } }
        System.Collections.Hashtable readMethods = null;
        public override System.Collections.Hashtable ReadMethods {
            get {
                if (readMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Monobjc.Tools.Sdp.Model.dictionary:::False:"] = @"Read34_dictionary";
                    _tmp[@"Monobjc.Tools.Sdp.Model.suite:::False:"] = @"Read35_suite";
                    _tmp[@"Monobjc.Tools.Sdp.Model.cocoa:::False:"] = @"Read36_cocoa";
                    _tmp[@"Monobjc.Tools.Sdp.Model.yorn::"] = @"Read37_yorn";
                    _tmp[@"Monobjc.Tools.Sdp.Model.cocoaBooleanvalue::"] = @"Read38_cocoaBooleanvalue";
                    _tmp[@"Monobjc.Tools.Sdp.Model.class:::False:"] = @"Read39_class";
                    _tmp[@"Monobjc.Tools.Sdp.Model.contents:::False:"] = @"Read40_contents";
                    _tmp[@"Monobjc.Tools.Sdp.Model.type:::False:"] = @"Read41_type";
                    _tmp[@"Monobjc.Tools.Sdp.Model.contentsAccess::"] = @"Read42_contentsAccess";
                    _tmp[@"Monobjc.Tools.Sdp.Model.documentation:::False:"] = @"Read43_documentation";
                    _tmp[@"Monobjc.Tools.Sdp.Model.element:::False:"] = @"Read44_element";
                    _tmp[@"Monobjc.Tools.Sdp.Model.accessor:::False:"] = @"Read45_accessor";
                    _tmp[@"Monobjc.Tools.Sdp.Model.accessortype::"] = @"Read46_accessortype";
                    _tmp[@"Monobjc.Tools.Sdp.Model.elementAccess::"] = @"Read47_elementAccess";
                    _tmp[@"Monobjc.Tools.Sdp.Model.property:::False:"] = @"Read48_property";
                    _tmp[@"Monobjc.Tools.Sdp.Model.synonym:::False:"] = @"Read49_synonym";
                    _tmp[@"Monobjc.Tools.Sdp.Model.propertyAccess::"] = @"Read50_propertyAccess";
                    _tmp[@"Monobjc.Tools.Sdp.Model.respondsto::responds-to:False:"] = @"Read51_respondsto";
                    _tmp[@"Monobjc.Tools.Sdp.Model.xref:::False:"] = @"Read52_xref";
                    _tmp[@"Monobjc.Tools.Sdp.Model.classextension::class-extension:False:"] = @"Read53_classextension";
                    _tmp[@"Monobjc.Tools.Sdp.Model.command:::False:"] = @"Read54_command";
                    _tmp[@"Monobjc.Tools.Sdp.Model.directparameter::direct-parameter:False:"] = @"Read55_directparameter";
                    _tmp[@"Monobjc.Tools.Sdp.Model.parameter:::False:"] = @"Read56_parameter";
                    _tmp[@"Monobjc.Tools.Sdp.Model.result:::False:"] = @"Read57_result";
                    _tmp[@"Monobjc.Tools.Sdp.Model.enumeration:::False:"] = @"Read58_enumeration";
                    _tmp[@"Monobjc.Tools.Sdp.Model.enumerator:::False:"] = @"Read59_enumerator";
                    _tmp[@"Monobjc.Tools.Sdp.Model.event:::False:"] = @"Read60_event";
                    _tmp[@"Monobjc.Tools.Sdp.Model.recordtype::record-type:False:"] = @"Read61_recordtype";
                    _tmp[@"Monobjc.Tools.Sdp.Model.valuetype::value-type:False:"] = @"Read62_valuetype";
                    if (readMethods == null) readMethods = _tmp;
                }
                return readMethods;
            }
        }
        System.Collections.Hashtable writeMethods = null;
        public override System.Collections.Hashtable WriteMethods {
            get {
                if (writeMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Monobjc.Tools.Sdp.Model.dictionary:::False:"] = @"Write34_dictionary";
                    _tmp[@"Monobjc.Tools.Sdp.Model.suite:::False:"] = @"Write35_suite";
                    _tmp[@"Monobjc.Tools.Sdp.Model.cocoa:::False:"] = @"Write36_cocoa";
                    _tmp[@"Monobjc.Tools.Sdp.Model.yorn::"] = @"Write37_yorn";
                    _tmp[@"Monobjc.Tools.Sdp.Model.cocoaBooleanvalue::"] = @"Write38_cocoaBooleanvalue";
                    _tmp[@"Monobjc.Tools.Sdp.Model.class:::False:"] = @"Write39_class";
                    _tmp[@"Monobjc.Tools.Sdp.Model.contents:::False:"] = @"Write40_contents";
                    _tmp[@"Monobjc.Tools.Sdp.Model.type:::False:"] = @"Write41_type";
                    _tmp[@"Monobjc.Tools.Sdp.Model.contentsAccess::"] = @"Write42_contentsAccess";
                    _tmp[@"Monobjc.Tools.Sdp.Model.documentation:::False:"] = @"Write43_documentation";
                    _tmp[@"Monobjc.Tools.Sdp.Model.element:::False:"] = @"Write44_element";
                    _tmp[@"Monobjc.Tools.Sdp.Model.accessor:::False:"] = @"Write45_accessor";
                    _tmp[@"Monobjc.Tools.Sdp.Model.accessortype::"] = @"Write46_accessortype";
                    _tmp[@"Monobjc.Tools.Sdp.Model.elementAccess::"] = @"Write47_elementAccess";
                    _tmp[@"Monobjc.Tools.Sdp.Model.property:::False:"] = @"Write48_property";
                    _tmp[@"Monobjc.Tools.Sdp.Model.synonym:::False:"] = @"Write49_synonym";
                    _tmp[@"Monobjc.Tools.Sdp.Model.propertyAccess::"] = @"Write50_propertyAccess";
                    _tmp[@"Monobjc.Tools.Sdp.Model.respondsto::responds-to:False:"] = @"Write51_respondsto";
                    _tmp[@"Monobjc.Tools.Sdp.Model.xref:::False:"] = @"Write52_xref";
                    _tmp[@"Monobjc.Tools.Sdp.Model.classextension::class-extension:False:"] = @"Write53_classextension";
                    _tmp[@"Monobjc.Tools.Sdp.Model.command:::False:"] = @"Write54_command";
                    _tmp[@"Monobjc.Tools.Sdp.Model.directparameter::direct-parameter:False:"] = @"Write55_directparameter";
                    _tmp[@"Monobjc.Tools.Sdp.Model.parameter:::False:"] = @"Write56_parameter";
                    _tmp[@"Monobjc.Tools.Sdp.Model.result:::False:"] = @"Write57_result";
                    _tmp[@"Monobjc.Tools.Sdp.Model.enumeration:::False:"] = @"Write58_enumeration";
                    _tmp[@"Monobjc.Tools.Sdp.Model.enumerator:::False:"] = @"Write59_enumerator";
                    _tmp[@"Monobjc.Tools.Sdp.Model.event:::False:"] = @"Write60_event";
                    _tmp[@"Monobjc.Tools.Sdp.Model.recordtype::record-type:False:"] = @"Write61_recordtype";
                    _tmp[@"Monobjc.Tools.Sdp.Model.valuetype::value-type:False:"] = @"Write62_valuetype";
                    if (writeMethods == null) writeMethods = _tmp;
                }
                return writeMethods;
            }
        }
        System.Collections.Hashtable typedSerializers = null;
        public override System.Collections.Hashtable TypedSerializers {
            get {
                if (typedSerializers == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.yorn::", new yornSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.parameter:::False:", new parameterSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.accessortype::", new accessortypeSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.synonym:::False:", new synonymSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.contents:::False:", new contentsSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.property:::False:", new propertySerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.classextension::class-extension:False:", new classextensionSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.type:::False:", new typeSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.event:::False:", new eventSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.directparameter::direct-parameter:False:", new directparameterSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.enumerator:::False:", new enumeratorSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.elementAccess::", new elementAccessSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.accessor:::False:", new accessorSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.recordtype::record-type:False:", new recordtypeSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.cocoaBooleanvalue::", new cocoaBooleanvalueSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.valuetype::value-type:False:", new valuetypeSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.command:::False:", new commandSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.enumeration:::False:", new enumerationSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.class:::False:", new classSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.element:::False:", new elementSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.cocoa:::False:", new cocoaSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.documentation:::False:", new documentationSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.result:::False:", new resultSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.respondsto::responds-to:False:", new respondstoSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.suite:::False:", new suiteSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.xref:::False:", new xrefSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.propertyAccess::", new propertyAccessSerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.dictionary:::False:", new dictionarySerializer());
                    _tmp.Add(@"Monobjc.Tools.Sdp.Model.contentsAccess::", new contentsAccessSerializer());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.dictionary)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.suite)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.cocoa)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.yorn)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.cocoaBooleanvalue)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.@class)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.contents)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.type)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.contentsAccess)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.documentation)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.element)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.accessor)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.accessortype)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.elementAccess)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.property)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.synonym)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.propertyAccess)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.respondsto)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.xref)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.classextension)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.command)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.directparameter)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.parameter)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.result)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.enumeration)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.enumerator)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.@event)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.recordtype)) return true;
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.valuetype)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.dictionary)) return new dictionarySerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.suite)) return new suiteSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.cocoa)) return new cocoaSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.yorn)) return new yornSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.cocoaBooleanvalue)) return new cocoaBooleanvalueSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.@class)) return new classSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.contents)) return new contentsSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.type)) return new typeSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.contentsAccess)) return new contentsAccessSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.documentation)) return new documentationSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.element)) return new elementSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.accessor)) return new accessorSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.accessortype)) return new accessortypeSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.elementAccess)) return new elementAccessSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.property)) return new propertySerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.synonym)) return new synonymSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.propertyAccess)) return new propertyAccessSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.respondsto)) return new respondstoSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.xref)) return new xrefSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.classextension)) return new classextensionSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.command)) return new commandSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.directparameter)) return new directparameterSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.parameter)) return new parameterSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.result)) return new resultSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.enumeration)) return new enumerationSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.enumerator)) return new enumeratorSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.@event)) return new eventSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.recordtype)) return new recordtypeSerializer();
            if (type == typeof(global::Monobjc.Tools.Sdp.Model.valuetype)) return new valuetypeSerializer();
            return null;
        }
    }
}
