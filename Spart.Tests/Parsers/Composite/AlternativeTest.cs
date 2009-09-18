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
	using Spart.Parsers.NonTerminal;

	[TestFixture]
	public class AlternativeTest
	{
		[Test]
		public void FirstMatch()
		{
			var d = new Rule();
			var l = new Rule();
			d.Parser = Prims.Digit;
			l.Parser = Prims.Letter;
			var rp = l|d;			
			var scan = Provider.Scanner;
			var m = rp.Parse(scan);

			m.Success.Should().Be.True();
			m.Length.Should().Be.EqualTo(1);
			scan.Offset.Should().Be.EqualTo(1);
		}

		[Test]
		public void SecondMatch()
		{
			var d = Rule.AssignParser(null,Prims.Digit);
			var l = Rule.AssignParser(null,Prims.Letter);
			var rp = d|l;			
			var scan = Provider.Scanner;
			var m = rp.Parse(scan);

			m.Success.Should().Be.True();
			m.Length.Should().Be.EqualTo(1);
			scan.Offset.Should().Be.EqualTo(1);
		}

		[Test]
		public void ThirdMatch()
		{
			var d = Rule.AssignParser(null,Prims.Digit);
			var l = Rule.AssignParser(null,Prims.Letter);
			var rp = d|d|l;			
			var scan = Provider.Scanner;
			var m = rp.Parse(scan);

			m.Success.Should().Be.True();
			m.Length.Should().Be.EqualTo(1);
			scan.Offset.Should().Be.EqualTo(1);
		}

		[Test]
		public void NoMatchMatch()
		{
			var d = Rule.AssignParser(null,Prims.Digit);
			var rp = d|d|d;			
			var scan = Provider.Scanner;
			var m = rp.Parse(scan);

			m.Success.Should().Be.False();
			scan.Offset.Should().Be.EqualTo(0);
		}
	}
}
