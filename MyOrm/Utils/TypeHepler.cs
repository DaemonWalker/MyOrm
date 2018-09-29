﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MyOrm.Utils
{
    static class TypeHepler
    {
        public static Type GetClassGenericType(Type t)
        {
            return t.GenericTypeArguments[0];
        }
        public static T DeepCopy<T>(this T t)
        {
            var bin = new BinaryFormatter();
            var ms = new MemoryStream();
            bin.Serialize(ms, t);
            ms.Position = 0;
            return (T)bin.Deserialize(ms);
        }
    }
}