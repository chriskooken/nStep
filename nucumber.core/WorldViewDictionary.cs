using System;
using Nucumber.Framework;

namespace Nucumber.Core
{
    public class WorldViewDictionary : System.Collections.Generic.Dictionary<Type,Object>, IWorldViewDictionary
    {
        public void Import(IProvideWorldView worldViewProvider)
        {
            Add(worldViewProvider.WorldViewType, worldViewProvider.WorldView);
        }

        public new void Add(Type key, Object value)
        {
            try
            {
                base.Add(key, value);
            }
            catch (ArgumentException)
            {
                throw new OnlyOneWorldViewTypeCanExistAtATimeException(key);
            }
        }
        
    }
}