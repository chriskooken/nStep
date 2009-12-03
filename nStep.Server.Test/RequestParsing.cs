using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Server.Messages;
using NUnit.Framework;

namespace nStep.Server.Test
{
	[TestFixture]
	public class RequestParsing
	{
		[Test]
		public void It_Can_Parse_BeginScenario()
		{
			var strRequest = "[\"begin_scenario\",null]";
			var request = Request.ParseFromJson(strRequest);

			request.Should().Be.OfType<BeginScenarioRequest>();
		}

		[Test]
		public void It_Can_Parse_EndScenario()
		{
			var strRequest = "[\"end_scenario\",null]";
			var request = Request.ParseFromJson(strRequest);

			request.Should().Be.OfType<EndScenarioRequest>();
		}

		[Test]
		public void It_Can_Parse_StepMatches()
		{
			var stepName = "we're all wired";
			var strRequest = string.Format("[\"step_matches\",{{\"name_to_match\":\"{0}\"}}]", stepName);
			var request = Request.ParseFromJson(strRequest);

			request.Should().Be.OfType<StepMatchesRequest>();
			(request as StepMatchesRequest).NameToMatch.Should().Be.EqualTo(stepName);
		}

		[Test]
		public void It_Can_Parse_Invoke()
		{
			var stepId = new Guid("65E1B906-F21B-4354-888B-3983C80DEF47");
			var args = new[] { "wired" };
			var strRequest = string.Format("[\"invoke\",{{\"id\":\"{0}\",\"args\":[{1}]}}]", stepId, string.Join(",", args));
			var request = Request.ParseFromJson(strRequest);

			request.Should().Be.OfType<InvokeRequest>();
			(request as InvokeRequest).StepId.Should().Be.EqualTo(stepId);
			(request as InvokeRequest).Arguments.Should().Be.SubsetOf(args);
			args.Should().Be.SubsetOf((request as InvokeRequest).Arguments);
		}
	}
}
