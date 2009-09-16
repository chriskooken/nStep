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
            cut = StepDefinitions.First();
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
            cut.ParamsType.Should().Be.EqualTo(typeof (string));
        }

        [Test]
        public void it_should_register_the_default_values_for_params()
        {
            var defaultParams = cut.DefaultParams as string;
            defaultParams.Should().Be.EqualTo("");
        }
	}

    [TestFixture]
    public class StepRegistrationForComplexTypes : StepSetBase<string>
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            var act = CreateAction(new {a = "default value", b = Guid.Empty}, foo => { return; });
            Given("test", new { a = "default value", b = Guid.Empty }, act);
            action = act;
            cut = StepDefinitions.First();
        }


        private Action<TParams> CreateAction<TParams>(TParams example, Action<TParams> action)
        {
            defaultValue = example;
            return action;
        }

        private object defaultValue;
        private StepDefinition cut;
        private object action;

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
            cut.ParamsType.Should().Be.EqualTo(defaultValue.GetType());
        }

        [Test]
        public void it_should_register_the_default_values_for_params()
        {
            cut.DefaultParams.Should().Be.EqualTo(defaultValue);
        }
    }
}
