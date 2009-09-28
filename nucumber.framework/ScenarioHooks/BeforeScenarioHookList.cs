using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.Framework.ScenarioHooks
{
    public class BeforeScenarioHookList : List<BeforeScenarioHook>, IBeforeScenarioHookList
    {
        public void Import(IProvideScenarioHooks hookProvider)
        {
            AddRange(hookProvider.BeforeHooks);
        }

        public void Import(IEnumerable<IProvideScenarioHooks> hookProviders)
        {
            foreach (var provider in hookProviders)
                Import(provider);
        }
    }
}
