using System.Collections.Generic;

namespace nStep.Framework.ScenarioHooks
{
    public interface IBeforeScenarioHookList : IList<BeforeScenarioHook> , IHaveImportScenariosIntoList
    {

    }
}