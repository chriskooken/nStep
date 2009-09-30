using Nucumber.Core.Features;
using NUnit.Framework;
using Nucumber.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class StepsCanCallOtherSteps
    {
        private class StringWorldView : IAmWorldView
        {

        }

        private Nucumber.Core.WorldViewDictionary worldViews;

        [SetUp]
        public void Setup()
        {
            worldViews = new Nucumber.Core.WorldViewDictionary();
            worldViews.Add(typeof(StringWorldView), new StringWorldView());
        }


        private class StepSet : StepSetBase<StringWorldView>
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
            var mother = new Nucumber.Core.StepMother(worldViews, null);
            mother.AdoptSteps(set);
            var step = new FeatureStep(kind) { FeatureLine = "Call Me" };
            mother.ProcessStep(step);
            set.providedName.Should().Be.EqualTo("Brendan");
        }
    }
}
