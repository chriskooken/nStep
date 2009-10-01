using System;

namespace nStep.Core
{
    public class OnlyOneEnvironmentMayBeDefinedException : ApplicationException
    {
        public OnlyOneEnvironmentMayBeDefinedException() : base("You can only define the enviornment subclass once")
        {
            
        }
    }
}