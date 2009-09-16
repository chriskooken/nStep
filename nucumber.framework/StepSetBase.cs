using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nucumber.Framework
{
    public abstract class StepSetBase<TWorldView>: IProvideSteps where TWorldView : class
    {
        IList<StepDefinition> stepDefinitions = new List<StepDefinition>();

        public IEnumerable<StepDefinition> StepDefinitions
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

        private void AddNewStepDefinition(StepKinds kind, string stepText, object action)
        {
            stepDefinitions.Add(new StepDefinition
                                    {
                                        Regex = new Regex(stepText),
                                        Kind = kind,
                                        Action = action,
                                        ParamsTypes = action.GetType().GetGenericArguments()
                                    });
        }

        #region Given StepDefinitions

		protected void Given<T1>(string regex, Action<T1> action)
		{
            AddNewStepDefinition(StepKinds.Given, regex, action);
		}

		protected void Given<T1, T2>(string regex, Action<T1, T2> action)
		{
            AddNewStepDefinition(StepKinds.Given, regex, action);
		}

		protected void Given<T1, T2, T3>(string regex, Action<T1, T2, T3> action)
		{
            AddNewStepDefinition(StepKinds.Given, regex, action);
		}

		protected void Given<T1, T2, T3, T4>(string regex, Action<T1, T2, T3, T4> action)
		{
            AddNewStepDefinition(StepKinds.Given, regex, action);
		}

		protected void Given(string regex, Action action)
		{
			AddNewStepDefinition(StepKinds.Given, regex, action);
		}

		protected void Given(string regex, Action<string> action)
		{
            AddNewStepDefinition(StepKinds.Given, regex, action);
		}

		protected void Given(string regex, Action<string, string> action)
		{
            AddNewStepDefinition(StepKinds.Given, regex, action);
		}

		protected void Given(string regex, Action<string, string, string> action)
		{
            AddNewStepDefinition(StepKinds.Given, regex, action);
		}

		protected void Given(string regex, Action<string, string, string, string> action)
		{
            AddNewStepDefinition(StepKinds.Given, regex, action);
		}

        #endregion

        #region When StepDefinitions
        protected void When<T1>(string regex, Action<T1> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action);
        }

        protected void When<T1, T2>(string regex, Action<T1, T2> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action);
        }

        protected void When<T1, T2, T3>(string regex, Action<T1, T2, T3> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action);
        }

        protected void When<T1, T2, T3, T4>(string regex, Action<T1, T2, T3, T4> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action);
        }

        protected void When(string regex, Action action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action);
        }

        protected void When(string regex, Action<string> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action);
        }

        protected void When(string regex, Action<string, string> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action);
        }

        protected void When(string regex, Action<string, string, string> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action);
        }

        protected void When(string regex, Action<string, string, string, string> action)
        {
            AddNewStepDefinition(StepKinds.When, regex, action);
        }
		#endregion

        #region Then StepDefinitions
        protected void Then<T1>(string regex, Action<T1> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action);
        }

        protected void Then<T1, T2>(string regex, Action<T1, T2> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action);
        }

        protected void Then<T1, T2, T3>(string regex, Action<T1, T2, T3> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action);
        }

        protected void Then<T1, T2, T3, T4>(string regex, Action<T1, T2, T3, T4> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action);
        }

        protected void Then(string regex, Action action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action);
        }

        protected void Then(string regex, Action<string> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action);
        }

        protected void Then(string regex, Action<string, string> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action);
        }

        protected void Then(string regex, Action<string, string, string> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action);
        }

        protected void Then(string regex, Action<string, string, string, string> action)
        {
            AddNewStepDefinition(StepKinds.Then, regex, action);
        }
		#endregion

        protected void Transform<TReturn>(string match, Func<string,TReturn> action)
        {
            
        }
    }
}