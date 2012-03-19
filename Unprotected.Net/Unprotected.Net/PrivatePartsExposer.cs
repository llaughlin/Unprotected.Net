namespace Unprotected.Net
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;


    public class PrivatePartsExposer<T> : DynamicObject
    {
        private Dictionary<string, PropertyInfo> Properties { get; set; }

        private Dictionary<string, FieldInfo> Fields { get; set; }

        private T Source { get; set; }

        public PrivatePartsExposer(T source)
        {
            Source = source;
            var type = typeof(T);
            Properties = type.GetProperties()
                .Union(type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
                .ToDictionary(pi => pi.Name);
            Fields = type.GetFields()
                .Union(type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                .ToDictionary(fi => fi.Name);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            PropertyInfo property;
            FieldInfo field;
            result = null;
            if (Fields.TryGetValue(binder.Name, out field))
            {
                result = field.GetValue(Source);
                return true;
            }
            if (Properties.TryGetValue(binder.Name, out property))
            {
                result = property.GetValue(Source, null);
                return true;
            }
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            PropertyInfo property;
            FieldInfo field;
            if (Fields.TryGetValue(binder.Name, out field))
            {
                field.SetValue(Source, value);
                return true;
            }
            if (Properties.TryGetValue(binder.Name, out property))
            {
                property.SetValue(Source, value, null);
                return true;
            }
            return false;
        }
    }

    public static class ExposerExtensions
    {
        public static PrivatePartsExposer<T> ExposePrivateParts<T>(this T source)
        {
            return new PrivatePartsExposer<T>(source);
        }
    }
}
