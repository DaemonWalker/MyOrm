using MyOrm.Attributes;
using MyOrm.DataBase;
using MyOrm.DBContext;
using MyOrm.DBOperator;
using MyOrm.EntityDataBaseConvert;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyOrm.Factories
{
    public class ClassFactory
    {
        public static PrimaryKeyAttribute PKAttr { get; set; } = new PrimaryKeyAttribute();
        internal static AbsDataOperator GetDataOperator()
        {
            return new DataOperator();
        }
        internal static IDataBase GetDataBase()
        {
            return new MSSqliteDataBase();
        }
        public static AbsDBContext<T> GetDBContext<T>() where T : class
        {
            return new DBContext<T>();
        }
        internal static AbsEntityDataBaseConvert GetEntityDBConvert()
        {
            return new SqliteEntityDBConvert();
        }
        public static Type GetPKAttrType
        {
            get
            {
                return ClassFactory.PKAttr.GetType();
            }
        }
    }
}
