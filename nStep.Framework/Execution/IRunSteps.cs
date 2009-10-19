using nStep.Framework.StepDefinitions;

namespace nStep.Framework.Execution
{
	public interface IRunSteps
	{
		void RunStep(StepKinds kind, string featureStepToProcess);
	}
}