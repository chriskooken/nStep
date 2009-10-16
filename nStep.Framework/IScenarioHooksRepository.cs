using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Framework.ScenarioHooks;

namespace nStep.Framework
{
	public interface IScenarioHooksRepository
	{
		BeforeScenarioHookList BeforeScenarioHooks { get; set; }
		AfterScenarioHookList AfterScenarioHooks { get; set; }
	}

	public class ScenarioHooksRepository : IScenarioHooksRepository
	{
		public ScenarioHooksRepository(IEnumerable<IProvideSteps> stepSets)
		{
			BeforeScenarioHooks = new BeforeScenarioHookList();
			BeforeScenarioHooks.Import(stepSets);

			AfterScenarioHooks = new AfterScenarioHookList();
			AfterScenarioHooks.Import(stepSets);
		}

		public BeforeScenarioHookList BeforeScenarioHooks { get; set; }
		public AfterScenarioHookList AfterScenarioHooks { get;  set; }
	}
}