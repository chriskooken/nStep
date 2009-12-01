using nStep.Framework.StepDefinitions;
using nStep.Framework.Steps;

namespace nStep.Framework.Execution
{
	public interface IRunSteps
	{
		void RunStep(Step step);
	}
}