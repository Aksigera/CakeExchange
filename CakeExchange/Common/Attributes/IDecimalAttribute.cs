namespace CakeExchange.Common.Attributes
{
    public interface IDecimalAttribute
    {
        object Parse(string modelValue, out bool success);
    }
}