using MyOrm.DataBase;
using MyOrm.DBContext;
using MyOrm.Factories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace MyOrm.DBOperator
{
    abstract class AbsDataOperator
    {
        public AbsDataOperator()
        {
            this.DataBase = ClassFactory.GetDataBase();
        }
        protected IDataBase DataBase { get; set; }
        public abstract List<T> QueryObject<T>(string sql);
        public abstract DataTable QuerySql(string sql);
        public abstract int ExecuteNonQuery(Func<DbCommand, int> executeFunc);
        public abstract object ExecuteSclar(string sql);
    }
}
