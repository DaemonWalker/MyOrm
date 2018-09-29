using MyOrm.Enums;
using MyOrm.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyOrm.DBContext
{
    public class EntityEntry<T>
    {
        public EntityEntry(T entity, EntityState entityState)
        {
            this.Entity = entity.DeepCopy();
            this.EntityState = entityState;
        }
        public T Entity { get; private set; }
        public EntityState EntityState { get; internal set; }
    }
}
