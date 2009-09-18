namespace Nucumber.Framework
{
    public interface IRunStepsFromStrings
    {
        void ProcessStep(StepKinds kind, string featureStepToProcess);
    }
}
