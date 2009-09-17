using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucumber.Core;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class PassedResults
    {
        private class StepSet : Nucumber.Framework.StepSetBase<string>
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
            Set = new StepSet();
            this.mother = new Nucumber.Core.StepMother(Set.CombinedStepDefinitions);
            var featureStep = new FeatureStep { FeatureLine = "My Name is \"Chris\"" };
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
                Set.CombinedStepDefinitions.GivenStepDefinitions.First());
        }

        [Test]
        public void It_should_set_LastException_to_null()
        {
            mother.LastProcessStepException.Should().Be.Null();
        }
    }
}
