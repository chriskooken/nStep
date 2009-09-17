using System;
using Nucumber.Framework;

namespace Nucumber.Core
{
	public interface IConsoleWriter
	{
        void WriteException(FeatureStep featureStep, Exception ex);
        void WritePassedFeatureLine(FeatureStep featureStep, StepDefinition stepDefinition);
        void WritePendingFeatureLine(FeatureStep featureStep);
	    void WriteFeatureHeading(Feature feature);
	    void Complete();
	    void WriteScenarioTitle(Scenario scenario);
	    void WriteBackgroundHeading(Scenario background);
	    void WriteSkippedFeatureLine(FeatureStep featureStep);
	}
}
