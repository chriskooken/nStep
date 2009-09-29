using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucumber.Framework;
using Nucumber.Framework.ScenarioHooks;
using NUnit.Framework;

namespace Specs.ScenarioHookLists
{
    [TestFixture]
    public class ImportHooks
    {
        public class foo : Nucumber.Framework.ScenarioHooksBase
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
