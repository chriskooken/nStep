using System;
using System.Collections.Generic;

namespace Cucumber
{
    public class StepSetBase<TWorldView>: StepBase, IProvideSteps where TWorldView : class
    {
        IDictionary<Step, object> steps;
        public StepSetBase()
        {
            steps = new Dictionary<Step, object>();
        }

        public IDictionary<Step,object> Steps
        {
            get
            {
                return steps;
            }
        }

        public TWorldView World { get; internal set; }

        public void SetWorldView(object worldView)
        {
            World = worldView as TWorldView;
        }

        private void AddNewStep(string stepText, object action)
        {
            Step step = new Step();
            step.StepText = stepText;
            steps.Add(step, action);
        }

        #region Given Steps
        protected void Given(string s, Action<string> action)
        {
            //Add to steps table
            AddNewStep(s,action);
        }


        protected void Given(string s, Action<string, string> action)
        {
            AddNewStep(s,action);
        }


        protected void Given(string s, Action<string, string, string> action)
        {
            AddNewStep(s,action);
        }

        protected void Given(string s, Action<string, string, string, string> action)
        {
            AddNewStep(s,action);
        }
        #endregion

        #region When Steps
        protected void When(string s, Action<string> action)
        {
            //Add to steps table
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

        #region Then Steps
        protected void Then(string s, Action<string> action)
        {
            //Add to steps table
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

        #region But Steps
        protected void But(string s, Action<string> action)
        {
            //Add to steps table
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

    public class StepBase   
    {
    }
}