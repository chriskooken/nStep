using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucumber.Core.Parsers;
using NUnit.Framework;
using Spart.Scanners;

namespace Specs.Parsers
{
	[TestFixture]
	public class ScenarioParserTest
	{
		[Test]
		public void It_Matches_Single_Line_Scenarios()
		{
			var scenario = "Scenario: Some determinable business situation\n";

			var scenarioMatch = GherkinParser.ScenarioParser.Parse(new StringScanner(scenario));
			scenarioMatch.Success.Should().Be.True();
		}

		[Test]
		public void It_Does_Not_Match_Single_Line_Unterminated_Scenarios()
		{
			var scenario = "Scenario: Some determinable business situation";

			var scenarioMatch = GherkinParser.ScenarioParser.Parse(new StringScanner(scenario));
			scenarioMatch.Success.Should().Be.False();
		}

		[Test]
		public void It_Does_Not_Match_Unterminated_Scenarios()
		{
			var scenario =
@"				Scenario: Some determinable business situation
					Given some precondition
						And some other precondition
					When some action by the actor
						And some other action
						And yet another action
					Then some testable outcome is achieved
						And something else we can check happens too";

			var scenarioMatch = GherkinParser.ScenarioParser.Parse(new StringScanner(scenario));
			scenarioMatch.Success.Should().Be.False();
		}

		[Test]
		public void It_Matches_Well_Formed_Scenarios()
		{
			var scenario =
@"
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
