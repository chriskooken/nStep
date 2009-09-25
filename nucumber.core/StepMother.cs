using System;
using System.Collections.Generic;
using System.Linq;
using Nucumber.Framework;

namespace Nucumber.Core
{
    public class StepMother : IRunStepsFromStrings
    {
        private readonly IWorldViewDictionary worldViews;
        private IList<StepDefinition> givens;
        private IList<StepDefinition> whens;
        private IList<StepDefinition> thens;
        private IList<TransformDefinition> transforms;
        IList<FeatureStep> failedSteps;
        private IList<FeatureStep> pendingSteps;
        IList<FeatureStep> passedSteps;
        
        public StepMother(IWorldViewDictionary worldViews)
		{
            this.worldViews = worldViews;
            failedSteps = new List<FeatureStep>();
            pendingSteps = new List<FeatureStep>();
            passedSteps = new List<FeatureStep>();
		    givens = new List<StepDefinition>();
            whens = new List<StepDefinition>();
            thens = new List<StepDefinition>();
            transforms = new List<TransformDefinition>();
		}

        public IList<FeatureStep> PassedSteps
        {
            get { return passedSteps; }
            set { passedSteps = value; }
        }

        public IList<FeatureStep> PendingSteps
        {
            get { return pendingSteps; }
            set { pendingSteps = value; }
        }

        public IList<FeatureStep> FailedSteps
        {
            get { return failedSteps; }
            set { failedSteps = value; }
        }

        public void AdoptSteps(IProvideSteps stepSet)
        {
            if (worldViews != null)
                stepSet.WorldView = worldViews[stepSet.WorldViewType];

            givens = givens.Union(stepSet.StepDefinitions.Givens).ToList();
            whens = whens.Union(stepSet.StepDefinitions.Whens).ToList();
            thens = thens.Union(stepSet.StepDefinitions.Thens).ToList();

            transforms = transforms.Union(stepSet.TransformDefinitions).ToList();
        }

        public void AdoptSteps(IEnumerable<IProvideSteps> stepSets)
        {
            foreach (var set in stepSets)
                AdoptSteps(set);
        }

        public Exception LastProcessStepException { get; private set; }

        public StepRunResults LastProcessStepResult { get; private set; }

        public StepDefinition LastProcessStepDefinition { get; private set; }

        public void ProcessStep(StepKinds kind, string featureStepToProcess)
        {
            var stepDefinition = GetStepDefinition(kind, featureStepToProcess);
            ExecuteStepDefinitionWithLine(stepDefinition, featureStepToProcess);
        }

        public void ChekForMissingStep(FeatureStep featureStep)
        {
            try
            {
                GetStepDefinition(featureStep.Kind, featureStep.FeatureLine);
            }
            catch(StepMissingException ex)
            {
                pendingSteps.Add(featureStep);
            }
            catch(Exception e)
            {
                //we dont care about any other exceptions, 
                //they are already handled in the process step
            }
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
            catch (StepMissingException ex)
            {
                pendingSteps.Add(featureStepToProcess);
                LastProcessStepException = ex;
                return StepRunResults.Pending;
            }
            catch (StepPendingException ex)
            {
                LastProcessStepException = ex;
                return StepRunResults.Pending;
            }
            catch (StepAmbiguousException ex)
            {
                LastProcessStepException = ex;
                return StepRunResults.Failed;
            }
            catch (Exception ex)
            {
                if ((ex.InnerException != null) && (typeof(StepPendingException) == ex.InnerException.GetType()))
                {
                    LastProcessStepException = ex.InnerException;
                    return StepRunResults.Pending;
                }
                failedSteps.Add(featureStepToProcess);
                LastProcessStepException = ex.InnerException;
                return StepRunResults.Failed;
            }
           
            passedSteps.Add(featureStepToProcess);
            return StepRunResults.Passed;
        }

        private void ExecuteStepDefinitionWithLine(StepDefinition stepDefinition, string lineText)
        {
            stepDefinition.StepSet.StepFromStringRunner = this;

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
