using System;

namespace Monobjc.MSBuild.Tests
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            {
//              CallReceigenTests tests = new CallReceigenTests();
//              tests.TestTask();
            }
            {
                CodeSigningTests tests = new CodeSigningTests();
                tests.TestCodeSignBundle();
                tests.TestCodeSignLibrary();
                tests.TestCodeSignLibraries();
                tests.TestCodeSignFramework();
                tests.TestCodeSignFrameworks();
            }
            {
                CopyFrameworksTests tests = new CopyFrameworksTests();
                tests.TestCopyFramework();
                tests.TestCopyFrameworks();
            }
            {
                EncryptFilesTests tests = new EncryptFilesTests();
                tests.TestNoSourceFiles();
                tests.TestDifferentSourceAndDestination();
                tests.TestDestinationFilesAndFolder();
                tests.TestNoDestinationFolder();
                tests.TestFilesToFolder();
                tests.TestFilesToFiles();
            }
            {
                CombineArtworkTests tests = new CombineArtworkTests();
                tests.TestTask();
            }
        }
    }
}
