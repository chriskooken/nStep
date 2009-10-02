using System;

namespace nStep.Core.Exceptions
{
    public class InvalidScenarioLineNumberException : ApplicationException
    {
        public InvalidScenarioLineNumberException(string message): base(message)
        {
            
        }
    }
}