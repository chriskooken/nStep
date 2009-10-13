using System.Text.RegularExpressions;
using nStep.Core.Exceptions;
using nStep.Core.Features;
using nStep.Framework;
using nStep.Framework.StepDefinitions;
using NUnit.Framework;

namespace Specs.StepMother
{
    [TestFixture]
    public class BasicMatching
    {
        private class StringWorldView : IAmWorldView
        {

        }

        private nStep.Core.WorldViewDictionary worldViews;

        [SetUp]
        public void Setup()
        {
            worldViews = new nStep.Core.WorldViewDictionary();
            worldViews.Add(typeof(StringWorldView),new StringWorldView());
        }

        private class StepSet : StepSetBase<StringWorldView>
        {
            public string providedName { get; private set; }
            public string Before { get; set; }
            public string After { get; set; }

            public StepSet()
            {
                Given("^My Name is \"([^\"]*)\"$", name =>
                    {
                        providedName = name;
                    });

                Given("^This is a (bad) step \"([^\"]*)\"$", name =>
                {
                    providedName = name;
                });
            }


            public override void BeforeStep()
            {
                Before = "This was executed before";
            }

            public override void AfterStep()
            {
                After = "This was executed after";
            }
        }

        [Test]
        public void It_should_generate_all_permutations_of_regex()
        {
            //var pattern = "^My (?:biological)? Name is \"([^\"]*)\"$";
            //var regex = new Regex(pattern);
            //regex.
        }

        [Test]
        public void it_should_match_correct_step_Defition_given_a_feature_line()
        {
            var set = new StepSet();

            var mother = new nStep.Core.StepMother(worldViews, null);
            mother.AdoptSteps(set);

			var step = new FeatureStep(StepKinds.Given) { FeatureLine = "Given My Name is \"Chris\"" };
            mother.ProcessStep(step);
            set.providedName.Should().Be.EqualTo("Chris");
        }

        [Test]
        public void it_should_match_correct_step_Defition_with_and()
        {
            var set = new StepSet();

            var mother = new nStep.Core.StepMother(worldViews, null);
            mother.AdoptSteps(set);

            var step = new FeatureStep(StepKinds.Given) { FeatureLine = "And My Name is \"Chris\"" };
            mother.ProcessStep(step);
            set.providedName.Should().Be.EqualTo("Chris");
        }

        [Test]
        public void it_should_match_correct_step_Defition_with_but()
        {
            var set = new StepSet();

            var mother = new nStep.Core.StepMother(worldViews, null);
            mother.AdoptSteps(set);

            var step = new FeatureStep(StepKinds.Given) { FeatureLine = "And My Name is \"Chris\"" };
            mother.ProcessStep(step);
            set.providedName.Should().Be.EqualTo("Chris");
        }

        [Test]
        public void Before_Step_Gets_executed()
        {
            var set = new StepSet();

            var mother = new nStep.Core.StepMother(worldViews, null);
            mother.AdoptSteps(set);

			var step = new FeatureStep(StepKinds.Given) { FeatureLine = "Given My Name is \"Chris\"" };
            mother.ProcessStep(step);
            set.Before.Should().Be.EqualTo("This was executed before");
        }

        [Test]
        public void After_Step_Gets_executed()
        {
            var set = new StepSet();

            var mother = new nStep.Core.StepMother(worldViews, null);
            mother.AdoptSteps(set);

			var step = new FeatureStep(StepKinds.Given) { FeatureLine = "Given My Name is \"Chris\"" };
            mother.ProcessStep(step);
            set.After.Should().Be.EqualTo("This was executed after");
        }        
        [Test]
        public void It_should_throw_parameter_mismatch_exception()
        {
            var set = new StepSet();

            var mother = new nStep.Core.StepMother(worldViews, null);
            mother.AdoptSteps(set);

            var step = new FeatureStep(StepKinds.Given) { FeatureLine = "Given This is a bad step \"Bobcat\"" };
            mother.ProcessStep(step);
            mother.LastProcessStepException.Should().Be.OfType<ParameterMismatchException>();
        }
    }

}