using System.Linq;
using Nucumber.Core;
using Nucumber.Core.Features;
using Nucumber.Framework;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class CallsTransforms
    {

        private class StringWorldView : IAmWorldView
        {

        }

        private Nucumber.Core.WorldViewDictionary worldViews;

        [SetUp]
        public void Setup()
        {
            worldViews = new Nucumber.Core.WorldViewDictionary();
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

                Given("^My Name is \"([^\"]*)\"$", (NameObject name)=>
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

			var mother = new Nucumber.Core.StepMother(worldViews, null, null);
            mother.AdoptSteps(set);

			var step = new FeatureStep(StepKinds.Given) { FeatureLine = "My Name is \"Chris\"" };
            mother.ProcessStep(step).Should().Be.EqualTo(StepRunResults.Passed);
            set.providedName.Should().Be.EqualTo("Chris");

        }
    }
}
