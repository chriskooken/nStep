using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Server.Messages;

namespace nStep.Server
{
	public interface IProcessor
	{
		Response Process(Request request);
	}

	public class Processor : IProcessor
	{
		private bool scenarioInProgress;

		public Response Process(Request request)
		{
			var beginScenarioRequest = request as BeginScenarioRequest;
			var endScenarioRequest = request as EndScenarioRequest;
			var stepMatchesRequest = request as StepMatchesRequest;
			var invokeRequest = request as InvokeRequest;

			if (beginScenarioRequest != null)
				return ProcessBeginScenario();
			if (endScenarioRequest != null)
				return ProcessEndScenario();
			else
				throw new NotImplementedException();
		}

		private Response ProcessEndScenario()
		{
			if (scenarioInProgress)
			{
				scenarioInProgress = false;
				return new SuccessResponse();
			}
			else
			{
				scenarioInProgress = true;
				return new YikesResponse();
			}
		}

		private Response ProcessBeginScenario()
		{
			if (scenarioInProgress)
			{
				scenarioInProgress = false;
				return new YikesResponse();
			}
			else
			{
				scenarioInProgress = true;
				return new SuccessResponse();
			}
		}
	}
}
