using System;

namespace Nucumber.Core
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
            if (parms != null)
            {
                action.DynamicInvoke(parms);
                return;
            }
            action.DynamicInvoke();
        }
    }
}
