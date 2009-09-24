using System;

namespace Nucumber.Framework
{
    public class OnlyOneWorldViewTypeCanExistAtATimeException : ApplicationException
    {
        public OnlyOneWorldViewTypeCanExistAtATimeException(Type keyType) : base(string.Format("Only one WorldView of type: {0} can be used at a time.", keyType.FullName)){}
    }
}