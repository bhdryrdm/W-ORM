using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.MSSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class TINYINT : BaseAttribute
    {
        public TINYINT() : base("Type", "TINYINT")
        {
        }
        public override object TypeId => 1000;
    }

}
