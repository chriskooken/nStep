using System.Collections.Generic;

namespace nStep.Framework.ScenarioHooks
{
    public interface IAfterScenarioHookList : IList<AfterScenarioHook>, IHaveImportScenariosIntoList
    {
    }
}