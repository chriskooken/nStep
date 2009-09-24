using System;
using System.Collections.Generic;
using Nucumber.Framework;

namespace Nucumber.Core
{
    public class WorldViewDictionary : System.Collections.Generic.Dictionary<Type,Object>, IWorldViewDictionary
    {
        public void Import(IProvideWorldView worldViewProvider)
        {
            Add(worldViewProvider.WorldViewType, worldViewProvider.WorldView);
        }

        public T GetWorldViewOfType<T>()
        {
            return (T) this[typeof (T)];
        }

        public void Import(IEnumerable<IProvideWorldView> worldViewProviders)
        {
            foreach (var worldViewProvider in worldViewProviders)
            {
                Import(worldViewProvider);
            }
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