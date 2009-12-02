using System;
using System.Reflection;

namespace nStep.Core
{
    public class ExecutableAction
    {
        private readonly Delegate action;
        private readonly object[] parms;

        public ExecutableAction(Delegate action)
        {
            this.action = action;
        }

        public ExecutableAction(Delegate action, params Object[] parms) : this(action)
        {
            this.parms = parms;
        }

        public object Call()
        {
            try
            {
                return parms != null ? action.DynamicInvoke(parms) : action.DynamicInvoke();
            }
            catch (TargetInvocationException exception)
            {
                throw new NStepInvocationException(exception.InnerException.Message, exception.InnerException);
            }
           
        }
    }

    public class NStepInvocationException : ApplicationException
    {
        public NStepInvocationException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
