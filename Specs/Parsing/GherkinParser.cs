using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucumber.Core.Parsers;
using NUnit.Framework;
using Spart.Parsers;
using Spart.Scanners;

namespace Specs.Parsing {
	[TestFixture]
	public class GherkinParserTest {
		[Test]
		public void dummy()
		{
			var parser = Prims.Rol;
			var text = "AA\n";
			var parserMatch = parser.Parse(new StringScanner(text));
			parserMatch.Success.Should().Be.True();
		}

		[Test]
		public void It_Matches_Comment_Without_Leading_Whitespace()
		{
			var comment = "# Comment\n";
			var parserMatch = GherkinParser.CommentParser.Parse(new StringScanner(comment));
			parserMatch.Success.Should().Be.True();
		}

		[Test]
		public void It_Matches_Comment_With_Leading_Whitespace()
		{
			var comment = "\t\t# Comment\n";
			var parserMatch = GherkinParser.CommentParser.Parse(new StringScanner(comment));
			parserMatch.Success.Should().Be.True();
		}

		[Test]
		public void It_Matches_Steps()
		{
			var given = "\tGiven that the world is flat\n";
			var when = "when: you look at the sun\n";
			var then = "Then you go blind\n";
			var but = "but: The face is the best part\t\n";

			var givenMatch = GherkinParser.StepParser.Parse(new StringScanner(given));
			givenMatch.Success.Should().Be.True();
			var whenMatch = GherkinParser.StepParser.Parse(new StringScanner(when));
			whenMatch.Success.Should().Be.True();
			var thenMatch = GherkinParser.StepParser.Parse(new StringScanner(then));
			thenMatch.Success.Should().Be.True();
			var butMatch = GherkinParser.StepParser.Parse(new StringScanner(but));
			butMatch.Success.Should().Be.True();
		}

		[Test]
		public void It_Does_Not_Match_Unterminated_Steps()
		{
			var given = "\tGiven that the world is flat";

			var givenMatch = GherkinParser.StepParser.Parse(new StringScanner(given));
			givenMatch.Success.Should().Be.False();
		}

		[Test]
		public void It_Matches_Scenarios() {
			var scenario = @"
				Scenario: Some determinable business situation
					Given some precondition
						And some other precondition
					When some action by the actor
						And some other action
						And yet another action
					Then some testable outcome is achieved
						And something else we can check happens too
";


			var scenarioMatch = GherkinParser.ScenarioParser.Parse(new StringScanner(scenario));
			scenarioMatch.Success.Should().Be.True();
		}
	}
}
