using System.Linq;
using Nucumber.Core;
using Nucumber.Core.Features;
using Nucumber.Framework;
using NUnit.Framework;
using System.Collections.Generic;

namespace Specs.StepMother
{
    [TestFixture]
    public class PendingStepDefinition
    {
        private class StepSet : StepSetBase<string>
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
            }
        }
        private Nucumber.Core.StepMother mother;
        private StepRunResults result;
        private StepSet Set;

        [SetUp]
        public void Setup()
        {
            Set = new StepSet();
			mother = new Nucumber.Core.StepMother(null, null, null);
            mother.AdoptSteps(Set);
			var featureStep = new FeatureStep(StepKinds.Given) { FeatureLine = "My Name is \"Chris\"" };
            result = mother.ProcessStep(featureStep);
        }

        [Test]
        public void it_should_return_Pending()
        {
            result.Should().Be.EqualTo(StepRunResults.Pending);
            mother.LastProcessStepResult.Should().Be.EqualTo(StepRunResults.Pending);
        }

        [Test]
        public void it_should_Set_LastProcessStepException_to_PendingStepException()
        {
            mother.LastProcessStepException.Should().Be.OfType<Nucumber.Framework.StepPendingException>();
        }

        [Test]
        public void it_should_set_LastProcess_StepDefinition_to_the_pending_Step()
        {
            mother.LastProcessStepDefinition.Should().Be.EqualTo(Set.StepDefinitions.Givens.First());
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

        [Test]
        public void it_should_turn_a_pending_feature_line_into_suggestable_syntax_3_params()
        {
            ISuggestSyntax syntaxSuggester = new CSharpSyntaxSuggester();
            var featureStep = new FeatureStep(StepKinds.When) { FeatureLine = "When I type \"dogs\" in the \"search\" field and \"bob\""};

            syntaxSuggester.TurnFeatureIntoSnippet(featureStep).Should().Be.
                 EqualTo("When(\"I type \\\"([^\\\"]*)\\\" in the \\\"([^\\\"]*)\\\" field and \\\"([^\\\"]*)\\\"\", (string arg1, string arg2, string arg3) =>\n{\n\tPending();\n});");
        }

        [Test]
        public void it_should_turn_a_pending_feature_line_into_suggestable_syntax_2_params()
        {
            ISuggestSyntax syntaxSuggester = new CSharpSyntaxSuggester();
			var featureStep = new FeatureStep(StepKinds.When) { FeatureLine = "When I type \"dogs\" in the \"search\" field" };


            syntaxSuggester.TurnFeatureIntoSnippet(featureStep).Should().Be.
                 EqualTo("When(\"I type \\\"([^\\\"]*)\\\" in the \\\"([^\\\"]*)\\\" field\", (string arg1, string arg2) =>\n{\n\tPending();\n});");
        }

        [Test]
        public void it_should_turn_a_pending_feature_line_into_suggestable_syntax_1_param()
        {
            ISuggestSyntax syntaxSuggester = new CSharpSyntaxSuggester();
			var featureStep = new FeatureStep(StepKinds.When) { FeatureLine = "When I type \"dogs\" in google" };


            syntaxSuggester.TurnFeatureIntoSnippet(featureStep).Should().Be.
                 EqualTo("When(\"I type \\\"([^\\\"]*)\\\" in google\", (string arg1) =>\n{\n\tPending();\n});");
        }

        [Test]
        public void it_should_turn_a_pending_feature_line_into_suggestable_syntax_no_params()
        {
            ISuggestSyntax syntaxSuggester = new CSharpSyntaxSuggester();
			var featureStep = new FeatureStep(StepKinds.When) { FeatureLine = "When I type in google" };


            syntaxSuggester.TurnFeatureIntoSnippet(featureStep).Should().Be.
                 EqualTo("When(\"I type in google\", () =>\n{\n\tPending();\n});");
        }
    }
}
