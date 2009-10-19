using System;
using nStep.Framework.WorldViews;

namespace nStep.Framework
{
    public abstract class WorldViewProviderBase<TWorldView> : IProvideWorldView where TWorldView : IAmWorldView
    {
        protected abstract TWorldView InitializeWorldView();
        public Object WorldView { get{ return InitializeWorldView(); }}
        public Type WorldViewType { get { return typeof (TWorldView); }}
    }
}
