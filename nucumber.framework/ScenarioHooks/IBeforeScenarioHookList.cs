using System.Collections.Generic;

namespace Nucumber.Framework.ScenarioHooks
{
    public interface IBeforeScenarioHookList : IList<BeforeScenarioHook> , IHaveImportScenariosIntoList
    {

    }
}