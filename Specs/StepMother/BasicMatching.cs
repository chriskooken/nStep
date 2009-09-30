using Nucumber.Core.Features;
using Nucumber.Framework;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class BasicMatching
    {
        private class StepSet : StepSetBase<string>
        {
            public string providedName { get; private set; }
            public string Before { get; set; }
            public string After { get; set; }

            public StepSet()
            {
                Given("^My Name is \"([^\"]*)\"$", name =>
                    {
                        providedName = name;
                    });
            }

            public override void BeforeStep()
            {
                Before = "This was executed before";
            }

            public override void AfterStep()
            {
                After = "This was executed after";
            }
        }

        [Test]
        public void it_should_match_correct_step_Defition_given_a_feature_line()
        {
            var set = new StepSet();

			var mother = new Nucumber.Core.StepMother(null, null, null);
            mother.AdoptSteps(set);

			var step = new FeatureStep(StepKinds.Given) { FeatureLine = "My Name is \"Chris\"" };
            mother.ProcessStep(step);
            set.providedName.Should().Be.EqualTo("Chris");

        }

        [Test]
        public void Before_Step_Gets_executed()
        {
            var set = new StepSet();

			var mother = new Nucumber.Core.StepMother(null, null, null);
            mother.AdoptSteps(set);

			var step = new FeatureStep(StepKinds.Given) { FeatureLine = "My Name is \"Chris\"" };
            mother.ProcessStep(step);
            set.Before.Should().Be.EqualTo("This was executed before");
        }

        [Test]
        public void After_Step_Gets_executed()
        {
            var set = new StepSet();

			var mother = new Nucumber.Core.StepMother(null, null, null);
            mother.AdoptSteps(set);

			var step = new FeatureStep(StepKinds.Given) { FeatureLine = "My Name is \"Chris\"" };
            mother.ProcessStep(step);
            set.After.Should().Be.EqualTo("This was executed after");
        }
       
    }
}