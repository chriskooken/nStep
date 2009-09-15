using System;

namespace Nucumber.Framework
{
    public abstract class WorldViewProviderBase<TWorldView> : IProvideWorldView
    {
        protected abstract TWorldView InitializeWorldView();
        public Object WorldView { get{return InitializeWorldView();}}
    }
}
