using System;

namespace Nucumber.Framework
{
    public class StepPendingException : ApplicationException
    {
        public StepPendingException() : base("The StepDefinition is Pending.")
        {}
    }
}