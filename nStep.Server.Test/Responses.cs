using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jayrock.Json.Conversion;
using nStep.Server.Messages;
using NUnit.Framework;

namespace nStep.Server.Test
{
	[TestFixture]
	public class Responses
	{
		[Test]
		public void Success_Generates_Correct_JsonText()
		{
			var successResponse = new SuccessResponse();
			successResponse.JsonText.Should().Be.EqualTo("[\"success\",null]");
		}

		[Test]
		public void Yikes_Generates_Correct_JsonText()
		{
			var yikesResponse = new YikesResponse();
			yikesResponse.JsonText.Should().Be.EqualTo("[\"yikes\"]");
		}

		[Test]
		public void StepMatches_Generates_Correct_JsonText()
		{
			var stepId = Guid.NewGuid();
			var arguments = new Dictionary<string, string> { { "ProviderLocation", "Lauren's Office" },
															 { "Kiosk", "Kiosk 1" } };
			var stepMatchesResponse = new StepMatchesResponse(stepId, arguments);

			var argumentString = JsonConvert.ExportToString(arguments);
			var json = string.Format("[\"step_matches\",[{{\"id\":\"{0}\",\"args\":[{1}]}}]]", stepId, argumentString);

			stepMatchesResponse.JsonText.Should().Be.EqualTo(json);
		}

		[Test]
		public void StepFailed_Generates_Correct_JsonText()
		{
			var message = "The wires are down";
			var stepFailedResponse = new StepFailedResponse(message);

			var json = string.Format("[\"step_failed\",{{\"message\":\"{0}\"}}]", message);

			stepFailedResponse.JsonText.Should().Be.EqualTo(json);
		}
	}
}
