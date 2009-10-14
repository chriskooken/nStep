using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Framework;
using NUnit.Framework;

namespace Specs
{
    [TestFixture]
	public class StepCaller
	{
    
        public class StepSet : StepSetBase<string>
        {

            public string Output { get; set; }

            public StepSet()
            {
                Given("This is a test: (.*)", val =>
                    {
                        Output = val;
                    });

                Given("This is a test: (\\d+)", (int val) =>
                    {
                        Output = val.ToString();
                    });
            }
        }

        [Test]
        public void it_should_call_the_action_with_the_correct_params_as_string()
        {
            var set = new StepSet();
            var step = set.StepDefinitions.Givens.First();

            var featureLine = "This is a test: Boo";

            var caller = new nStep.Core.StepCaller(step, new nStep.Core.TypeCaster(null));
            caller.Call(featureLine);

            set.Output.Should().Be.EqualTo("Boo");
        }

        [Test]
        public void it_should_call_the_action_with_the_correct_params_as_int()
        {
            var set = new StepSet();
            var step = set.StepDefinitions.Givens.Last();

            var featureLine = "This is a test: 42";

            var caller = new nStep.Core.StepCaller(step, new nStep.Core.TypeCaster(null));
            caller.Call(featureLine);

            set.Output.Should().Be.EqualTo("42");
        }
	}
}
