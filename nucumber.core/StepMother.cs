using System;
using System.Collections.Generic;
using System.Linq;
using Nucumber.Framework;

namespace Nucumber.Core
{
    public class StepMother : IRunStepsFromStrings
    {
        private IList<StepDefinition> givens;
        private IList<StepDefinition> whens;
        private IList<StepDefinition> thens;
        private IList<TransformDefinition> transforms;
        
        public StepMother()
		{
		    givens = new List<StepDefinition>();
            whens = new List<StepDefinition>();
            thens = new List<StepDefinition>();
            transforms = new List<TransformDefinition>();
		}

        public void ImportSteps(IProvideSteps stepSet)
        {
            givens = givens.Union(stepSet.StepDefinitions.Givens).ToList();
            whens = whens.Union(stepSet.StepDefinitions.Whens).ToList();
            thens = thens.Union(stepSet.StepDefinitions.Thens).ToList();

            transforms = transforms.Union(stepSet.TransformDefinitions).ToList();
        }

        public void ImportSteps(IEnumerable<IProvideSteps> stepSets)
        {
            foreach (var set in stepSets)
                ImportSteps(set);
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
                    results = givens.Where(definition => definition.Regex.IsMatch(lineText));
                    break;
                case StepKinds.When:
                    results = whens.Where(definition => definition.Regex.IsMatch(lineText));
                    break;
                case StepKinds.Then:
                    results = thens.Where(definition => definition.Regex.IsMatch(lineText));
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
