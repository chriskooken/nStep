using System;
using Nucumber.Framework;

namespace Nucumber.Core
{
    public interface IWorldViewDictionary : System.Collections.Generic.IDictionary<Type,Object>
    {
        void Import(IProvideWorldView worldViewProvider);
    }
}