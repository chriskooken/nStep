using System;

namespace nStep.Core.Exceptions
{
    public class UnInitializedWorldViewException : ApplicationException
    {
        public UnInitializedWorldViewException(string s): base(s)
        {

        }
    }
}