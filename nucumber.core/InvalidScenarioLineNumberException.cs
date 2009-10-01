using System;

namespace nStep.Core
{
    public class InvalidScenarioLineNumberException : ApplicationException
    {
        public InvalidScenarioLineNumberException(string message): base(message)
        {
            
        }
    }
}