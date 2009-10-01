using System;
using System.Reflection;

namespace nStep.Core
{
    public class ActionCaller
    {
        private readonly Delegate action;
        private readonly object[] parms;

        public ActionCaller(Delegate action)
        {
            this.action = action;
        }

        public ActionCaller(Delegate action, params Object[] parms) : this(action)
        {
            this.parms = parms;
        }

        public void Call()
        {
            try
            {
                if (parms != null)
                {
                    action.DynamicInvoke(parms);
                    return;
                }
                action.DynamicInvoke();
            }
            catch (TargetInvocationException exception)
            {
               throw new ApplicationException(exception.InnerException.Message, exception.InnerException);
            }
           
        }
    }

}
