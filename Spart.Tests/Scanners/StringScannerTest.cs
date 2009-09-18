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

namespace Spart.Tests.Scanners
{
	using Spart.Scanners;
	using Spart.Parsers;
	using NUnit.Framework;

	[TestFixture]
	public class StringScannerTest
	{
		public String Text
		{
			get { return Provider.Text; }
		}

		public long Offset
		{
			get { return 5; }
		}

		[Test]
		public void Constructor()
		{
			var scanner = new StringScanner(Text);
			scanner.InputString.Should().Be.EqualTo(Text);
		}


		[Test]
		public void Constructor2()
		{
			var scanner = new StringScanner(Text, Offset);
			scanner.InputString.Should().Be.EqualTo(Text);
			scanner.Offset.Should().Be.EqualTo(Offset);
		}

		[Test]
		public void Substring()
		{
			var scanner = new StringScanner(Text, Offset);
			scanner.Substring(3, 6).Should().Be.EqualTo(Text.Substring(3, 6));
		}

		[Test]
		public void ReadAndPeek()
		{
			var scanner = new StringScanner(Text);
			var i = 0;

			while (!scanner.AtEnd)
			{
				i.Should().Be.LessThan(Text.Length);
				scanner.Peek().Should().Be.EqualTo(Text[i]);
				scanner.Read();
				++i;
			}

			i.Should().Be.EqualTo(Text.Length);
		}

		[Test]
		public void ReadAndPeekOffset()
		{
			var scanner = new StringScanner(Text, Offset);
			var i = (int) Offset;

			while (!scanner.AtEnd)
			{
				i.Should().Be.LessThan(Text.Length);
				scanner.Peek().Should().Be.EqualTo(Text[i]);
				scanner.Read();
				++i;
			}

			i.Should().Be.EqualTo(Text.Length);
		}

		[Test]
		public void Seek()
		{
			var scanner = new StringScanner(Text);
			var i = (int) Offset;
			scanner.Seek(Offset);

			while (!scanner.AtEnd)
			{
				i.Should().Be.LessThan(Text.Length);
				scanner.Peek().Should().Be.EqualTo(Text[i]);
				scanner.Read();
				++i;
			}

			i.Should().Be.EqualTo(Text.Length);
		}

		[Test]
		public void NoMatch()
		{
			var scanner = new StringScanner(Text);
			var m = scanner.NoMatch;
			m.Success.Should().Be.False();
		}

		[Test]
		public void EmptyMatch()
		{
			var scanner = new StringScanner(Text);
			var m = scanner.EmptyMatch;
			m.Success.Should().Be.True();
			m.Empty.Should().Be.True();
		}


		[Test]
		public void Match()
		{
			var scanner = new StringScanner(Text);
			var m = scanner.CreateMatch(Offset, 2);
			m.Success.Should().Be.True();
			m.Empty.Should().Be.False();
			m.Length.Should().Be.EqualTo(2);
			m.Value.Should().Be.EqualTo(Text.Substring((int) Offset, 2));
		}

	}
}
