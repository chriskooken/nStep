using System;

namespace nStep.Core
{
    public class UnInitializedWorldViewException : ApplicationException
    {
        public UnInitializedWorldViewException(string s): base(s)
        {

        }
    }
}