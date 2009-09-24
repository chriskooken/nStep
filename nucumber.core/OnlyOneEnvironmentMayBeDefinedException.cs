using System;

namespace Nucumber.Core
{
    public class OnlyOneEnvironmentMayBeDefinedException : ApplicationException
    {
        public OnlyOneEnvironmentMayBeDefinedException() : base("You can only define the enviornment subclass once")
        {
            
        }
    }
}