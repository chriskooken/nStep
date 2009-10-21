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

		public Exception LastProcessStepException { get; private set; }
		public StepRunResultCode LastProcessStepResultCode { get; private set; }
		public StepDefinition LastProcessStepDefinition { get; private set; }

		public IList<Step> PassedSteps { get; private set; }
		public IList<Step> PendingSteps { get; private set; }
		public IList<Step> MissingSteps { get; private set; }
		public IList<Step> FailedSteps { get; private set; }

		public StepMother(IWorldViewDictionary worldViews, IScenarioHooksRepository hooksRepository)
		{
			if(worldViews == null)
				throw new ArgumentNullException("world views dictionary cannot be null");
			this.worldViews = worldViews;
			FailedSteps = new List<Step>();
			PendingSteps = new List<Step>();
			MissingSteps = new List<Step>();
			PassedSteps = new List<Step>();
			givens = new List<StepDefinition>();
			whens = new List<StepDefinition>();
			thens = new List<StepDefinition>();
			transforms = new List<TransformDefinition>();
			beforeScenarioHooks = hooksRepository == null ? null : hooksRepository.BeforeScenarioHooks;
			afterScenarioHooks = hooksRepository == null ? null : hooksRepository.AfterScenarioHooks;
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

		public void CheckForMissingStep(Step featureStep)
		{
			try
			{
				GetStepDefinition(featureStep);
			}
			catch(StepMissingException ex)
			{
				MissingSteps.Add(featureStep);
			}
			catch(Exception e)
			{
				//we dont care about any other exceptions, 
				//they are already handled in the process step
			}
		}

		public void RunStep(Step step)
		{
			var stepDefinition = GetStepDefinition(step);
			ExecuteStepDefinitionWithStep(stepDefinition, step);
		}

		public StepRunResult ProcessStep(Step step)
		{
			LastProcessStepException = null;
			LastProcessStepDefinition = null;
	
			try
			{
				LastProcessStepDefinition = GetStepDefinition(step);
				ExecuteStepDefinitionWithStep(LastProcessStepDefinition, step);
			}
			catch (StepMissingException ex)
			{
				MissingSteps.Add(step);
				LastProcessStepException = ex;
				LastProcessStepResultCode = StepRunResultCode.Missing;
			}
			catch (StepPendingException ex)
			{
				FailedSteps.Add(step);
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
					FailedSteps.Add(step);
					LastProcessStepException = ex;
					LastProcessStepResultCode = StepRunResultCode.Failed;
				}
			}
            
			PassedSteps.Add(step);

			CheckForMissingStep(step);
			var result = new StepRunResult { ResultCode = LastProcessStepResultCode, MatchedStepDefinition = LastProcessStepDefinition, Exception = LastProcessStepException };
			return result;
		}

		private void ExecuteStepDefinitionWithStep(StepDefinition stepDefinition, Step step)
		{
			stepDefinition.StepSet.StepRunner = this;

			stepDefinition.StepSet.BeforeStep();
			try
			{
				new StepCaller(stepDefinition,
				               new TypeCaster(this.transforms)).Call(step);
			}
			catch (IndexOutOfRangeException e)
			{
				throw new ParameterMismatchException(
					"The number of paramters is not equal to the number of captured groups in the step definition in",
					stepDefinition.StepSet.GetType().Name + "  on regex \n" + stepDefinition.Regex);
			}

			stepDefinition.StepSet.AfterStep();
		}

		private StepDefinition GetStepDefinition(Step step)
		{
			IEnumerable<StepDefinition> results;
			switch (step.Kind)
			{
				case StepKinds.Given:
					results = givens.Where(definition => definition.Regex.IsMatch(step.Body));
					break;
				case StepKinds.When:
					results = whens.Where(definition => definition.Regex.IsMatch(step.Body));
					break;
				case StepKinds.Then:
					results = thens.Where(definition => definition.Regex.IsMatch(step.Body));
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if (results.Count() == 0) throw new StepMissingException();

			if (results.Count() > 1) throw new StepAmbiguousException(step.Body);

			CheckForEmptyAction(results.First().Action);

			return results.First();
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