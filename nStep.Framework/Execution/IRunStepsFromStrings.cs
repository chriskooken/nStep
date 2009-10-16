using nStep.Framework.StepDefinitions;

namespace nStep.Framework.Execution
{
	public interface IRunStepsFromStrings
	{
		void ProcessStep(StepKinds kind, string featureStepToProcess);
	}
}