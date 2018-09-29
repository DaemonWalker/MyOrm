using MyOrm.DBOperator;
using MyOrm.Factories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyOrm.EntityDataBaseConvert
{
    abstract class AbsEntityDataBaseConvert
    {
        protected AbsDataOperator dataOperator;
        public AbsEntityDataBaseConvert()
        {
            dataOperator = ClassFactory.GetDataOperator();
        }
        public abstract T Insert<T>(T t);
        public abstract List<T> Insert<T>(IEnumerable<T> ts);
        public abstract T Update<T>(T t);
        public abstract List<T> Update<T>(IEnumerable<T> ts);
        public abstract T Delete<T>(T t);
        public abstract List<T> Delete<T>(IEnumerable<T> ts);
        public abstract List<T> Select<T>(Expression where);
        public abstract List<T> Select<T>(Expression where, Expression orderBy);
        public abstract int Count<T>(Expression where);
        public abstract object Sum<T>(Expression where, Expression prop);
    }
}
