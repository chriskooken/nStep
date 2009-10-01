using System;

namespace Nucumber.Core.WireProtocol.Responses
{
    public class StepFailure : VersionOneMessage
    {
        public static StepFailure FromException(Exception exception)
        {
            return new StepFailure
                       {
                           step_failed = Failed.FromException(exception)
                       };
        }

        public Failed step_failed { get; set; }
    }
}