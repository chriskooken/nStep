using System;

namespace nStep.Framework.ScenarioHooks
{
    public class BeforeScenarioHook : ScenarioHook
    {
        public Action Action{get;set;}
    }
}