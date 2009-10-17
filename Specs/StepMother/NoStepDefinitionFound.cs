using System;
using nStep.Core;
using nStep.Framework.Exceptions;
using nStep.Framework.Execution.Results;
using nStep.Framework.Features;
using nStep.Framework.StepDefinitions;
using nStep.Framework.WorldViews;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class NoStepDefinitionFound
    {
        private class StringWorldView : IAmWorldView
        {

        }

        private nStep.Framework.WorldViews.WorldViewDictionary worldViews;




        private class StepSet : nStep.Framework.StepSetBase<StringWorldView>
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
                
            }
        }
        private nStep.Framework.StepMother mother;
        private StepRunResultCode resultCode;
        private StepSet Set;

        [SetUp]
        public void Setup()
        {
            worldViews = new nStep.Framework.WorldViews.WorldViewDictionary();
            worldViews.Add(typeof(StringWorldView), new StringWorldView());
            Set = new StepSet();
			mother = new nStep.Framework.StepMother(worldViews, null);
            mother.AdoptSteps(Set);
			var featureStep = new Step(StepKinds.Given) { FeatureLine = "My Name is \"Chris\"" };
            resultCode = mother.ProcessStep(featureStep).ResultCode;
        }

        [Test]
        public void it_should_return_Missing()
        {
            resultCode.Should().Be.EqualTo(StepRunResultCode.Missing);
            mother.LastProcessStepResultCode.Should().Be.EqualTo(StepRunResultCode.Missing);
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
