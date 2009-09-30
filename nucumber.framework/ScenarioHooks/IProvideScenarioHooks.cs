using System.Collections.Generic;

namespace Nucumber.Framework.ScenarioHooks
{
    public interface IProvideScenarioHooks
    {
        IEnumerable<BeforeScenarioHook> BeforeScenarioHooks { get; }
        IEnumerable<AfterScenarioHook> AfterScenarioHooks { get; }
    }
}