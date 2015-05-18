// FileInformation: nyanya/nyanya.Xingye/ModelNameHelper.cs
// CreatedTime: 2014/09/01   10:42 AM
// LastUpdatedTime: 2014/09/01   10:46 AM

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace nyanya.Xingye.Areas.HelpPage.ModelDescriptions
{
    internal static class ModelNameHelper
    {
        // Modify this to provide custom model name mapping.
        public static string GetModelName(Type type)
        {
            ModelNameAttribute modelNameAttribute = type.GetCustomAttribute<ModelNameAttribute>();
            if (modelNameAttribute != null && !String.IsNullOrEmpty(modelNameAttribute.Name))
            {
                return modelNameAttribute.Name;
            }

            string modelName = type.Name;
            if (type.IsGenericType)
            {
                // Format the generic type name to something like: GenericOfAgurment1AndArgument2
                Type genericType = type.GetGenericTypeDefinition();
                Type[] genericArguments = type.GetGenericArguments();
                string genericTypeName = genericType.Name;

                // Trim the generic parameter counts from the name
                genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
                string[] argumentTypeNames = genericArguments.Select(t => GetModelName(t)).ToArray();
                modelName = String.Format(CultureInfo.InvariantCulture, "{0}Of{1}", genericTypeName, String.Join("And", argumentTypeNames));
            }

            return modelName;
        }
    }
}