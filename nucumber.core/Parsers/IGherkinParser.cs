using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.Core.Parsers
{
    public interface IGherkinParser
    {
        SimpleTreeNode<LineValue> GetParseTree(string filename);
    }
}