using MyOrm.Attributes;
using MyOrm.DBContext;
using MyOrm.Exceptions;
using MyOrm.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyOrm.Utils
{
    static class EntityCheck
    {
        public static PropertyInfo GetPKProperty<T>(this AbsDBContext<T> db, Type t) where T : class
        {
            foreach (var prop in t.GetProperties())
            {
                if (prop.CustomAttributes
                    .Where(p => p.AttributeType == ClassFactory.GetPKAttrType)
                    .Count() != 0)
                {
                    return prop;
                }
            }
            throw new NoPrimaryKeyException(t);
        }
    }
}
