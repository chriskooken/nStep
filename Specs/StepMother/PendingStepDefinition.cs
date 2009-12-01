using System.Linq;
using nStep.Core;
using nStep.Framework;
using nStep.Framework.Exceptions;
using nStep.Framework.Execution.Results;
using nStep.Framework.Features;
using nStep.Framework.StepDefinitions;
using NUnit.Framework;
using System.Collections.Generic;
using Specs.WorldViewDictionary;

namespace Specs.StepMother
{
    [TestFixture]
    public class PendingStepDefinition
    {
        private nStep.Framework.WorldViews.WorldViewDictionary worldViews;

        private class StepSet : StepSetBase<ImportWorldViews.StringWorldView>
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
                    Pending();
                });

                Given("^I am a pending step with no body \"([^\"]*)\"$", name =>
                {
                    
                });
            }
        }
        private nStep.Framework.StepMother mother;
        private StepRunResult result;
        private StepSet Set;

        [SetUp]
        public void Setup()
        {
            worldViews = new nStep.Framework.WorldViews.WorldViewDictionary();
            worldViews.Add(typeof(ImportWorldViews.StringWorldView), new ImportWorldViews.StringWorldView());
            Set = new StepSet();
            mother = new nStep.Framework.StepMother(worldViews, null);
            mother.AdoptSteps(Set);
			var featureStep = new Step { FeatureLine = "Given My Name is \"Chris\"" };
            result = mother.ProcessStep(featureStep);
        }

        [Test]
        public void it_should_return_Pending()
        {
            result.ResultCode.Should().Be.EqualTo(StepRunResultCode.Pending);
            mother.LastProcessStepResultCode.Should().Be.EqualTo(StepRunResultCode.Pending);
        }

        [Test]
        public void it_should_return_pending_if_there_is_no_method_body()
        {
            var featureStep = new Step { FeatureLine = "Given I am a pending step with no body \"Chris\"" };
            result = mother.ProcessStep(featureStep);
            result.ResultCode.Should().Be.EqualTo(StepRunResultCode.Pending);
            mother.LastProcessStepResultCode.Should().Be.EqualTo(StepRunResultCode.Pending);
        }


        [Test]
        public void it_should_Set_LastProcessStepException_to_PendingStepException()
        {
            mother.LastProcessStepException.Should().Be.OfType<StepPendingException>();
        }

        [Test]
        public void it_should_Set_Result_Exception_to_PendingStepException()
        {
            result.Exception.Should().Be.OfType<StepPendingException>();
        }

        [Test]
        public void it_should_set_LastProcess_StepDefinition_to_the_pending_Step()
        {
            mother.LastProcessStepDefinition.Should().Be.EqualTo(Set.StepDefinitions.Givens.First());
        }

        [Test]
        public void it_should_set_Result_StepDefinition_to_the_pending_Step()
        {
            result.MatchedStepDefinition.Should().Be.EqualTo(Set.StepDefinitions.Givens.First());
        }

        [Test]
        public void it_should_execute_BeforeStep()
        {
            Set.Before.Should().Not.Be.Null();
        }

        [Test]
        public void it_should_not_execute_AfterStep()
        {
            Set.After.Should().Be.Null();
        }
    }
}
