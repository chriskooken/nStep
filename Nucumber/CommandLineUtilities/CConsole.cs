using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nucumber.core;

namespace nucumber.app.CommandLineUtilities
{
    public class CConsole : IConsoleWriter 
    {
        public void WriteLineLevel1(string line)
        {
            Console.WriteLine(line);
        }

        public void WriteLevel1(string text)
        {
            Console.Write(text);
        }

        public void WriteLineLevel2(string line)
        {
            Console.WriteLine("  " + line);
        }

        public void WriteLineLevel3(string line)
        {
            Console.WriteLine("    " + line);
        }

        public void WriteLineLevel4(string line)
        {
            Console.WriteLine("      " + line);
        }
		
		public void WriteException(Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			WriteLineLevel3(ex.Message);
			WriteLineLevel4(ex.StackTrace);
			Console.ForegroundColor = ConsoleColor.Gray;	
		}
    }
}