using System;
using System.Collections.Generic;
using nStep.Framework;

namespace nStep.Core
{
    public class WorldViewDictionary : Dictionary<Type,Object>, IWorldViewDictionary
    {
        public void Import(IProvideWorldView worldViewProvider)
        {
            Add(worldViewProvider.WorldViewType, worldViewProvider.WorldView);
        }

        public T GetWorldViewOfType<T>() where T : IAmWorldView
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