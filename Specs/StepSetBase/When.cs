﻿using Nucumber.Framework;
using NUnit.Framework;
using System.Linq;

namespace StepSetBase
{

    [TestFixture]
    public class WhenHandling : StepSetBase<string>
    {
        [Test]
        public void it_should_register_as_a_When()
        {
            When("something", () => { return; });
            WhenStepDefinitions.First().Kind.Should().Be.EqualTo(StepKinds.When);
        }

    }

}
