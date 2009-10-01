using System;

namespace Nucumber.Core.WireProtocol.Responses
{
    public class GeneralFailure : VersionOneMessage
    {
        public static GeneralFailure FromException(Exception exception)
        {
            return new GeneralFailure
                       {
                           failed = Failed.FromException(exception)
                       };
        }

        public Failed failed { get; set; }
    }
}