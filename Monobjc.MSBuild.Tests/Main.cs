using System;

namespace Monobjc.MSBuild.Tests
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			{
//				CallReceigenTests tests = new CallReceigenTests();
//				tests.TestTask();
			}
			{
				EncryptFilesTests tests = new EncryptFilesTests ();
				tests.TestNoSourceFiles ();
				tests.TestDifferentSourceAndDestination();
				tests.TestDestinationFilesAndFolder();
				tests.TestNoDestinationFolder ();
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
