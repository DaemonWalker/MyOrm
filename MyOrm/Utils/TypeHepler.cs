using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MyOrm.Utils
{
    static class TypeHepler
    {
        public static Type GetClassGenericType(Type t)
        {
            if (t.IsGenericType)
            {
                return t.GenericTypeArguments[0];
            }
            else
            {
                return GetClassGenericType(t.BaseType);
            }
        }
        public static T DeepCopy<T>(this T t)
        {
            //var bin = new BinaryFormatter();
            //var ms = new MemoryStream();
            //bin.Serialize(ms, t);
            //ms.Position = 0;
            //return (T)bin.Deserialize(ms);
            var newT = Activator.CreateInstance<T>();
            foreach (var prop in typeof(T).GetProperties().Where(p => p.CanWrite))
            {
                prop.SetValue(newT, prop.GetValue(t));
            }
            return newT;
        }
    }
}
