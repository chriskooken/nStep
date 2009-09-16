using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.App.CommandLineUtilities
{
    public class ConsoleOptionsBase
    {

        public static ConsoleOptionsBase Parse(string[] args)
        {
            throw new NotImplementedException();
        }

    }

    public class Required : Attribute
    {
        public Required()
        {
            
        }

        public Required(string shortHand)
        {
            
        }
    }

    public class Optional : Attribute
    {
        public Optional()
        {
            
        }

        public Optional(string shortHand)
        {
            
        }
    }
}
