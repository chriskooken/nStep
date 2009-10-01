using System.Collections.Generic;

namespace nStep.Framework.ScenarioHooks
{
    public interface IProvideScenarioHooks
    {
        IEnumerable<BeforeScenarioHook> BeforeScenarioHooks { get; }
        IEnumerable<AfterScenarioHook> AfterScenarioHooks { get; }
    }
}