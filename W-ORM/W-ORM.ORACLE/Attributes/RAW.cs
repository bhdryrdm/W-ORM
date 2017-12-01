using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using W_ORM.Layout.Attributes;

namespace W_ORM.ORACLE.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class RAW : BaseAttribute
    {
        public RAW(int maxlength = 2000) : base("Type", "RAW")
        {
            this.maxlength = maxlength;
        }
        private int maxlength;
        public int MaxLength
        {
            get { return maxlength; }
            set { maxlength = value; }
        }
    }
}
