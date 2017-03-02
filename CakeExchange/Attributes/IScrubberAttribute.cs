namespace CakeExchange.Attributes
{
    public interface IScrubberAttribute
    {
        object Scrub(string modelValue, out bool success);
    }
}