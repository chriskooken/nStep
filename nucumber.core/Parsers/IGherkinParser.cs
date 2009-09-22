using System;
using System.Linq;
using System.Text;
using Nucumber.Core.Parsers.DataStructures;

namespace Nucumber.Core.Parsers
{
    public interface IGherkinParser
    {
        SimpleTreeNode<LineValue> GetParseTree(string filename);
    }
}