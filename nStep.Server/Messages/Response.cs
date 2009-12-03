using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nStep.Server.Messages
{
	public abstract class Response
	{
	}

	public class BeginScenarioResponse : Response
	{ }

	public class EndScenarioResponse : Response
	{ }

	public class StepMatchesResponse : Response
	{
	}
}
