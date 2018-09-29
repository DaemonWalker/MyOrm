using System;
using System.Collections.Generic;
using System.Text;

namespace MyOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}
