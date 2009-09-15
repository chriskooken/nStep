
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace nucumber.Framework
{
    public interface IProvideSteps
    {
        IDictionary<Regex,object> Steps {get;}
        void SetWorldView(object worldView);
    }
}