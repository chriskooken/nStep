using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucumber.core
{
    public interface IGherkinParser
    {
        SimpleTreeNode<LineValue> GetParseTree(string filename);
    }
}
