using MyOrm.DBOperator;
using MyOrm.Enums;
using MyOrm.Factories;
using MyOrm.QueryProvider;
using MyOrm.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MyOrm.DBContext
{
    public abstract class AbsDBContext<T> : IQueryable<T>, IOrderedQueryable<T> where T : class
    {
        private Dictionary<T, EntityEntry<T>> entityDict = new Dictionary<T, EntityEntry<T>>();
        private Dictionary<object, T> pkDict = new Dictionary<object, T>();
        private PropertyInfo entityPKProp;
        public AbsDBContext()
        {
            this.entityPKProp = this.GetPKProperty(this.ElementType);
            this.provider = new QueryProvider<T>();
            this.expression = Expression.Constant(this);

        }
        public AbsDBContext(IQueryProvider provider, Expression expression)
        {
            this.entityPKProp = this.GetPKProperty(this.ElementType);
            this.provider = provider;
            this.expression = expression;
        }
        #region 4Select
        private readonly IQueryProvider provider;
        private readonly Expression expression;
        public Type ElementType
        {
            get
            {
                return TypeHepler.GetClassGenericType(this.GetType());
            }
        }

        public Expression Expression
        {
            get
            {
                return this.expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return this.provider;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            var result = this.provider.Execute<List<T>>(expression);
            if (result == null)
            {
                yield break;
            }
            foreach (var item in result)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void InsertEntityEntry(T entity)
        {
            var pk = this.entityPKProp.GetValue(entity);
            if (pkDict.ContainsKey(pk))
            {
                return;
            }
            var entry = new EntityEntry<T>(entity, EntityState.Select);
            this.entityDict.Add(entity, entry);
            this.pkDict.Add(pk, entity);
        }
        #endregion
        public virtual T Add(T t)
        {
            return t;
        }
        public virtual List<T> AddRange(IEnumerable<T> ts)
        {
            return ts.ToList();
        }

        public int SaveChanges()
        {
            foreach (var kv in this.pkDict)
            {
                if (kv.Key != this.entityPKProp.GetValue(kv.Value))
                {
                    throw new InvalidOperationException("You Can't Modify Entity's Primary Key");
                }
            }
            var insert = new List<T>();
            var delete = new List<T>();
            var update = new List<T>();
            foreach (var kv in this.entityDict)
            {
                switch (kv.Value.EntityState)
                {
                    case EntityState.Insert:
                        insert.Add(kv.Key);
                        break;
                    case EntityState.Update:
                        update.Add(kv.Key);
                        break;
                    case EntityState.Delete:
                        delete.Add(kv.Key);
                        break;
                }
            }

            return 0;
        }
    }
}
