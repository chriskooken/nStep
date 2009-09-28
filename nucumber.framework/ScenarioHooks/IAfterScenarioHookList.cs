using System.Collections.Generic;

namespace Nucumber.Framework.ScenarioHooks
{
    public interface IAfterScenarioHookList : IList<AfterScenarioHook>, IHaveImportScenariosIntoList
    {
    }
}