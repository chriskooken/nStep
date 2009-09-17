using Nucumber.Core;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class NoStepDefinitionFound
    {
        private class StepSet : Nucumber.Framework.StepSetBase<string>
        {
            public StepSet()
            {
                
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
        public void it_should_Set_LastProcessStepException_to_MissingStepException()
        {
            mother.LastProcessStepException.Should().Be.OfType<StepMissingException>();
        }

        [Test]
        public void it_should_set_LastProcess_StepDefinition_to_null()
        {
            mother.LastProcessStepDefinition.Should().Be.Null();
        }
       
    }
}
