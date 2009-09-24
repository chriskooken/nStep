using System;

namespace Nucumber.Core
{
    public class InvalidScenarioLineNumberException : ApplicationException
    {
        public InvalidScenarioLineNumberException(string message): base(message)
        {
            
        }
    }
}