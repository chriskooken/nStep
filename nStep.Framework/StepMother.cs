using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using nStep.Framework.Exceptions;
using nStep.Framework.Execution;
using nStep.Framework.Execution.Results;
using nStep.Framework.Features;
using nStep.Framework.ScenarioHooks;
using nStep.Framework.StepDefinitions;
using nStep.Framework.WorldViews;

namespace nStep.Framework
{
	public class StepMother : IRunSteps, IProcessSteps, IProcessScenarioHooks
	{
		private readonly IWorldViewDictionary worldViews;
		private IList<StepDefinition> givens;
		private IList<StepDefinition> whens;
		private IList<StepDefinition> thens;
		private IList<TransformDefinition> transforms;
		private IEnumerable<BeforeScenarioHook> beforeScenarioHooks;
		private IEnumerable<AfterScenarioHook> afterScenarioHooks;
		private IList<Step> failedSteps;
		private IList<Step> pendingSteps;
		private IList<Step> missingSteps;
		private IList<Step> passedSteps;


		public StepMother(IWorldViewDictionary worldViews, IScenarioHooksRepository hooksRepository)
		{
			if(worldViews == null)
				throw new ArgumentNullException("world views dictionary cannot be null");
			this.worldViews = worldViews;
			failedSteps = new List<Step>();
			pendingSteps = new List<Step>();
			missingSteps = new List<Step>();
			passedSteps = new List<Step>();
			givens = new List<StepDefinition>();
			whens = new List<StepDefinition>();
			thens = new List<StepDefinition>();
			transforms = new List<TransformDefinition>();
			beforeScenarioHooks = hooksRepository == null ? null : hooksRepository.BeforeScenarioHooks;
			afterScenarioHooks = hooksRepository == null ? null : hooksRepository.AfterScenarioHooks;
		}

		public IList<Step> PassedSteps
		{
			get { return passedSteps; }
			set { passedSteps = value; }
		}

		public IList<Step> PendingSteps
		{
			get { return pendingSteps; }
			set { pendingSteps = value; }
		}    
        
		public IList<Step> MissingSteps
		{
			get { return missingSteps; }
			set { missingSteps = value; }
		}

		public IList<Step> FailedSteps
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

		public StepRunResultCode LastProcessStepResultCode { get; private set; }

		public StepDefinition LastProcessStepDefinition { get; private set; }

		public void RunStep(Step step)
		{
			var stepDefinition = GetStepDefinition(step.Kind, step.Body);
			ExecuteStepDefinitionWithLine(stepDefinition, step.Body);
		}

		public void CheckForMissingStep(Step featureStep)
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

		public StepRunResult ProcessStep(Step featureStepToProcess)
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
				LastProcessStepResultCode = StepRunResultCode.Missing;
			}
			catch (StepPendingException ex)
			{
				failedSteps.Add(featureStepToProcess);
				LastProcessStepException = ex;
				LastProcessStepResultCode = StepRunResultCode.Pending;
			}
			catch (StepAmbiguousException ex)
			{
				LastProcessStepException = ex;
				LastProcessStepResultCode = StepRunResultCode.Failed;
			}
			catch (Exception ex)
			{
				if ((ex.InnerException != null) && (typeof(StepPendingException) == ex.InnerException.GetType()))
				{
					LastProcessStepException = ex.InnerException;
					LastProcessStepResultCode = StepRunResultCode.Pending;
				}
				else
				{
					failedSteps.Add(featureStepToProcess);
					LastProcessStepException = ex;
					LastProcessStepResultCode = StepRunResultCode.Failed;
				}
			}
            
			passedSteps.Add(featureStepToProcess);

			CheckForMissingStep(featureStepToProcess);
			var result = new StepRunResult { ResultCode = LastProcessStepResultCode, MatchedStepDefinition = LastProcessStepDefinition, Exception = LastProcessStepException };
			return result;
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

		public void ProcessAfterScenarioHooks(IEnumerable<string> tags, ScenarioResult result)
		{
			foreach (var hook in afterScenarioHooks)
			{
				if (ShouldHookExecute(hook, tags))
					hook.Action.Invoke(result);
			}
		}

		public void ProcessBeforeScenarioHooks(IEnumerable<string> tags)
		{
			foreach (var hook in beforeScenarioHooks)
			{
				if (ShouldHookExecute(hook, tags))
					hook.Action.Invoke();
			}
		}

		protected static bool ShouldHookExecute(ScenarioHook hook, IEnumerable<string> tags)
		{
			var shouldExecute = true;

			if (hook.Tags == null) return true;

			if (hook.Tags.Count() > 1)
			{
				shouldExecute = false;
				if (tags == null) return false;
				foreach (var tag in tags)
				{
					if (!hook.Tags.Contains(tag)) continue;
					shouldExecute = true;
					break;
				}
			}
			return shouldExecute;
		}
	}
}