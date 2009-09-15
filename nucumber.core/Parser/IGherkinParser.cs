using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.Core.Parser
{
    public interface IGherkinParser
    {
        SimpleTreeNode<LineValue> GetParseTree(string filename);
    }
}