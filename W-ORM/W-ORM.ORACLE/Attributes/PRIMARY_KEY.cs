using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.ORACLE.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PRIMARY_KEY : BaseAttribute
    {
        public PRIMARY_KEY() : base("PKey", "PRIMARY_KEY")
        {

        }
        public override object TypeId => 4000;
    }
}
