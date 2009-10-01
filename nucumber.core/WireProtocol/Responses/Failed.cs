using System;

namespace Nucumber.Core.WireProtocol.Responses
{
    public class Failed
    {
        public string exception { get; set; }

        public string message { get; set; }

        public string backtrace { get; set; }

        public static Failed FromException(Exception exception)
        {
            return new Failed
                       {
                           exception = exception.GetType().FullName,
                           message = exception.Message,
                           backtrace = exception.StackTrace
                       };
        }
    }
}