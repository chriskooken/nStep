using System.Collections.Generic;

namespace nStep.Framework.ScenarioHooks
{
    public interface IHaveImportScenariosIntoList
    {
        void Import(IProvideScenarioHooks hookProvider);
        void Import(IEnumerable<IProvideSteps> hookProviders);
    }
}