using System.Collections.Generic;
using System.IO;

namespace Nucumber.App.CommandLineUtilities
{
    public class NucumberOptions : ConsoleOptionsBase
    {
        [Default]
        [Required]
        public string FeatureFiles { get;  set; }

        [Switch("r", "require")]
        public IList<string> Assemblies { get; set; }

        [Switch("v", "verbose")]
        public bool Verbose { get; set; }

        public OutputFormat Format { get; set; }

    }

    public enum OutputFormat
    {
        Html,
        Text,
        Xml
    }
}
