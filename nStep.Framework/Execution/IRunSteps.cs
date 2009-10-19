using nStep.Framework.StepDefinitions;

namespace nStep.Framework.Execution
{
	public interface IRunSteps
	{
		void ProcessStep(StepKinds kind, string featureStepToProcess);
	}
}