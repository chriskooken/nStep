
using System;
using System.Collections.Generic;

namespace Cucumber
{
    public interface IProvideSteps
    {
        IDictionary<Step,object> Steps {get;}
        void SetWorldView(object worldView);
    }
}