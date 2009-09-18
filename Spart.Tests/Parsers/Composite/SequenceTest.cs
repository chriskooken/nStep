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


namespace Spart.Tests.Parsers.Composite
{
	using NUnit.Framework;
	using Spart.Parsers;
	using Spart.Parsers.Composite;
	using Spart.Parsers.Primitives;

	[TestFixture]
	public class SequenceTest
	{
		public StringParser First
		{
			get
			{
				return new StringParser( Provider.Text.Substring(0,3));
			}
		}
		public StringParser Second
		{
			get
			{
				return new StringParser( Provider.Text.Substring(3,4));
			}
		}
		public StringParser Second2
		{
			get
			{
				return new StringParser( Provider.Text.Substring(5,2));
			}
		}

		[Test]
		public void Constructor()
		{
			Parser f  = First;
			Parser s = Second;
			var parser = new SequenceParser(f,s);
			parser.FirstParser.Should().Be.EqualTo(f);
			parser.SecondParser.Should().Be.EqualTo(s);
		}

		[Test]
		public void Success()
		{
			var scanner = Provider.Scanner;
			var parser = new SequenceParser(First,Second);

			var m = parser.Parse(scanner);
			m.Success.Should().Be.True();
			m.Offset.Should().Be.EqualTo(0);
			scanner.Offset.Should().Be.EqualTo(m.Offset+m.Length);
			m.Value.Should().Be.EqualTo(First.MatchedString+Second.MatchedString);
		}


		[Test]
		public void FailureFirst()
		{
			var scanner = Provider.Scanner;
			var parser = new SequenceParser(Second,Second2);

			var m = parser.Parse(scanner);
			m.Success.Should().Be.False();
			scanner.Offset.Should().Be.EqualTo(0);
		}

		[Test]
		public void FailureSecond()
		{
			var scanner = Provider.Scanner;
			var parser = new SequenceParser(First,Second2);

			var m = parser.Parse(scanner);
			m.Success.Should().Be.False();
			scanner.Offset.Should().Be.EqualTo(0);
		}
	}
}
