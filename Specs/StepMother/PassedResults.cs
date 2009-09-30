using System.Linq;
using Nucumber.Core;
using Nucumber.Core.Features;
using Nucumber.Framework;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class PassedResults
    {
        private class StringWorldView : IAmWorldView
        {

        }

        private Nucumber.Core.WorldViewDictionary worldViews;

        private class StepSet : Nucumber.Framework.StepSetBase<StringWorldView>
        {

            public StepSet()
            {
                Given("^My Name is \"([^\"]*)\"$", name =>
                {

                });

            }
        }

        
        private Nucumber.Core.StepMother mother;
        private StepRunResults result;
        private StepSet Set;

        [SetUp]
        public void Setup()
        {
            worldViews = new Nucumber.Core.WorldViewDictionary();
            worldViews.Add(typeof(StringWorldView), new StringWorldView());
            Set = new StepSet();
            mother = new Nucumber.Core.StepMother(worldViews, null, null);
            mother.AdoptSteps(Set);
            var featureStep = new FeatureStep(StepKinds.Given) { FeatureLine = "My Name is \"Chris\"" };
            result = mother.ProcessStep(featureStep);
        }

        [Test]
        public void It_should_return_Passed()
        {
            result.Should().Be.EqualTo(StepRunResults.Passed);
            mother.LastProcessStepResult.Should().Be.EqualTo(StepRunResults.Passed);
        }

        [Test]
        public void it_should_set_last_used_StepDefinition_to_the_used_StepDefinition()
        {
            mother.LastProcessStepDefinition.Should().Be.EqualTo(
                Set.StepDefinitions.Givens.First());
        }

        [Test]
        public void It_should_set_LastException_to_null()
        {
            mother.LastProcessStepException.Should().Be.Null();
        }
    }
}
