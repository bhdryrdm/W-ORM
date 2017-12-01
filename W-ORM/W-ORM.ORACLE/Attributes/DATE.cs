using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using W_ORM.Layout.Attributes;

namespace W_ORM.ORACLE.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class DATE : BaseAttribute
    {
        public DATE() : base("Type", "DATE")
        {
        }
        public override object TypeId => 1000;

    }
}
