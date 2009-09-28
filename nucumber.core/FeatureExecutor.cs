using System;
using System.Collections.Generic;
using System.Linq;
using Nucumber.Core.Features;
using Nucumber.Framework;
using Nucumber.Framework.ScenarioHooks;

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
        private readonly IFormatOutput console;
        private readonly StepMother stepMother;
        private readonly IEnumerable<BeforeScenarioHook> beforeScenarioHooks;
        private readonly IEnumerable<AfterScenarioHook> afterScenarioHooks;
        bool backgroundDisplayedOnce;

        public FeatureExecutor(IFormatOutput console, StepMother stepMother, IEnumerable<BeforeScenarioHook> beforeScenarioHooks, IEnumerable<AfterScenarioHook> afterScenarioHooks)
        {
            this.console = console;
            this.stepMother = stepMother;
            this.beforeScenarioHooks = beforeScenarioHooks;
            this.afterScenarioHooks = afterScenarioHooks;
        }

        public void ExecuteFeature(Feature feature, int lineNmber)
        {
            switch (feature.WhatIsAtLine(lineNmber))
            {
                case FeatureParts.Feature:
                    ExecuteFeature(feature);
                    break;
                case FeatureParts.Background:
                    console.WriteFeatureHeading(feature);
                    ExecuteBackground(feature);
                    break;
                case FeatureParts.Scenario:
                    console.WriteFeatureHeading(feature);
                    ExecuteScenario(feature.GetScenarioAt(lineNmber), feature);
                    break;
                case FeatureParts.ScenarioOutlineExample:
                    throw new NotImplementedException();
            }

        }

        public void ExecuteFeature(Feature feature)
        {
            console.WriteFeatureHeading(feature);

            foreach (var item in feature.Items)
            {
				// TODO: Remove the assumption that item is a scenario
				if (item is Scenario)
					ExecuteScenario(item as Scenario, feature);
            }
        }

        private void ExecuteScenario(Scenario scenario, Feature feature)
        {
            ExecuteBeforeScenarioHooks(scenario.Tags);
			ExecuteBackground(feature);
            SkippingSteps = false;
            console.WriteScenarioTitle(scenario);
            foreach (var step in scenario.Steps)
            {
                ExecuteStep(step);
            }
            ExecuteAfterScenarioHooks(scenario.Tags, new ScenarioResult(null)); //TODO: Load an appropriate scenarioResult here...
            console.WriteLineBreak();
        }

        private void ExecuteAfterScenarioHooks(IEnumerable<string> tags, ScenarioResult scenarioResult)
        {
            foreach (var hook in afterScenarioHooks)
            {
                if (ShouldHookExecute(hook, tags))
                    hook.Action.Invoke(scenarioResult);
            }
        }

        private void ExecuteBeforeScenarioHooks(IEnumerable<string> tags)
        {
            foreach (var hook in beforeScenarioHooks)
            {
                if (ShouldHookExecute(hook, tags))
                    hook.Action.Invoke();
            }
        }

        private static bool ShouldHookExecute(ScenarioHook hook, IEnumerable<string> tags)
        {
            var shouldExecute = true;
                
            if (hook.Tags.Count()>1)
            {
                shouldExecute = false;
                foreach (var tag in tags)
                {
                    if (!hook.Tags.Contains(tag)) continue;
                    shouldExecute = true;
                    break;
                }
            }
            return shouldExecute;
        }

        private void ExecuteBackground(Feature feature)
        {
			if (feature.Background == null)
				return;

            if (feature.Background.Steps.Count > 0)
                console.WriteBackgroundHeading(feature.Background);

            SkippingSteps = false;
            foreach (var step in feature.Background.Steps)
            {
                ExecuteStep(step);
            }

            console.WriteLineBreak();
        }

        private bool SkippingSteps { get; set; }

        private void ExecuteStep(FeatureStep s)
        {
            stepMother.ChekForMissingStep(s);

            if (SkippingSteps)
            {
                console.WriteSkippedFeatureLine(s);
                return;
            }
            SkippingSteps = true;
            switch (stepMother.ProcessStep(s))
            {
                case StepRunResults.Passed:
                    SkippingSteps = false;
                    console.WritePassedFeatureLine(s, stepMother.LastProcessStepDefinition);
                    break;
                case StepRunResults.Failed:
                    console.WriteException(s, stepMother.LastProcessStepException);
                    break;
                case StepRunResults.Pending:
                    console.WritePendingFeatureLine(s);
                    break;
                case StepRunResults.Missing:
                    console.WritePendingFeatureLine(s);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}