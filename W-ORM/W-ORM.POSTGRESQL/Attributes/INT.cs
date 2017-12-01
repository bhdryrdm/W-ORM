using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.POSTGRESQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class INT : BaseAttribute
    {
        public INT() : base("Type", "INT")
        {
        }
        public override object TypeId => 1000;

    }
}
