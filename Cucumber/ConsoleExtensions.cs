using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cucumber
{
    public static class CConsole
    {
        public static void WriteLineLevel1(string line)
        {
            Console.WriteLine(line);
        }

        public static void WriteLevel1(string text)
        {
            Console.Write(text);
        }

        public static void WriteLineLevel2(string line)
        {
            Console.WriteLine("  " + line);
        }

        public static void WriteLineLevel3(string line)
        {
            Console.WriteLine("    " + line);
        }

        public static void WriteLineLevel4(string line)
        {
            Console.WriteLine("      " + line);
        }
    }
}
