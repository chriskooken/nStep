using System.Linq;
using Nucumber.Framework.ScenarioHooks;
using NUnit.Framework;

namespace Specs.ScenarioHookLists
{
    [TestFixture]
    public class ImportHooksFromStepSetBase
    {
        public class foo : Nucumber.Framework.StepSetBase<string>
        {
            public foo()
            {
                Before(() => { return;});
                After(scen=>{return;});
            }
        }

        [Test]
        public void It_should_get_befores_from_the_provided_provider()
        {
            var hookList = new BeforeScenarioHookList();
            hookList.Import(new foo());
            hookList.Single().Should().Not.Be.Null();
        }

        [Test]
        public void It_should_get_afters_from_the_provided_provider()
        {
            var hookList = new AfterScenarioHookList();
            hookList.Import(new foo());
            hookList.Single().Should().Not.Be.Null();
        }
    }
}
