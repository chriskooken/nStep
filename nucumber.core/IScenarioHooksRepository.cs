using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucumber.Framework;
using Nucumber.Framework.ScenarioHooks;

namespace Nucumber.Core
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
