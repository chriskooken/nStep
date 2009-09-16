using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.App.CommandLineUtilities
{
    public class ConsoleOptionsBase
    {
        public TConsoleOptions Parse<TConsoleOptions>(string[] args) where TConsoleOptions : class
        {
            

            return this as TConsoleOptions;
        }

    }

    public class Required : Attribute
    {
        public Required()
        {
            
        }

        public Required(params string[] switches)
        {
            
        }
    }

    public class Optional : Attribute
    {
        public Optional()
        {
            
        }

        public Optional(params string[] switches)
        {
            
        }
    }

}
