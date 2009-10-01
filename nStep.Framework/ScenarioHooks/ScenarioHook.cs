using System.Collections.Generic;

namespace nStep.Framework.ScenarioHooks
{
    public abstract class ScenarioHook
    {
        public IEnumerable<string> Tags { get; set; }
    }
}