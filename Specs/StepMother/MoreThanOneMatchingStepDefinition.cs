using System;
using Nucumber.Core;
using Nucumber.Framework;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class MoreThanOneMatchingStepDefinition
    {
        private class StepSet : StepSetBase<string>
        {

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
        private Nucumber.Core.StepMother mother;
        private StepRunResults result;
        private StepSet Set;

        [SetUp]
        public void Setup()
        {
            Set = new StepSet();
            this.mother = new Nucumber.Core.StepMother(Set.CombinedStepDefinitions);
            var featureStep = new FeatureStep { FeatureLine = "My Name is \"Chris\"" };
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
            mother.LastProcessStepException.Should().Be.OfType<Nucumber.Core.StepAmbiguousException>();            
        }

        [Test]
        public void it_should_set_LastProcess_StepDefinition_to_null()
        {
            mother.LastProcessStepDefinition.Should().Be.Null();
        }
       
    }
}