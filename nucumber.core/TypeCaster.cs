using System;

namespace Nucumber.Core
{
    public class TypeCaster
    {
        public object MakeIntoType(string value, Type type)
        {
            if (type == typeof(string))
                return value;
            if (type == typeof(Int32))
                return Int32.Parse(value);

            throw new InvalidCastException("Don't know how to convert: " + value + " into a " + type.Name);
        }
    }
}