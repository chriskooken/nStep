using System.Linq;
using nStep.Core;
using nStep.Core.Features;
using nStep.Framework.StepDefinitions;
using NUnit.Framework;
using Specs.WorldViewDictionary;

namespace Specs.StepMother
{
    [TestFixture]
    public class PassedResults
    {
        private nStep.Framework.WorldViews.WorldViewDictionary worldViews;

        private class StepSet : nStep.Framework.StepSetBase<ImportWorldViews.StringWorldView>
        {

            public StepSet()
            {
                Given("^My Name is \"([^\"]*)\"$", name =>
                {
                    return;
                });

            }
        }

        
        private nStep.Core.StepMother mother;
        private StepRunResults result;
        private StepSet Set;

        [SetUp]
        public void Setup()
        {
            worldViews = new nStep.Framework.WorldViews.WorldViewDictionary();
            worldViews.Add(typeof(ImportWorldViews.StringWorldView), new ImportWorldViews.StringWorldView());
            Set = new StepSet();
            mother = new nStep.Core.StepMother(worldViews, null);
            mother.AdoptSteps(Set);
            var featureStep = new FeatureStep(StepKinds.Given) { FeatureLine = "Given My Name is \"Chris\"" };
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
