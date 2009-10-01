using System;
using LitJson;

namespace Nucumber.Core.WireProtocol
{
    public class VersionOneMessage
    {
        public string version 
        {
            get { return "1.0"; }
            set { if (value != "1.0") throw new InvalidOperationException
                ("Version 1.0 only supported.");}
        }

        public string ToJson()
        {
            return JsonMapper.ToJson(this);
        }
    }
}