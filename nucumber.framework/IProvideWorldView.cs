using System;

namespace Nucumber.Framework
{
    public interface IProvideWorldView
    {
        Object WorldView { get; }
        Type WorldViewType { get;  }
    }
}