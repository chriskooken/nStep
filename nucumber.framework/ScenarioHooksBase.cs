using System;

namespace Nucumber.Framework
{
    /// <summary>
    /// http://wiki.github.com/aslakhellesoy/cucumber/hooks
    /// </summary>
    public abstract class ScenarioHooksBase
    {
        protected void Before(string[] tags, Action action)
        {
            throw new NotImplementedException();
        }

        protected void Before(Action action)
        {
            throw new NotImplementedException();
        }

        protected void After(string[] tags, Action<ScenarioResult> action)
        {
            throw new NotImplementedException();
        }

        protected void After(Action<ScenarioResult> action)
        {
            throw new NotImplementedException();
        }

    }
}