﻿using MyOrm.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyOrm.EntityDataBaseConvert
{
    class SqliteEntityDBConvert : AbsEntityDataBaseConvert
    {
        public override int Count<T>(Expression where)
        {
            throw new NotImplementedException();
        }

        public override T Delete<T>(T t)
        {
            throw new NotImplementedException();
        }

        public override List<T> Delete<T>(IEnumerable<T> ts)
        {
            throw new NotImplementedException();
        }

        public override T Insert<T>(T t)
        {
            var tableName = typeof(T).Name;
            var col = new StringBuilder();
            var parms = new StringBuilder();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.CanWrite &&
                    prop.CustomAttributes.Where(p => p.AttributeType.Name == "").Count() == 0)
                {
                    col.AppendFormat("{0}, ", prop.Name);
                    parms.AppendFormat("{0}, ", prop.GetValue(t));
                }
            }
            col.Length = col.Length - 2;
            parms.Length = parms.Length - 2;
            var sql = $@"
insert into {tableName} 
       ({col.ToString()})
values ({parms.ToString()})";

            var pkProp = typeof(T).GetProperties().First(
                p => p.CustomAttributes.Count(p2 => p2.AttributeType.Name == "") > 0);
            var pk = $@"
SELECT {pkProp.Name}
  FROM {tableName}
 WHERE changes() = 1 
   AND {pkProp.Name} = last_insert_rowid()";

            Func<DbCommand, int> execute = (dbCommand) =>
            {
                dbCommand.CommandText = sql;
                var result = dbCommand.ExecuteNonQuery();

                dbCommand.CommandText = pk;
                pkProp.SetValue(t, dbCommand.ExecuteScalar());
                return result;
            };

            this.dataOperator.ExecuteNonQuery(execute);
            return t;
        }

        public override List<T> Insert<T>(IEnumerable<T> ts)
        {
            var tableName = typeof(T).Name;
            

            var pkProp = typeof(T).GetProperties().First(
                p => p.CustomAttributes.Count(p2 => p2.AttributeType.Name == "") > 0);
            var pk = $@"
SELECT {pkProp.Name}
  FROM {tableName}
 WHERE changes() = 1 
   AND {pkProp.Name} = last_insert_rowid()";

            Func<DbCommand, int> execute = (dbCommand) =>
            {
                var result = 0;
                foreach (var t in ts)
                {
                    var col = new StringBuilder();
                    var parms = new StringBuilder();
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        if (prop.CanWrite &&
                            prop.CustomAttributes.Where(p => p.AttributeType.Name == "").Count() == 0)
                        {
                            col.AppendFormat("{0}, ", prop.Name);
                            parms.AppendFormat("{0}, ", prop.GetValue(t));
                        }
                    }
                    col.Length = col.Length - 2;
                    parms.Length = parms.Length - 2;
                    var sql = $@"
insert into {tableName} 
       ({col.ToString()})
values ({parms.ToString()})";
                    dbCommand.CommandText = sql;
                    result = result + dbCommand.ExecuteNonQuery();

                    dbCommand.CommandText = pk;
                    pkProp.SetValue(t, dbCommand.ExecuteScalar());
                }
                return result;
            };

            this.dataOperator.ExecuteNonQuery(execute);
            return ts.ToList();
        }

        public override List<T> Select<T>(Expression where)
        {
            var tableName = typeof(T).Name;
            var sql = new StringBuilder($@"select * from {tableName} where {ExpressionAnalyze.Where(where)}");
            return dataOperator.QueryObject<T>(sql.ToString());
        }

        public override List<T> Select<T>(Expression where, Expression orderBy)
        {
            var tableName = typeof(T).Name;
            var sql = new StringBuilder($@"
select * from {tableName} 
 where {ExpressionAnalyze.Where(where)}
 order by {ExpressionAnalyze.OrderBy(orderBy)}");
            return dataOperator.QueryObject<T>(sql.ToString());
        }

        public override object Sum<T>(Expression where, Expression prop)
        {
            throw new NotImplementedException();
        }

        public override T Update<T>(T t)
        {
            throw new NotImplementedException();
        }

        public override List<T> Update<T>(IEnumerable<T> ts)
        {
            throw new NotImplementedException();
        }
    }
}