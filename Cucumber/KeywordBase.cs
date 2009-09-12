using System;
using System.Collections.Generic;

namespace Cucumber
{
    public class StepMother
    {
        Dictionary<string, object> steps;
        public StepMother()
        {
            steps = new Dictionary<string, object>();
        }

        public Dictionary<string,object> Steps
        {
            get
            {
                return steps;
            }
        }

        #region Given Steps
        protected void Given(string s, Action<string> action)
        {
            //Add to steps table
            steps.Add(s, action);
        }


        protected void Given(string s, Action<string, string> action)
        {
            steps.Add(s, action);
        }


        protected void Given(string s, Action<string, string, string> action)
        {
            steps.Add(s, action);
        }
        #endregion

        #region When Steps
        protected void When(string s, Action<string> action)
        {
            //Add to steps table
            steps.Add(s, action);
        }


        protected void When(string s, Action<string, string> action)
        {
            steps.Add(s, action);
        }


        protected void When(string s, Action<string, string, string> action)
        {
            steps.Add(s, action);
        }
        #endregion

        #region Then
        protected void Then(string s, Action<string> action)
        {
            //Add to steps table
            steps.Add(s, action);
        }


        protected void Then(string s, Action<string, string> action)
        {
            steps.Add(s, action);
        }


        protected void Then(string s, Action<string, string, string> action)
        {
            steps.Add(s, action);
        }
        #endregion
    }
}