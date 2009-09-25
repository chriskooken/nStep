using Nucumber.Framework;

namespace Cucumber
{
    public class ScenarioHooks : ScenarioHooksBase
    {
        public ScenarioHooks()
        {
            Before(()=>
                {
                    
                });
            After(scenario =>
                {
                    
                });

            Before(new[]{"tag1", "tag2"}, () => { });
            After(new[]{"@tag1"}, scenario => { });
        }
    }
}