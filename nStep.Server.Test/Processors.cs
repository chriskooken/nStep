using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Server.Messages;
using NUnit.Framework;

namespace nStep.Server.Test
{
	[TestFixture]
	public class Processors
	{
		#region Beginning and Ending Scenarios

		[Test]
		public void Single_BeginScenario_Responds_With_Success()
		{
			var processor = new Processor();
			var request = new BeginScenarioRequest();
			var response = processor.Process(request);

			response.Should().Be.InstanceOf<SuccessResponse>();
		}

		[Test]
		public void Nested_BeginScenario_Responds_With_Yikes()
		{
			var processor = new Processor();
			var request = new BeginScenarioRequest();
			processor.Process(request);
			var response = processor.Process(request);

			response.Should().Be.InstanceOf<YikesResponse>();
		}

		[Test]
		public void BeginScenario_Then_EndScenario_Responds_With_Success()
		{
			var processor = new Processor();
			var beginRequest = new BeginScenarioRequest();
			var endRequest = new EndScenarioRequest();
			processor.Process(beginRequest);
			var response = processor.Process(endRequest);

			response.Should().Be.InstanceOf<SuccessResponse>();
		}

		[Test]
		public void EndScenario_Without_BeginScenario_Responds_With_Yikes()
		{
			var processor = new Processor();
			var endRequest = new EndScenarioRequest();
			var response = processor.Process(endRequest);

			response.Should().Be.InstanceOf<YikesResponse>();
		}

		#endregion
	}
}
