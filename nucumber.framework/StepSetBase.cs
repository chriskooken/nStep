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

        private void AddNewStepDefinition(string stepText, object action)
        {
            var regex = new Regex(stepText);
            stepDefinitions.Add(regex, action);
        }

        #region Given StepDefinitions
        protected void Given(string regex, Action action)
        {
            AddNewStepDefinition(regex, action);
        }

        protected void Given(string regex, Action<string> action)
        {
            AddNewStepDefinition(regex, action);
        }


        protected void Given(string regex, Action<string, string> action)
        {
            AddNewStepDefinition(regex, action);
        }


        protected void Given(string regex, Action<string, string, string> action)
        {
            AddNewStepDefinition(regex, action);
        }

        protected void Given(string regex, Action<string, string, string, string> action)
        {
            AddNewStepDefinition(regex, action);
        }
        #endregion

        #region When StepDefinitions
        protected void When(string s, Action<string> action)
        {
            //Add to stepDefinitions table
            AddNewStepDefinition(s,action);
        }


        protected void When(string s, Action<string, string> action)
        {
            AddNewStepDefinition(s,action);
        }


        protected void When(string s, Action<string, string, string> action)
        {
            AddNewStepDefinition(s,action);
        }

        protected void When(string s, Action<string, string, string,string> action)
        {
            AddNewStepDefinition(s,action);
        }
        #endregion

        #region Then StepDefinitions
        protected void Then(string s, Action<string> action)
        {
            //Add to stepDefinitions table
            AddNewStepDefinition(s,action);
        }


        protected void Then(string s, Action<string, string> action)
        {
            AddNewStepDefinition(s,action);
        }


        protected void Then(string s, Action<string, string, string> action)
        {
            AddNewStepDefinition(s,action);
        }    
        
        protected void Then(string s, Action<string, string, string, string> action)
        {
            AddNewStepDefinition(s,action);
        }
        #endregion
    }
}