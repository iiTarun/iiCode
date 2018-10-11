using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done
{
    public class AbstractSearchModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var derivedModelType = GetDerivedType(controllerContext, bindingContext);

            if (derivedModelType == null)
            {
                throw new InvalidOperationException("Invalid ModelTypeName");
            }

            return base.CreateModel(controllerContext, bindingContext, derivedModelType);
        }

        protected override System.ComponentModel.PropertyDescriptorCollection GetModelProperties(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return TypeDescriptor.GetProperties(GetDerivedType(controllerContext, bindingContext));
        }

        private static Type GetDerivedType(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var modelTypeValue = controllerContext.Controller.ValueProvider.GetValue(bindingContext.ModelName + ".ModelTypeName");

            if (modelTypeValue == null)
            {
                throw new InvalidOperationException("View does not contain ModelTypeName");
            }

            string modelTypeName = modelTypeValue.AttemptedValue;

            return Type.GetType(modelTypeName);
        }
    }
}