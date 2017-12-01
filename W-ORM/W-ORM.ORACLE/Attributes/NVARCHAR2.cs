using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.ORACLE.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NVARCHAR2 : BaseAttribute
    {
        public NVARCHAR2(int maxLength = 4000) : base("Type", "NVARCHAR2")
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
