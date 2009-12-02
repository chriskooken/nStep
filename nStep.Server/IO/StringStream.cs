using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace nStep.Server.IO
{
	public class StringStream : MemoryStream
	{
		public StringStream(string str, bool writable) :
			base(Encoding.Unicode.GetBytes(str), writable) { }
	}
}
