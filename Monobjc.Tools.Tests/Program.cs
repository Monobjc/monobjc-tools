using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monobjc.Tools
{
    public class Program
    {
        public static void Main(String[] args)
        {
            XIBLoadTests xibLoadTests = new XIBLoadTests();
            xibLoadTests.TestMainMenuReading001();
            xibLoadTests.TestMainMenuReading002();
            xibLoadTests.TestMainMenuReading003();
            xibLoadTests.TestMainMenuReading004();
            xibLoadTests.TestMainMenuReading005();
            xibLoadTests.TestMainMenuReading006();
            xibLoadTests.TestMainMenuReading007();
            xibLoadTests.TestMainMenuReading008();
            xibLoadTests.TestMainMenuReading010();
            xibLoadTests.TestMyDocumentReading005();

            PListLoadTests pListLoadTests = new PListLoadTests();
            pListLoadTests.TestPListReading001();
            pListLoadTests.TestPListReading002();
            pListLoadTests.TestPListReading004();
            pListLoadTests.TestPListReading005();
            pListLoadTests.TestPListReading010();
        }
    }
}
