using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TinyListener
{
    public static class Helper
    {
        public static string ToLowerFirst(this string s)
        {
            return Char.ToLowerInvariant(s[0]) + s.Substring(1);
        }

        private static Dictionary<Type, Dictionary<string, PropertyInfo>> props =
            new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        public static Dictionary<string, object> GetProperties(this object o)
        {
            Dictionary<string, PropertyInfo> dict = null;
            var ret = new Dictionary<string, object>();
            var t = o.GetType();

            if (props.ContainsKey(t))
            {
                dict = props[t];
            }
            else
            {
                dict = new Dictionary<string, PropertyInfo>();
                var prps = t.GetRuntimeProperties().Where(d => d.CanRead);

                foreach (var prp in prps)
                {
                    var key = prp.Name.ToLowerFirst();
                    dict.Add(key, prp);
                }

                props.Add(t, dict);
            }

            foreach (var kv in dict)
            {
                var val = kv.Value.GetValue(o, null);

                if (val != null)
                {
                    ret.Add(kv.Key, val);
                }
            }

            return ret;
        }
    }
}
