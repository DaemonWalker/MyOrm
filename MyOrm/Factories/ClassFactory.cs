using MyOrm.DataBase;
using MyOrm.DBContext;
using MyOrm.DBOperator;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyOrm.Factories
{
    public class ClassFactory
    {
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
    }
}
