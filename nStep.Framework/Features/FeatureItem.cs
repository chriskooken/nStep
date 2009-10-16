using System.Collections.Generic;
using System.Linq;
using nStep.Framework.Execution.Results;
using nStep.Framework.ScenarioHooks;

namespace nStep.Framework.Features
{
	public abstract class FeatureItem : StepSequence
	{
		#region Properties

		public Feature Feature { get; set; }
		public IEnumerable<string> Tags { get; set; }

		#endregion


		public FeatureItem(IList<FeatureStep> steps)
			: base(steps)
		{ }


		protected static void ExecuteAfterScenarioHooks(IEnumerable<string> tags, StepMother stepMother, ScenarioResult scenarioResult)
		{
			foreach (var hook in stepMother.AfterScenarioHooks)
			{
				if (ShouldHookExecute(hook, tags))
					hook.Action.Invoke(scenarioResult);
			}
		}

		protected static void ExecuteBeforeScenarioHooks(IEnumerable<string> tags, StepMother stepMother)
		{
			foreach (var hook in stepMother.BeforeScenarioHooks)
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