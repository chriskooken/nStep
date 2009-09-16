using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.App.CommandLineUtilities
{
    public class ConsoleOptions : ConsoleOptionsBase
    {
        [Required]
        public string FeatureFiles { get; set; }
        
        [Optional("r","require")]
        public string Assemblies { get; set; }

        [Optional("v","verbose")]
        public bool Verbose { get; set; }

    }
}
