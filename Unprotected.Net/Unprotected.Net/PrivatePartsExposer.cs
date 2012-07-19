namespace Unprotected.Net
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;


    public class PrivatePartsExposer<T> : DynamicObject
    {
        private const BindingFlags NonPublicInstanceflags = BindingFlags.NonPublic | BindingFlags.Instance;
        private readonly Type _type;
        private Dictionary<string, PropertyInfo> Properties { get; set; }
        private Dictionary<string, FieldInfo> Fields { get; set; }

        private Dictionary<string, MethodInfo> Methods { get; set; }

        private T Source { get; set; }

        public PrivatePartsExposer(T source)
        {
            Source = source;
            _type = typeof(T);
            Properties = _type.GetProperties()
                .Union(_type.GetProperties(NonPublicInstanceflags))
                .ToDictionary(pi => pi.Name);
            Fields = _type.GetFields()
                .Union(_type.GetFields(NonPublicInstanceflags))
                .ToDictionary(fi => fi.Name);
            Methods = _type.GetMethods()
                .Union(_type.GetMethods(NonPublicInstanceflags | BindingFlags.InvokeMethod))
                .ToDictionary(mi => mi.Name);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            PropertyInfo property;
            FieldInfo field;
            result = null;
            if(Fields.TryGetValue(binder.Name, out field))
            {
                result = field.GetValue(Source);
                return true;
            }
            if(Properties.TryGetValue(binder.Name, out property))
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
            if(Fields.TryGetValue(binder.Name, out field))
            {
                field.SetValue(Source, value);
                return true;
            }
            if(Properties.TryGetValue(binder.Name, out property))
            {
                property.SetValue(Source, value, null);
                return true;
            }
            return false;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;
            MethodInfo method;
            if(Methods.TryGetValue(binder.Name, out method))
            {
                const BindingFlags flags = NonPublicInstanceflags | BindingFlags.Public | BindingFlags.InvokeMethod;
                result = method.Invoke(Source, flags, Type.DefaultBinder, args, CultureInfo.CurrentCulture);
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
