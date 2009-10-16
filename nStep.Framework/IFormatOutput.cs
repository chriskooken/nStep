using System;
using System.Collections.Generic;
using nStep.Framework.Features;
using nStep.Framework.StepDefinitions;

namespace nStep.Framework
{
	public interface IFormatOutput
	{
		bool SkippingSteps { get; set; }

		void WriteException(Step featureStep, Exception ex);
		void WritePassedFeatureLine(Step featureStep, StepDefinition stepDefinition);
		void WritePendingFeatureLine(Step featureStep, Exception exception);
		void WriteFeatureHeading(Feature feature);
		void WriteResults(StepMother stepMother);
		void WriteScenarioTitle(Scenario scenario);
		void WriteScenarioOutlineTitle(ScenarioOutline scenarioOutline);
		void WriteBackgroundHeading(Background background);
		void WriteSkippedFeatureLine(Step featureStep);
		void WriteLineBreak();
		void WriteMissingFeatureLine(Step step);
	}
}