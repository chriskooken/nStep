using System.Collections.Generic;

namespace Nucumber.Framework.ScenarioHooks
{
    public interface IProvideScenarioHooks
    {
        IEnumerable<BeforeScenarioHook> BeforeHooks { get; }
        IEnumerable<AfterScenarioHook> AfterHooks { get; }
    }
}