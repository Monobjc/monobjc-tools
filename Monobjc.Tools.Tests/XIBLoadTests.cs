using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Monobjc.Tools.InterfaceBuilder;
using Monobjc.Tools.InterfaceBuilder.Visitors;
using Monobjc.Tools.Properties;
using NUnit.Framework;

namespace Monobjc.Tools
{
    [TestFixture]
    [Category("XIB")]
    [Category("Parsing")]
    public class XIBLoadTests
    {
        [Test]
        public void TestMainMenuReading001()
        {
            String content = ReadResource(Resources.MainMenu_001);
            IBDocument document = IBDocument.LoadFromXml(content);
            CheckDocument(document);
        }

        [Test]
        public void TestMainMenuReading002()
        {
            String content = ReadResource(Resources.MainMenu_002);
            IBDocument document = IBDocument.LoadFromXml(content);
            CheckDocument(document);
        }

        [Test]
        public void TestMainMenuReading003()
        {
            String content = ReadResource(Resources.MainMenu_003);
            IBDocument document = IBDocument.LoadFromXml(content);
            CheckDocument(document);
        }

        [Test]
        public void TestMainMenuReading004()
        {
            String content = ReadResource(Resources.MainMenu_004);
            IBDocument document = IBDocument.LoadFromXml(content);
            CheckDocument(document);
        }

        [Test]
        public void TestMainMenuReading005()
        {
            String content = ReadResource(Resources.MainMenu_005);
            IBDocument document = IBDocument.LoadFromXml(content);
            CheckDocument(document);
        }

        [Test]
        public void TestMainMenuReading006()
        {
            String content = ReadResource(Resources.MainMenu_006);
            IBDocument document = IBDocument.LoadFromXml(content);
            CheckDocument(document);
        }

        [Test]
        public void TestMainMenuReading007()
        {
            String content = ReadResource(Resources.MainMenu_007);
            IBDocument document = IBDocument.LoadFromXml(content);
            CheckDocument(document);
        }

        [Test]
        public void TestMainMenuReading008()
        {
            String content = ReadResource(Resources.MainMenu_008);
            IBDocument document = IBDocument.LoadFromXml(content);
            CheckDocument(document);
        }

        [Test]
        public void TestMyDocumentReading005()
        {
            String content = ReadResource(Resources.MyDocument_005);
            IBDocument document = IBDocument.LoadFromXml(content);
            CheckDocument(document);
        }

        [Test]
        public void TestMainMenuReading010()
        {
            String content = ReadResource(Resources.MainMenu_010);
            IBDocument document = IBDocument.LoadFromXml(content);
            CheckDocument(document);

            ClassDescriptionCollector collector = new ClassDescriptionCollector();
            document.Root.Accept(collector);
            Assert.AreEqual(2, collector.ClassNames.Count());
            Assert.IsTrue(collector.ClassNames.Contains("TestCocoa41AppDelegate"));
            IEnumerable<IBPartialClassDescription> classDescriptions = collector["TestCocoa41AppDelegate"];
            IEnumerable<IBOutletDescriptor> outlets = classDescriptions.SelectMany(d => d.Outlets);
            Assert.AreEqual(2, outlets.Count());
            IEnumerable<IBActionDescriptor> actions = classDescriptions.SelectMany(d => d.Actions);
            Assert.AreEqual(1, actions.Count());
        }

        private static String ReadResource(byte[] resource)
        {
            using (MemoryStream stream = new MemoryStream(resource))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static void CheckDocument(IBDocument document)
        {
            Assert.IsNotNull(document.Root);
        }
    }
}