using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nucumber.Framework
{
    public interface IProvideSteps
    {
        IDictionary<Regex,object> Steps {get;}
        void SetWorldView(object worldView);
    }
}