using System;

namespace nStep.Core.Exceptions
{
    public class OnlyOneEnvironmentMayBeDefinedException : ApplicationException
    {
        public OnlyOneEnvironmentMayBeDefinedException() : base("You can only define the enviornment subclass once")
        {
            
        }
    }
}