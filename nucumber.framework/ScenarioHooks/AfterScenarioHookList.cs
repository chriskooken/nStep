using System.Collections.Generic;

namespace Nucumber.Framework.ScenarioHooks
{
    public class AfterScenarioHookList : List<AfterScenarioHook>, IAfterScenarioHookList
    {
        public void Import(IProvideScenarioHooks hookProvider)
        {
            AddRange(hookProvider.AfterScenarioHooks);
        }

        public void Import(IEnumerable<IProvideSteps> hookProviders)
        {
            foreach (var provider in hookProviders)
                Import(provider);
        }
    }
}
