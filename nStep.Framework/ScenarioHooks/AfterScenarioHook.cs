using System;
using nStep.Framework.Execution.Results;

namespace nStep.Framework.ScenarioHooks
{
    public class AfterScenarioHook : ScenarioHook
    {
        public Action<ScenarioResult> Action { get; set; }
    }
}