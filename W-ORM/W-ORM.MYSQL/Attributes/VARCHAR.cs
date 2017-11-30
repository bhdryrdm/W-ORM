using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W_ORM.Layout.Attributes;

namespace W_ORM.MYSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class VARCHAR : BaseAttribute
    {
        public VARCHAR(int maxlength = 4000) : base("Type", "VARCHAR")
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
