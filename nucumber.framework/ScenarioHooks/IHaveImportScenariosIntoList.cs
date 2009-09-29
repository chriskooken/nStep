using System.Collections.Generic;

namespace Nucumber.Framework.ScenarioHooks
{
    public interface IHaveImportScenariosIntoList
    {
        void Import(IProvideScenarioHooks hookProvider);
        void Import(IEnumerable<IProvideScenarioHooks> hookProviders);
    }
}