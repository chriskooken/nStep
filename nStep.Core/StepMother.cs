using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using nStep.Core.Exceptions;
using nStep.Core.Features;
using nStep.Framework;
using nStep.Framework.ScenarioHooks;
using nStep.Framework.StepDefinitions;

namespace nStep.Core
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
        private IList<FeatureStep> missingSteps;
        IList<FeatureStep> passedSteps;

		public IEnumerable<BeforeScenarioHook> BeforeScenarioHooks { get; private set; }
		public IEnumerable<AfterScenarioHook> AfterScenarioHooks { get; private set; }


		public StepMother(IWorldViewDictionary worldViews,IScenarioHooksRepository hooksRepository)
		{
            if(worldViews == null)
                throw new ArgumentNullException("world views dictionary cannot be null");
		    this.worldViews = worldViews;
            failedSteps = new List<FeatureStep>();
            pendingSteps = new List<FeatureStep>();
            missingSteps = new List<FeatureStep>();
            passedSteps = new List<FeatureStep>();
		    givens = new List<StepDefinition>();
            whens = new List<StepDefinition>();
            thens = new List<StepDefinition>();
            transforms = new List<TransformDefinition>();
            BeforeScenarioHooks = hooksRepository == null ? null : hooksRepository.BeforeScenarioHooks;
            AfterScenarioHooks = hooksRepository == null ? null : hooksRepository.AfterScenarioHooks;
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
        
        public IList<FeatureStep> MissingSteps
        {
            get { return missingSteps; }
            set { missingSteps = value; }
        }

        public IList<FeatureStep> FailedSteps
        {
            get { return failedSteps; }
            set { failedSteps = value; }
        }

        public void AdoptSteps(IProvideSteps stepSet)
        {
            try
            {
                stepSet.WorldView = worldViews[stepSet.WorldViewType];
            }
            catch (KeyNotFoundException e)
            {
                throw new UnInitializedWorldViewException(stepSet.WorldViewType.Name + " may not have been initialized");
            }
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

        public void CheckForMissingStep(FeatureStep featureStep)
        {
            var lineText = RemoveGivenWhenThensForWholeLineMatching(featureStep.FeatureLine);
            try
            {
                GetStepDefinition(featureStep.Kind, lineText);
            }
            catch(StepMissingException ex)
            {
                missingSteps.Add(featureStep);
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

            var lineText = RemoveGivenWhenThensForWholeLineMatching(featureStepToProcess.FeatureLine);
            try
            {
                LastProcessStepDefinition = GetStepDefinition(featureStepToProcess.Kind, lineText);
                ExecuteStepDefinitionWithLine(LastProcessStepDefinition, lineText);
            }
            catch (StepMissingException ex)
            {
                missingSteps.Add(featureStepToProcess);
                LastProcessStepException = ex;
                return StepRunResults.Missing;
            }
            catch (StepPendingException ex)
            {
                failedSteps.Add(featureStepToProcess);
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
                LastProcessStepException = ex;
                return StepRunResults.Failed;
            }
            
            passedSteps.Add(featureStepToProcess);
            return StepRunResults.Passed;
        }

        private void ExecuteStepDefinitionWithLine(StepDefinition stepDefinition, string lineText)
        {
            stepDefinition.StepSet.StepFromStringRunner = this;

            stepDefinition.StepSet.BeforeStep();
            try
            {
                new StepCaller(stepDefinition,
                               new TypeCaster(this.transforms)).Call(lineText);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ParameterMismatchException(
                    "The number of paramters is not equal to the number of captured groups in the step definition in",
                    stepDefinition.StepSet.GetType().Name + "  on regex \n" + stepDefinition.Regex);
            }

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

            CheckForEmptyAction(results.First().Action);

            return results.First();
        }

        string RemoveGivenWhenThensForWholeLineMatching(string text)
        {
            var regexPattern = "^(Given|When|Then|And|But|given:|when:|then:|and:|but:)(.*)";
            var regex = new Regex(regexPattern);
            return regex.Match(text).Groups[2].Value.Trim();
        }

        private static void CheckForEmptyAction(Delegate action)
        {
            var bytes = action.Method.GetMethodBody().GetILAsByteArray();
            var emptyMethodWithDebugSymbols = new byte[] { 0, 42 };
            var emptyMethodWithoutDebugSymbols = new byte[] { 42 };


            if (bytes.SequenceEqual(emptyMethodWithDebugSymbols) || bytes.SequenceEqual(emptyMethodWithoutDebugSymbols))
                throw new StepPendingException();
        }
    }
}
