using System;
using System.Reflection;

namespace Utils
{
    public static class AssetsInjector
    {
        private static readonly Type _injectAssetAttributeType = typeof(InjectAssetAttribute);
        
        public static T Inject<T>(this AssetsContext context, T target)
        {
            Type targetType = target.GetType();
            FieldInfo[] allFields = targetType.GetFields(BindingFlags.NonPublic
                                                         | BindingFlags.Public
                                                         | BindingFlags.Instance);

            for (int i = 0; i < allFields.Length; i++)
            {
                FieldInfo fieldInfo = allFields[i];
                
                if (fieldInfo.GetCustomAttribute(_injectAssetAttributeType) is not InjectAssetAttribute injectAssetAttribute)
                    continue;

                UnityEngine.Object objectToInject = context.GetObjectOfType(fieldInfo.FieldType, injectAssetAttribute.AssetName);
                fieldInfo.SetValue(target, objectToInject);
            }	

            return target;
        }
    }
}