using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nucumber.Framework
{
    public abstract class StepSetBase<TWorldView>: IProvideSteps where TWorldView : class
    {
        protected StepSetBase()
        {
            givenStepDefinitions = new List<StepDefinition>();
            whenStepDefinitions = new List<StepDefinition>();
            thenStepDefinitions = new List<StepDefinition>();
            transformDefinitions = new List<TransformDefinition>();
        }

        private IList<StepDefinition> givenStepDefinitions;
        public IEnumerable<StepDefinition> GivenStepDefinitions
        {
            get { return givenStepDefinitions; }
        }

        private IList<StepDefinition> whenStepDefinitions;
        public IEnumerable<StepDefinition> WhenStepDefinitions
        {
            get { return whenStepDefinitions; }
        }

        private IList<StepDefinition> thenStepDefinitions;
        public IEnumerable<StepDefinition> ThenStepDefinitions
        {
            get { return thenStepDefinitions; }
        }

        private IList<TransformDefinition> transformDefinitions;
        public IEnumerable<TransformDefinition> TransformDefinitions
        {
            get { return transformDefinitions; }
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

        private void AddNewStepDefinition(StepKinds kind, string stepText, Delegate action)
        {
            var def = new StepDefinition
                                    {
                                        Regex = new Regex(stepText),
                                        Kind = kind,
                                        Action = action,
                                        ParamsTypes = action.GetType().GetGenericArguments()
                                    };
            switch (kind)
            {
                case StepKinds.Given:
                    givenStepDefinitions.Add(def);
                    break;
                case StepKinds.When:
                    whenStepDefinitions.Add(def);
                    break;
                case StepKinds.Then:
                    thenStepDefinitions.Add(def);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("kind");
            }

            
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
            //TODO: Stick the Transform def in here....
        }
    }
}