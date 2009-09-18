using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucumber.Core;
using NUnit.Framework;
using Nucumber.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class StepsCanCallOtherSteps
    {
        private class StepSet : StepSetBase<string>
        {
            public string providedName { get; private set; }

            public StepSet()
            {
                Given("^My Name is \"([^\"]*)\"$", name =>
                {
                    providedName = name;
                });

                Given("Call Me", () =>
                {
                    Given("My Name is \"Brendan\"");
                });

                When("^My Name is \"([^\"]*)\"$", name =>
                {
                    providedName = name;
                });

                When("Call Me", () =>
                {
                    When("My Name is \"Brendan\"");
                });

                Then("^My Name is \"([^\"]*)\"$", name =>
                {
                    providedName = name;
                });

                Then("Call Me", () =>
                {
                    Then("My Name is \"Brendan\"");
                });
            }

        }

        [Test]
        public void Given()
        {
            AssertItWorks(StepKinds.Given);
        }

        [Test]
        public void When()
        {
            AssertItWorks(StepKinds.When);
        }

        [Test]
        public void Then()
        {
            AssertItWorks(StepKinds.Then);
        }

        private void AssertItWorks(StepKinds kind)
        {
            var set = new StepSet();
            var mother = new Nucumber.Core.StepMother();
            mother.ImportSteps(set);
            var step = new FeatureStep { FeatureLine = "Call Me", Kind = kind };
            mother.ProcessStep(step);
            set.providedName.Should().Be.EqualTo("Brendan");
        }
    }
}
