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
using System;
using System.IO;
using Monobjc.Tools.Properties;
using Monobjc.Tools.PropertyList;
using NUnit.Framework;

namespace Monobjc.Tools
{
    [TestFixture]
    [Category("XIB")]
    [Category("Parsing")]
    public class PListLoadTests
    {
        [Test]
        public void TestPListReading001()
        {
            String content = ReadResource(Resources.Info_001);
            PListDocument document = PListDocument.LoadFromXml(content);
            CheckDocument(document);

            Assert.AreEqual(String.Empty, (String) (PListString) document.Root.Dict["CFBundleIconFile"]);
            Assert.AreEqual("com.Perspx.${PRODUCT_NAME:rfc1034identifier}", (String) (PListString) document.Root.Dict["CFBundleIdentifier"]);
        }

        [Test]
        public void TestPListReading002()
        {
            String content = ReadResource(Resources.Info_002);
            PListDocument document = PListDocument.LoadFromXml(content);
            CheckDocument(document);

            Assert.AreEqual(String.Empty, (String) (PListString) document.Root.Dict["CFBundleIconFile"]);
            Assert.AreEqual("se.hunch.${PRODUCT_NAME:rfc1034identifier}", (String) (PListString) document.Root.Dict["CFBundleIdentifier"]);
        }

        [Test]
        public void TestPListReading004()
        {
            String content = ReadResource(Resources.Info_004);
            PListDocument document = PListDocument.LoadFromXml(content);
            CheckDocument(document);

            Assert.AreEqual(String.Empty, (String) (PListString) document.Root.Dict["CFBundleIconFile"]);
            Assert.AreEqual("com.yourcompany.${PRODUCT_NAME:rfc1034identifier}", (String) (PListString) document.Root.Dict["CFBundleIdentifier"]);
        }

        [Test]
        public void TestPListReading005()
        {
            String content = ReadResource(Resources.Info_005);
            PListDocument document = PListDocument.LoadFromXml(content);
            CheckDocument(document);

            Assert.IsTrue(document.Root.Dict.ContainsKey("CFBundleDocumentTypes"));
            PListItemBase item = document.Root.Dict["CFBundleDocumentTypes"];
            Assert.IsTrue(item is PListArray);
            PListArray array = (PListArray) item;
            Assert.AreEqual(1, array.Count);
        }

        [Test]
        public void TestPListReading010()
        {
            String content = ReadResource(Resources.Info_010);
            PListDocument document = PListDocument.LoadFromXml(content);
            CheckDocument(document);

            Assert.AreEqual(String.Empty, (String) (PListString) document.Root.Dict["CFBundleIconFile"]);
            Assert.AreEqual("net.monobjc.${PRODUCT_NAME:rfc1034identifier}", (String) (PListString) document.Root.Dict["CFBundleIdentifier"]);
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

        private static void CheckDocument(PListDocument document)
        {
            Assert.IsNotNull(document.Root);
            Assert.IsNotNull(document.Root.Dict);
            Assert.IsTrue(document.Root.Dict.ContainsKey("CFBundleDevelopmentRegion"));
            Assert.IsTrue(document.Root.Dict.ContainsKey("CFBundleExecutable"));
            Assert.IsTrue(document.Root.Dict.ContainsKey("CFBundleIconFile"));
            Assert.IsTrue(document.Root.Dict.ContainsKey("CFBundleIdentifier"));
            Assert.IsTrue(document.Root.Dict.ContainsKey("CFBundleInfoDictionaryVersion"));
            Assert.IsTrue(document.Root.Dict.ContainsKey("CFBundleName"));
            Assert.IsTrue(document.Root.Dict.ContainsKey("CFBundlePackageType"));
            Assert.IsTrue(document.Root.Dict.ContainsKey("CFBundleSignature"));
            Assert.IsTrue(document.Root.Dict.ContainsKey("CFBundleShortVersionString"));
            Assert.IsTrue(document.Root.Dict.ContainsKey("LSMinimumSystemVersion"));
            Assert.IsTrue(document.Root.Dict.ContainsKey("CFBundleVersion"));
            Assert.AreEqual("1", (String) (PListString) document.Root.Dict["CFBundleVersion"]);
            Assert.IsTrue(document.Root.Dict.ContainsKey("NSMainNibFile"));
            Assert.AreEqual("MainMenu", (String) (PListString) document.Root.Dict["NSMainNibFile"]);
            Assert.IsTrue(document.Root.Dict.ContainsKey("NSPrincipalClass"));
            Assert.AreEqual("NSApplication", (String) (PListString) document.Root.Dict["NSPrincipalClass"]);
        }
    }
}