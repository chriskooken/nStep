/// Spart License (zlib/png)
/// 
/// 
/// Copyright (c) 2003 Jonathan de Halleux
/// 
/// This software is provided 'as-is', without any express or implied warranty. 
/// In no event will the authors be held liable for any damages arising from 
/// the use of this software.
/// 
/// Permission is granted to anyone to use this software for any purpose, 
/// including commercial applications, and to alter it and redistribute it 
/// freely, subject to the following restrictions:
/// 
/// 1. The origin of this software must not be misrepresented; you must not 
/// claim that you wrote the original software. If you use this software in a 
/// product, an acknowledgment in the product documentation would be 
/// appreciated but is not required.
/// 
/// 2. Altered source versions must be plainly marked as such, and must not be 
/// misrepresented as being the original software.
/// 
/// 3. This notice may not be removed or altered from any source distribution.
/// 
/// Author: Jonathan de Halleux
/// 
/// 9/18/2009: Altered by Adam Moss

using System;

namespace Spart.Tests.Parsers.Primitives
{
	using NUnit.Framework;
	using Spart.Parsers;

	[TestFixture]
	public class StringParserTest
	{
		public String MatchedString
		{
			get { return Provider.Text.Substring(0, 3); }
		}

		public String NonMatchedString
		{
			get { return Provider.Text.Substring(3, 4); }
		}

		[Test]
		public void Constructor()
		{
			var parser = Prims.Str(MatchedString);

			parser.MatchedString.Should().Be.EqualTo(MatchedString);
		}

		[Test]
		public void SuccessParse()
		{
			var scanner = Provider.Scanner;
			var parser = Prims.Str(MatchedString);

			var m = parser.Parse(scanner);
			m.Success.Should().Be.True();
			m.Offset.Should().Be.EqualTo(0);
			m.Length.Should().Be.EqualTo(3);
			m.Scanner.Offset.Should().Be.EqualTo(3);
		}

		[Test]
		public void FailParse()
		{
			var scanner = Provider.Scanner;
			var parser = Prims.Str(NonMatchedString);

			var m = parser.Parse(scanner);
			m.Success.Should().Be.False();
			scanner.Offset.Should().Be.EqualTo(0);
		}
	}
}
