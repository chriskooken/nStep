using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cucumber
{
    public abstract class Environment
    {
        public virtual void Session_Start()
        {
            
        }

        public virtual void Session_End()
        {

        }

        public virtual void Before_Scenario()
        {

        }

        public virtual void After_Scenario()
        {

        }

        public virtual void Before_Step()
        {

        }

        public virtual void After_Step()
        {

        }
    }
}
