using System;
using System.Collections.Generic;
using Nucumber.Core.Features;
using Nucumber.Framework;

namespace Nucumber.Core
{
	public interface IFormatOutput
	{
        void WriteException(FeatureStep featureStep, Exception ex);
        void WritePassedFeatureLine(FeatureStep featureStep, StepDefinition stepDefinition);
        void WritePendingFeatureLine(FeatureStep featureStep);
	    void WriteFeatureHeading(Feature feature);
	    void WriteResults(StepMother stepMother);
	    void WriteScenarioTitle(Scenario scenario);
	    void WriteBackgroundHeading(Scenario background);
	    void WriteSkippedFeatureLine(FeatureStep featureStep);
	    void WriteLineBreak();
	}
}
