using System.Linq;
using Nucumber.Core;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class PendingStepDefinition
    {
        private class StepSet : Nucumber.Framework.StepSetBase<string>
        {
            public StepSet()
            {
                Given("^My Name is \"([^\"]*)\"$", name =>
                {
                    Pending();
                });
            }
        }
        private Nucumber.Core.StepMother mother;
        private StepRunResults result;
        private StepSet Set;

        [SetUp]
        public void Setup()
        {
            Set = new StepSet();
            mother = new Nucumber.Core.StepMother(Set.CombinedStepDefinitions);
            var featureStep = new FeatureStep { FeatureLine = "My Name is \"Chris\"" };
            result = mother.ProcessStep(featureStep);
        }

        [Test]
        public void it_should_return_Pending()
        {
            result.Should().Be.EqualTo(StepRunResults.Pending);
            mother.LastProcessStepResult.Should().Be.EqualTo(StepRunResults.Pending);
        }

        [Test]
        public void it_should_Set_LastProcessStepException_to_PendingStepException()
        {
            mother.LastProcessStepException.Should().Be.OfType<Nucumber.Framework.StepPendingException>();
        }

        [Test]
        public void it_should_set_LastProcess_StepDefinition_to_the_pending_Step()
        {
            mother.LastProcessStepDefinition.Should().Be.EqualTo(Set.GivenStepDefinitions.First());
        }
    }
}
