using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cucumber
{
    public interface IGherkinParser
    {
        void Parse(string filename);
    }
}
