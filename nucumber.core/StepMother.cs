using System;
using System.Collections.Generic;
using System.Linq;
using Nucumber.Framework;

namespace Nucumber.Core
{
    public enum StepRunResults
    {
        Passed,
        Failed,
        Pending,
        Missing
    }
    public class StepMother
    {
		private CombinedStepDefinitions combinedStepDefinitions;

        public StepMother(CombinedStepDefinitions stepDefinitions)
		{
		    this.combinedStepDefinitions = stepDefinitions;
		}

        public Exception LastProcessStepException { get; private set; }

        public StepRunResults LastProcessStepResult { get; private set; }

        public StepDefinition LastProcessStepDefinition { get; private set; }

        public StepRunResults ProcessStep(FeatureStep featureStepToProcess)
        {
            LastProcessStepResult =  DoProcessStep(featureStepToProcess);
            return LastProcessStepResult;
        }

        private StepRunResults DoProcessStep(FeatureStep featureStepToProcess)
        {
            LastProcessStepException = null;
            LastProcessStepDefinition = null;

            var lineText = featureStepToProcess.FeatureLine;
            try
            {
                LastProcessStepDefinition = GetStepDefinition(featureStepToProcess, lineText);

                LastProcessStepDefinition.StepSet.BeforeStep();
                new StepCaller(LastProcessStepDefinition,
                               new TypeCaster()).Call(lineText);
                LastProcessStepDefinition.StepSet.AfterStep();

            }
            catch(StepMissingException ex)
            {
                LastProcessStepException = ex;
                return StepRunResults.Pending;
            }
            catch(StepPendingException ex)
            {
                LastProcessStepException = ex;
                return StepRunResults.Pending;
            }
            catch (Exception ex)
            {
                LastProcessStepException = ex;
                return StepRunResults.Failed;
            }
           
            return StepRunResults.Passed;
        }

        private StepDefinition GetStepDefinition(FeatureStep featureStepToProcess, string lineText)
        {
            IEnumerable<StepDefinition> results;
            switch (featureStepToProcess.Kind)
            {
                case StepKinds.Given:
                    results = combinedStepDefinitions.GivenStepDefinitions.Where(definition => definition.Regex.IsMatch(lineText));
                    break;
                case StepKinds.When:
                    results = combinedStepDefinitions.WhenStepDefinitions.Where(definition => definition.Regex.IsMatch(lineText));
                    break;
                case StepKinds.Then:
                    results = combinedStepDefinitions.ThenStepDefinitions.Where(definition => definition.Regex.IsMatch(lineText));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (results.Count() == 0) throw new StepMissingException();

            if (results.Count() > 1) throw new StepAmbiguousException(lineText);

            return results.First();
        }
    }
}
