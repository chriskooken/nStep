using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Framework;
using nStep.Framework.StepDefinitions;
using NUnit.Framework;

namespace StepSetBase
{
    [TestFixture]
	public class StepRegistrationForStrings : StepSetBase<string>
	{
        [TestFixtureSetUp]
        public void Setup()
        {
            action = foo => { return; };
            Given("test", action);
            cut = StepDefinitions.Givens.First();
        }

        private StepDefinition cut;
        private Action<string> action;

        [Test]
        public void it_should_register_a_compiled_regex()
        {
            cut.Regex.IsMatch("test").Should().Be.True();
        }

        [Test]
        public void it_should_register_the_action()
        {
            cut.Action.Should().Be.EqualTo(action);
        }

        [Test]
        public void it_should_register_the_param_type()
        {
            cut.InputParamsTypes.Should().Contain(typeof (string));
        }
	}
}
