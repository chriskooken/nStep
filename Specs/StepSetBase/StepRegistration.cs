using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucumber.Framework;
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
            cut = GivenStepDefinitions.First();
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
            cut.ParamsTypes.Should().Contain(typeof (string));
        }
	}
}
