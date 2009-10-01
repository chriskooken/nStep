using System;

namespace nStep.Framework
{
    public interface IProvideWorldView
    {
        Object WorldView { get; }
        Type WorldViewType { get;  }
    }
}