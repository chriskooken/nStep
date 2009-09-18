using System;

namespace Nucumber.Framework
{
    public abstract class StepDefinitionDsl<TWorldView> where TWorldView : class
    {
        protected IRunStepsFromStrings StepRunner;

        protected abstract void AddNewStepDefinition(StepKinds kind, string stepText, Delegate action);

        protected abstract void AddTransform<TReturn>(Delegate func, string regex);

        public virtual void BeforeStep()
        { }

        public virtual void AfterStep()
        { }

        #region Given StepDefinitions

        protected void Given(string featureLine)
        {
            StepRunner.ProcessStep(StepKinds.Given, featureLine);
        }

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
        protected void When(string featureLine)
        {
            StepRunner.ProcessStep(StepKinds.When, featureLine);
        }

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
        protected void Then(string featureLine)
        {
            StepRunner.ProcessStep(StepKinds.Then, featureLine);
        }

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

        protected TWorldView World { get; set; }

        protected void Pending()
        {
            throw new StepPendingException();
        }

        #region Transform
        protected void Transform<TReturn>(string regex, Func<TReturn> func)
        {
            AddTransform<TReturn>(func, regex);
        }

        protected void Transform<TReturn>(string regex, Func<string, TReturn> func)
        {
            AddTransform<TReturn>(func, regex);
        }

        protected void Transform<TReturn>(string regex, Func<string, string, TReturn> func)
        {
            AddTransform<TReturn>(func, regex);
        }

        protected void Transform<TReturn>(string regex, Func<string, string, string, string, TReturn> func)
        {
            AddTransform<TReturn>(func, regex);
        }

        protected void Transform<TReturn, T1>(string regex, Func<T1, TReturn> func)
        {
            AddTransform<TReturn>(func, regex);
        }

        protected void Transform<TReturn, T1, T2>(string regex, Func<T1, T2, TReturn> func)
        {
            AddTransform<TReturn>(func, regex);
        }

        protected void Transform<TReturn, T1, T2, T3>(string regex, Func<T1, T2, T3, TReturn> func)
        {
            AddTransform<TReturn>(func, regex);
        }

        protected void Transform<TReturn, T1, T2, T3, T4>(string regex, Func<T1, T2, T3, T4, TReturn> func)
        {
            AddTransform<TReturn>(func, regex);
        }
        #endregion
    }
}