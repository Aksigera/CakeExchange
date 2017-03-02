using System;
using System.Globalization;

namespace CakeExchange.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CurrencyScrubberAttribute : Attribute, IScrubberAttribute
    {
        private static NumberStyles _currencyStyle = NumberStyles.Currency;
        private CultureInfo _culture = new CultureInfo("ru-RU");

        public object Scrub(string modelValue, out bool success)
        {
            decimal modelDecimal;
            success = decimal.TryParse(
                modelValue,
                _currencyStyle,
                _culture,
                out modelDecimal
            );

            return modelDecimal;
        }
    }
}