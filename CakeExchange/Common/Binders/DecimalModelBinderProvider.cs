using System;
using System.Linq;
using System.Reflection;
using CakeExchange.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CakeExchange.Common.Binders
{
        public class DecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!context.Metadata.IsComplexType)
            {
                // Look for scrubber attributes
                var propName = context.Metadata.PropertyName;
                var propInfo = context.Metadata.ContainerType.GetProperty(propName);

                // Only one scrubber attribute can be applied to each property
                var attribute = propInfo.GetCustomAttributes(typeof(DecimalAttribute), false).FirstOrDefault();
                if (attribute != null) return new DecimalModelBinder(context.Metadata.ModelType, attribute as IDecimalAttribute);
            }

            return null;
        }
    }

}