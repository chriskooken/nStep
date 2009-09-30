using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucumber.Core;

namespace Nucumber.Framework.ScenarioHooks
{
    public class AfterScenarioHookList : List<AfterScenarioHook>, IAfterScenarioHookList
    {
        public void Import(IProvideScenarioHooks hookProvider)
        {
            AddRange(hookProvider.AfterHooks);
        }

        public void Import(IEnumerable<IProvideScenarioHooks> hookProviders)
        {
            foreach (var provider in hookProviders)
                Import(provider);
        }
    }
}
