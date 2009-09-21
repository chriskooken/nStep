using Spart.Scanners;
using Spart.Actions;
using Spart.Parsers.NonTerminal;

namespace Spart.Parsers.Primitives {
	public class RolParser : TerminalParser {
		public override ParserMatch ParseMain(IScanner scan) {
			long offset = scan.Offset;
			int len = 0;
			bool lineTerminated = false;

			while(!scan.AtEnd && scan.Peek() != '\r' && scan.Peek() != '\n')
			{
				scan.Read();
				len++;
			}

			if (!scan.AtEnd && scan.Peek() == '\r')    // CR
			{
				scan.Read();
				len++;
				lineTerminated = true;
			}

			if (!scan.AtEnd && scan.Peek() == '\n')    // LF
			{
				scan.Read();
				len++;
				lineTerminated = true;
			}

			if (len > 0 && lineTerminated) {
				ParserMatch m = scan.CreateMatch(offset, len);
				return m;
			}

			scan.Seek(offset);
			return scan.NoMatch;
		}
	}
}
