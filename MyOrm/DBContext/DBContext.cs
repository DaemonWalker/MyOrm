using MyOrm.DBOperator;
using MyOrm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyOrm.DBContext
{
    sealed class DBContext<T> : AbsDBContext<T> where T : class
    {
    }
}
