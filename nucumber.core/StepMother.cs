using System;
using System.Collections.Generic;
using System.Linq;
using Nucumber.Framework;

namespace Nucumber.Core
{
    public class StepMother : IRunStepsFromStrings
    {
		private CombinedStepDefinitions combinedStepDefinitions;

        public StepMother(CombinedStepDefinitions stepDefinitions)
		{
		    this.combinedStepDefinitions = stepDefinitions;
		}

        public Exception LastProcessStepException { get; private set; }

        public StepRunResults LastProcessStepResult { get; private set; }

        public StepDefinition LastProcessStepDefinition { get; private set; }

        public void ProcessStep(StepKinds kind, string featureStepToProcess)
        {
            var stepDefinition = GetStepDefinition(kind, featureStepToProcess);
            ExecuteStepDefinitionWithLine(stepDefinition, featureStepToProcess);
        }

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
                LastProcessStepDefinition = GetStepDefinition(featureStepToProcess.Kind, lineText);
                ExecuteStepDefinitionWithLine(LastProcessStepDefinition, lineText);

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

        private void ExecuteStepDefinitionWithLine(StepDefinition stepDefinition, string lineText)
        {
            stepDefinition.StepSet.SetStepFromStringRunner(this);

            stepDefinition.StepSet.BeforeStep();
            new StepCaller(stepDefinition,
                           new TypeCaster()).Call(lineText);
            stepDefinition.StepSet.AfterStep();
        }

        private StepDefinition GetStepDefinition(StepKinds stepKind, string lineText)
        {
            IEnumerable<StepDefinition> results;
            switch (stepKind)
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
