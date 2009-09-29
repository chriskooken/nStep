using System.Collections.Generic;

namespace Nucumber.Framework.ScenarioHooks
{
    public abstract class ScenarioHook
    {
        public IEnumerable<string> Tags { get; set; }
    }
}