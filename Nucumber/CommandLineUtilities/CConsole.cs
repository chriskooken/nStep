using System;
using Nucumber.Core;

namespace Nucumber.App.CommandLineUtilities
{
    public class CConsole : IConsoleWriter 
    {
        private string LevelPad(int indent, string text)
        {
            switch (indent)
            {
                case 1:
                    return text;
                default:
                    return string.Empty.PadLeft(indent * 3) + text;
            }
        }
        public void WriteLineLevel1(string line)
        {
            Console.WriteLine(LevelPad(1, line));
        }

        public void WriteLineLevel2(string line)
        {
            Console.WriteLine(LevelPad(2, line));
        }

        public void WriteLineLevel3(string line)
        {
            Console.WriteLine(LevelPad(3, line));
        }

        public void WriteLineLevel4(string line)
        {
            Console.WriteLine(LevelPad(4, line));
        }
		
		public void WriteException(Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			WriteLineLevel3(ex.Message);
			WriteLineLevel4(ex.StackTrace);
			Console.ForegroundColor = ConsoleColor.Gray;	
		}

        public void WriteLineAtLevel(int level, string line)
        {
            Console.WriteLine(LevelPad(level, line));
        }
    }
}