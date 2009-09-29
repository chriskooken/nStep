using System;
using Nucumber.Framework;

namespace Cucumber
{
    public class ScenarioHooks : ScenarioHooksBase
    {
        public ScenarioHooks()
        {
            Before(()=>
                {
                    Console.WriteLine("before scenario");
                });
            After(scenario =>
                {
                    Console.WriteLine("after scenario");
                });

            Before(new[]{"tag1", "tag2"}, () => { });
            After(new[]{"@tag1"}, scenario => { });
        }
    }
}