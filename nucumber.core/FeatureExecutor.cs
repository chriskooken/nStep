using System;
using System.Linq;

namespace Nucumber.Core
{
    public enum FeatureParts
    {
        Feature,
        Background,
        Scenario,
        ScenarioOutlineExample
    }
    public class FeatureExecutor
    {
        private readonly IFormatOutput Console;
        private readonly StepMother StepMother;
        bool backgroundDisplayedOnce;

        public FeatureExecutor(IFormatOutput console, StepMother stepMother)
        {
            this.Console = console;
            this.StepMother = stepMother;
        }

        public void ExecuteFeature(Feature feature, int lineNmber)
        {
            switch (feature.WhatIsAtLine(lineNmber))
            {
                case FeatureParts.Feature:
                    ExecuteFeature(feature);
                    break;
                case FeatureParts.Background:
                    Console.WriteFeatureHeading(feature);
                    ExecuteBackground(feature);
                    break;
                case FeatureParts.Scenario:
                    Console.WriteFeatureHeading(feature);
                    ExecuteScenario(feature.GetScenarioAt(lineNmber), feature);
                    break;
                case FeatureParts.ScenarioOutlineExample:
                    throw new NotImplementedException();
            }

        }

        public void ExecuteFeature(Feature feature)
        {
            Console.WriteFeatureHeading(feature);

            foreach (var scenario in feature.Scenarios)
            {
                ExecuteScenario(scenario, feature);
            }
        }

        private void ExecuteScenario(Scenario scenario, Feature feature)
        {
            ExecuteBackground(feature);
            SkippingSteps = false;
            Console.WriteScenarioTitle(scenario);
            foreach (var step in scenario.Steps)
            {
                ExecuteStep(step);
            }

            Console.WriteLineBreak();
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

            Console.WriteLineBreak();
        }

        private bool SkippingSteps { get; set; }

        private void ExecuteStep(FeatureStep s)
        {
            StepMother.ChekForMissingStep(s);

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