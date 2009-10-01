using System;
using nStep.Core.Features;

namespace nStep.Core
{
    public class TypeCaster
    {
        public object MakeIntoType(string value, Type type)
        {
            try
            {
                if (type == typeof(string))
                    return value;

                if (type == typeof(Int32))
                    return Int32.Parse(value);

                if (type == typeof(Int64))
                    return Int64.Parse(value);

                if (type == typeof(double))
                    return double.Parse(value);

                if (type == typeof(decimal))
                    return decimal.Parse(value);

                if (type == typeof(float))
                    return float.Parse(value);

                if (type == typeof(DateTime))
                    return DateTime.Parse(value);

                if (type == typeof(bool))
                    return bool.Parse(value);

                if (type == typeof(Guid))
                    return new Guid(value);

            }
            catch (Exception ex)
            {
                var message = "Unable to convert: \"" + value + "\" into a " + type.Name;
                var newException = new InvalidCastException(message);
                throw new InvalidCastException(message, newException);
            }
                throw new InvalidCastException("Don't know how to convert: \"" + value + "\" into a " + type.Name);
        }
    }
}