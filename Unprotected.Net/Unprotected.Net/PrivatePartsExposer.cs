namespace Unprotected.Net
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;


    public class PrivatePartsExposer<T> : DynamicObject
    {
        private Dictionary<string, PropertyInfo> Properties { get; set; }

        private T Source { get; set; }

        public PrivatePartsExposer(T source)
        {
            Source = source;
            var type = typeof(T);
            Properties = type.GetProperties()
                .Union(type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
                .ToDictionary(pi => pi.Name);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            PropertyInfo property;
            result = null;
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
            if (Properties.TryGetValue(binder.Name, out property))
            {
                property.SetValue(Source, value, null);
                return true;
            }
            return false;
        }
    }
}
