using System;

namespace CakeExchange.Attributes
{
    public interface IDecimalAttribute
    {
        object Parse(string modelValue, out bool success);
    }
}