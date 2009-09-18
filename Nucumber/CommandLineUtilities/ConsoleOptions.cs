using System.Collections.Generic;

namespace Nucumber.App.CommandLineUtilities
{
    public class ConsoleOptions : ConsoleOptionsBase
    {   
        [Default]
        [Required]
        public string FeatureFiles { get; set; }
        
        [Switch("r","require")]
        public string Assemblies { get; set; }

        [Switch("v","verbose")]
        public bool Verbose { get; set; }

        public string Format { get; set; }
    }
}
