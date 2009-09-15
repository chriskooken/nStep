using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Nucumber.Framework
{
    public class StepSetBase<TWorldView>: IProvideSteps where TWorldView : class
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

        private void AddNewStep(string stepText, object action)
        {
            var regex = new Regex(stepText);
            stepDefinitions.Add(regex, action);
        }

        #region Given StepDefinitions
        protected void Given(string regex, Action action)
        {
            AddNewStep(regex, action);
        }

        protected void Given(string regex, Action<string> action)
        {
            AddNewStep(regex, action);
        }


        protected void Given(string regex, Action<string, string> action)
        {
            AddNewStep(regex, action);
        }


        protected void Given(string regex, Action<string, string, string> action)
        {
            AddNewStep(regex, action);
        }

        protected void Given(string regex, Action<string, string, string, string> action)
        {
            AddNewStep(regex, action);
        }
        #endregion

        #region When StepDefinitions
        protected void When(string s, Action<string> action)
        {
            //Add to stepDefinitions table
            AddNewStep(s,action);
        }


        protected void When(string s, Action<string, string> action)
        {
            AddNewStep(s,action);
        }


        protected void When(string s, Action<string, string, string> action)
        {
            AddNewStep(s,action);
        }

        protected void When(string s, Action<string, string, string,string> action)
        {
            AddNewStep(s,action);
        }
        #endregion

        #region Then StepDefinitions
        protected void Then(string s, Action<string> action)
        {
            //Add to stepDefinitions table
            AddNewStep(s,action);
        }


        protected void Then(string s, Action<string, string> action)
        {
            AddNewStep(s,action);
        }


        protected void Then(string s, Action<string, string, string> action)
        {
            AddNewStep(s,action);
        }    
        
        protected void Then(string s, Action<string, string, string, string> action)
        {
            AddNewStep(s,action);
        }
        #endregion

        #region But StepDefinitions
        protected void But(string s, Action<string> action)
        {
            //Add to stepDefinitions table
            AddNewStep(s,action);
        }


        protected void But(string s, Action<string, string> action)
        {
            AddNewStep(s,action);
        }


        protected void But(string s, Action<string, string, string> action)
        {
            AddNewStep(s,action);
        }

        protected void But(string s, Action<string, string, string, string> action)
        {
            AddNewStep(s,action);
        }
        #endregion
    }
}