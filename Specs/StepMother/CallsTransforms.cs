using System.Linq;
using nStep.Core;
using nStep.Framework;
using nStep.Framework.Execution.Results;
using nStep.Framework.Features;
using nStep.Framework.StepDefinitions;
using nStep.Framework.WorldViews;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class CallsTransforms
    {

        private class StringWorldView : IAmWorldView
        {

        }

        private nStep.Framework.WorldViews.WorldViewDictionary worldViews;

        [SetUp]
        public void Setup()
        {
            worldViews = new nStep.Framework.WorldViews.WorldViewDictionary();
            worldViews.Add(typeof(StringWorldView), new StringWorldView());
        }


        public class NameObject
        {
            public string Value;
        }

        private class StepSet : StepSetBase<StringWorldView>
        {
            public string providedName { get; private set; }
            
            public StepSet()
            {
                Transform("is \"([^\"]*)\"", name =>
                    {
                        return new NameObject {Value = name};
                    });

                Given("^My Name (is \"[^\"]*\")$", (NameObject name)=>
                {
                    providedName = name.Value;
                });
            }
        }


        [Test]
        public void it_should_load_the_transform_definition()
        {
            var set = new StepSet();
            set.TransformDefinitions.Count().Should().Be.EqualTo(1);
        }

        [Test]
        public void it_should_call_through_the_Transform()
        {
            var set = new StepSet();

			var mother = new nStep.Framework.StepMother(worldViews, null);
            mother.AdoptSteps(set);

			var step = new Step { FeatureLine = "Given My Name is \"Chris\"" };
            mother.ProcessStep(step).ResultCode.Should().Be.EqualTo(StepRunResultCode.Passed);
            set.providedName.Should().Be.EqualTo("Chris");

        }
    }
}
