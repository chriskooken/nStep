using System;
using System.Collections.Generic;
using nStep.Core.Features;
using nStep.Framework;
using System.Linq;
namespace nStep.Core
{
    public class TypeCaster
    {
        private readonly IEnumerable<TransformDefinition> transforms;

        public TypeCaster(IEnumerable<TransformDefinition> transforms)
        {
            this.transforms = transforms ?? new List<TransformDefinition>();
        }

        public object MakeIntoType(string valueBeingCast, Type typeBeingCastTo)
        {
            try
            {
                if (typeBeingCastTo == typeof(string))
                    return valueBeingCast;

                if (typeBeingCastTo == typeof(Int32))
                    return Int32.Parse(valueBeingCast);

                if (typeBeingCastTo == typeof(Int64))
                    return Int64.Parse(valueBeingCast);

                if (typeBeingCastTo == typeof(double))
                    return double.Parse(valueBeingCast);

                if (typeBeingCastTo == typeof(decimal))
                    return decimal.Parse(valueBeingCast);

                if (typeBeingCastTo == typeof(float))
                    return float.Parse(valueBeingCast);

                if (typeBeingCastTo == typeof(DateTime))
                    return DateTime.Parse(valueBeingCast);

                if (typeBeingCastTo == typeof(bool))
                    return bool.Parse(valueBeingCast);

                if (typeBeingCastTo == typeof(Guid))
                    return new Guid(valueBeingCast);

                var defs = transforms.Where(x => x.ReturnType == typeBeingCastTo && x.Regex.Match(valueBeingCast).Success);
                if (defs.Any())
                    return new TransformCaller(defs.First(), this).Call(valueBeingCast);

            }
            catch (Exception ex)
            {
                var message = "Unable to convert: \"" + valueBeingCast + "\" into a " + typeBeingCastTo.Name;
                var newException = new InvalidCastException(message);
                throw new InvalidCastException(message, newException);
            }
                throw new InvalidCastException("Don't know how to convert: \"" + valueBeingCast + "\" into a " + typeBeingCastTo.Name);
        }
    }
}