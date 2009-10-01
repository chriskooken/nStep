using System;

namespace nStep.Framework.ScenarioHooks
{
    public class AfterScenarioHook : ScenarioHook
    {
        public Action<ScenarioResult> Action { get; set; }
    }
}