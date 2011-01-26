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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Monobjc.Tools.Generator.Model.Entities;
using Monobjc.Tools.Generator.Utilities;

namespace Monobjc.Tools.Generator.Parsers.Xhtml
{
    /// <summary>
    ///   XHTML parser dedicated to constants.
    /// </summary>
    public class XhtmlConstantParser : XhtmlBaseParser
    {
        private static readonly Regex ENUMERATION_REGEX = new Regex(@"(typedef )?enum( ?[_A-z]+)? ?\{(.+)\};?( ?typedef)?( ?[A-z0-9_]+ ?)?([A-z]+)?");
        private static readonly Regex CONSTANT_REGEX = new Regex(@"(const )?(id ?|unsigned ?|double ?|float ?\*?|NSString ?\*? ?|CFStringRef|CIFormat|CATransform3D|CLLocationDistance ?)(const ?)?([A-z0-9]+)$");

        /// <summary>
        ///   Initializes a new instance of the <see cref = "XhtmlNotificationParser" /> class.
        /// </summary>
        /// <param name = "settings">The settings.</param>
        /// <param name = "typeManager">The type manager.</param>
        public XhtmlConstantParser(NameValueCollection settings, TypeManager typeManager) : base(settings, typeManager) {}

        /// <summary>
        ///   Parses the specified entity.
        /// </summary>
        /// <param name = "entity">The entity.</param>
        /// <param name = "reader">The reader.</param>
        public override void Parse(BaseEntity entity, TextReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Parses the specified constant element.
        /// </summary>
        /// <param name = "constantElement">The constant element.</param>
        public List<BaseEntity> Parse(XElement constantElement)
        {
            // Get the name
            String name = constantElement.TrimAll();

            // Get the abstract
            XElement summaryElement = (from el in constantElement.ElementsAfterSelf("p")
                                       where (String) el.Attribute("class") == "abstract"
                                       select el).FirstOrDefault();
            String summary = summaryElement.TrimAll();

            // Get the declaration
            XElement declarationElement = (from el in constantElement.ElementsAfterSelf("pre")
                                           where (String) el.Attribute("class") == "declaration"
                                           select el).FirstOrDefault();
            String declaration = declarationElement.TrimAll();

            // Make various tests
            bool isDefine = declaration.StartsWith("#define");
            bool isEnum = declaration.StartsWith("enum") || declaration.StartsWith("typedef enum") || declaration.Contains(" enum ");

            if (isDefine)
            {
                List<BaseEntity> entities = ExtractDefine(constantElement, name, summary, declaration);
                return entities;
            }

            if (isEnum)
            {
                List<BaseEntity> entities = this.ExtractEnumeration(constantElement, name, summary, declaration);
                if (entities != null)
                {
                    return entities;
                }
            }

            return this.ExtractConstants(constantElement, name, summary, declaration);
        }

        private static List<BaseEntity> ExtractDefine(XElement constantElement, String name, String summary, String declaration)
        {
            return null;
        }

        private List<BaseEntity> ExtractEnumeration(XElement constantElement, String name, String summary, String declaration)
        {
            declaration = declaration.Trim(';');

            // Default values
            String type = "NOTYPE";
            String values;

            // Match the enumeration definition
            Match r = ENUMERATION_REGEX.Match(declaration);
            if (r.Success)
            {
                String v2 = r.Groups[2].Value.Trim();
                String v5 = r.Groups[5].Value.Trim();
                String v6 = r.Groups[6].Value.Trim();

                values = r.Groups[3].Value.Trim();

                // Name can be before enumeration values
                if (!String.IsNullOrEmpty(v5) && !String.IsNullOrEmpty(v6))
                {
                    type = v5;
                    name = v6;
                }
                else if (!String.IsNullOrEmpty(v5) && String.IsNullOrEmpty(v6))
                {
                    name = v5;
                }
                else if (!String.IsNullOrEmpty(v2) && !String.IsNullOrEmpty(v5))
                {
                    name = v5;
                }
                else if (!String.IsNullOrEmpty(v2) && String.IsNullOrEmpty(v6))
                {
                    name = v2;
                }

                // Clean results
                name = name.Trim(';');

                // Make sure name is ok
                name = " -:—".Aggregate(name, (current, c) => current.Replace(c, '_'));

                bool isOut;
                bool isByRef;
                bool isBlock;
                type = this.TypeManager.ConvertType(type, out isOut, out isByRef, out isBlock);

                //Console.WriteLine("Enumeration found '{0}' of type '{1}'", name, type);
            }
            else
            {
                Console.WriteLine("FAILED to parse enum '{0}'", declaration);
                return null;
            }

            // Create the enumeration
            EnumerationEntity enumerationEntity = new EnumerationEntity();
            enumerationEntity.Name = name;
            enumerationEntity.BaseType = type;
            enumerationEntity.Namespace = "MISSING";
            enumerationEntity.Summary.Add(summary);

            // Parse the values
            //int startBrace = declaration.IndexOf("{");
            //int endBrace = declaration.IndexOf("}");
            //String valuesBlock = declaration.Substring(startBrace + 1, endBrace - startBrace - 1);
            string[] pairs = values.Split(',');
            foreach (string pair in pairs)
            {
                if (String.Equals(pair.TrimAll(), String.Empty))
                {
                    continue;
                }

                String key;
                String value = String.Empty;
                if (pair.IndexOf('=') != -1)
                {
                    string[] parts = pair.Split('=');
                    key = parts[0].Trim();
                    value = parts[1].Trim();
                }
                else
                {
                    key = pair.Trim();
                }

                // Add a new value
                EnumerationValueEntity enumerationValueEntity = new EnumerationValueEntity();
                enumerationValueEntity.Name = key;
                enumerationValueEntity.Value = value;
                enumerationEntity.Values.Add(enumerationValueEntity);
            }

            // Get the definitions
            XElement termList = (from el in constantElement.ElementsAfterSelf("dl")
                                 where (String) el.Attribute("class") == "termdef"
                                 select el).FirstOrDefault();
            IEnumerable<XElement> dtList = termList.Elements("dt");
            IEnumerable<XElement> ddList = termList.Elements("dd");

            if (dtList.Count() == ddList.Count())
            {
                // Iterate over definitions
                for (int i = 0; i < dtList.Count(); i++)
                {
                    String term = dtList.ElementAt(i).Value.TrimAll();
                    IEnumerable<String> summaries = ddList.ElementAt(i).Elements("p").Select(p => p.Value.TrimAll());

                    // Find the enumeration value
                    EnumerationValueEntity enumerationValueEntity = enumerationEntity.Values.Find(v => v.Name == term);
                    if (enumerationValueEntity != null)
                    {
                        foreach (string sum in summaries)
                        {
                            if (CommentHelper.IsAvailability(sum))
                            {
                                enumerationValueEntity.MinAvailability = CommentHelper.ExtractAvailability(sum);
                                break;
                            }
                            enumerationValueEntity.Summary.Add(sum);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Term with no match '" + term + "'");
                    }
                }
            }
            else
            {
                Console.WriteLine("MISMATCH in terms");
            }

            // Make sure availability is ok
            enumerationEntity.AdjustAvailability();

            return new List<BaseEntity> {enumerationEntity};
        }

        private string PatchEnumeration(string declaration)
        {
            // PATCH: Several (wrong type)
            if (declaration.Contains("1UL"))
            {
                declaration = declaration.Replace("1UL", "1U");
            }
            //// PATCH: FoundationFramework (missing ',')
            //if (declaration.Contains("NSFileWriteVolumeReadOnlyError"))
            //{
            //    declaration = declaration.Replace("642m", "642,");
            //    declaration = declaration.Replace("4095 ", "4095,");
            //}
            //// PATCH: NSAttributedString (Wrong type)
            //if (declaration.Contains("NSAttributedStringEnumerationOptions"))
            //{
            //    declaration = declaration.Replace("UL", "U");
            //}
            //// PATCH: NSCalendar (missing ',')
            //if (declaration.Contains("NSQuarterCalendarUnit"))
            //{
            //    declaration = declaration.Replace("NSQuarterCalendarUnit", ", NSQuarterCalendarUnit");
            //}
            //// PATCH: NSData (missing ',')
            //if (declaration.Contains("NSMappedRead"))
            //{
            //    declaration = declaration.Replace("NSMappedRead", ", NSMappedRead");
            //}
            //// PATCH: NSData (missing ',')
            //if (declaration.Contains("NSAtomicWrite"))
            //{
            //    declaration = declaration.Replace("NSAtomicWrite", ", NSAtomicWrite");
            //}
            //// PATCH: NSDocument (missing ',')
            //if (declaration.Contains("NSAutosaveOperation"))
            //{
            //    declaration = declaration.Replace("NSAutosaveOperation", ", NSAutosaveOperation");
            //}
            //// PATCH: NSDraggingInfo (wrong constant)
            //if (declaration.Contains("NSDragOperationEvery"))
            //{
            //    declaration = declaration.Replace("NSUIntegerMax", "UInt32.MaxValue");
            //}
            //// PATCH: NSEvent (missing ',')
            //if (declaration.Contains("NSEventTypeGesture"))
            //{
            //    declaration = declaration.Replace("= 27", "= 27,");
            //}
            //// PATCH: NSEvent (missing ',')
            //if (declaration.Contains("NSTouchEventSubtype"))
            //{
            //    declaration = declaration.Replace("NSTouchEventSubtype", ", NSTouchEventSubtype");
            //}
            // PATCH: NSEvent (macro to remove)
            if (declaration.Contains("NSEventMaskFromType"))
            {
                declaration = declaration.Replace("NSUIntegerNSEventMaskFromType(NSEventTypetype) { return (1 &lt;&lt; type); };", String.Empty);
            }
            // PATCH: NSEvent (missing prefix)
            if (declaration.Contains("NSLeftMouseDownMask"))
            {
                declaration = declaration.Replace("= 1 &lt;&lt; NS", "= (uint) 1 &lt;&lt; (int) NSEventType.NS");
                declaration = declaration.Replace("= 1U &lt;&lt; NS", "= (uint) 1 &lt;&lt; (int) NSEventType.NS");
            }
            // PATCH: NSEvent (missing constant)
            if (declaration.Contains("NX_TABLET_POINTER_UNKNOWN"))
            {
                declaration = declaration.Replace("NX_TABLET_POINTER_UNKNOWN", "0");
                declaration = declaration.Replace("NX_TABLET_POINTER_PEN", "1");
                declaration = declaration.Replace("NX_TABLET_POINTER_CURSOR", "2");
                declaration = declaration.Replace("NX_TABLET_POINTER_ERASER", "3");
            }
            // PATCH: NSEvent (missing constant)
            if (declaration.Contains("NX_SUBTYPE_DEFAULT"))
            {
                declaration = declaration.Replace("NX_SUBTYPE_DEFAULT", "0");
                declaration = declaration.Replace("NX_SUBTYPE_TABLET_POINT", "1");
                declaration = declaration.Replace("NX_SUBTYPE_TABLET_PROXIMITY", "2");
                declaration = declaration.Replace("NX_SUBTYPE_MOUSE_TOUCH", "3");
            }
            // PATCH: NSEvent (missing constant)
            if (declaration.Contains("NX_TABLET_BUTTON_PENTIPMASK"))
            {
                declaration = declaration.Replace("NX_TABLET_BUTTON_PENTIPMASK", "1");
                declaration = declaration.Replace("NX_TABLET_BUTTON_PENLOWERSIDEMASK", "2");
                declaration = declaration.Replace("NX_TABLET_BUTTON_PENUPPERSIDEMASK", "4");
            }
            // PATCH: NSFileManager (wrong type)
            if (declaration.Contains("NSVolumeEnumerationOptions"))
            {
                declaration = declaration.Replace("1L ", "1 ");
            }
            // PATCH: NSFileManager (wrong type)
            if (declaration.Contains("NSDirectoryEnumerationOptions"))
            {
                declaration = declaration.Replace("1L ", "1 ");
            }
            // PATCH: NSHashtable (missing prefix)
            if (declaration.Contains("NSHashTableStrongMemory"))
            {
                declaration = declaration.Replace(" = NSPointerFunction", " = NSPointerFunctionsOptions.NSPointerFunction");
            }
            // PATCH: NSMaptable (missing prefix)
            if (declaration.Contains("NSMapTableStrongMemory"))
            {
                declaration = declaration.Replace(" = NSPointerFunction", " = NSPointerFunctionsOptions.NSPointerFunction");
            }
            // PATCH: NSMenu (missing ',')
            if (declaration.Contains("NSMenuPropertyItemKeyEquivalent"))
            {
                declaration = declaration.Replace("NSMenuPropertyItemKeyEquivalent", ", NSMenuPropertyItemKeyEquivalent");
            }
            // PATCH: NSPanel (missing ',')
            if (declaration.Contains("NSHUDWindowMask"))
            {
                declaration = declaration.Replace("NSHUDWindowMask", ", NSHUDWindowMask");
            }
            // PATCH: NSParagraphStyle (missing space)
            if (declaration.Contains("NSLineBreakMode"))
            {
                declaration = declaration.Replace("NSLineBreakMode", " NSLineBreakMode");
            }
            // PATCH: NSString (inverted typedef)
            if (declaration.Contains("NSStringEnumerationOptions"))
            {
                declaration = declaration.Replace("typedef NSUInteger NSStringEnumerationOptions;", "");
                declaration = declaration + "typedef NSUInteger NSStringEnumerationOptions;";
            }
            // PATCH: NSTextCheckingResult (type to change)
            if (declaration.Contains("NSTextCheckingType"))
            {
                declaration = declaration.Replace("ULL", "UL");
            }
            // PATCH: NSTextCheckingResult (word to remove/type to change)
            if (declaration.Contains("NSTextCheckingTypes"))
            {
                declaration = declaration.Replace("purposes", "");
                declaration = declaration.Replace("ULL", "UL");
            }
            //// PATCH: NSTouch (wrong constant)
            //if (declaration.Contains("NSTouchPhaseAny"))
            //{
            //    declaration = declaration.Replace("NSUIntegerMax", "UInt32.MaxValue");
            //}
            // PATCH: NSWindow (concat enumeration)
            if (declaration.Contains("; enum {"))
            {
                declaration = declaration.Replace("}; enum {", " , ");
            }
            // PATCH: NSXMLNode (word to remove)
            if (declaration.Contains("NSXMLNodeCompactEmptyElement"))
            {
                declaration = declaration.Replace("// &lt;a>&lt;/a>", "");
                declaration = declaration.Replace("// &lt;a/>", "");
            }
            //// PATCH: PDFAnnotationButtonWidget (inverted typedef)
            //if (declaration.Contains("PDFWidgetControlType"))
            //{
            //    declaration = declaration.Replace("typedef NSUInteger PDFWidgetControlType;", "");
            //    declaration = declaration + "typedef NSUInteger PDFWidgetControlType;";
            //}
            return declaration;
        }

        private List<BaseEntity> ExtractConstants(XElement constantElement, String name, String summary, String declaration)
        {
            List<BaseEntity> constants = new List<BaseEntity>();

            // Extract types and names
            string[] declarations = declaration.Split(';');
            foreach (string part in declarations)
            {
                if (part.Trim() == String.Empty)
                {
                    continue;
                }

                //Console.WriteLine("Parsing constant '{0}'...", part.Trim());

                String stripped = part;
                stripped = stripped.Replace("extern", "");
                stripped = stripped.Replace("const", "");
                stripped = stripped.TrimAll();

                Match r = CONSTANT_REGEX.Match(stripped);
                if (r.Success)
                {
                    String type = r.Groups[2].Value.Trim(' ', '*', ' ');

                    bool isOut;
                    bool isByRef;
                    bool isBlock;
                    type = this.TypeManager.ConvertType(type, out isOut, out isByRef, out isBlock);

                    ConstantEntity constantEntity = new ConstantEntity();
                    constantEntity.Type = type;
                    constantEntity.Name = r.Groups[4].Value.Trim();
                    constants.Add(constantEntity);

                    //Console.WriteLine("Constant found '{0}' of type '{1}'", constantEntity.Name, constantEntity.Type);
                }
                else
                {
                    Console.WriteLine("FAILED to parse constant '{0}'", part);
                    return null;
                }
            }

            // Get the definitions
            XElement termDefinitions = (from el in constantElement.ElementsAfterSelf("dl")
                                        where (String) el.Attribute("class") == "termdef"
                                        select el).FirstOrDefault();
            if (termDefinitions == null)
            {
                Console.WriteLine("MISSING terms");
                return null;
            }

            IEnumerable<XElement> termName = termDefinitions.Elements("dt");
            IEnumerable<XElement> termDefinition = termDefinitions.Elements("dd");

            if (termName.Count() == termDefinition.Count())
            {
                // Iterate over definitions
                for (int i = 0; i < termName.Count(); i++)
                {
                    String term = termName.ElementAt(i).Value.TrimAll();
                    IEnumerable<String> summaries = termDefinition.ElementAt(i).Elements("p").Select(p => p.Value.TrimAll());

                    // Find the enumeration value
                    BaseEntity baseEntity = constants.Find(c => c.Name == term);
                    if (baseEntity != null)
                    {
                        foreach (string sum in summaries)
                        {
                            if (CommentHelper.IsAvailability(sum))
                            {
                                baseEntity.MinAvailability = CommentHelper.ExtractAvailability(sum);
                                break;
                            }
                            baseEntity.Summary.Add(sum);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Term with no match '" + term + "'");
                    }
                }
            }
            else
            {
                Console.WriteLine("MISMATCH in terms");
                return null;
            }

            return constants;
        }
    }
}