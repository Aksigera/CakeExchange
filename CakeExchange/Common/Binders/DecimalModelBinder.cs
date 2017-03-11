using System;
using System.Threading.Tasks;
using CakeExchange.Common.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace CakeExchange.Common.Binders
{
    public class DecimalModelBinder : IModelBinder
    {
        IDecimalAttribute _attribute;
        SimpleTypeModelBinder _baseBinder;

        public DecimalModelBinder(Type type, IDecimalAttribute attribute)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            _attribute = attribute;
            _baseBinder = new SimpleTypeModelBinder(type);
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            // Check the value sent in
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult != ValueProviderResult.None)
            {
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                // Attempt to scrub the input value
                var valueAsString = valueProviderResult.FirstValue;
                bool success;
                var result = _attribute.Parse(valueAsString, out success);
                if (success)
                {
                    bindingContext.Result = ModelBindingResult.Success(result);
                    return Task.CompletedTask;
                }
            }

            // If we haven't handled it, then we'll let the base SimpleTypeModelBinder handle it
            return _baseBinder.BindModelAsync(bindingContext);
        }
    }
}