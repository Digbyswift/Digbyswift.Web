#if NET462
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Digbyswift.Web.Mvc.ModelBinders
{
    public class CommaSeparatedModelBinder : DefaultModelBinder
    {
        private static readonly MethodInfo ToArrayMethod = typeof(Enumerable).GetMethod("ToArray");

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return BindCsv(bindingContext.ModelType, bindingContext.ModelName, bindingContext)
                   ?? base.BindModel(controllerContext, bindingContext);
        }

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            return BindCsv(propertyDescriptor.PropertyType, propertyDescriptor.Name, bindingContext)
                   ?? base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }

        private object BindCsv(Type type, string name, ModelBindingContext bindingContext)
        {
            if (type.GetInterface(nameof(IEnumerable)) == null)
                return null;
            
            var actualValue = bindingContext.ValueProvider.GetValue(name);
            if (actualValue == null)
                return null;
            
            var valueType = type.GetElementType() ?? type.GetGenericArguments().FirstOrDefault();
            if (valueType?.GetInterface(nameof(IConvertible)) == null)
                return null;
            
            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(valueType));

            foreach (var splitValue in actualValue.AttemptedValue.Split(new[] { ',' }))
            {
                if (!String.IsNullOrWhiteSpace(splitValue))
                    list.Add(Convert.ChangeType(splitValue, valueType));
            }

            return type.IsArray
                ? ToArrayMethod.MakeGenericMethod(valueType).Invoke(this, new[] { list })
                : list;
        }
    }
}
#endif