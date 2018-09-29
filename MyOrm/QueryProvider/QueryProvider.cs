using MyOrm.DBOperator;
using MyOrm.EntityDataBaseConvert;
using MyOrm.Factories;
using MyOrm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyOrm.QueryProvider
{
    public class QueryProvider<T> : IQueryProvider
    {
        AbsDataOperator DataOperator = ClassFactory.GetDataOperator();
        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new Query<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            MethodCallExpression methodCall = (expression as MethodCallExpression);//.Arguments[0] as MethodCallExpression;
            if (methodCall != null && methodCall.Arguments.Count < 2)
            {
                methodCall = methodCall.Arguments[0] as MethodCallExpression;
            }
            LambdaExpression whereParm = null;
            LambdaExpression orderbyParm = null;
            while (methodCall != null)
            {
                Expression method = methodCall.Arguments[0];
                Expression lambda = methodCall.Arguments[1];
                LambdaExpression right = (lambda as UnaryExpression).Operand as LambdaExpression;
                if (methodCall.Method.Name == "Where")
                {
                    if (whereParm == null)
                    {
                        whereParm = Expression.Lambda(right.Body, right.Parameters);
                    }
                    else
                    {
                        Expression left = (whereParm as LambdaExpression).Body;
                        Expression temp = Expression.AndAlso(right.Body, left);
                        whereParm = Expression.Lambda(temp, whereParm.Parameters);
                    }
                }
                else
                {
                    if (orderbyParm == null)
                    {
                        orderbyParm = Expression.Lambda(right.Body, right.Parameters);
                    }
                    else
                    {
                        Expression left = (orderbyParm as LambdaExpression).Body;
                        Expression temp = Expression.Block(right.Body, left);
                        orderbyParm = Expression.Lambda(temp, orderbyParm.Parameters);
                    }
                }
                methodCall = method as MethodCallExpression;
            }

            AbsEntityDataBaseConvert convert = ClassFactory.GetEntityDBConvert();
            object result = null;
            if (orderbyParm != null)
            {
                result = convert.Select<T>(whereParm, orderbyParm);
            }
            else
            {
                result = convert.Select<T>(whereParm);
            }
            //if(result.GetType().GetInterface())
            return (TResult)result;
        }
    }
}
