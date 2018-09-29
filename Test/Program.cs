using MyOrm.Attributes;
using MyOrm.DBContext;
using MyOrm.Factories;
using MyOrm.QueryProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var ddd = ClassFactory.GetDBContext<DAY_INFO>();
            var d = ddd.ToList().First();
            ddd.Delete(d);
            ddd.SaveChanges();
            //Expression<Func<Money_INFO, DateTime>> exp = p => p.DateTime;
            //Expression<Func<Money_INFO, dynamic>> foo = Expression.Lambda<Func<Money_INFO, dynamic>>(exp.Body, exp.Parameters);
            // foo.Compile();
        }

        class DAY_INFO
        {
            [PrimaryKey]
            public int DAY_ID { get; set; }
            public string DATE { get; set; }
            public override string ToString()
            {
                return $"ID:{DAY_ID} DATE:{DATE}";
            }
        }

        class Money_INFO
        {
            public int MONEYINFO_ID { get; set; }
            public string USE_WAY { get; set; }
            public double USE_AMOUNT { get; set; }
            public int TYPE_ID { get; set; }
            public int DAY_ID { get; set; }

            public DateTime DateTime { get; set; }
        }
    }
    class Exp
    {
        public static string ExpressionAnalyze(Expression expression)
        {
            if (expression is BinaryExpression)
            {
                var binExp = expression as BinaryExpression;
                var left = ExpressionAnalyze(binExp.Left);
                var right = ExpressionAnalyze(binExp.Right);
                var op = string.Empty;
                switch (binExp.NodeType)
                {
                    case ExpressionType.Equal:
                        op = "=";
                        break;
                    case ExpressionType.AndAlso:
                        op = "and";
                        break;
                    case ExpressionType.OrElse:
                        op = "or";
                        break;
                    case ExpressionType.NotEqual:
                        op = "!=";
                        break;
                    case ExpressionType.GreaterThan:
                        op = ">";
                        break;
                    case ExpressionType.Add:
                        op = "+";
                        break;
                    case ExpressionType.Subtract:
                        op = "-";
                        break;
                    case ExpressionType.Multiply:
                        op = "*";
                        break;
                    case ExpressionType.Divide:
                        op = "/";
                        break;
                }
                return $"({left}  {op}  {right})";
            }
            else if (expression is MemberExpression)
            {
                var memExp = expression as MemberExpression;
                return $"{memExp.Member.DeclaringType.Name}.{ memExp.Member.Name}";
            }
            else if (expression is ConstantExpression)
            {
                var conExp = expression as ConstantExpression;
                if (conExp.Type == typeof(string))
                {
                    return $"'{conExp.Value.ToString()}'";
                }
                else if (conExp.Type == typeof(int))
                {
                    return conExp.Value.ToString();
                }
                else if (conExp.Type == typeof(bool))
                {
                    if (((bool)conExp.Value) == true)
                    {
                        return "(1=1)";
                    }
                    else
                    {
                        return "(1=0)";
                    }
                }
            }

            throw new ArgumentException("Invaild Expression!");
        }
    }
}
