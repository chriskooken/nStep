using nStep.Core;
using nStep.Core.Exceptions;
using nStep.Core.Features;
using nStep.Framework;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class MoreThanOneMatchingStepDefinition
    {

        private class StringWorldView : IAmWorldView
        {

        }

        private nStep.Core.WorldViewDictionary worldViews;

        private class StepSet : StepSetBase<StringWorldView>
        {

            public override void BeforeStep()
            {
                Before = "This was executed before";
            }

            public string Before { get; private set; }

            public override void AfterStep()
            {
                After = "This was executed after";
            }

            public string After { get; private set; }

            public StepSet()
            {

                Given("^My Name is \"([^\"]*)\"$", name =>
                    {
                        
                    });

                Given("^My Name is \"([^\"]*)\"$", name =>
                    {
                        
                    });
            }
        }
        private nStep.Core.StepMother mother;
        private StepRunResults result;
        private StepSet Set;

        [SetUp]
        public void Setup()
        {            
            worldViews = new nStep.Core.WorldViewDictionary();
            worldViews.Add(typeof(StringWorldView), new StringWorldView());
            Set = new StepSet();
			mother = new nStep.Core.StepMother(worldViews, null);
            mother.AdoptSteps(Set);
			var featureStep = new FeatureStep(StepKinds.Given) { FeatureLine = "Given My Name is \"Chris\"" };
            result = mother.ProcessStep(featureStep);
        }

        [Test]
        public void it_should_return_Failed()
        {
            result.Should().Be.EqualTo(StepRunResults.Failed);
            mother.LastProcessStepResult.Should().Be.EqualTo(StepRunResults.Failed);
        }

        [Test]
        public void it_should_Set_LastProcessStepException_to_AmbigousStepException()
        {
            mother.LastProcessStepException.Should().Be.OfType<StepAmbiguousException>();            
        }

        [Test]
        public void it_should_set_LastProcess_StepDefinition_to_null()
        {
            mother.LastProcessStepDefinition.Should().Be.Null();
        }


        [Test]
        public void it_should_not_execute_BeforeStep()
        {
            Set.Before.Should().Be.Null();
        }

        [Test]
        public void it_should_not_execute_AfterStep()
        {
            Set.After.Should().Be.Null();
        }
    }
}