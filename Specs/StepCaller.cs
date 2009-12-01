using System.Linq;
using nStep.Framework;
using nStep.Framework.StepDefinitions;
using nStep.Framework.Steps;
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

            var body = "This is a test: Boo";

            var caller = new nStep.Framework.Execution.StepCaller(step, new nStep.Framework.Execution.TypeCaster(null));
			caller.Call(new Step { Body = body, Kind = StepKinds.Given });

            set.Output.Should().Be.EqualTo("Boo");
        }

        [Test]
        public void it_should_call_the_action_with_the_correct_params_as_int()
        {
            var set = new StepSet();
            var step = set.StepDefinitions.Givens.Last();

            var body = "This is a test: 42";

            var caller = new nStep.Framework.Execution.StepCaller(step, new nStep.Framework.Execution.TypeCaster(null));
			caller.Call(new Step { Body = body, Kind = StepKinds.Given });

            set.Output.Should().Be.EqualTo("42");
        }
	}
}
