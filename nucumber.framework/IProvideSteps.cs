
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cucumber
{
    public interface IProvideSteps
    {
        IDictionary<Regex,object> Steps {get;}
        void SetWorldView(object worldView);
    }
}