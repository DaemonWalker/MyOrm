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
    public abstract class AbsDBContext<TEntity> : IQueryable<TEntity>, IOrderedQueryable<TEntity>, IDisposable where TEntity : class
    {
        private Dictionary<TEntity, EntityEntry<TEntity>> entityDict = new Dictionary<TEntity, EntityEntry<TEntity>>();
        private Dictionary<object, TEntity> pkDict = new Dictionary<object, TEntity>();
        private PropertyInfo entityPKProp;
        public bool DetectEntityChange { get; set; } = true;
        public AbsDBContext()
        {
            this.entityPKProp = this.GetPKProperty(this.ElementType);
            this.provider = new QueryProvider<TEntity>();
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
        public IEnumerator<TEntity> GetEnumerator()
        {
            var result = this.provider.Execute<List<TEntity>>(expression);
            if (result == null)
            {
                yield break;
            }
            foreach (var item in result)
            {
                this.InsertEntityEntry(item);
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void InsertEntityEntry(TEntity entity)
        {
            var pk = this.entityPKProp.GetValue(entity);
            if (pkDict.ContainsKey(pk))
            {
                return;
            }
            var entry = new EntityEntry<TEntity>(entity, EntityState.Select);
            this.entityDict.Add(entity, entry);
            this.pkDict.Add(pk, entity);
        }
        #endregion
        public virtual EntityEntry<TEntity> Add(TEntity t)
        {
            EntityEntry<TEntity> entry = null;
            if (this.entityDict.ContainsKey(t) == false)
            {
                entry = new EntityEntry<TEntity>(t, EntityState.Insert);
                this.entityDict.Add(t, entry);
            }
            else
            {
                entry = this.entityDict[t];
            }
            return entry;
        }
        public virtual List<EntityEntry<TEntity>> AddRange(IEnumerable<TEntity> ts)
        {
            var list = new List<EntityEntry<TEntity>>();
            foreach (var item in ts)
            {
                list.Add(this.Add(item));
            }
            return list;
        }
        public virtual EntityEntry<TEntity> Delete(TEntity t)
        {
            this.entityDict[t].EntityState = EntityState.Delete;
            return this.entityDict[t];
        }
        public virtual List<EntityEntry<TEntity>> DeleteRange(IEnumerable<TEntity> ts)
        {
            var list = new List<EntityEntry<TEntity>>();
            foreach (var t in ts)
            {
                list.Add(this.Delete(t));
            }
            return list;
        }
        public virtual EntityEntry<TEntity> Update(TEntity t)
        {
            this.entityDict[t].EntityState = EntityState.Update;
            return this.entityDict[t];
        }
        public virtual List<EntityEntry<TEntity>> UpdateRange(IEnumerable<TEntity> ts)
        {
            return ts.Select(p =>
            {
                this.entityDict[p].EntityState = EntityState.Update;
                return this.entityDict[p];
            }).ToList();
        }
        public virtual void SaveChanges()
        {
            foreach (var kv in this.pkDict)
            {
                if (kv.Key.Equals(this.entityPKProp.GetValue(kv.Value)) == false)
                {
                    throw new InvalidOperationException("You Can't Modify Entity's Primary Key");
                }
            }
            if (this.DetectEntityChange)
            {
                var props = typeof(TEntity).GetProperties();
                foreach (var kv in this.entityDict)
                {
                    if (kv.Value.EntityState != EntityState.Select)
                    {
                        continue;
                    }
                    foreach (var prop in props)
                    {
                        if (prop.GetValue(kv.Key).Equals(kv.Value.Entity) == false)
                        {
                            kv.Value.EntityState = EntityState.Update;
                            break;
                        }
                    }
                }
            }
            var insert = new List<TEntity>();
            var delete = new List<TEntity>();
            var update = new List<TEntity>();
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
            var dbConvert = ClassFactory.GetEntityDBConvert();
            insert = dbConvert.Insert(insert);
            foreach (var item in insert)
            {
                this.pkDict.Add(this.entityPKProp.GetValue(item), item);
                this.entityDict[item].EntityState = EntityState.Select;
            }

            delete = dbConvert.Delete(delete);
            update = dbConvert.Update(update);

        }
        public void Dispose()
        {
        }
    }
}
