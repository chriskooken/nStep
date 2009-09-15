using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace nucumber.Framework
{
    public class StepSetBase<TWorldView>: StepBase, IProvideSteps where TWorldView : class
    {
        IDictionary<Regex, object> steps = new Dictionary<Regex, object>();

        public IDictionary<Regex,object> Steps
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
            var regex = new Regex(stepText);
            steps.Add(regex, action);
        }

        #region Given Steps
        protected void Given(string s, Action<string> action)
        {
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