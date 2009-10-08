using System;
using System.Collections.Generic;
using nStep.App.CommandLineUtilities;

namespace nStep.App.CommandLineUtilities
{
    [Serializable]
    public class nStepOptions : ConsoleOptionsBase
    {
        [Default]
        [Required]
        [Help("Default option\nRequired\nPath to feature file or directory of files.")]
        public string FeatureFiles { get;  set; }

        [Switch("r", "require")]
        [Help("Path to required dlls")]
        public IList<string> Assemblies { get; set; }

        [Switch("v", "verbose")]
        [Help("v means loud")]
        public bool Verbose { get; set; }

        [Help("Html, Text, Xml.\nDescribes the output format.")]
        public OutputFormat Format { get; set; }

        [Switch("d", "debug")]
        [Help("Allows user to set up debugger before executing.")]
        public bool Debug { get; set; }

        [Switch("rr", "rerun")]
        [Help("Allows the user to re-run with the last command-line arguments")]
        public bool Rerun { get; set; }

    }

    public enum OutputFormat
    {
        Html,
        Text,
        Xml
    }
}
