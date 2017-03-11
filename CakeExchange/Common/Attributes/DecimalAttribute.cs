using System;
using System.Globalization;

namespace CakeExchange.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalAttribute : Attribute, IDecimalAttribute
    {
        public object Parse(string modelValue, out bool success)
        {
            string wantedSeperator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            string alternateSeperator = (wantedSeperator == "," ? "." : ",");

            // ReSharper disable once StringIndexOfIsCultureSpecific.1
            if (modelValue.IndexOf(wantedSeperator) == -1
            // ReSharper disable once StringIndexOfIsCultureSpecific.1
                && modelValue.IndexOf(alternateSeperator) != -1)
            {
                modelValue = modelValue.Replace(alternateSeperator, wantedSeperator);
            }

            decimal modelDecimal;
            success = decimal.TryParse(
                modelValue,
                NumberStyles.Any,
                CultureInfo.CurrentCulture,
                out modelDecimal
            );

            return modelDecimal;
        }
    }
}