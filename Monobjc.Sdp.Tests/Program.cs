using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Monobjc.Tools.Sdp.Model;
using Mvp.Xml.Common;
using Mvp.Xml.XInclude;

namespace Monobjc.Sdp.Tests
{
	class Program
	{
		static void Main (String[] args)
		{
			TestLoading ();
		}

		static void TestLoading ()
		{
			// All files test
			String[] files = Directory.GetFiles ("Definitions", "*.sdef");
			foreach (String file in files) {
				Load (file);
			}
			
			// Single file test
			//Load("Definitions/QuickTime.sdef");
		}

		static void Load (String inputFile)
		{
			Console.WriteLine ("Probing " + Path.GetFileName (inputFile));
			String outputFile = Path.ChangeExtension (inputFile, ".xml");
			
			using (XIncludingReader reader = new XIncludingReader(inputFile)) {
				using (StreamWriter writer = new StreamWriter(outputFile)) {
					XmlDocument doc = new XmlDocument ();
					doc.Load (reader);
					
					// Remove "xml:base attribute"
					RemoveXmlBase (doc.DocumentElement);
					
					doc.Save (writer);
					
					XDocument doc2 = ToXDocument(doc);
					Console.WriteLine (doc2.Elements().Count());
					
					//dictionary dictionary = new dictionary(doc2.Root);
					//Console.WriteLine (dictionary.title);
				}					
			}
			
			using (StreamReader reader = new StreamReader(outputFile)) {
				XmlSerializer serializer = new XmlSerializer (typeof(dictionary));
				dictionary dict = (dictionary)serializer.Deserialize (reader);
				Console.WriteLine (dict.title);
			}
		}
		
		static void RemoveXmlBase (XmlDocument document)
		{
			RemoveXmlBase (document.DocumentElement);
		}
		
		static void RemoveXmlBase (XmlElement element)
		{
			element.RemoveAttribute ("xml:base");
			if (!element.HasChildNodes) {
				return;
			}
			IEnumerable<XmlElement> children = element.ChildNodes.OfType<XmlElement> ();
			foreach (XmlElement child in children) {
				RemoveXmlBase (child);
			}
		}
		
		static XDocument ToXDocument (XmlDocument xmlDocument)
		{
			using (XmlNodeReader nodeReader = new XmlNodeReader(xmlDocument)) {
				nodeReader.MoveToContent ();
				return XDocument.Load (nodeReader);
			}
		}		
	}
}
