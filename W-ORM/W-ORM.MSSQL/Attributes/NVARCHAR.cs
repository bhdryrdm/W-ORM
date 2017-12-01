using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.MSSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NVARCHAR : BaseAttribute
    {
        public NVARCHAR(int maxLength = 4000) : base("Type","NVARCHAR")
        {
            this.maxLength = maxLength;
        }
        private int maxLength;

        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }
        public override object TypeId => 1000;
    }
}
