using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyOrm.QueryProvider
{
    public class Query<T> : IQueryable<T>, IOrderedQueryable<T>
    {
        private readonly IQueryProvider queryProvider;
        private readonly Expression expression;
        public Query(IQueryProvider queryProvider, Expression expression)
        {
            this.queryProvider = queryProvider;
            this.expression = expression;
        }

        public Query()
        {
            this.queryProvider = new QueryProvider<T>();
            this.expression = Expression.Constant(this);
        }

        public Type ElementType
        {
            get
            {
                return typeof(T);
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
                return this.queryProvider;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var result = this.queryProvider.Execute<List<T>>(expression);
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
            return ((IEnumerable)this.Provider.Execute(this.expression)).GetEnumerator();
        }
    }
}
