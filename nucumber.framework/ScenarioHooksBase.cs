using System;
using System.Collections.Generic;
using Nucumber.Framework.ScenarioHooks;

namespace Nucumber.Framework
{
    /// <summary>
    /// http://wiki.github.com/aslakhellesoy/cucumber/hooks
    /// </summary>
    public abstract class ScenarioHooksBase : IProvideScenarioHooks 
    {
        private IList<BeforeScenarioHook> befores = new List<BeforeScenarioHook>();
        private IList<AfterScenarioHook> afters = new List<AfterScenarioHook>();

        protected void Before(string[] tags, Action action)
        {
            befores.Add(new BeforeScenarioHook{Action = action, Tags = tags});
        }

        protected void Before(Action action)
        {
            befores.Add(new BeforeScenarioHook { Action = action});
        }

        protected void After(string[] tags, Action<ScenarioResult> action)
        {
            afters.Add(new AfterScenarioHook { Action = action, Tags = tags });
        }

        protected void After(Action<ScenarioResult> action)
        {
            afters.Add(new AfterScenarioHook { Action = action});
        }

        public IEnumerable<BeforeScenarioHook> BeforeHooks
        {
            get { return befores; }
        }

        public IEnumerable<AfterScenarioHook> AfterHooks
        {
            get { return afters; }
        }
    }
}