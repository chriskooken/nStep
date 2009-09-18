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

namespace Spart.Tests.Parsers.Composite
{
	using NUnit.Framework;
	using Spart.Parsers;
	using Spart.Parsers.Composite;
	using Spart.Scanners;

	[TestFixture]
	public class RepetitionTest
	{
		public Parser Parser
		{
			get { return Prims.Ch('a'); }
		}

		[Test]
		public void Constructor()
		{
			var p = Parser;
			var rp = new RepetitionParser(p, 10, 20);
			rp.LowerBound.Should().Be.EqualTo(10);
			rp.UpperBound.Should().Be.EqualTo(20);
			rp.Parser.Should().Be.EqualTo(p);
		}

		[Test]
		public void Constructor2()
		{
			new Action(() => new RepetitionParser(null, 0, 1)).Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void Constructor3()
		{
			new Action(() => new RepetitionParser(Parser, 1, 0)).Should().Throw<ArgumentException>();
		}

		[Test]
		public void PositiveSuccess1AtEnd()
		{
			var rp = +Parser;
			var s = "a";
			var scan = new StringScanner(s);
			var m = rp.Parse(scan);
			m.Success.Should().Be.True();
			m.Length.Should().Be.EqualTo(1);
			scan.AtEnd.Should().Be.True();
		}

		[Test]
		public void PositiveSuccess2AtEnd()
		{
			var rp = +Parser;
			var s = "aa";
			var scan = new StringScanner(s);
			var m = rp.Parse(scan);
			m.Success.Should().Be.True();
			m.Length.Should().Be.EqualTo(2);
			scan.AtEnd.Should().Be.True();
		}

		[Test]
		public void PositiveSuccessNotAtEnd()
		{
			var rp = +Parser;
			var s = "aaa ";
			var scan = new StringScanner(s);
			var m = rp.Parse(scan);
			m.Success.Should().Be.True();
			m.Length.Should().Be.EqualTo(3);
			scan.AtEnd.Should().Be.False();
		}

		[Test]
		public void PositiveFailure()
		{
			var rp = +Parser;
			var s = "b";
			var scan = new StringScanner(s);
			var m = rp.Parse(scan);
			m.Success.Should().Be.False();
		}

		[Test]
		public void KleneeSuccess1AtEnd()
		{
			var rp = Ops.Klenee(Parser);
			var s = "a";
			var scan = new StringScanner(s);
			var m = rp.Parse(scan);
			m.Success.Should().Be.True();
			m.Length.Should().Be.EqualTo(1);
			scan.AtEnd.Should().Be.True();
		}

		[Test]
		public void KleneeSuccess2AtEnd()
		{
			var rp = Ops.Klenee(Parser);
			var s = "aa";
			var scan = new StringScanner(s);
			var m = rp.Parse(scan);
			m.Success.Should().Be.True();
			m.Length.Should().Be.EqualTo(2);
			scan.AtEnd.Should().Be.True();
		}

		[Test]
		public void KleneeSuccessNotAtEnd()
		{
			var rp = Ops.Klenee(Parser);
			var s = "aaa ";
			var scan = new StringScanner(s);
			var m = rp.Parse(scan);
			m.Success.Should().Be.True();
			m.Length.Should().Be.EqualTo(3);
			scan.AtEnd.Should().Be.False();
		}

		[Test]
		public void KleneeSuccess0()
		{
			var rp = Ops.Klenee(Parser);
			var s = "b";
			var scan = new StringScanner(s);
			var m = rp.Parse(scan);
			m.Success.Should().Be.True();
			m.Empty.Should().Be.True();
			scan.AtEnd.Should().Be.False();
		}

		[Test]
		public void OptionalSuccess()
		{
			var rp = !Parser;
			var s = "aa";
			var scan = new StringScanner(s);
			var m = rp.Parse(scan);
			m.Success.Should().Be.True();
			m.Length.Should().Be.EqualTo(1);
			scan.Offset.Should().Be.EqualTo(1);
		}

		[Test]
		public void OptionalSuccess0()
		{
			var rp = !Parser;
			var s = "";
			var scan = new StringScanner(s);
			var m = rp.Parse(scan);
			m.Success.Should().Be.True();
			m.Empty.Should().Be.True();
		}
	}
}
