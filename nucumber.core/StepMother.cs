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

        public StepMother( CombinedStepDefinitions stepDefinitions)
		{
		    this.combinedStepDefinitions = stepDefinitions;
		}

        public Exception LastProcessStepException { get; private set; }

        public StepRunResults ProcessStep(FeatureStep featureStepToProcess)
        {
            LastProcessStepException = null;

            var lineText = featureStepToProcess.FeatureLine;
            try
            {
               new StepCaller(GetPossibleStepDefinitions(featureStepToProcess, lineText),
                    new TypeCaster()).Call(lineText);
            }
            catch (Exception ex)
            {
                LastProcessStepException = ex;
                return StepRunResults.Failed;
            }
           
            return StepRunResults.Passed;
        }

        private StepDefinition GetPossibleStepDefinitions(FeatureStep featureStepToProcess, string lineText)
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

            if (results.Count() == 0) throw new MissingStepException(lineText);

            if (results.Count() > 1) throw new AmbiguousStepException(lineText);

            return results.First();
        }
    }
}
