using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nStep.Framework.ScenarioHooks
{
    public class BeforeScenarioHookList : List<BeforeScenarioHook>, IBeforeScenarioHookList
    {
        public void Import(IProvideScenarioHooks hookProvider)
        {
            AddRange(hookProvider.BeforeScenarioHooks);
        }

        public void Import(IEnumerable<IProvideSteps> hookProviders)
        {
            foreach (var provider in hookProviders)
                Import(provider);
        }
    }
}
