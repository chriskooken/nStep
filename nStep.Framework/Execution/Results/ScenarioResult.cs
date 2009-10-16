using System;

namespace nStep.Framework.Execution.Results
{
	public class ScenarioResult
	{
		public ScenarioResult(Exception exception)
		{
			Exception = exception;
			Passed = true;
			if (Exception == null) return;
			Passed = false;
			Failed = true;
		}
		public bool Failed { get; private set; }
		public bool Passed { get; private set; }
		public Exception Exception { get; private set; }
	}
}