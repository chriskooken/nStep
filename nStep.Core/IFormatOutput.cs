using System;
using System.Collections.Generic;
using nStep.Core.Features;
using nStep.Framework.StepDefinitions;

namespace nStep.Core
{
	public interface IFormatOutput
	{
		bool SkippingSteps { get; set; }

        void WriteException(FeatureStep featureStep, Exception ex);
        void WritePassedFeatureLine(FeatureStep featureStep, StepDefinition stepDefinition);
        void WritePendingFeatureLine(FeatureStep featureStep, Exception exception);
	    void WriteFeatureHeading(Feature feature);
	    void WriteResults(StepMother stepMother);
	    void WriteScenarioTitle(Scenario scenario);
		void WriteScenarioOutlineTitle(ScenarioOutline scenarioOutline);
	    void WriteBackgroundHeading(Background background);
	    void WriteSkippedFeatureLine(FeatureStep featureStep);
	    void WriteLineBreak();
	    void WriteMissingFeatureLine(FeatureStep step);
	}
}
