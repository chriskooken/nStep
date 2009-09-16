using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nucumber.Framework
{
    public abstract class StepSetBase<TWorldView>: IProvideSteps where TWorldView : class
    {
        IDictionary<Regex, object> stepDefinitions = new Dictionary<Regex, object>();

        public IDictionary<Regex,object> StepDefinitions
        {
            get
            {
                return stepDefinitions;
            }
        }

        public TWorldView World { get; internal set; }

        public void SetWorldView(object worldView)
        {
            World = worldView as TWorldView;
        }

        public virtual void BeforeStep()
        {}

        public virtual void AfterStep()
        {}

        private void AddNewStepDefinition<TParams>(StepKinds kind, string stepText, object action, TParams defaultParams )
        {
            var regex = new Regex(stepText);
            stepDefinitions.Add(regex, action);
        }

        #region Given StepDefinitions

        protected void Given<TParams>(string regex, TParams defaultParams, Action<TParams> action)
        {
            AddNewStepDefinition(StepKinds.Given, regex, action, defaultParams);
        }

        protected void Given(string regex, Action action)
        {
            AddNewStepDefinition(StepKinds.Given, regex, action, string.Empty);
        }

        protected void Given(string regex, Action<string> action)
        {
            AddNewStepDefinition(StepKinds.Given, regex, action, string.Empty);
        }

        protected void Given(string regex, Action<string, string> action)
        {
            AddNewStepDefinition(StepKinds.Given, regex, action, new { p = string.Empty, p2 = string.Empty });
        }

        protected void Given(string regex, Action<string, string, string> action)
        {
            AddNewStepDefinition(StepKinds.Given, regex, action, new { p = string.Empty, p2 = string.Empty, p3 = string.Empty });
        }

        protected void Given(string regex, Action<string, string, string, string> action)
        {
            AddNewStepDefinition(StepKinds.Given, regex, action, new { p = string.Empty, p2 = string.Empty, p3 = string.Empty, p4 = string.Empty });
        }
        #endregion

        #region When StepDefinitions
        protected void When<TParams>(string regex, TParams defaultParams, Action<TParams> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action, defaultParams);
        }

        protected void When(string regex, Action action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action, string.Empty);
        }

        protected void When(string regex, Action<string> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action, string.Empty);
        }

        protected void When(string regex, Action<string, string> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action, new { p = string.Empty, p2 = string.Empty });
        }

        protected void When(string regex, Action<string, string, string> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action, new { p = string.Empty, p2 = string.Empty, p3 = string.Empty });
        }

        protected void When(string regex, Action<string, string, string, string> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action, new { p = string.Empty, p2 = string.Empty, p3 = string.Empty, p4 = string.Empty });
        }
        #endregion

        #region Then StepDefinitions
        protected void Then<TParams>(string regex, TParams defaultParams, Action<TParams> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action, defaultParams);
        }

        protected void Then(string regex, Action action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action, string.Empty);
        }

        protected void Then(string regex, Action<string> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action, string.Empty);
        }

        protected void Then(string regex, Action<string, string> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action, new { p = string.Empty, p2 = string.Empty });
        }

        protected void Then(string regex, Action<string, string, string> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action, new { p = string.Empty, p2 = string.Empty, p3 = string.Empty });
        }

        protected void Then(string regex, Action<string, string, string, string> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action, new { p = string.Empty, p2 = string.Empty, p3 = string.Empty, p4 = string.Empty });
        }
        #endregion
    }
}