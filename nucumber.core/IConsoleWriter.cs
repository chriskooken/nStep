
using System;

namespace nucumber.core
{
	public interface IConsoleWriter
	{
		void WriteLineLevel1(string line);
		void WriteLineLevel2(string line);
		void WriteLineLevel3(string line);
		void WriteLineLevel4(string line);
		void WriteException(Exception ex);
	}
}
