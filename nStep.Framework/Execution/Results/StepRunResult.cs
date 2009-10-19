using System;
using nStep.Framework.StepDefinitions;

namespace nStep.Framework.Execution.Results
{
	public enum StepRunResultCode
	{
		Passed  = 0,
		Failed  = 1,
		Pending = 2,
		Missing = 3
	}

	public class StepRunResult
	{
		public StepRunResultCode ResultCode { get; set; }
		public StepDefinition MatchedStepDefinition { get; set; }
		public Exception Exception { get; set; }
	}
}