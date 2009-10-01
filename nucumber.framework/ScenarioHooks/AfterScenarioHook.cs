using System;

namespace Nucumber.Framework.ScenarioHooks
{
    public class AfterScenarioHook : ScenarioHook
    {
        public Action<ScenarioResult> Action { get; set; }
    }
}