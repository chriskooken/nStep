using System;

namespace Nucumber.Core
{
    public class ParameterMismatchExcepsion : ApplicationException
    {

        private readonly string _source;

        public ParameterMismatchExcepsion(string s, string source) : base (s)
        {
            _source = source;
        }

        public override string StackTrace
        {
            get
            {
                return _source;
            }
        }
    }
}