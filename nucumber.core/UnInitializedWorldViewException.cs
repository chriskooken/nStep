using System;

namespace Nucumber.Core
{
    public class UnInitializedWorldViewException : ApplicationException
    {
        public UnInitializedWorldViewException(string s): base(s)
        {

        }
    }
}