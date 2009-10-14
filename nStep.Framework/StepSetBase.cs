using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using nStep.Framework.ScenarioHooks;

namespace nStep.Framework
{
    public abstract class StepSetBase<TWorldView> : StepDefinitionDsl<TWorldView>, IProvideScenarioHooks, IProvideSteps where TWorldView : class
    {
        protected StepSetBase()
        {
            GivenStepDefinitions = new List<StepDefinition>();
            WhenStepDefinitions = new List<StepDefinition>();
            ThenStepDefinitions = new List<StepDefinition>();
            transformDefinitions = new List<TransformDefinition>();
        }

        private IList<StepDefinition> GivenStepDefinitions { get; set; }

        private IList<StepDefinition> WhenStepDefinitions { get; set; }

        private IList<StepDefinition> ThenStepDefinitions { get; set; }

        private IList<TransformDefinition> transformDefinitions;
        public IEnumerable<TransformDefinition> TransformDefinitions
        {
            get { return transformDefinitions; }
        }

        public CombinedStepDefinitions StepDefinitions
        {
            get
            {
                return new CombinedStepDefinitions
                           {
                               Givens = GivenStepDefinitions,
                               Whens = WhenStepDefinitions,
                               Thens = ThenStepDefinitions
                           };
            }
           
        }

        public object WorldView
        {
            set { World = value as TWorldView; }
        }

        public Type WorldViewType
        {
            get { return typeof (TWorldView); }
        }

        public IRunStepsFromStrings StepFromStringRunner
        {
            set { StepRunner = value; }
        }

        protected override void AddNewStepDefinition(StepKinds kind, string stepText, Delegate action)
        {
            var def = new StepDefinition(new Regex(stepText), action, kind, this);
            
            switch (kind)
            {
                case StepKinds.Given:
                    GivenStepDefinitions.Add(def);
                    break;
                case StepKinds.When:
                    WhenStepDefinitions.Add(def);
                    break;
                case StepKinds.Then:
                    ThenStepDefinitions.Add(def);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("kind");
            }


        }

        protected override void AddTransform<TReturn>(Delegate func, string regex)
        {
            transformDefinitions.Add(new TransformDefinition(new Regex(regex), func, typeof (TReturn)));
        }

        public IEnumerable<BeforeScenarioHook> BeforeScenarioHooks
        {
            get { return beforeScenarioHooks; }
        }

        public IEnumerable<AfterScenarioHook> AfterScenarioHooks
        {
            get { return afterScenarioHooks; }
        }
    }
}