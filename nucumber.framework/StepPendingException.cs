using System;

namespace nStep.Framework
{
    public class StepPendingException : ApplicationException
    {
        public StepPendingException() : base("The StepDefinition is Pending.")
        {}
    }
}