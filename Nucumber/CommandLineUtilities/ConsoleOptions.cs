using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.App.CommandLineUtilities
{
    public class ConsoleOptions : ConsoleOptionsBase
    {
        [Required]
        public string FeatureFile { get; set; }

        [Required]
        public string Assembly { get; set; }

        [Optional]
        public int LineNumber { get; set; }
    }
}
