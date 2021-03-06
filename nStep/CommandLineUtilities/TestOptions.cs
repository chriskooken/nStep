﻿using System.Collections.Generic;
using nStep.App.CommandLineUtilities;

namespace nStep.App.CommandLineUtilities
{
    public class TestOptions : ConsoleOptionsBase
    {   
        [Default]
        [Required]
        public string FeatureFiles { get; set; }

        [Switch("r","require")]
        public IList<string> Assemblies { get; set; }

        [Switch("v","verbose")]
        public bool Verbose { get; set; }

        public Format Format { get; set; }

    }

    public enum Format
    {
        Html,
        Text,
        Xml
    }
}
