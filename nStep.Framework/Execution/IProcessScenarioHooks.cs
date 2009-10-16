using System.Collections.Generic;
using nStep.Framework.Execution.Results;

namespace nStep.Framework.Execution
{
	public interface IProcessScenarioHooks
	{
		void ProcessAfterScenarioHooks(IEnumerable<string> tags, ScenarioResult result);
		void ProcessBeforeScenarioHooks(IEnumerable<string> tags);
	}
}
