using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.ORACLE.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NOTNULL : BaseAttribute
    {
        public NOTNULL() : base("Null", "NOTNULL")
        {
        }

    }
}
