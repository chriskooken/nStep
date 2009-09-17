using System;

namespace Nucumber.Core
{
    public class TypeCaster
    {
        public object MakeIntoType(string value, Type type)
        {
            if (type == typeof (string))
                return value;

            if (type == typeof (Int32))
                return Int32.Parse(value);

            if (type == typeof (Int64))
                return Int64.Parse(value);

            if (type == typeof (double))
                return double.Parse(value);

            if (type == typeof (decimal))
                return decimal.Parse(value);

            if (type == typeof (float))
                return float.Parse(value);

            if (type == typeof (DateTime))
                return DateTime.Parse(value);

            if (type == typeof (bool))
                return bool.Parse(value);

            if (type == typeof(Guid))
                return new Guid(value);

            throw new InvalidCastException("Don't know how to convert: " + value + " into a " + type.Name);
        }
    }
}