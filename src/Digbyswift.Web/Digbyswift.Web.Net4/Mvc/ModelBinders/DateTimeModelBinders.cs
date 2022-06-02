#if !NET5_0_OR_GREATER
using System;
using System.Globalization;
using System.Web.Mvc;

namespace Digbyswift.Web.Net4.Mvc.ModelBinders
{
    public class DateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            return value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
        }
    }

    public class NullableDateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null)
                return null;

            return DateTime.TryParse(value.AttemptedValue, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out var workingDate)
                ? (object)workingDate
                : null;
        }
    }
}
#endif