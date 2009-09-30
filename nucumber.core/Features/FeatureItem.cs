using System.Collections.Generic;
using System.Linq;
using Nucumber.Framework;
using Nucumber.Framework.ScenarioHooks;

namespace Nucumber.Core.Features
{
	public abstract class FeatureItem : StepSequence
	{
		public int LineNumber { get; set; }
		public Feature Feature { get; set; }

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
