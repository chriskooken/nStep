using Nucumber.Core;
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

            public StepSet()
            {
                Given("^My Name is \"([^\"]*)\"$", name =>
                    {
                        providedName = name;
                    });
            }
        }

        [Test]
        public void it_should_match_correct_step_Defition_given_a_feature_line()
        {
            var set = new StepSet();

            var mother = new Nucumber.Core.StepMother(set.CombinedStepDefinitions);

            var step = new FeatureStep {FeatureLine = "My Name is \"Chris\""};
            mother.ProcessStep(step);
            set.providedName.Should().Be.EqualTo("Chris");

        }
       
    }
}