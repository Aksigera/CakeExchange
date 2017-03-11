using System.ComponentModel.DataAnnotations;

namespace CakeExchange.Common.Attributes
{
    public class PositiveAttribute : ValidationAttribute
    {
        public override bool IsValid(object number)
        {
            return decimal.Parse(number.ToString()) > 0;
        }
    }
}