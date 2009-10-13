using nStep.Framework.StepDefinitions;

namespace nStep.Framework
{
    public interface IRunStepsFromStrings
    {
        void ProcessStep(StepKinds kind, string featureStepToProcess);
    }
}
