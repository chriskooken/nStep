using System;

namespace Nucumber.Core
{
    public class FeatureExecutor
    {
        private readonly IConsoleWriter Console;
        private readonly StepMother StepMother;
        bool backgroundDisplayedOnce;

        public FeatureExecutor(IConsoleWriter console, StepMother stepMother)
        {
            this.Console = console;
            this.StepMother = stepMother;
        }

        public void ExecuteFeature(Feature feature)
        {
            Console.WriteFeatureHeading(feature);

            ExecuteBackground(feature);

            foreach (var scenario in feature.Scenarios)
            {
                ExecuteScenario(scenario);
            }
        }

        private void ExecuteScenario(Scenario scenario)
        {
            SkippingSteps = false;
            Console.WriteScenarioTitle(scenario);
            foreach (var step in scenario.Steps)
            {
                ExecuteStep(step);
            }
        }

        private void ExecuteBackground(Feature feature)
        {
            if (feature.Background.Steps.Count > 0)
                Console.WriteBackgroundHeading(feature.Background);

            SkippingSteps = false;
            foreach (var step in feature.Background.Steps)
            {
                ExecuteStep(step);
            }
        }

        private bool SkippingSteps { get; set; }

        private void ExecuteStep(FeatureStep s)
        {
            if (SkippingSteps)
            {
                Console.WriteSkippedFeatureLine(s);
                return;
            }
            SkippingSteps = true;
            switch (StepMother.ProcessStep(s))
            {
                case StepRunResults.Passed:
                    SkippingSteps = false;
                    Console.WritePassedFeatureLine(s, StepMother.LastProcessStepDefinition);
                    break;
                case StepRunResults.Failed:
                    Console.WriteException(s, StepMother.LastProcessStepException);
                    break;
                case StepRunResults.Pending:
                    Console.WritePendingFeatureLine(s);
                    break;
                case StepRunResults.Missing:
                    Console.WritePendingFeatureLine(s);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}