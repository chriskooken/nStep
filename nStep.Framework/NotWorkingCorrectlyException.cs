using System;

namespace nStep.Framework
{
    public class NotWorkingCorrectlyException : ApplicationException
    {
        public NotWorkingCorrectlyException():base("This is a placeholder to notify you that this is not working correctly and needs to be examined")
        {
            
        }
    }
}