using System;

namespace Nucumber.Framework.ScenarioHooks
{
    public class BeforeScenarioHook : ScenarioHook
    {
        public Action Action{get;set;}
    }
}