using System;
using Nucumber.Core;
using Nucumber.Framework;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class AmbiguousStepException
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

        [Test]
        public void it_should_throw_if_there_is_more_than_one_matching_step()
        {
            var mother = new Nucumber.Core.StepMother(new StepSet().CombinedStepDefinitions);
            var step = new FeatureStep {FeatureLine = "My Name is \"Chris\""};

            new Action(() => mother.ProcessStep(step)).Should().Throw<Nucumber.Core.AmbiguousStepException>();
            
        }
       
    }
}