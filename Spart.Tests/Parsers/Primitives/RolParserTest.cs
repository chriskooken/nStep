using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Spart.Parsers;
using Spart.Scanners;


namespace Spart.Tests.Parsers.Primitives {
	[TestFixture]
	public class RolParserTest {
		[Test]
		public void It_Should_Match_Newlines()
		{
			var text = "\n";

			var match = Prims.Rol.Parse(new StringScanner(text));
			match.Success.Should().Be.True();
		}

		[Test]
		public void It_Should_Match_Text_And_Newlines()
		{
			var text = "\tblah\n";

			var match = Prims.Rol.Parse(new StringScanner(text));
			match.Success.Should().Be.True();
		}

		[Test]
		public void It_Should_Not_Match_Empty_String()
		{
			var text = "";

			var match = Prims.Rol.Parse(new StringScanner(text));
			match.Success.Should().Be.False();
		}

		[Test]
		public void It_Should_Not_Match_Unterminated_Text()
		{
			var text = "\tblah";

			var match = Prims.Rol.Parse(new StringScanner(text));
			match.Success.Should().Be.False();
		}

		[Test]
		public void It_Should_Not_Match_Unterminated_Multiline_Text()
		{
			var text = "\tblah\nblah";

			var scanner = new StringScanner(text);
			var match = Prims.Rol.Parse(scanner);
			match.Success.Should().Be.True();
			match = Prims.Rol.Parse(scanner);
			match.Success.Should().Be.False();
		}

		[Test]
		public void It_Should_Match_Multiline_Text()
		{
			var text = "\tblah\nblah\n";

			var scanner = new StringScanner(text);
			var match = Prims.Rol.Parse(scanner);
			match.Success.Should().Be.True();
			match = Prims.Rol.Parse(scanner);
			match.Success.Should().Be.True();
		}
	}
}
